using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daedalus.MCP;
using System.Windows.Forms;

namespace Daedalus.PluginModel
{
    public interface IPlugin
    {
        /// <summary>
        /// The MCP packages implemented by the plugin.
        /// </summary>
        Type[] MCPPackages { get; }
        /// <summary>
        /// The tabs the plugin puts on the options dialogue.
        /// </summary>
        TabPage[] Options { get; }
        /// <summary>
        /// A list of types the plugin might want to store in the settings file.
        /// </summary>
        Type[] SettingsTypes { get; }
        /// <summary>
        /// This method is called when a new Connection is created.  
        /// Add your dispacher hooks here.
        /// </summary>
        /// <param name="connection">the new connection.</param>
        void NewConnection(IConnection connection);
        /// <summary>
        /// A new main form is created.  
        /// Add your window decorations here.
        /// </summary>
        /// <param name="form"></param>
        void NewForm(MainForm form);

    }
}
