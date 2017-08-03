using CSReader.Command;
using CSReader.DB;
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
            var command = CommandCreator.Create(null, args);
            Assert.AreEqual(typeof(AnalyzeCommand), command.GetType());
        }

        [TestMethod]
        public void EmptyArgsCreateTest()
        {
            var args = new string[] { "analyze" };
            var command = CommandCreator.Create(null, args);
            Assert.AreEqual(typeof(AnalyzeHelpCommand), command.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void SolutionNotFoundTest()
        {
            var dataBase = new InMemoryDataBase();

            var args = new string[] { "analyze", "dummy.sln" };
            var command = CommandCreator.Create(dataBase, args);
            command.Execute();

            dataBase.Disconnect();
        }
    }
}
