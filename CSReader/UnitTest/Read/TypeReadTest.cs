using CSReader.Analyze;
using CSReader.DB;
using CSReader.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.TestTarget;

namespace UnitTest.Read
{
    [TestClass]
    public class TypeReadTest
    {
        private static DataBase _dataBase;

        [TestInitialize]
        public void Initialize()
        {
            var solutionPath = Target.GetSolutionPath("Simple");
            _dataBase = new DataBase();
            _dataBase.Connect(solutionPath);

            var analyzer = new Analyzer(solutionPath, _dataBase);
            analyzer.Analyze();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _dataBase.Disconnect();
        }

        [TestMethod]
        public void ClassTypeReadTest()
        {
            var reader = new TypeReader(_dataBase);
            var readInfo = reader.Read("Program");

            Assert.AreEqual("Program", readInfo.Name);
            Assert.AreEqual("Simple", readInfo.NameSpace);
        }
    }
}
