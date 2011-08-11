using System;
using System.Collections.Generic;
using System.Text;

namespace Daedalus.MCP
{
    public interface MCPPackage
    {
        string PackageName { get; }
        string minVer { get; }
        string maxVer { get; }

        void HandleMessage(string command, Dictionary<string, string> KeyVals);
        void Negotiated();
        void Disconnected();
    }
}
