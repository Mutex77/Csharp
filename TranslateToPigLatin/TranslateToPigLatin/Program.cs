using System;
using System.Linq;
using System.Globalization;

namespace TranslateToPigLatin
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            string output;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string pigLatin;
            string str;
            char punctuation;
            char[] vowels = { 'A', 'a', 'E', 'e', 'I', 'i', 'O', 'o', 'U', 'u' };
            bool done = false;

            while(!done)
            {
                Console.WriteLine("Enter a sentence to convert to pig latin. Enter 0 to exit.");
                input = Console.ReadLine();
                output = "";

                if (input == "0")
                    done = true;
                else
                {
                    foreach (string word in input.Split(' '))
                    {
                        pigLatin = "";
                        punctuation = '\0';
                        str = "";
                        if (vowels.Contains(word.ElementAt(0)))
                        {
                            if (!char.IsLetterOrDigit(word.ElementAt(word.Length - 1)))
                            {
                                punctuation = word.ElementAt(word.Length - 1);
                                str = word.Substring(0, word.Length - 1);
                            }
                            else
                                str = word;

                            if (output.Length > 0)
                                output += " " + str + "yay" + punctuation;
                            else
                                output = str + "yay" + punctuation;
                        }
                        else if (char.IsLetter(word.ElementAt(0)))
                        {
                            if (!char.IsLetterOrDigit(word.ElementAt(word.Length - 1)))
                            {
                                punctuation = word.ElementAt(word.Length - 1);
                                str = word.Substring(0, word.Length - 1);
                            }
                            else
                                str = word;

                            for (int i = 0; i < str.Length; i++)
                            {
                                if (vowels.Contains(str.ElementAt(i)))
                                {
                                    pigLatin = str.Substring(i);
                                    pigLatin += str.Substring(0, i);
                                    pigLatin += "ay";
                                    i = str.Length;
                                }
                            }
                            if (output.Length > 0)
                                output += " " + pigLatin + punctuation;
                            else
                                output = pigLatin + punctuation;
                        }
                        else
                        {
                            if (output.Length > 0)
                                output += " " + word;
                            else
                                output = word;
                        }
                    }
                    Console.WriteLine(textInfo.ToTitleCase(output) + "\n");
                }
            }
        }
    }
}
