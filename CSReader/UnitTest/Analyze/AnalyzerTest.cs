using CSReader.Analyze;
using CSReader.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.TestTarget;

namespace UnitTest.Analyze
{
    [TestClass]
    public class AnalyzerTest
    {
        [TestMethod]
        public void AnalyzeTest()
        {
            var solutionPath = Target.GetSolutionPath("Simple");
            using (var dataBase = new DataBase())
            {
                dataBase.Connect(solutionPath);

                var analyzer = new Analyzer(solutionPath, dataBase);
                analyzer.Analyze();
            }
        }
    }
}
