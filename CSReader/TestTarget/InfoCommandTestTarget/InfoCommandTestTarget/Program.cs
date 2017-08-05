namespace InfoCommandTestTarget
{
    class Program
    {
        static void Main(string[] args)
        {
            var printer = new Test1.Printer();
            printer.Print("Test message.");
        }
    }
}
