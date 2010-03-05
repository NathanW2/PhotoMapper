using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using PhotoMapper.CommandLine;

namespace PhotoMapper
{
    static class Program
    {

        static StreamWriter reportwriter;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var listener = new TextWriterTraceListener(File.Create("C:\\Temp\\PhotoMapper.Log.txt"));
            Trace.AutoFlush = true;
            Trace.Listeners.Add(listener);
            Log.WriteLine("App started on: " + DateTime.Now);

            string[] args = Environment.GetCommandLineArgs();

            CommandLineArgs commandline = CommandLineArgs.ParseCommandLine(args);

            /// Print the help information.
            if (commandline.ContainsArg("?"))
            {
                PrintHelp();
            }

            bool argsvaild = commandline.IsVaild(() =>
                                            {
                                                return commandline.ContainsArg("i") &&
                                                       commandline.ContainsArg("outdir") &&
                                                       commandline.ContainsArg("outname") &&
                                                       commandline.ContainsArg("format");
                                            });

            if (argsvaild)
            {
                using (reportwriter = new StreamWriter(Application.StartupPath + "PhotoMapper.Report.txt", false))
                {
                    string[] files = Directory.GetFiles(commandline.InDir, "*.jpg");

                    if (files.Length == 0)
                    {
                        reportwriter.WriteLine("No files to process");
                        return;
                    }

                    reportwriter.WriteLine("Building list of photos to process");
                    reportwriter.WriteLine("Thinking about next holiday....");
                    List<Picture> pics = new List<Picture>();
                    foreach (var file in files)
                    {
                        Picture pic = new Picture(file);
                        if (pic.HasGPSInformation)
                            pics.Add(new Picture(file));
                        else
                            reportwriter.WriteLine(pic.FileName + " has not GPS information");
                    }

                    if (pics.Count == 0)
                    {
                        reportwriter.WriteLine("No files that where selected have GPS information");
                    }
                    else
                    {
                        ImageProcessor process = new ImageProcessor();
                        process.ProgessUpdated += new Action<string>(process_ProgessUpdated);

                        process.ProcessPictures(commandline.OutDir, commandline.OutName, pics, commandline.Format);
                    }
                    reportwriter.WriteLine("Complete! " + pics.Count + " pictures processed");
                }                
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }

        static void process_ProgessUpdated(string obj)
        {
            reportwriter.WriteLine(obj);
        }

        public static void PrintHelp()
        {
            using (StreamWriter writer = new StreamWriter(Application.StartupPath + "PhotoMapper.CommandHelp.txt", false))
            {
                writer.WriteLine(@"======Photo Mapper Command Help===========
==========by Nathan Woodrow===============

       PhotoMapper.exe /indir:[path] /outdir:[path] /outname:[path] /format:[format] [/?]
       
           /indir:      Directory that contains the files to be processed, must be in quotes.
           /outdir:     Directory where the final MIF or TAB will be generated.
           /outname:    Name of the generated MIF or TAB file.
           /format:     The output format needed, use
                                MIF -> Generates just MIF file.
                                TAB -> Generates just TAB file.
                                MIF|TAB -> Generates both files.
                        Example: /format:MIF|TAB
           /?           Prints this help message


        Results from the processing will be saved in the PhotoMapper.Report.txt file in the PhotoMappers current directory");
            }
            return; 
        }
    }

    public static class Log
    {
        public static BooleanSwitch debugswitch = new BooleanSwitch("LogMode", "Turn logging on in the app");

        public static void WriteLine(string msg)
        {
            if (debugswitch.Enabled)
                Trace.WriteLine(msg);
        }
    }
}
