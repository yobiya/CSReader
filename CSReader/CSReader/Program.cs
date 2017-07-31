using System;
using CSReader.Command;

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
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}
