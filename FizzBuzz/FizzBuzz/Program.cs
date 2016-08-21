using System;

namespace FizzBuzz
{
    class Program
    {
        static void Main(string[] args)
        {
            string str;

            for(int i = 1; i <= 100; i++)
            {
                str = "";

                if (i % 3 == 0)
                    str += "Fizz";

                if (i % 5 == 0)
                    str += "Buzz";

                if (str.Length == 0)
                    str += i;

                Console.WriteLine(str);
            }
            Console.ReadKey();
        }
    }
}
