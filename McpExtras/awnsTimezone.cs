using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daedalus.MCP;

namespace McpExtras
{
    class awnsTimezone : MCPPackage
    {
        MCPHandler handler;
        public awnsTimezone(MCPHandler handler)
        {
            this.handler = handler;
        }
        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-com-awns-timezone"; }
        }

        public string minVer { get { return "1.0"; } }

        public string maxVer { get { return "1.0"; } }

        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            throw new NotImplementedException();
        }

        public void Negotiated(string MinVersion, string MaxVersion)
        {
            handler.SendOOB("dns-com-awns-timezone", MCPHandler.CreateKeyvals("Timezone", Daedalus.MOO.Interop.Escape(System.TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now) ? System.TimeZone.CurrentTimeZone.DaylightName : System.TimeZone.CurrentTimeZone.StandardName)));
        }

        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        public bool Supported {get;set;}

        #endregion
    }
}
