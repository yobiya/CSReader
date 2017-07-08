using CSReader.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Command
{
    [TestClass]
    public class CommandCreateTest
    {
        [TestMethod]
        public void UsageCommandCreateTest()
        {
			// 引数がなければ、ヘルプコマンドを生成する
            var args = new string[0];
            var command = CommandCreator.Create(args);
            Assert.IsTrue(command is HelpCommand);
        }

        [TestMethod]
        public void HelpCommandCreateTest()
        {
            var args = new string[] { "help" };
            var command = CommandCreator.Create(args);
            Assert.IsTrue(command is HelpCommand);
        }

        [TestMethod]
        public void UnsupportedCommandCreateTest()
        {
            var args = new string[] { "unknown" };
            var command = CommandCreator.Create(args);
            Assert.IsTrue(command is UnsupportedCommand);
        }
    }
}
