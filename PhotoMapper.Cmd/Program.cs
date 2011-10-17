using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using PhotoMapper.Core;
using PhotoMapper.Core.CommandLine;

namespace PhotoMapper.Cmd
{
    class Program
    {
        /// <summary>
        /// Gets the logger for this class.
        /// </summary>
        private static readonly ILog log = Logging.GetLog(typeof(Program));

        static void Main(string[] args)
        {
            ImageProcessor imageProcessor = new ImageProcessor();

            //Prints the status of the ImageProcessor to the console window.
            imageProcessor.ProgessUpdated += Console.WriteLine;

            CommandLineArgs commandline = CommandLineArgs.ParseCommandLine(args);

            // Print the help information.
            if (commandline.ContainsArg("?"))
                PrintHelp();

            if (commandline.ContainsArg("about"))
                PrintAbout();

            if (commandline.ContainsArg("indir") &&
                commandline.ContainsArg("outdir") &&
                commandline.ContainsArg("outname") &&
                commandline.Format != ImageProcessor.FormatFlags.None)
            {

                bool recursive = commandline.ContainsArg("r");

                List<Picture> pictures = GetPhotos(commandline.InDir, recursive);
                imageProcessor.ProcessPictures(commandline.OutDir, commandline.OutName, pictures, commandline.Format);

                Console.WriteLine("Complete! " + pictures.Count + " pictures processed");

#if DEBUG
                Console.Read();
#endif
                return;
            }

            if (commandline.ContainsArg("checkgps")
                && commandline.ContainsArg("outdir") 
                && commandline.ContainsArg("indir"))
            {
                bool recursive = commandline.ContainsArg("r");

                List<Picture> pictures = GetPhotos(commandline.InDir, recursive);
                imageProcessor.GenerateTxtFileForNullGPS(pictures, commandline.OutDir);
#if DEBUG
                Console.Read();
#endif
                return; 
            }

            if (commandline.ContainsArg("fillgps") &&
                commandline.ContainsArg("infofile"))
            {
                string infofile = commandline["infofile"];
                imageProcessor.UpdatePhotos(infofile);
                return;
            }

            //If we get here then we have invaild args.
            PrintHelp();
#if DEBUG
                Console.Read();
#endif
            return; 
        }

        public static List<Picture> GetPhotos(string inputFrom, bool R)
        {
            SearchOption option = R ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            string[] files = Directory.GetFiles(inputFrom, "*.jpg", option);

            if (files.Length == 0)
            {
                Console.WriteLine("No files to process");
                return null;
            }

            Console.WriteLine("Building list of photos to process");
            List<Picture> pics = new List<Picture>();
            foreach (var file in files)
            {
                Picture pic = new Picture(file);
                pics.Add(new Picture(file));
            }

            return pics;
        }

        private static void PrintAbout()
        {
            Console.WriteLine(About.AboutString);
        }

        public static void PrintHelp()
        {
            Console.WriteLine(@"

============Photo Mapper Command Help============
============by Nathan Woodrow============

To generate map files from photos:
    PhotoMapper.exe /indir:[path] /outdir:[path] /outname:[path] [/mif] [/tab] [/?] [/infofile:[path]] 

To check photos for missing GPS info:
    PhotoMapper.exe /checkgps /indir:[path] /outdir:[path] 

To update photos with GPS info:
    PhotoMapper.exe /fillgps /indir:[path] /infofile:[path]    
   
       /indir:      Directory that contains the files to be processed, must be in quotes.
       /outdir:     Directory where the final MIF or TAB will be generated.
       /outname:    Name of the generated MIF or TAB file.
       /r Process current folder and all sub folders. 
       /mif         Generates MIF file.
       /tab         Generates TAB file.
       /checkgps    Checks each photo for GPS information, exports a tab delimited file that can be used to update photos.
       /fillgps     Updates each photo listed in the /infofile with GPS info in /infofile
       /infofile:   A tab delimited file that contains the list of photos and the GPS info to update.
       /?           Prints this help message");
        }
    }
}
