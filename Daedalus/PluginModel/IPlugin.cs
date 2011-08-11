using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daedalus.MCP;

namespace Daedalus.PluginModel
{
    public interface IPlugin
    {
        Type[] MCPPackages { get; }
        void NewConnection(IConnection connection);
    }
}
