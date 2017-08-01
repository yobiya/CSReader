using CSReader.Command;
using System;

namespace CSReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var command = CommandCreator.Create(args);

                var text = command.Execute();
                Console.WriteLine(text);
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.Exit(1);
            }
        }
    }
}
