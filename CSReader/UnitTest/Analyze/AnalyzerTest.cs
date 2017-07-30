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
            using (var dataBase = new DataBase())
            {
                dataBase.ConnectInMemory(false);

                var solutionPath = Target.GetSolutionPath("Simple");
                var analyzer = new Analyzer(solutionPath, dataBase);
                analyzer.Analyze();
            }
        }
    }
}
