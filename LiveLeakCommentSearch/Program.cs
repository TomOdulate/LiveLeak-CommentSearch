using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atenta.ELL;

namespace LiveLeakCommentSearch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);
            Application.ThreadException += new ThreadExceptionEventHandler(HandleEvent); 

            Form f = new FrmCommentMonitor();
            Application.Run(f);
        }

        public static void HandleEvent(Object Object, ThreadExceptionEventArgs e)
        {
            var log = new Ell("LCS", "LiveLeakCommentSearch");
            log.LogEvent(
                e.Exception.Message + ".\n" 
                + e.Exception.Source + e.Exception.StackTrace
                , EventLogEntryType.Error);
            Application.Exit();
        }
    }
}
