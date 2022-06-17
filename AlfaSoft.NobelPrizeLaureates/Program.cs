using System;
using System.IO;

namespace AlfaSoft.NobelPrizeLaureates
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryStr = "";
            if (args.Length == 0)
            {
                Console.WriteLine("Directory (full path) to a file:\n");
                directoryStr = Console.ReadLine();

                if (string.IsNullOrEmpty(directoryStr))
                {
                    do
                    {
                        Console.WriteLine("Please, insert a directory (full path. Ex.: C:\\Users\\Administrator\\Desktop\\File Ex\\Example.txt) to a file:\n");
                        directoryStr = Console.ReadLine();
                    } while (string.IsNullOrEmpty(directoryStr));
                }

                Console.WriteLine("Checking if file exists or is valid...");

                if (File.Exists(directoryStr))
                {
                    Console.WriteLine("Validation complete! File exists");
                }
                else
                {
                    Console.WriteLine("Validation failed. Make sure your file is of valid type (.txt)");
                }
            }
            else
            {
                foreach (var item in args)
                {
                    directoryStr += item.ToString();
                }
                Console.WriteLine("Current directory is: " + directoryStr);
                Console.WriteLine("Is this correct? (y/n)");
                string userInput = Console.ReadLine();
                
                if(userInput.ToLower() == "y" || userInput.ToLower() == "yes")
                {

                }
            }
        }
    }
}
