using CSReader.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Command
{
    [TestClass]
    public class AnalyzeCommandTest
    {
        [TestMethod]
        public void CreateTest()
        {
            var args = new string[] { "analyze", "dummy.sln" };
            var command = CommandCreator.Create(args);
            Assert.AreEqual(typeof(AnalyzeCommand), command.GetType());
        }

        [TestMethod]
        public void EmptyArgsCreateTest()
        {
            var args = new string[] { "analyze" };
            var command = CommandCreator.Create(args);
            Assert.AreEqual(typeof(AnalyzeHelpCommand), command.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void SolutionNotFoundTest()
        {
            var args = new string[] { "analyze", "dummy.sln" };
            var command = CommandCreator.Create(args);
            command.Execute();
        }
    }
}
