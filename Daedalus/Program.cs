using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Chiroptera.Base;
using System.IO;
using System.Reflection;

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
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            }

            MainForm form = new MainForm();
            if (Settings.Default.BasicMode != true)
                form.Show();
            foreach (SavedSession session in SessionManager.Default.Sessions)
            {
                if (session.AutoConnect)
                    form.NewWorldWindow(session);
            }
            Application.Run();
        }

        // This nifty code searches for Assemblies, should they fail to load via the normal procedures.
        // This happens if a Plugin within a subfolder is attempting to load an assembly within that subfolder.
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dll = args.Name.Split(',')[0] + ".*"; // TODO: Check that the file found is an EXE or DLL.
            dll = Directory.GetFiles(".", dll, SearchOption.AllDirectories).FirstOrDefault();
            dll = Path.GetFullPath(dll);
            return Assembly.LoadFile(dll);
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
                System.IO.File.WriteAllText("Error.txt", e.ToString());
                System.Diagnostics.Process.Start("Error.txt");
                Application.Exit();
            }
        }
    }
}
