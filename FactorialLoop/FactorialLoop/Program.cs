using System;

namespace FactorialLoop
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            int num, factorial;
            bool done = false;

            while(!done)
            {
                Console.WriteLine("Enter an positive integer to calculate the factorial. Enter 0 to exit.");
                input = Console.ReadLine();
                factorial = 1;

                if (input == "0")
                    done = true;
                else if (int.TryParse(input, out num) && num > 0)
                {
                    for(int i = 0; i < num; i++)
                    {
                        factorial *= num - i;
                    }
                    Console.WriteLine(num + " factorial = " + factorial + "\n");
                }
                else
                    Console.WriteLine("You did not enter a positive integer.\n");
            }
            
        }
    }
}
