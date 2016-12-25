using CSReader.Command;

namespace CSReader
{
    class Program
    {
        static void Main(string[] args)
        {
			var command = CommandCreator.Create(args);
			command.Execute();
        }
    }
}
