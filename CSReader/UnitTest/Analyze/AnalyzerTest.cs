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
            var dataBase = new InMemoryDataBase();
            dataBase.Connect(null, false);

            var solutionPath = Target.GetSolutionPath("Simple");
            var analyzer = new Analyzer(dataBase, solutionPath);
            analyzer.Analyze();

            dataBase.Disconnect();
        }
    }
}
