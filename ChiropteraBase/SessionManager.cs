using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Chiroptera.Base
{
    public class SessionManager
    {
        private static SessionManager defaultManager = new SessionManager();

        public static SessionManager Default
        {
            get { return SessionManager.defaultManager; }
          //set { SessionManager.defaultManager = value; }
        }

        public List<SavedSession> Sessions { get; set; }
        public SessionManager()
        {
            if (File.Exists("Sessions.dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = null;
                try
                {
                    stream = File.OpenRead("Sessions.dat");
                    Sessions = formatter.Deserialize(stream) as List<SavedSession>;
                }
                catch (Exception) { Sessions = new List<SavedSession>(); }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
            else
                Sessions = new List<SavedSession>();
        }

        ~SessionManager()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = File.OpenWrite("Sessions.dat");
            formatter.Serialize(stream, Sessions);
            stream.Close();
        }
    }
}
