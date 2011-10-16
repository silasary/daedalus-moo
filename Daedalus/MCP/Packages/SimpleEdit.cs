using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daedalus.MCP.Packages
{
    class SimpleEdit : MCPPackage
    {
        MCPHandler Handler;
        public SimpleEdit(MCPHandler mcp)
        {
            Handler = mcp;
        }

        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-org-mud-moo-simpleedit"; }
        }

        public string minVer
        {
            get { return "1.0"; }
        }

        public string maxVer
        {
            get { return "1.0"; }
        }
        public Dictionary<string, IDE> IDEs = new Dictionary<string, IDE>();
        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            if (command == "dns-org-mud-moo-simpleedit-content")
            {
                if (!MCPHandler.ContainsKeys(KeyVals, "reference", "name", "type", "_data-tag"))
                    return;
                IDEs.Add(KeyVals["_data-tag"], new IDE(KeyVals["reference"], KeyVals["name"], KeyVals["type"], KeyVals["_data-tag"]));
                Handler.RegisterMultilineHandler(KeyVals["_data-tag"], "dns-org-mud-moo-simpleedit-MLcontent");
            }
            else if (command == "dns-org-mud-moo-simpleedit-MLcontent")
            {
                if (MCPHandler.ContainsKeys(KeyVals, "content"))
                    IDEs[KeyVals["_data-tag"]].TextArea.Text += KeyVals["content"] + "\r\n";
            }
            else if (command == "dns-org-mud-moo-simpleedit-MLcontent-close")
            {
                IDEs[KeyVals["_data-tag"]].Show();
                this.IDEs.Remove(KeyVals["_data-tag"]); // We're done here.  Allow the GC to act when it wishes.
            }
        }

        public void Negotiated(string MinVersion, string MaxVersion)
        {
            
        }

        #endregion

        #region MCPPackage Members


        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        #endregion
        public bool Supported { get; set; }
    }
}
