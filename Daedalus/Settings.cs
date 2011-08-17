using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace Daedalus
{
    public partial class Settings : INotifyPropertyChanged
    {
        private string clientName;
        public string ClientName
        {
            get
            {
                if (clientName == null)
                    return "Daedalus";
                return clientName;
            }
            set
            {
                clientName = value;
                OnPropertyChanged("ClientName");
            }
        }

        private bool basicMode;
        public bool BasicMode
        {
            get
            {
                return basicMode;
            }
            set
            {
                basicMode = value;
                OnPropertyChanged("BasicMode");
            }
        }

        private string basicModePort;
        public string BasicModePort
        {
            get
            {
                if (basicModePort == null)
                    return "1111";
                return basicModePort;
            }
            set
            {
                basicModePort = value;
                OnPropertyChanged("BasicModePort");
            }
        }

        private string basicModeServer;
        public string BasicModeServer
        {
            get
            {
                if (basicModeServer == null)
                    return "moo.thc-gaming.co.uk";
                return basicModeServer;
            }
            set
            {
                basicModeServer = value;
                OnPropertyChanged("BasicModeServer");
            }
        }

        private bool vmooIcons;
        public bool UseVMooIcons
        {
            get { return vmooIcons; }
            set { vmooIcons = value; OnPropertyChanged("UseVMooIcons"); }
        }


        #region INotifyPropertyChanged Members
        private bool changed;  // For the serializer.
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
            changed = true;
        }
        #endregion
    }
}
