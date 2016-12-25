using CSReader.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Command
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateTest()
        {
            var args = new string[] { "analyze", "dummy.sln" };
            var command = CommandCreator.Create(args);
            Assert.AreEqual(typeof(AnalyzeCommand), command.GetType());
        }

        [TestMethod]
        public void CreateEmptyArgsTest()
        {
            var args = new string[] { "analyze" };
            var command = CommandCreator.Create(args);
            Assert.AreEqual(typeof(AnalyzeHelpCommand), command.GetType());
        }
    }
}
