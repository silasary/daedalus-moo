using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Chiroptera.Base
{
    [Serializable]
    public class SavedSession : INotifyPropertyChanged
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
            set
            {
                server = value;
                OnPropertyChanged("Server");
            }
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
            set
            {
                port = value;
                OnPropertyChanged("Port");
            }
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
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
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
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        //public string PasswordEncrypted {get{

        private string name;

        public string Name
        {
            get
            {
                if (name == "" || name == null)
                    return Server + ":" + Port;
                return name;

            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
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
                OnPropertyChanged("LoginString");
            }
        }

        private bool autoConnect;
        /// <summary>
        /// Whether to automatically launch this session when loading Daedalus.
        /// </summary>
        public bool AutoConnect
        {
            get { return autoConnect; }
            set
            {
                autoConnect = value;
                OnPropertyChanged("AutoConnect");
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
            if ((propname == "Server" || propname == "Port") && (name == "" || name == null))
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
        }


        #endregion
    }
}
