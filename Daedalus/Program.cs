﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Daedalus
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
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            }
            new MainForm().Show();
            Application.Run();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowErrorDialog(e.ExceptionObject);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ShowErrorDialog(e.Exception);
        }

        static void ShowErrorDialog(object e)
        {
            try
            {
                ErrorDialog dlg = new ErrorDialog(e);
                DialogResult res = dlg.ShowDialog();
                if (res == DialogResult.Abort)
                    Application.Exit();
            }
            catch
            {
                MessageBox.Show("Fatal Error in error reporting");
                Application.Exit();
            }
        }
    }
}