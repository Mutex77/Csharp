using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReverseString
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            string output;
            bool done = false;

            while(!done)
            {
                Console.WriteLine("Enter a string and I will reverse it for you! Enter 0 to exit.");
                input = Console.ReadLine();
                output = "";
                if (input == "0")
                    done = true;
                else
                {
                    for (int i = input.Length - 1; i >= 0; i--)
                    {
                        output += input.ElementAt(i);
                    }

                    Console.WriteLine(output + "\n");
                }
            }
        }
    }
}
