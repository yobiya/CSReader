using CSReader.Command;
using CSReader.Command.Print;
using CSReader.DB;
using CSReader.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.TestTarget;

namespace UnitTest.Command
{
    [TestClass]
    public class InfoCommandTest
    {
        private const string SOLUTION_NAME = "InfoCommandTestTarget";
        private static readonly DataBaseBase _dataBase = new InMemoryDataBase();

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            var solutionPath = Target.GetSolutionPath(SOLUTION_NAME);
            CommandCreator.Create(_dataBase, new [] { "analyze", solutionPath }).Execute();

            System.Environment.CurrentDirectory = Target.GetSolutionDirectoryPath(SOLUTION_NAME);
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
            _dataBase.Disconnect();
        }

        /// <summary>
        /// Printerクラスの情報出力をテストする
        /// </summary>
        [TestMethod]
        public void ClassInfoTest()
        {
            var command = CommandCreator.Create(_dataBase, new [] { "info", "-t", "Printer" });
            var result = command.Execute();

            string expected =
@"name
  Printer

namespace
  InfoCommandTestTarget.Test1

methods
  Print
  PrintImpl";

            Assert.AreEqual(expected, result);
        }
    }
}
