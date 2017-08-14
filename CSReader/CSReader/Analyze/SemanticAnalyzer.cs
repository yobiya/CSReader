using CSReader.Analyze.Row;
using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Analyze
{
    public class SemanticAnalyzer
    {
        private readonly DataBaseBase _dataBase;
        private readonly SemanticModel _semanticModel;
        private readonly SyntaxNode _rootNode;
        private readonly UniqueIdGenerator _idGenerator;

        public SemanticAnalyzer(DataBaseBase dataBase, SemanticModel semanticModel, SyntaxNode rootNode, UniqueIdGenerator idGenerator)
        {
            _dataBase = dataBase;
            _semanticModel = semanticModel;
            _rootNode = rootNode;
            _idGenerator = idGenerator;
        }

        public void BuildMethodInvocation()
        {
            AnalyzeNodesSemantic(new [] { _rootNode });
        }

        private void AnalyzeNodesSemantic(IEnumerable<SyntaxNode> nodes)
        {
            foreach (var node in nodes)
            {
                var symbolInfo = _semanticModel.GetSymbolInfo(node);
                if (symbolInfo.Symbol != null)
                {
                    AnalyzeNode((dynamic)node, symbolInfo);
                }

                AnalyzeNodesSemantic(node.ChildNodes());
            }
        }

        private void AnalyzeNode(InvocationExpressionSyntax node, SymbolInfo symbolInfo)
        {
            var methodSymbol = (IMethodSymbol)symbolInfo.Symbol;
            var methodFullName = methodSymbol.ToString();

            var names = methodFullName.Split(new [] { '.' });
            var length = names.Length;
            if (length == 1)
            {
                // ローカルメソッドなので、終了する
                return;
            }

            var methodName = names[length - 1] + " " + methodSymbol.ReturnType.ToString();
            var parentTypeName = names[length - 2];
            var namespaceName = (names.Length <= 2) ? null : names.Take(length - 2).Aggregate((a, b) => $"{a}.{b}");

            var methodRows = _dataBase.SelectInfos<MethodDeclarationRow>(i => i.UnieuqName == methodName);
            var parentTypeRows = _dataBase.SelectInfos<TypeDeclarationRow>(i => i.Name == parentTypeName);
            var namespaceRow = (namespaceName == null) ? null : _dataBase.SelectInfo<NamespaceDeclarationRow>(i => i.Name == namespaceName);

            if (!methodRows.Any() || !parentTypeRows.Any() || namespaceRow == null)
            {
                // 定義がなかったので、終了する
                //todo namespaceは無い場合があるので、適切に対応する
                return;
            }

            try
            {
                var parentTypeRow = parentTypeRows.Where(i => i.ParentId == namespaceRow.Id).Single();
                var methodRow = methodRows.Where(i => i.ParentTypeId == parentTypeRow.Id).Single();

                var row = new MethodInvocationRow
                {
                    Id = _idGenerator.Generate(),
                    Name = node.ToString(),
                    MethodDeclarationId = methodRow.Id
                };

                _dataBase.Insert<MethodInvocationRow>(row);
            }
            catch (System.Exception e)
            {
                //todo 未対応の処理で例外が起きたので、ログを残す
                System.Console.WriteLine("Warning : " + e.ToString());
            }
        }

        private void AnalyzeNode(SyntaxNode node, SymbolInfo symbolInfo)
        {
            //todo 未対応のノードなので無視する
        }
    }
}
