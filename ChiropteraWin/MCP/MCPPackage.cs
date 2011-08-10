using System;
using System.Collections.Generic;
using System.Text;

namespace Chiroptera.Win.MCP
{
    interface MCPPackage
    {
        string PackageName { get; }
        string minVer { get; }
        string maxVer { get; }

        void HandleMessage(string command, Dictionary<string, string> KeyVals);
        void Negotiated();
    }
}
