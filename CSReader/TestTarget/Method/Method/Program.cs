namespace Method
{
    class Program
    {
        static void Main(string[] args)
        {
            var subClass = new SubClass();

            subClass.PublicMethod();

            var calculator = new NoNamespace(5);
            calculator.AddValue(2);

            System.Console.WriteLine(calculator.Value.ToString());
        }
    }
}
