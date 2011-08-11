using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Daedalus.MCP.Packages
{
    class VMooUserlist : MCPPackage
    {
        MCPHandler Handler;
        Connection connection;
        public VMooUserlist(MCPHandler handler)
        {
            Handler = handler;
            connection = handler.CurrentConnection;
        }
        ListView UserList;
        MOO.MOOObject you = new Daedalus.MOO.MOOObject("#-1");
        string[] Fields;
        List<UserListPlayer> Players = new List<UserListPlayer>();

        #region MCPPackage Members

        public string PackageName
        {
            get { return "dns-com-vmoo-userlist"; }
        }

        public string minVer
        {
            get { return "1.2"; }
        }

        public string maxVer
        {
            get { return "1.2"; }
        }

        public void HandleMessage(string command, Dictionary<string, string> KeyVals)
        {
            if (command == "dns-com-vmoo-userlist")
            {
                //S->C: #$#dns-com-vmoo-userlist ^W^,z_ icons*: "" fields*: "" d*: "" _data-tag: 10756867489590
                if (!MCPHandler.ContainsKeys(KeyVals,"_data-tag"))
                    return;
                Handler.RegisterMultilineHandler(KeyVals["_data-tag"], "dns-com-vmoo-userlist-multiline");
            }
            else if (command == "dns-com-vmoo-userlist-you")
            {
                if (!MCPHandler.ContainsKeys(KeyVals, "nr"))
                    return;
                you = new Daedalus.MOO.MOOObject(KeyVals["nr"]);
            }
            else if (command == "dns-com-vmoo-userlist-multiline")
            {
                if (MCPHandler.ContainsKeys(KeyVals, "fields"))
                {
                    UserList.Columns.Clear();
                    Fields = MOO.Interop.ParseMOOstruct(KeyVals["fields"]).Select(o => o as string).ToArray(); // Protocol says this will be a list of strings.
                }
                if (MCPHandler.ContainsKeys(KeyVals, "d"))
                {
                    char modifier = KeyVals["d"][0];
                    List<object> Data = MOO.Interop.ParseMOOstruct(KeyVals["d"].Substring(1));
                    UserListPlayer player;
                    switch (modifier)
                    {
                        case '=': // Set the userlist.
                            Players.Clear();
                            foreach (List<object> row in Data)
                            {
                                Players.Add(player = new UserListPlayer(Fields, row));
                                UserList.Items.Add(player.LVI);
                            }
                            break;
                        case '+': // Logged On
                            Players.Add(player = new UserListPlayer(Fields, Data));
                            UserList.Items.Add(player.LVI);
                            break;
                        case '-': // Logged Off
                            Players.Remove(player = Players.FirstOrDefault(p => p.props["Object"].Equals(Data[0])));
                            UserList.Items.Clear();
                            UserList.Items.AddRange(Players.Select(p => p.LVI).ToArray()); // TODO:  Switch to using a virtual ListView.
                            break;
                        case '<': // They went idle

                        case '>': // No longer idle

                        case '[': // Away

                        case ']': // Back

                        case '(': // Cloak

                        case ')': // Decloak

                        default:
                            break;
                    }
                }
            }
            else
            {


            }
        }

        public void Negotiated()
        {
            UserList = new ListView();
            UserList.Dock = DockStyle.Right;
            UserList.Width = 120;
            UserList.View = View.List;
            UserList.Columns.Add("Name");
            connection.AddWidgit(UserList);
        }

        #endregion
        
        class UserListPlayer
        {
            public Dictionary<string, object> props = new Dictionary<string, object>();
            public UserListPlayer(string[] fields, List<object> values)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    this.props.Add(fields[i], values[i]);
                }
            }

            public ListViewItem LVI
            {
                get
                {
                    return new ListViewItem(props["Name"] as string);
                }
            }
        }

        #region MCPPackage Members


        public void Disconnected()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
