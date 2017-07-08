using CSReader.Analyze;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Analyze
{
    [TestClass]
    public class AnalyzerTest
    {
        [TestMethod]
        public void AnalyzeTest()
        {
			var analyzer = new Analyzer(GetSolutionPath("Simple"));
			analyzer.Analyze();
        }

		private static string GetSolutionPath(string name)
		{
			return "../../../TestTarget/" + name + "/" + name + ".sln";
		}
    }
}
