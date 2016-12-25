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
    }
}
