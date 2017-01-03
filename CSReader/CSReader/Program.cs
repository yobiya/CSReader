using CSReader.Analyze;
using CSReader.Command;

namespace CSReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var analyzer = new Analyzer(@"C:\Users\nanas\prj\tool\csr\CSReader\TestTarget\Simple\Simple.sln");
            analyzer.Analyze();

//            var command = CommandCreator.Create(args);
//			command.Execute();
        }
    }
}
