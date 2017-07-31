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
                    NameSpace = "Simple"
                };
            var text = PrintConverter.Convert(info);

            string nl = System.Environment.NewLine;
            string expectedText = "name" + nl;
            expectedText += "  Program" + nl;
            expectedText += nl;
            expectedText += "namespace" + nl;
            expectedText += "  Simple" + nl;

            Assert.AreEqual(expectedText, text);
        }
    }
}
