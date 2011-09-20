using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
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

        private bool autoReconnect;
        public bool AutoReconnect
        {
            get
            {
                return autoReconnect;
            }
            set
            {
                autoReconnect = value; 
                OnPropertyChanged("AutoReconnect");
            }
        }

        private Font consoleFont;
        [XmlIgnore]
        public Font ConsoleFont
        {
            get
            {
                if (consoleFont == null)
                    return new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                return consoleFont;
            }
            set { consoleFont = value; }
        }
        public SerializableFont ConsoleFontSetting // Xml Serialization can't do fonts the normal way.
        {
            get
            {
                return ConsoleFont;
            }
            set
            {
                consoleFont = value;
            }
        }

        private Font inputFont;
        [XmlIgnore]
        public Font InputFont
        {
            get
            {
                if (inputFont == null)
                    return new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                return inputFont;
            }
            set { inputFont = value; }
        }
        public SerializableFont InputFontSetting // Xml Serialization can't do fonts the normal way.
        {
            get
            {
                return InputFont;
            }
            set
            {
                inputFont = value;
            }
        }

        #region INotifyPropertyChanged Members
        private bool changed;  // For the serializer.
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String propname)
        {
            if (typeof(Settings).GetProperty(propname) == null)
                throw new MissingMemberException("Settings", propname);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
            changed = true;
        }
        #endregion
    }
}
