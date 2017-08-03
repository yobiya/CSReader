using CSReader.Command;
using CSReader.Command.Find;
using CSReader.Command.Info;
using CSReader.DB;
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
            var command = CommandCreator.Create(null, args);
            Assert.IsTrue(command is HelpCommand);
        }

        [TestMethod]
        public void HelpCommandCreateTest()
        {
            var args = new string[] { "help" };
            var command = CommandCreator.Create(null, args);
            Assert.IsTrue(command is HelpCommand);
        }

        [TestMethod]
        public void AnalyzeCommandCreateTest()
        {
            var dataBase = new InMemoryDataBase();
            var args = new string[] { "analyze", "test.sln" };
            var command = CommandCreator.Create(dataBase, args);
            Assert.IsTrue(command is AnalyzeCommand);

            dataBase.Disconnect();
        }

        [TestMethod]
        public void InfoCommandCreateTest()
        {
            var dataBase = new InMemoryDataBase();
            var args = new string[] { "info", "-t", "class" };
            var command = CommandCreator.Create(dataBase, args);
            Assert.IsTrue(command is TypeInfoCommand);

            dataBase.Disconnect();
        }

        [TestMethod]
        public void FindCommandCreateTest()
        {
            var dataBase = new InMemoryDataBase();
            var args = new string[] { "find", "-m", "method" };
            var command = CommandCreator.Create(dataBase, args);
            Assert.IsTrue(command is FindMethodCommand);

            dataBase.Disconnect();
        }

        [TestMethod]
        public void UnsupportedCommandCreateTest()
        {
            var dataBase = new InMemoryDataBase();
            var args = new string[] { "unknown" };
            var command = CommandCreator.Create(dataBase, args);
            Assert.IsTrue(command is UnsupportedCommand);

            dataBase.Disconnect();
        }
    }
}
