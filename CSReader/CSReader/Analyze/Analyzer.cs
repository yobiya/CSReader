﻿using CSReader.Analyze.Row;
using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System.Linq;

namespace CSReader.Analyze
{
    /// <summary>
    /// ソリューションの解析器
    /// </summary>
    public class Analyzer
    {
        private readonly DataBaseBase _dataBase;
        private readonly string _solutionPath;
        private readonly UniqueIdGenerator _idGenerator = new UniqueIdGenerator();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataBase">接続済みのデータベース</param>
        /// <param name="solutionPath">ソリューションのパス</param>
        public Analyzer(DataBaseBase dataBase, string solutionPath)
        {
            _dataBase = dataBase;
            _solutionPath = solutionPath;
        }

        /// <summary>
        /// 解析を実行する
        /// </summary>
        public void Analyze()
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(_solutionPath).Result;

            foreach (var project in solution.Projects)
            {
                var rootNodes = project.Documents.Select(d => d.GetSyntaxRootAsync().Result).ToArray();
                foreach (var rootNode in rootNodes)
                {
                    AnalyzeDocumentSyntax(rootNode);
                }

                // 構文解析が終わってから、意味解析を行う
                var compilation = project.GetCompilationAsync().Result;
                foreach (var rootNode in rootNodes)
                {
                    AnalyzeDocumentSemantic(compilation, rootNode);
                }
            }
        }

        /// <summary>
        /// ドキュメントを構文解析する
        /// </summary>
        /// <param name="rootNode">ドキュメントのルートノード</param>
        private void AnalyzeDocumentSyntax(SyntaxNode rootNode)
        {
            var syntaxWalker = new SyntaxWalker();
            syntaxWalker.Visit(rootNode);

            var infoBuilder = new SyntaxInfoBuilder(_dataBase, syntaxWalker, _idGenerator);
            infoBuilder.BuildNamespaceInfos();
            infoBuilder.BuildTypeInfos();
            infoBuilder.BuildMethodInfos();
        }

        /// <summary>
        /// ドキュメントを構文解析する
        /// </summary>
        /// <param name="compilation">コンパイル情報</param>
        /// <param name="rootNode">ドキュメントのルートノード</param>
        private void AnalyzeDocumentSemantic(Compilation compilation, SyntaxNode rootNode)
        {
            var semanticModel = compilation.GetSemanticModel(rootNode.SyntaxTree);
            var infoBuilder = new SemanticInfoBuilder(_dataBase, semanticModel, rootNode, _idGenerator);
            infoBuilder.BuildMethodInvocation();
        }
    }
}
