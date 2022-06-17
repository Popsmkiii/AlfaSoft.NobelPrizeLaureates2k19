using System;
using System.IO;
using System.Timers;
using System.Threading;

namespace AlfaSoft.NobelPrizeLaureates
{
    class Program
    {
        static string directoryStr = "";
        //if true can make requests
        static bool appWasRan60sAgo = true;
        static void Main(string[] args)
        {
            AppLastOpened();

            if (args.Length != 0)
            {
                foreach (var item in args)
                {
                    directoryStr += item.ToString() + " ";
                }

                Console.WriteLine("Current directory is: " + directoryStr);
                Console.WriteLine("Is this correct? (y/n)");
                string userInput = Console.ReadLine();

                if (userInput.ToLower() == "y" || userInput.ToLower() == "yes")
                {
                    ArchiveProcessor();
                }
                else if(userInput.ToLower() == "n" || userInput.ToLower() == "no")
                {
                    DirectoryChooser();
                }
            }
            else
            {
                DirectoryChooser();
                ArchiveProcessor();
            }

            ClosingApp();
        }

        public static void ArchiveProcessor()
        {
            //archive process here...
            Console.WriteLine("Initializing archive processing ...");
            string[] lines = File.ReadAllLines(directoryStr);

            string name = "";
            int year = 0;

            foreach (var line in lines)
            {
                int index = line.IndexOf(";");

                year = Convert.ToInt32(line.Substring(line.LastIndexOf(";") + 1));

                if(index > 0)
                {
                    name = line.Substring(0, index);
                }

                if(!string.IsNullOrEmpty(name) && year != 0)
                {
                    Console.WriteLine("Saving the category '" + name + "' and year '" + year + "'. Please wait ...");
                    Prizes prizes = new Prizes(name, year);
                    name = "";
                    year = 0;

                    //save to db ...

                    Console.WriteLine("Waiting 15 seconds before the next request ...");
                    Thread.Sleep(15000);
                }
            }

        }

        public static void DirectoryChooser()
        {
            Console.WriteLine("Directory (full path) to a file:");
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
                ArchiveProcessor();
            }
            else
            {
                Console.WriteLine("Validation failed. Make sure your file is of valid type (.txt)");
                ClosingApp();
            }
        }

        public static void AppLastOpened()
        {
            DateTime appOpen = DateTime.Now;
            DateTime lastClose = DateTime.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\AppLastClose.txt"));
            TimeSpan span = appOpen.Subtract(lastClose);

            if(span.TotalSeconds > 60)
            {
                Console.WriteLine("App was last closed on: " + lastClose + " which was " + span.TotalMinutes + " minutes ago.");

            }
            else
            {
                appWasRan60sAgo = false;
                Console.WriteLine("App was last closed on: " + lastClose + " which was " + span.TotalSeconds + " seconds ago.");
                Console.WriteLine("Please wait " + (span.TotalSeconds - 60) + " seconds in order to be able to make requests");
                int secondsSleep = 60 - span.Seconds;
                secondsSleep = Convert.ToInt32(secondsSleep.ToString() + "000");
                Thread.Sleep(secondsSleep);
                Console.WriteLine("Waiting time complete! Please continue");
            }
        }

        public static void ClosingApp()
        {
            Console.WriteLine("Closing console in 5 seconds ...");
            Thread.Sleep(5000);
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\AppLastClose.txt", DateTime.Now.ToString());
            Environment.Exit(0);
        }
    }
}
