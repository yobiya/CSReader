using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using CSReader.Analyze.Row;

namespace CSReader.Analyze.Info
{
    public class SemanticInfoBuilder
    {
        private readonly DataBaseBase _dataBase;
        private readonly SemanticModel _semanticModel;
        private readonly SyntaxNode _rootNode;
        private readonly UniqueIdGenerator _idGenerator;

        public SemanticInfoBuilder(DataBaseBase dataBase, SemanticModel semanticModel, SyntaxNode rootNode, UniqueIdGenerator idGenerator)
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
            var methodFullName = symbolInfo.Symbol.ToString();

            var names = methodFullName.Split(new [] { '.' });

            var length = names.Length;
            var methodName = names[length - 1];
            var parentTypeName = names[length - 2];
            var namespaceName = names.Take(length - 2).Aggregate((a, b) => $"{a}.{b}");

            var methodInfos = _dataBase.SelectInfos<MethodInfo>(i => i.UnieuqName == methodName);
            var parentTypeInfos = _dataBase.SelectInfos<TypeInfo>(i => i.Name == parentTypeName);
            var namespaceInfo = _dataBase.SelectInfo<NamespaceInfo>(i => i.Name == namespaceName);

            if (!methodInfos.Any() || !parentTypeInfos.Any() || namespaceInfo == null)
            {
                // 定義がなかったので、終了する
                return;
            }

            var parentTypeInfo = parentTypeInfos.Where(i => i.NamespaceId == namespaceInfo.Id).Single();
            var methodInfo = methodInfos.Where(i => i.ParentTypeId == parentTypeInfo.Id).Single();

            var row = new MethodInvocationRow
            {
                Id = _idGenerator.Generate(),
                Name = node.ToString(),
                MethodDeclarationId = methodInfo.Id
            };

            _dataBase.InsertInfo<MethodInvocationRow>(row);
        }

        private void AnalyzeNode(SyntaxNode node, SymbolInfo symbolInfo)
        {
            // 未対応のノードなので無視する
        }
    }
}
