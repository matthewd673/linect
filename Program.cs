using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace linect
{
    class Program
    {
        static void Main(string[] args) //FORMAT: linect path [ext]
        {

            long count = 0;

            if(args.Length == 0) //no args provided
                Console.WriteLine("Provide a filename or directory & extension.");
            else
            {
                string path = args[0];
                string ext = "*";
                if(args.Length > 1)
                    ext = args[1];

                string countInfo = path + " *." + ext;

                if(IsDirectory(path))
                {
                    count = GetTotalLines(path, ext);
                }
                else
                    count = GetFileLines(path);

                Output(count, countInfo);
            }
        }

        static bool IsDirectory(string path)
        {
            FileAttributes attributes = File.GetAttributes(path);

            if(attributes == FileAttributes.Directory)
                return true;
            else
                return false;
        }

        static long GetFileLines(string filename)
        {
            return File.ReadLines(filename).Count(); 
        }

        static long GetTotalLines(string directory, string extension)
        {
            string[] filenames;
            filenames = Directory.GetFiles(directory, "*." + extension, SearchOption.AllDirectories);

            long count = 0;
            foreach(string f in filenames)
            {
                count += GetFileLines(f);
            }

            return count;

        }

        static string FormatLong(long number)
        {
            char[] chars = number.ToString().ToCharArray();
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
