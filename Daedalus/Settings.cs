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
    }
}
