using CSReader.Analyze.Row;
using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Analyze
{
    /// <summary>
    /// 構文解析を行う
    /// </summary>
    public class SyntaxAnalyzer
    {
        private readonly DataBaseBase _dataBase;
        private readonly UniqueIdGenerator _idGenerator;
        private readonly DeclarationAnalyzer _declarationAnalyzer;

        public SyntaxAnalyzer(DataBaseBase dataBase, UniqueIdGenerator idGenerator)
        {
            _dataBase = dataBase;
            _idGenerator = idGenerator;
            _declarationAnalyzer = new DeclarationAnalyzer(dataBase, idGenerator);
        }

        public void Analyze(CompilationUnitSyntax rootSyntax)
        {
            AnalyzeChildNodes(rootSyntax, 0);
        }

        private void AnalyzeChildNodes(SyntaxNode syntaxNode, int parentId)
        {
            foreach (var node in syntaxNode.ChildNodes())
            {
                AnalyzeSyntax((dynamic)node, parentId);
            }
        }

        private void AnalyzeSyntax(NamespaceDeclarationSyntax syntax, int parentId)
        {
            var row = _declarationAnalyzer.AnalyzeNamespace(syntax.Name.ToString());

            AnalyzeChildNodes(syntax, row.Id);
        }

        private void AnalyzeSyntax(BaseTypeDeclarationSyntax syntax, int parentId)
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

            AnalyzeChildNodes(syntax, row.Id);
        }
/*
        private void AnalyzeSyntax(MethodDeclarationSyntax syntax, int parentId)
        {
            // 必ず新しいメソッドの構文が入ってくるので、チェック処理が行われる DeclarationAnalyzer は使用しない
            var row
                = new MethodDeclarationRow
                    {
                        Id = _idGenerator.Generate(),
                        Name = syntax.Identifier.Text,
                        ParentTypeId = parentId,
                        UnieuqName = ConvertUniqueName(syntax),
                        QualifierValue = ConvertQualifier(syntax.Modifiers)
                    };

            _dataBase.Insert(row);
        }
 */
        private void AnalyzeSyntax(SyntaxNode syntax, int parentId)
        {
            // 未対応の定義は無視する
        }

        private string ConvertUniqueName(MethodDeclarationSyntax syntax)
        {
            string name = syntax.Identifier.Text + "(";

            var argTypeNameList = new List<string>();
            foreach (var param in syntax.ParameterList.Parameters)
            {
                var nodes = param.ChildNodes();
                var first = nodes.FirstOrDefault();
                if (first == null)
                {
                    break;
                }

                if (first.ToString() == "this")
                {
                    nodes = nodes.Skip(1);
                }

                argTypeNameList.Add(nodes.First().ToString());
            }

            if (argTypeNameList.Any())
            {
                name += argTypeNameList.Aggregate((a, b) => $"{a}, {b}");
            }

            name += ") " + syntax.ReturnType.ToString();

            return name;
        }

        public static MethodDeclarationRow.Qualifier ConvertQualifier(SyntaxTokenList syntaxTokenList)
        {
            foreach (var token in syntaxTokenList)
            {
                switch (token.Text)
                {
                case "virtual":
                    return MethodDeclarationRow.Qualifier.Virtual;

                case "override":
                    return MethodDeclarationRow.Qualifier.Override;

                case "static":
                    return MethodDeclarationRow.Qualifier.Static;
                }
            }

            return MethodDeclarationRow.Qualifier.None;
        }
    }
}
