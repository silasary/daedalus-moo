using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daedalus
{
    public partial class Settings
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
            }
        }


    }
}
