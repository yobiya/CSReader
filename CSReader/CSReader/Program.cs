using CSReader.Command;

namespace CSReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = CommandCreator.Create(args);

            var resultCode = command.Execute();

            System.Environment.Exit((int)resultCode);
        }
    }
}
