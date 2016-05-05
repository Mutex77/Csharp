using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPalindrome
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            string str;
            bool done = false;
            bool isPalindrome;

            while(!done)
            {
                Console.WriteLine("Enter a string to see if it is a palindrome. Enter 0 to exit.");
                input = Console.ReadLine();
                isPalindrome = true;
                str = "";
                if (input == "0")
                    done = true;
                else if (input.Length < 2)
                    Console.WriteLine("Please enter a string at least 2 characters long.\n");
                else
                {
                    foreach(char c in input.ToCharArray())
                    {
                        if (char.IsLetterOrDigit(c))
                            str += c;
                    }

                    for(int i = 0; i < str.Length / 2; i++)
                    {
                        if(char.ToLower(str.ElementAt(i)) != char.ToLower(str.ElementAt(str.Length - i - 1)))
                        {
                            isPalindrome = false;
                            i = str.Length;
                        }
                    }
                    Console.WriteLine(isPalindrome + "\n");
                }
            }
        }
    }
}
