using CSReader.Command.Print;
using CSReader.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Command
{
    [TestClass]
    public class InfoCommandTest
    {
        [TestMethod]
        public void InfoTextTest()
        {
            var info
                = new TypeReader.Info
                {
                    Name = "Program",
                    NameSpace = "Simple",
                    Methods = new [] { "Main", "Print" }
                };
            var text = PrintConverter.Convert(info);

            string nl = System.Environment.NewLine;
            string expectedText =
@"name
  Program

namespace
  Simple

methods
  Main
  Print";

            Assert.AreEqual(expectedText, text);
        }
    }
}
