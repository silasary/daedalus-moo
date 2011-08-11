using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chiroptera.Base;
using System.Windows.Forms;

namespace Daedalus
{
    /// <summary>
    /// This class provides the basic /commands.
    /// </summary>
    class DefaultCommands
    {
        IConnection connection;
        CommandManager commandManager;

        public DefaultCommands(CommandManager commandManager, Connection connection)
        {
            this.commandManager = commandManager;
            this.connection = connection;
            commandManager.AddCommand("connect", ConnectCommandHandler, "Connect to a MOO", 
                    "usage: /connect [host [port]]\n\nConnects to a MOO or MUD.");
            commandManager.AddCommand("quit", QuitCommandHandler, "Quits " + Settings.Default.ClientName,
                    "usage: /quit\n\nQuits " + Settings.Default.ClientName);

        }

        int ConnectCommandHandler(string input)
        {
            SavedSession sess = connection.Session;
            string[] args = input == "" ? new String[] { } : input.Split(' ');
            string server = sess.Server;
            string port = sess.Port;
            if (args.Length > 0)
                server = args[0];
            if (args.Length > 1)
                port = args[1];
            if (args.Length > 2)
            {
                ChiConsole.WriteError("usage: /connect [host [port]]");
                return -1;
            }
            if (connection.IsConnected)
            {
                MainForm.FindMainForm().NewWorldWindow(new SavedSession() { Server = server, Port = port });
            }
            else
            {
                connection.Connect(server, int.Parse(port));
            }
            return 0;
        }
        int QuitCommandHandler(string input)
        {
            Application.Exit();
            return 0;
        }
    }
}
