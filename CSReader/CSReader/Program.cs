using CSReader.Command;
using CSReader.DB;
using System;

namespace CSReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var dataBase = new DataBase();
                var command = CommandCreator.Create(dataBase, args);

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
