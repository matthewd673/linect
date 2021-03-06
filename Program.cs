﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace linect
{
    class Program
    {

        static bool errorThrown = false;

        static void Main(string[] args) //FORMAT: linect path [ext]
        {

            long count = 0;

            if(args.Length == 0) //no args provided
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Provide a filename or directory & extension.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                string path = args[0];
                string pattern = "";
                if(args.Length > 1)
                    pattern = args[1];

                string countInfo = path + " " + pattern;

                if(!path.Contains("*") && IsDirectory(path))
                {
                    count = GetTotalLines(path, pattern);
                }
                else
                {
                    if(!path.Contains("*") && File.Exists(path))
                        count = GetFileLines(path); //search file
                    else
                        count = GetTotalLines(Directory.GetCurrentDirectory(), path); //search current directory based on pattern (path is assumed to be a pattern)
                }

                if(!errorThrown)
                    Output(count, countInfo);
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("An error occurred when counting the file lines.");
                    Console.WriteLine("Ensure that the file or directory specified exists.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        static bool IsDirectory(string path)
        {
            try
            {
            FileAttributes attributes = File.GetAttributes(path);

            if(attributes == FileAttributes.Directory)
                return true;
            else
                return false;
            }
            catch (Exception e)
            {
                errorThrown = true;
            }
            return false;
        }

        static long GetFileLines(string filename)
        {
            try
            {
                int count = File.ReadLines(filename).Count();
                return count;
            }
            catch (Exception e)
            {
                errorThrown = true;
            }
            return 0;
        }

        static long GetTotalLines(string directory, string pattern)
        {
            string[] filenames = GetFileNames(directory, pattern);

            long count = 0;
            foreach(string f in filenames)
            {
                count += GetFileLines(f);
            }

            return count;

        }

        static String[] GetFileNames(string directory, string pattern)
        {
            string[] filenames;
            filenames = Directory.GetFiles(directory, pattern, SearchOption.AllDirectories);
            return filenames;
        }

        static string FormatLong(long number)
        {
            String numString = number.ToString();

            //don't bother formatting if its 3 digits or less
            if(numString.Length <= 3)
                return numString;
            
            char[] chars = numString.ToCharArray();
            string formatted = "";

            for(int i = 0; i < chars.Length; i++)
            {
                formatted = formatted.Insert(formatted.Length, chars[i].ToString());
                
                if(i % 3 == 0 && i < chars.Length - 1)
                    formatted = formatted.Insert(formatted.Length, ",");
            }

            return formatted;
        }

        static void Output(long count = 0, string message = "")
        {
            string formatted = FormatLong(count);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("linect: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(formatted);
            Console.ForegroundColor = ConsoleColor.Gray;

            if(message != "")
                Console.Write(" - {0}", message);
            
            Console.WriteLine("");
        }

    }
}
