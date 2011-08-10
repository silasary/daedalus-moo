using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Daedalus.MOO
{
    public class MOOObject
    {
        public int id;
        public MOOObject(int id)
        {
            this.id = id;
        }
        public MOOObject(string id)
        {
            if (!id.StartsWith("#"))
                throw new ArgumentException("Moo objects start with a #");
            this.id = int.Parse(id.Trim().Substring(1));
        }

        private Dictionary<string, object> properties;

        public object this[string PropName]
        {
            get
            {
                if (properties == null)
                    properties = new Dictionary<string, object>();
                //if (!properties.ContainsKey(PropName))
                //{
                //    if (MCP.Packages.KamahlIDE._idepackage.Enabled)
                //        MCP.Packages.KamahlIDE._idepackage.RequestDetails(this);
                //    return new MissingFieldException(); // Not that big a deal, it's just not cached.
                //}
                return properties[PropName];
            }
        }

        /// <summary>
        /// Stores the property as in the object cache.  Does not update the actual object.
        /// </summary>
        /// <param name="PropName"></param>
        /// <param name="value"></param>
        public void StoreProp(string PropName, object value)
        {
            PropName = PropName.ToLower().Trim('"');
            if (properties == null)
                properties = new Dictionary<string, object>();
            if (!properties.ContainsKey(PropName))
                properties.Add(PropName, value);
            else
                properties[PropName] = value;
        }

        public override string ToString()
        {
            return "#" + this.id;
        }

        public override bool Equals(object obj)
        {
            if (obj is MOOObject)
                return this.id == (obj as MOOObject).id;
            return base.Equals(obj);
        }

        public void LoadPropList(List<object> moolist)
        {
            foreach (object propname in moolist)
            {
                if (propname is string)
                    LoadPropList(propname as string);
            }
        }
        internal void LoadPropList(string propname)
        {
            propname = propname.ToLower().Trim('"');
            //if (!this.properties.ContainsKey(propname))
            //{
            //    this.properties.Add(propname.ToLower(), null);
            //    if (MCP.Packages.KamahlIDE._idepackage.Enabled)
            //        MCP.Packages.KamahlIDE._idepackage.RequestProperty(this, propname);
            //}
        }

        public string[] Properties
        {
            get
            {
                return this.properties.Keys.ToArray();
            }
        }

        internal void SetProp(string p, object value)
        {
            throw new NotImplementedException();
        }
        private MOOObject _parent;
        public MOOObject Parent
        {
            get
            {
                if (this.id < 0) // Invalid
                    return null;
                //if (_parent == null && MCP.Packages.KamahlIDE._idepackage.Enabled)
                //    MCP.Packages.KamahlIDE._idepackage.RequestParent(this);
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
    }
}
