using CSReader;
using CSReader.Command;
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

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            var solutionPath = Target.GetSolutionPath(SOLUTION_NAME);
            CommandCreator.Create(new [] { "analyze", solutionPath }).Execute();

            System.Environment.CurrentDirectory = Target.GetSolutionDirectoryPath(SOLUTION_NAME);
        }

        [TestMethod]
        public void FindMethodNameTest()
        {
            var command = CommandCreator.Create(new [] { "find", "-m", "VirtualMethod" });

            var result = command.Execute();

            string expected = @"Method.SubClass.VirtualMethod
Method.SuperClass.VirtualMethod";

            Assert.AreEqual(expected, result);
        }
    }
}
