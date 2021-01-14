using System;
using System.Collections.Generic;
using System.Text;

namespace BaseClasses
{
    public class Constants
    {

        public static readonly Dictionary<int, string> HONAPOK = new Dictionary<int, string>( )
        {
            { 1, "Január" },
            { 2, "Február" },
            { 3, "Március" },
            { 4, "Április" },
            { 5, "Május" },
            { 6, "Június" },
            { 7, "Július" },
            { 8, "Augusztus" },
            { 9, "Szeptember" },
            { 10, "Október" },
            { 11, "November" },
            { 12, "December" },
        };

    }

    public class ServiceNames
    {
        public const string GEOPROJECT = "GeoProjectService";
    }

    public class ServiceAttributes
    {
        public const string USERNAME = "Username";
        public const string PASSWORD = "Password";
        public const string URL = "URL";
        public const string PROXY_ADDRESS = "ProxyAddress";
        public const string PROXY_PORT = "ProxyPort";
        public const string PROXY_USER = "ProxyUsername";
        public const string PROXY_PWD = "ProxyPassword";
        public const string AUTH_TYPE = "AuthType";
    }

}
