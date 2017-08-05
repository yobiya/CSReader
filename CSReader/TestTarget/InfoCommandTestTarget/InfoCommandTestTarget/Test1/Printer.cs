using System;

namespace InfoCommandTestTarget.Test1
{
    public class Printer
    {
        public virtual void Print(string message)
        {
            PrintImpl(message);
        }

        public void PrintImpl(string message)
        {
            Console.WriteLine(message);
        }
    }
}
