using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daedalus.MCP.Packages
{
    public class awnsGetSet : MCPPackage
    {
        MCPHandler handler;
        public awnsGetSet(MCPHandler h)
        {
            handler = h;
        }

        #region MCPPackage Members

        public string PackageName { get { return "dns-com-awns-getset"; } }

        public string minVer { get { return "1.0"; } }

        public string maxVer { get { return "1.0"; } }

        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            // dns-com-awns-getset-ack
            //S->C: #$#dns-com-awns-getset-ack 461808 id: getset120 value: "{new value}"
            Callbacks[KeyVals["id"]].Invoke(Properties[KeyVals["id"]], KeyVals["value"]);
            Callbacks.Remove(KeyVals["id"]);
            Properties.Remove(KeyVals["id"]);
        }

        public void Negotiated(string MinVersion, string MaxVersion)
        {
            throw new NotImplementedException();
        }

        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        public bool Supported { get; set; }

        #endregion
        int nextId = 0;
        public void Set(string property, string value)
        {
            handler.SendOOB("dns-com-awns-getset-set", MCPHandler.CreateKeyvals("id", nextId++.ToString(), "property", property, "value", value));
        }
        Dictionary<string, string> Properties;
        Dictionary<string, GetSetGotValue> Callbacks;

        public void Get(string property, GetSetGotValue ValueRecieved)
        {
            if (Callbacks == null)
                Callbacks = new Dictionary<string, GetSetGotValue>();
            if (Properties == null)
                Properties = new Dictionary<string, string>();
            Callbacks.Add(nextId.ToString(), ValueRecieved);
            Properties.Add(nextId.ToString(), property);
            handler.SendOOB("dns-com-awns-getset-get", MCPHandler.CreateKeyvals("id", nextId.ToString(), "property", property));
            nextId++;
        }

        public delegate string GetSetGotValue(string property, string value);

//C->S: #$#dns-com-awns-getset-get 461808 id: getset114 property: getset_app.text
//S->C: #$#dns-com-awns-getset-ack 461808 id: getset114 value: test
//C->S: #$#dns-com-awns-getset-set 461808 id: getset118 property: getset_app.text value: "{new value}"
//C->S: #$#dns-com-awns-getset-get 461808 id: getset120 property: getset_app.text
//S->C: #$#dns-com-awns-getset-ack 461808 id: getset120 value: "{new value}"

    }
}
