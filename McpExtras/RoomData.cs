using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McpExtras
{
    public class RoomData
    {
        public string Name { get; set; }
        public string id { get; set; }
        public Dictionary<String, RoomData> Connections { get; set; }
        public Dictionary<String, string> exits { get; set; }

        public RoomData(string id, string name, string exitstr)
        {
            this.id = id;
            this.Name = name;
            string[] exits = exitstr.Split(' ');
            for (int i = 0; i < exits.Length; i++)
            {
                this.exits.Add(exits[i], exits[++i]);
            }
        }
    }
}
