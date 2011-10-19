using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using PhotoMapper.Core;
using PhotoMapper.Core.CommandLine;
using NDesk.Options;
using System.Text;

namespace PhotoMapper.Cmd
{
    class Program
    {
        static int verbosity;
        /// <summary>
        /// Gets the logger for this class.
        /// </summary>
        private static readonly ILog log = Logging.GetLog(typeof(Program));

        static void Main(string[] args)
        {

            bool help = false;
            bool about = false;
            string name = "";
            bool recursive = false;
            ImageProcessor.FileFormat format = ImageProcessor.FileFormat.None;
            var p = new OptionSet()
            {
                { "f|format=",  "output format.  Can be 'mif','tab' or 'mif&tab' ",  v => 
                    {
                        switch(v)
                        {
                            case "mif":
                                format = ImageProcessor.FileFormat.MIF;
                                break;
                            case "tab":
                                format = ImageProcessor.FileFormat.TAB;
                                break;
                            case "mif&tab":
                                format = ImageProcessor.FileFormat.MIF | ImageProcessor.FileFormat.TAB;
                                break;
                            default:
                                format = ImageProcessor.FileFormat.None;
                                break;
                        }
                    }},
                { "n|name=",  "name of the output file",  v => name = v},
                { "r|recursive",  "process all photo in the current directory and all sub folders.",  v => recursive = v != null},
                { "v", "increase debug message verbosity", v => { if (v != null) ++verbosity; } },
                { "A|about",  "ouptut info about this program and exit.",  v => about = v != null},
                { "h|?|help",  "show this message and exit.",  v => help = v != null},
            };

            List<String> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("PhotoMapper.Cmd: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `PhotoMapper.Cmd --help' for more information.");
#if DEBUG
                Console.Read();
#endif
                return;
            }

            if (help)
                PrintHelp(p);
            if (about)
                PrintAbout();
            if (extra.Count < 2)
            {
                Debug("Missing input and output folder");
                PrintHelp(p);
            }
            else if (extra.Count == 2 && !String.IsNullOrEmpty(name) 
                && format != ImageProcessor.FileFormat.None)
            {
                var infolder = extra[0];
                var outfolder = extra[1];
                List<Picture> pictures = GetPhotos(infolder, recursive);
                if (pictures == null)
                {
                    Debug("No images found to process");
#if DEBUG
                    Console.Read();
#endif
                    return;
                }

                ImageProcessor proc = new ImageProcessor();
                proc.ProgessUpdated += Debug;
                proc.ProcessPictures(outfolder, name, pictures, format);

            }
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
                Debug("No files to process");
                return null;
            }

            Debug("Found {0} files ", files.Length);
            Debug("Building list of photos to process");
            List<Picture> pics = new List<Picture>();
            int count = 1;
            foreach (var file in files)
            {
                Picture pic = new Picture(file);
                pics.Add(new Picture(file));
                if (verbosity > 0)
                {
                    drawTextProgressBar(count, files.Length);
                }
                ++count;
            }

            return pics;
        }

        private static void PrintAbout()
        {
            Console.WriteLine(About.AboutString);
        }

        public static void PrintHelp(OptionSet p)
        {
            Console.WriteLine(@"
Usage: PhotoMapper.Cmd [OPTIONS]+ infolder outfolder 
Generates a MapInfo mif and/or tab from a photos with GPS coordinates.");
            p.WriteOptionDescriptions(Console.Out);
        }

        static void Debug(string format)
        {
            Debug(format,"");
        }

        static void Debug(string format, params object[] args)
        {
            if (verbosity > 0)
            {
                Console.Write("# ");
                Console.WriteLine(format, args);
            }
        }

        private static void drawTextProgressBar(int progress, int total)
        {
            StringBuilder builder = new StringBuilder("[");
            float onechunk = 30.0f / total;

            //draw filled part  
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.CursorLeft = position++;
                builder.Append("=");
            }

            //draw unfilled part  
            for (int i = position; i < 31; i++)
            {
                builder.Append(" ");
            }
            builder.Append("]");
            Console.Write("\r" + builder.ToString() + progress.ToString() + " of " + total.ToString() + " ");

            if (progress == total)
                Console.WriteLine();
        }
    }
}
