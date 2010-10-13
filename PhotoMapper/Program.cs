using System;
using System.Windows.Forms;
using System.Threading;
using log4net;
using PhotoMapper.Core;

namespace PhotoMapper
{
    static class Program
    {
        /// <summary>
        /// Gets the logger for this class.
        /// </summary>
        private static readonly ILog log = Logging.GetLog(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.GetDomain().UnhandledException += Program_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PhotoMapperUI());
        }

        static void Program_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Error("UnhandledException",(Exception)e.ExceptionObject);
            MessageBox.Show(((Exception)e.ExceptionObject).Message);
        }        
    }
}
