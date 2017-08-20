using CSReader.Command;
using CSReader.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.TestTarget;

namespace UnitTest.Command
{
    /// <summary>
    /// FindCommandTest の概要の説明
    /// </summary>
    [TestClass]
    public class FindCommandTest
    {
        private const string SOLUTION_NAME = "Method";
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

        [TestMethod]
        public void FindMethodNameTest()
        {
            var command = CommandCreator.Create(_dataBase, new [] { "find", "-m", "name==VirtualMethod" });

            var result = command.Execute();

            string expected =
@"Method.SubClass.VirtualMethod
Method.SuperClass.VirtualMethod";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindMethodVirtualQualifierTest()
        {
            var command = CommandCreator.Create(_dataBase, new [] { "find", "-m", "virtual" });

            var result = command.Execute();

            string expected = @"Method.SuperClass.VirtualMethod";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindMethodCountOneTest()
        {
            var command = CommandCreator.Create(_dataBase, new [] { "find", "-m", "call_count==1" });

            var result = command.Execute();

            string expected = 
@"NoNamespace.AddValue
Method.SuperClass.PublicMethod
System.Console.WriteLine
System.Int32.ToString";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindMethodHaveNotNamespaceTest()
        {
            var command = CommandCreator.Create(_dataBase, new [] { "find", "-m", "name==AddValue" });

            var result = command.Execute();

            string expected = @"NoNamespace.AddValue";

            Assert.AreEqual(expected, result);
        }
    }
}
