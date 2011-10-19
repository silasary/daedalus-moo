using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daedalus.MCP;
namespace McpExtras
{
    public class awnsVisual : MCPPackage
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
        
        Topology multiline;
        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            if (command == "dns-com-awns-visual-location")
            {
                this.location = KeyVals["id"];
                if (EnableAutoMapperMenuItem.Checked || true)
                    Map.UpdatePlayerLocation(location);
            }
            //else if (command == "dns-com-awns-visual-users")
            //#$#dns-com-awns-visual-users <auth> id: * name: * location: * idle: *
            else if (command == "dns-com-awns-visual-topology")
            {
                //#$#dns-com-awns-visual-topology <auth> id: * name: * exit: *
                handler.RegisterMultilineHandler(KeyVals["_data-tag"], "dns-com-awns-visual-topology-ml");
                multiline = new Topology();
            }
            else if (command == "dns-com-awns-visual-topology-ml")
            {
                if (KeyVals.ContainsKey("id"))
                    multiline.id.Add(KeyVals["id"]);
                else if (KeyVals.ContainsKey("name"))
                    multiline.name.Add(KeyVals["name"]);
                else if (KeyVals.ContainsKey("exit"))
                    multiline.exit.Add(KeyVals["exit"]);
            }
            else if (command == "dns-com-awns-visual-topology-ml-close")
            {
                for (int i = 0; i < multiline.id.Count; i++)
                {
                    Map.AddRoom(new RoomData(multiline.id[i], multiline.name[i], multiline.exit[i]));
                }
            }
            else if (command == "dns-com-awns-visual-self")
                this.self = KeyVals["id"];
            else
                throw new NotImplementedException();
        }

        public void Negotiated(string MinVersion, string MaxVersion)
        {
            handler.SendOOB("dns-com-awns-visual-getlocation", new Dictionary<string,string>());
            handler.SendOOB("dns-com-awns-visual-getself", new Dictionary<string,string>());
            handler.CurrentConnection.AddWidgit(EnableAutoMapperMenuItem);
            EnableAutoMapperMenuItem.CheckedChanged += new EventHandler(EnableAutoMapperMenuItem_CheckedChanged);
            Map = new AutomapForm(this);
            Map.Show();
        }

        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        public bool Supported { get; set; }

        public void RequestTopology(string roomid)
        {
            handler.SendOOB("dns-com-awns-visual-gettopology", MCPHandler.CreateKeyvals("location", roomid, "distance", "1"));
        }

        void EnableAutoMapperMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableAutoMapperMenuItem.Checked)
            {
                handler.CurrentConnection.WriteSystemLine("Automapper enabled.");
            }
            else
            {
                handler.CurrentConnection.WriteSystemLine("Automapper disabled.");
            }
        }

        string self = "#-1";
        string location = "#-1";

        System.Windows.Forms.ToolStripMenuItem EnableAutoMapperMenuItem = new System.Windows.Forms.ToolStripMenuItem("Automapper") { CheckOnClick = true };
        AutomapForm Map;

        class Topology
        {
            public List<string> id = new List<string>();
            public List<string> name = new List<string>();
            public List<string> exit = new List<string>();
        }
    }
}
