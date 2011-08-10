using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chiroptera.Base
{
    [Serializable]
    public class SavedSession
    {
        private string server;

        public string Server
        {
            get
            {
                if (server == null)
                    return "";
                return server;
            }
            set { server = value; }
        }
        private string port;

        public string Port
        {
            get
            {
                if (port == "" || port == null)
                    return "7777";
                return port; 
            }
            set { port = value; }
        }
        private string username;

        public string Username
        {
            get
            {
                if (username == null)
                    return "";
                return username;
            }
            set { username = value; }
        }

        private string password;

        public string Password
        {
            get
            {
                if (password == null)
                    return "";
                return password;
            }
            set { password = value; }
        }
        //public string PasswordEncrypted {get{

        private string name;

        public string Name
        {
            get { 
                if (name == "" || name == null)
                    return Server + ":" + Port; 
                return name;
                
            }
            set { name = value; }
        }

        private string loginString;

        public string LoginString
        {
            get
            {
                if (loginString == null)
                    return "co %u %p";
                return loginString;
            }
            set
            {
                loginString = value;
            }
        }

    }
}
