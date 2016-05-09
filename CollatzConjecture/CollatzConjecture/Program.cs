using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollatzConjecture
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            int num, count;
            bool done = false;

            while (!done)
            {
                Console.WriteLine("Enter an integer greater than 1 to calculate the number of steps in Collatz Conjecture. Enter 0 to exit.");
                input = Console.ReadLine();

                if (input == "0")
                    done = true;
                else if (int.TryParse(input, out num) && num > 1)
                {
                    count = 0;
                    while (num > 1)
                    {
                        if(num % 2 == 0)
                        {
                            num = num / 2;
                            count++;
                            Console.WriteLine(num);
                        } else
                        {
                            num = (num * 3) + 1;
                            count++;
                            Console.WriteLine(num);
                        }
                    }
                    Console.WriteLine("The number of steps in Collatz Conjecture = " + count + "\n");
                }
                else
                    Console.WriteLine("You did not enter an integer greater than 1.\n");
            }
        }
    }
}
