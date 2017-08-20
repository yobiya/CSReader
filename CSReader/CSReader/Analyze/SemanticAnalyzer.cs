using CSReader.Analyze.Row;
using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Analyze
{
    /// <summary>
    /// 意味解析を行う
    /// </summary>
    public class SemanticAnalyzer
    {
        private readonly DataBaseBase _dataBase;
        private readonly SemanticModel _semanticModel;
        private readonly UniqueIdGenerator _idGenerator;
        private readonly DeclarationAnalyzer _declarationAnalyzer;

        public SemanticAnalyzer(DataBaseBase dataBase, SemanticModel semanticModel, UniqueIdGenerator idGenerator)
        {
            _dataBase = dataBase;
            _semanticModel = semanticModel;
            _idGenerator = idGenerator;
            _declarationAnalyzer = new DeclarationAnalyzer(dataBase, idGenerator);
        }

        public void Analyze(SyntaxNode rootNode)
        {
            AnalyzeNodeSemantic((dynamic)rootNode, 0);
        }

        private void AnalyzeChildNodesSemantic(SyntaxNode node, int parentId)
        {
            foreach (var childNode in node.ChildNodes())
            {
                AnalyzeNodeSemantic((dynamic)childNode, parentId);
            }
        }

        private void AnalyzeNodeSemantic(NamespaceDeclarationSyntax syntax, int parentId)
        {
            var row = _declarationAnalyzer.AnalyzeNamespace(syntax.Name.ToString());

            AnalyzeChildNodesSemantic(syntax, row.Id);
        }

        private void AnalyzeNodeSemantic(BaseTypeDeclarationSyntax syntax, int parentId)
        {
           TypeKind typeKind = TypeKind.Unknown;
           if (syntax is ClassDeclarationSyntax) { typeKind = TypeKind.Class; }
           else if (syntax is StructDeclarationSyntax) { typeKind = TypeKind.Struct; }
           else if (syntax is InterfaceDeclarationSyntax) { typeKind = TypeKind.Interface; }
           else if (syntax is EnumDeclarationSyntax) { typeKind = TypeKind.Enum; }

            var row
                = _declarationAnalyzer
                    .AnalyzeType(
                        syntax.Identifier.Text,
                        parentId,
                        typeKind);

            AnalyzeChildNodesSemantic(syntax, row.Id);
        }

        private void AnalyzeNodeSemantic(MethodDeclarationSyntax node, int parentId)
        {
            // メソッドの定義は引数を最初に処理する
            var childNodes = node.ChildNodes();
            var returnType = childNodes.First();
            var parameterList = childNodes.Skip(1).First();

            var argTypeNameList = new List<string>();
            string uniqueName = node.Identifier.Text + "(";
            foreach (var parameter in parameterList.ChildNodes())
            {
                var pramChildNodes = parameter.ChildNodes();
                if(!pramChildNodes.Any())
                {
                    continue;
                }

                SyntaxNode first = pramChildNodes.First();
                if (first.ToString() == "this")
                {
                    uniqueName += "this ";
                    pramChildNodes = pramChildNodes.Skip(1);
                    first = pramChildNodes.First();
                }

                AnalyzeNodeSemantic(first, 0);
                var symbol= _semanticModel.GetSymbolInfo(first).Symbol;
                argTypeNameList.Add(symbol.ToString());
            }

            if (argTypeNameList.Any())
            {
                uniqueName += argTypeNameList.Aggregate((a, b) => $"{a}, {b}");
            }

            uniqueName += ") " + returnType.ToString();

            var qualifier = MethodDeclarationRow.Qualifier.None;
            foreach (var token in node.Modifiers)
            {
                switch (token.Text)
                {
                case "virtual": qualifier = MethodDeclarationRow.Qualifier.Virtual; break;
                case "override": qualifier = MethodDeclarationRow.Qualifier.Override; break;
                case "static": qualifier = MethodDeclarationRow.Qualifier.Static; break;
                }
            }

            var row
                = _declarationAnalyzer
                    .AnalyzeMethod(
                        node.Identifier.Text,
                        parentId,
                        uniqueName,
                        qualifier);

            foreach (var childNode in childNodes.Skip(2))
            {
                AnalyzeNodeSemantic((dynamic)childNode, row.Id);
            }
        }

        private void AnalyzeNodeSemantic(SyntaxNode node, int parentId)
        {
            int id = 0;
            var symbolInfo = _semanticModel.GetSymbolInfo(node);
            if (symbolInfo.Symbol != null)
            {
                id = AnalyzeNode((dynamic)node, symbolInfo);
            }

            AnalyzeChildNodesSemantic(node, id);
        }

        private int AnalyzeNode(InvocationExpressionSyntax node, SymbolInfo symbolInfo)
        {
            var methodSymbol = (IMethodSymbol)symbolInfo.Symbol;
            var methodFullName = methodSymbol.ToString();

            //todo '.'の数で判定する
            var names = methodFullName.Split(new [] { '.' });
            var length = names.Length;
            if (length == 1)
            {
                // ローカルメソッドなので、終了する
                return 0;
            }

            int definitionId = AnalyzeSymbol(methodSymbol.OriginalDefinition);

            try
            {
                var row = new MethodInvocationRow
                {
                    Id = _idGenerator.Generate(),
                    Name = node.ToString(),
                    MethodDeclarationId = definitionId
                };

                _dataBase.Insert<MethodInvocationRow>(row);

                return row.Id;
            }
            catch (System.Exception e)
            {
                //todo 未対応の処理で例外が起きたので、ログを残す
                System.Console.WriteLine("Warning : " + e.ToString());

                return 0;
            }
        }

        private int AnalyzeNode(IdentifierNameSyntax node, SymbolInfo symbolInfo)
        {
            var symbol = symbolInfo.Symbol;

            switch (symbol.Kind)
            {
            case SymbolKind.Method:
                return AnalyzeSymbol((IMethodSymbol)symbol);
            }

            return 0;
        }

        private int AnalyzeNode(SyntaxNode node, SymbolInfo symbolInfo)
        {
            //todo 未対応のノードなので無視する
            return 0;
        }

        private int AnalyzeSymbol(IMethodSymbol symbol)
        {
            int parentId = AnalyzeSymbol(symbol.ReceiverType);

            string uniqueName = symbol.Name + "(";

            if (symbol.Parameters.Any())
            {
                uniqueName += symbol.Parameters.Select(p => p.ToString()).Aggregate((a, b) => $"{a}, {b}");
            }

            uniqueName += ") " + symbol.ReturnType.ToString();

            MethodDeclarationRow.Qualifier qualifier = MethodDeclarationRow.Qualifier.None;
            if (symbol.IsVirtual) { qualifier = MethodDeclarationRow.Qualifier.Virtual; }
            else if (symbol.IsOverride) { qualifier = MethodDeclarationRow.Qualifier.Override; }
            else if (symbol.IsStatic) { qualifier = MethodDeclarationRow.Qualifier.Static; }

            return _declarationAnalyzer.AnalyzeMethod(symbol.Name, parentId, uniqueName, qualifier).Id;
        }

        private int AnalyzeSymbol(ITypeSymbol symbol)
        {
            int parentId = AnalyzeSymbol((dynamic)symbol.ContainingSymbol);
            return _declarationAnalyzer.AnalyzeType(symbol.Name, parentId, symbol.TypeKind).Id;
        }

        private int AnalyzeSymbol(INamespaceSymbol symbol)
        {
            if (symbol.IsGlobalNamespace)
            {
                return 0;
            }

            return _declarationAnalyzer.AnalyzeNamespace(symbol.ToString()).Id;
        }
    }
}
