using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daedalus.MCP;
namespace McpExtras
{
    class awnsVisual : MCPPackage
    {
        MCPHandler handler;
        public awnsVisual(MCPHandler h)
        {
            handler = h;
        }

        public string PackageName
        {
            get { return "dns-com-awns-visual"; }
        }

        public string minVer { get { return "1.0"; } }

        public string maxVer { get { return "1.0"; } }

        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            if (command == "dns-com-awns-visual-location") 
                this.location = KeyVals["id"];
            //else if (command == "dns-com-awns-visual-users")
                //#$#dns-com-awns-visual-users <auth> id: * name: * location: * idle: *
            //else if (command == "dns-com-awns-visual-topology")
                //#$#dns-com-awns-visual-topology <auth> id: * name: * exit: * idle: *
            else if (command == "dns-com-awns-visual-self")
                this.self = KeyVals["id"];
            else
                throw new NotImplementedException();
        }

        public void Negotiated(string MinVersion, string MaxVersion)
        {
            handler.SendOOB("dns-com-awns-visual-getlocation", new Dictionary<string,string>());
            handler.SendOOB("dns-com-awns-visual-getself", new Dictionary<string,string>());
        }

        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        public bool Supported { get; set; }

        string self = "#-1";
        string location = "#-1";

    }
}
