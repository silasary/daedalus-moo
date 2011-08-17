using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Daedalus
{
    partial class Settings
    {
        public static List<Type> SerializedTypes = new List<Type>();

        public void Save()
        {
            if (!changed)
                return;
            XmlSerializer serializer = new XmlSerializer(this.GetType(), SerializedTypes.ToArray());
            Stream stream = File.OpenWrite("Settings.xml");
            serializer.Serialize(stream, this);
            stream.Close();

            serializer = new XmlSerializer(typeof(List<Type>));
            try
            {
                stream = File.OpenWrite("Settings.types.xml");
                serializer.Serialize(stream, SerializedTypes);
            }
            catch (Exception v)
            {
                throw v;
            }
            finally
            {
                stream.Close();
            }
            changed = false;
        }

        private static Settings _default;
        public static Settings Default
        {
            get
            {
                if (_default == null)
                {
                    XmlSerializer serializer;
                    Stream stream;
                    try
                    {
                        serializer = new XmlSerializer(SerializedTypes.GetType());
                        stream = File.OpenRead("Settings.types.xml");
                        SerializedTypes = serializer.Deserialize(stream) as List<Type>;
                    }
                    catch { }
                    if (File.Exists("Settings.xml"))
                    {
                        serializer = new XmlSerializer(typeof(Settings), SerializedTypes.ToArray());
                        stream = File.OpenRead("Settings.xml");
                        try
                        {
                            _default = serializer.Deserialize(stream) as Settings;
                        }
                        catch { }
                        stream.Close();
                    }
                    else
                    {
                        _default = new Settings();
                        _default.changed = true;
                    }
                }
                return _default;
            }
        }

        ~Settings()
        {
            Save();
        }
    }
}
