using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace McpExtras
{
    public partial class AutomapForm : Form
    {
        awnsVisual Visual;
        public AutomapForm(awnsVisual plugin)
        {
            InitializeComponent();
            Visual = plugin;
        }

        internal void UpdatePlayerLocation(string location)
        {
            RoomData data = Rooms.FirstOrDefault(r => r.id == location);
            if (data == null)
                Visual.RequestTopology(location);
            else
            {
                this.BuildMap();
            }
        }

        private void BuildMap()
        {
            Dictionary<RoomData, Point> Reverse = new Dictionary<RoomData, Point>();
            List<RoomData> Unsorted = new List<RoomData>();
            Reverse.Add(here, new Point(0, 0));
            Unsorted.Add(here);
            RoomData temp;
            while ((temp = Unsorted.FirstOrDefault()) != null)
            {
                if (Reverse.ContainsKey(temp))
                {
                    Point pos = Reverse[temp];
                    foreach (string dir in temp.Connections.Keys)
                    {
                        RoomData dest = temp.Connections[dir];
                        switch (dir)
                        {
                            case "n":
                                Reverse.Add(dest, new Point(pos.X + 1, pos.Y));
                                break;
                            case "s":
                                Reverse.Add(dest, new Point(pos.X - 1, pos.Y));
                                break;
                            case "e":
                                Reverse.Add(dest, new Point(pos.X, pos.Y + 1));
                                break;
                            case "w":
                                Reverse.Add(dest, new Point(pos.X, pos.Y - 1));
                                break;
                        }
                        Unsorted.Add(dest);
                    }
                }
            }            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

        }

        Dictionary<Point, RoomData> Map = new Dictionary<Point, RoomData>();

        List<RoomData> Rooms = new List<RoomData>();
        RoomData here = null;

        public void AddRoom(RoomData room)
        {
            RoomData old;
            if ((old = Rooms.FirstOrDefault(r => r.id == room.id)) != null)
            {
                old.Name = room.Name;
                old.exits = room.exits;
                foreach (KeyValuePair<string, string> id in old.exits)
                {
                    old.Connections.Add(id.Key, this.Rooms.FirstOrDefault(r => r.id == id.Value));
                }
            }
            else
            {
                Rooms.Add(room);
                foreach (KeyValuePair<string, string> id in room.exits)
                {
                    room.Connections.Add(id.Key, this.Rooms.FirstOrDefault(r => r.id == id.Value));
                }
            }

            foreach (RoomData r in Rooms)
            {
                foreach (KeyValuePair<string,string> x in r.exits)
                {
                    if (x.Value == room.id)
                    {

                    }
                }
            }
        }
        public void AddRooms(params RoomData[] rooms)
        {
            foreach (RoomData room in rooms)
                AddRoom(room);

        }
    }
}
