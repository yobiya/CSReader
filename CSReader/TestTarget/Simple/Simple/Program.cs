using System;

namespace Simple
{
    /// <summary>
    /// 単純なプロジェクト
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
			Print(1 + 2);
        }

		private static void Print(int number)
		{
			Console.WriteLine("Sample print : " + number);
		}
    }
}
