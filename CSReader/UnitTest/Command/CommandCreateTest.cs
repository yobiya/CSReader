using CSReader.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Command
{
    [TestClass]
    public class CommandCreateTest
    {
        [TestMethod]
        public void CreateAnalyzeCommandTest()
        {
            var args = new string[] { "analyze" };
            var command = CommandCreator.Create(args);
            Assert.IsTrue(command is AnalyzeCommand);
        }
    }
}
