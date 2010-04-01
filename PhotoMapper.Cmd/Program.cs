using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PhotoMapper.CommandLine;
using System.Threading;

namespace PhotoMapper.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgs commandline = CommandLineArgs.ParseCommandLine(args);

            /// Print the help information.
            if (commandline.ContainsArg("?"))
            {
                PrintHelp();
            }

            bool argsvaild = commandline.IsVaild(() =>
            {
                return commandline.ContainsArg("indir") &&
                       commandline.ContainsArg("outdir") &&
                       commandline.ContainsArg("outname") &&
                       commandline.Format != ImageProcessor.FormatFlags.None;
            });

            if (!argsvaild)
            {
                PrintHelp();
                return;
            }

            string[] files = Directory.GetFiles(commandline.InDir, "*.jpg");

            if (files.Length == 0)
            {
                Console.WriteLine("No files to process");
                return;
            }

            Console.WriteLine("Building list of photos to process");
            Console.WriteLine("Thinking about next holiday....");
            List<Picture> pics = new List<Picture>();
            foreach (var file in files)
            {
                Picture pic = new Picture(file);
                if (pic.HasGPSInformation)
                    pics.Add(new Picture(file));
                else
                    Console.WriteLine(pic.FileName + " has not GPS information");
            }

            if (pics.Count == 0)
            {
                Console.WriteLine("No files that where selected have GPS information");
            }
            else
            {
                ImageProcessor process = new ImageProcessor();
                process.ProgessUpdated += new Action<string>(process_ProgessUpdated);

                process.ProcessPictures(commandline.OutDir, commandline.OutName, pics, commandline.Format);
            }
            Console.WriteLine("Complete! " + pics.Count + " pictures processed");

        }

        static void process_ProgessUpdated(string obj)
        {
            Console.WriteLine(obj);
        }

        public static void PrintHelp()
        {
            Console.WriteLine(@"

============Photo Mapper Command Help============
============by Nathan Woodrow============

   PhotoMapper.exe /indir:[path] /outdir:[path] /outname:[path] [/mif] [/tab] [/?]
   
       /indir:      Directory that contains the files to be processed, must be in quotes.
       /outdir:     Directory where the final MIF or TAB will be generated.
       /outname:    Name of the generated MIF or TAB file.
       /mif         Generates MIF file.
       /tab         Generates TAB file.
       /?           Prints this help message");
        }
    }
}
