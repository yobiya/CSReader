namespace InfoCommandTestTarget
{
    class Program
    {
        static void Main(string[] args)
        {
            var printer = new Test1.Printer();
            var message = new Test1.Printer.Message("Test ", "message");
            printer.Print(message);
        }
    }
}
