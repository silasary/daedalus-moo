using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daedalus.PluginModel;

namespace McpExtras
{
    /// <summary>
    /// For some of the more obscure MCP Packages, which don't need to be in the main binary.
    /// </summary>
    public class McpExtrasPlugin : IPlugin
    {

        public Type[] MCPPackages
        {
            get { return new Type[] { typeof(awnsVisual), typeof(awnsTimezone), typeof(AwnsPing) }; }
        }

        public System.Windows.Forms.TabPage[] Options
        {
            get { return new System.Windows.Forms.TabPage[] { }; }
        }

        public Type[] SettingsTypes
        {
            get { return new Type[] { }; }
        }

        public void NewConnection(Daedalus.IConnection connection)
        {
            
        }

        public void NewForm(Daedalus.MainForm form)
        {
            
        }
    }
}
