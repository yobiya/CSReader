using CSReader.Command;
using System;
using System.Linq;

namespace CSReader
{
    class Program
    {
        static void Main(string[] args)
        {
			if (args.Length == 0)
			{
                Console.WriteLine("Usage : csr help");
				Environment.Exit(0);
			}
        }

		private static ICommand CreateCommand(string[] args)
		{
			var commandName = args[1];
			var commandArgs = args.Skip(1);
			switch (commandName)
			{
			case AnalyzeCommand.COMMAND_NAME:
				return AnalyzeCommand.Create(commandArgs);
			}

			return new UnsupportedCommand(commandName);
		}
    }
}
