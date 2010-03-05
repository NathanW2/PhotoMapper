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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PhotoMapper());
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
