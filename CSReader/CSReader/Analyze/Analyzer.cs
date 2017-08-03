using CSReader.Analyze.Info;
using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

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
                foreach (var document in project.Documents)
                {
                    AnalyzeDocument(document);
                }
            }
        }

        /// <summary>
        /// ドキュメントを解析する
        /// </summary>
        /// <param name="document">ドキュメント</param>
        private void AnalyzeDocument(Document document)
        {
            var rootNode = document.GetSyntaxRootAsync().Result;

            var syntaxWalker = new SyntaxWalker();
            syntaxWalker.Visit(rootNode);

            var infoBuilder = new SyntaxInfoBuilder(_dataBase, syntaxWalker, _idGenerator);
            infoBuilder.BuildNamespaceInfos();
            infoBuilder.BuildTypeInfos();
            infoBuilder.BuildMethodInfos();
        }
    }
}
