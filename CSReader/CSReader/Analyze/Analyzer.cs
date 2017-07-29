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
        private readonly string _solutionPath;
        private readonly DataBase _dataBase;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="solutionPath">ソリューションのパス</param>
        /// <param name="dataBase">接続済みのデータベース</param>
        public Analyzer(string solutionPath, DataBase dataBase)
        {
            _solutionPath = solutionPath;
            _dataBase = dataBase;
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

            var infoBuilder = new SyntaxInfoBuilder(syntaxWalker);
            _dataBase.InsertTypeInfos(infoBuilder.BuildTypeInfo());
            _dataBase.InsertMethodInfos(infoBuilder.BuildMethodInfo());
        }
    }
}
