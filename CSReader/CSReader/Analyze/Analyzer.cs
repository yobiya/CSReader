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
		private readonly string _solutionPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="solutionPath">ソリューションのパス</param>
		public Analyzer(string solutionPath)
		{
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
		}
    }
}
