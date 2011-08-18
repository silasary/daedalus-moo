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
        ImageList icons;


        public VMooUserlist(MCPHandler handler)
        {
            Handler = handler;
            connection = handler.CurrentConnection;
        }
        ListView UserList;
        MOO.MOOObject you = new Daedalus.MOO.MOOObject("#-1");
        string[] Fields;
        string[] Icons;
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
                if (MCPHandler.ContainsKeys(KeyVals, "icons"))
                {
                    Icons = MOO.Interop.ParseMOOstruct(KeyVals["icons"]).Select(o => o as string).ToArray();
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
                            UserList.Items.Clear();
                            foreach (List<object> row in Data)
                            {
                                Players.Add(player = new UserListPlayer(Fields, row));
                                player.props["Icon"] = Icons[(int)player.props["Icon"] - 1];
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
                            UserList.Items.AddRange(Players.Select(p => p.LVI).ToArray()); // TODO: Switch to using a virtual ListView.
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
            UserList.Items.Clear(); 
            icons = new ImageList();
            if (Settings.Default.UseVMooIcons)
                icons.Images.AddStrip(Properties.Resources.Userlist_VMoo);
            else
                icons.Images.AddStrip(Daedalus.Properties.Resources.Userlist);
            SetImageKeys(icons);
            UserList.SmallImageList = icons;
            
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
                    return new ListViewItem(props["Name"] as string) { ImageKey = (string)this.props["Icon"] };
                }
            }
        }

        #region MCPPackage Members


        public void Disconnected()
        {
            this.UserList.Items.Clear();
        }

        #endregion

        private static void SetImageKeys(ImageList icons)
        {
            // We may want to make this configurable later on.
            icons.Images.SetKeyName(0, "Blank");
            icons.Images.SetKeyName(1, "Idle");
            icons.Images.SetKeyName(2, "Away");
            icons.Images.SetKeyName(3, "Idle+Away");
            icons.Images.SetKeyName(4, "Friend");
            icons.Images.SetKeyName(5, "Newbie");
            icons.Images.SetKeyName(6, "Inhabitant");
            icons.Images.SetKeyName(7, "Inhabitant+");
            icons.Images.SetKeyName(8, "Schooled");
            icons.Images.SetKeyName(9, "Key");
            icons.Images.SetKeyName(10, "Star");
            icons.Images.SetKeyName(11, "Wizard");

        }

    }
}
