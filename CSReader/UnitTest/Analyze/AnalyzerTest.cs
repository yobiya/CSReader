using CSReader.Analyze;
using CSReader.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Analyze
{
    [TestClass]
    public class AnalyzerTest
    {
        [TestMethod]
        public void AnalyzeTest()
        {
            var solutionPath = GetSolutionPath("Simple");
            using (var dataBase = new DataBase())
            {
                dataBase.Connect(solutionPath);

                var analyzer = new Analyzer(solutionPath, dataBase);
                analyzer.Analyze();
            }
        }

        private static string GetSolutionPath(string name)
        {
            return "../../../TestTarget/" + name + "/" + name + ".sln";
        }
    }
}
