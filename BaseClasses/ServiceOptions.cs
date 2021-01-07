using System;
using System.Collections.Generic;
using System.Text;

namespace BaseClasses
{

    public class ServiceOptions
    {
        public string ServiceName { get; set; }
        public string URL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthType { get; set; }
        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }
    }

}
