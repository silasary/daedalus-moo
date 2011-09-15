using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daedalus.MOO;

namespace Daedalus.MCP.Packages
{
    class KamahlIDE : MCPPackage
    {
        MCPHandler Handler;
        public KamahlIDE(MCPHandler handler)
        {
            Handler = handler;
        }

        public Dictionary<string, IDE> IDEs = new Dictionary<string, IDE>();

        public bool Enabled { get; private set; }

        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-silasary-IDE"; }
        }

        public string minVer
        {
            get { return "1.0"; }
        }

        public string maxVer
        {
            get { return "1.0"; }
        }

        public void HandleMessage(string command, Dictionary<string, string> KV)
        {
            if (command == "dns-silasary-IDE-initIDE")
            {
                if (!MCPHandler.ContainsKeys(KV, "object"))
                    return;
                IDE ide = new IDE(KV["object"], "", "Full-IDE", "FULL-IDE");
                ide.Show();
                this.IDEs.Add("FULL-IDE", ide);
            }
            else if (command == "dns-silasary-IDE-parent")
            {
                if (!MCPHandler.ContainsKeys(KV, "object", "parent"))
                    return;
                foreach (IDE ide in this.IDEs.Values)
                {
                    ide.FindObject(KV["object"]).Parent = ide.FindObject(KV["parent"]);
                    ide.BuildTreeView();
                    requestedParents.Remove(ide.FindObject(KV["object"]));
                }
            }
            else if (command == "dns-silasary-IDE-property")
            {
                if (!MCPHandler.ContainsKeys(KV, "object", "property", "value"))
                    return;
                foreach (IDE ide in this.IDEs.Values)
                {
                    ide.FindObject(KV["object"]).StoreProp(KV["property"], MOO.Interop.ParseMOOstruct("{" + KV["value"] + "}")[0]);
                    if (KV["property"] == "Name")
                        ide.BuildTreeView();
                }
            }
            else if (command == "dns-silasary-IDE-details")
            {
                if (!MCPHandler.ContainsKeys(KV, "object"))
                    return;
                if (MCPHandler.ContainsKeys(KV, "_data-tag"))
                {
                    Handler.RegisterMultilineHandler(KV["_data-tag"], "dns-silasary-IDE-details-" + KV["object"].ToString());
                    return;
                }
                if (!MCPHandler.ContainsKeys(KV, "all_properties", "defined_verbs", "defined_properties"))
                    return;
                foreach (IDE ide in this.IDEs.Values)
                {
                    ide.FindObject(KV["object"]).LoadPropList(Interop.ParseMOOstruct(Interop.ParseMOOstruct("{" + KV["all_properties"] + "}")[0] as string) as List<Object>); // MOO is stupid.  I need to toliteral it so it doesn't Multilist it (We don't currently want that)  As such, I need to decode it twice.
                    requestedDetails.Remove(ide.FindObject(KV["object"]));
                }
            }
            else if (command.StartsWith("dns-silasary-IDE-details-"))
            {
                string obj = command.Substring(25);
                if (MCPHandler.ContainsKeys(KV, "all_properties"))
                    foreach (IDE ide in this.IDEs.Values)
                    {
                        ide.FindObject(obj).LoadPropList(KV["all_properties"] as string);
                    }
            }
        }

        public void Negotiated()
        {
            Enabled = true;
        }

        #endregion
        
        List<MOOObject> requestedParents = new List<MOOObject>();
        internal void RequestParent(Daedalus.MOO.MOOObject mOOObject)
        {
            if (requestedParents.Contains(mOOObject))
                return;
            Handler.SendOOB("dns-silasary-IDE-parent", MCPHandler.CreateKeyvals("object", mOOObject.ToString()));
            requestedParents.Add(mOOObject);
        }
        List<MOOObject> requestedDetails = new List<MOOObject>();
        internal void RequestDetails(Daedalus.MOO.MOOObject mOOObject)
        {
            if (requestedDetails.Contains(mOOObject))
                return;
            Handler.SendOOB("dns-silasary-IDE-details", MCPHandler.CreateKeyvals("object", mOOObject.ToString()));
            requestedDetails.Add(mOOObject);
        }

        internal void RequestProperty(Daedalus.MOO.MOOObject mOOObject, string propname)
        {
            Handler.SendOOB("dns-silasary-IDE-property", MCPHandler.CreateKeyvals("object", mOOObject.ToString(), "property", propname));
        }

        #region MCPPackage Members


        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        #endregion
        public bool Supported { get; set; }
    }
}
