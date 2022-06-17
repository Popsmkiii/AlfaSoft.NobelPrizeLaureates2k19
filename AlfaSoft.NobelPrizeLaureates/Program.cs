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
            //Check when was the last time the app was opened
            AppLastOpened();

            //If there are args it will check and use the requested directory 
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
                    //If the file doesn't exist it will ask again for the correct path
                    if (!File.Exists(directoryStr))
                    {
                        Console.WriteLine("Directory not found or not accessible, please review this matter before proceeding..");
                        DirectoryChooser();
                    }
                    //Finally, If the file exists and is the correct path
                    ArchiveProcessor();
                }
                else if(userInput.ToLower() == "n" || userInput.ToLower() == "no")
                {
                    ClosingApp(false);
                }
            }
            else
            {
                DirectoryChooser();
                ArchiveProcessor();
            }

            ClosingApp(true);
        }

        public static void ArchiveProcessor()
        {
            //archive process here...
            Console.WriteLine("Initializing archive processing ...");
            //Get all txt lines to array
            string[] lines = File.ReadAllLines(directoryStr);

            string name = "";
            int year = 0;

            //Foreach line there will be a result with the needed name and year that is in the txt
            foreach (var line in lines)
            {
                int index = line.IndexOf(";");

                year = Convert.ToInt32(line.Substring(line.LastIndexOf(";") + 1));

                if(index > 0)
                {
                    name = line.Substring(0, index);
                }

                //for each it will be saved and resat the variables in order to save the next ones
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

            //If the file directory is incorrect or not accessible let the user know and try again:
            do
            {
                if (!File.Exists(directoryStr))
                {
                    Console.WriteLine("File validation failed. Make sure the file is of valid type (.txt)");
                }

                Console.WriteLine("Please, insert a directory (full path. Ex.: C:\\Users\\Administrator\\Desktop\\File Ex\\Example.txt) to a file:");
                directoryStr = Console.ReadLine();

            } while (string.IsNullOrEmpty(directoryStr) && !File.Exists(directoryStr));

        }

        public static void AppLastOpened()
        {
            //To save last time it was opened
            DateTime appOpen = DateTime.Now;
            //If the txt doesn't exist it will be created
            bool txtExists = File.Exists(Directory.GetCurrentDirectory() + "\\AppLastClose.txt");
            if (!txtExists)
            {
                File.CreateText(Directory.GetCurrentDirectory() + "\\AppLastClose.txt");
                //Since the file wasn't created it will skip the cooldown check
                return;
            }

            DateTime lastClose = DateTime.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\AppLastClose.txt"));
            TimeSpan span = appOpen.Subtract(lastClose);

            //Calculation in case the last time opened the app was under 60 seconds therefore a cooldown was needed
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

        public static void ClosingApp(bool success)
        {
            if (success)
            {
                //5 seconds timer before the app closing + saving the last time the app was opened on txt
                Console.WriteLine("Closing console in 5 seconds ...");
                Thread.Sleep(5000);
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\AppLastClose.txt", DateTime.Now.ToString());
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Closing app with no changes made");
                Environment.Exit(0);
            }
        }
    }
}
