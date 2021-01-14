using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoProjectDemo.Helpers
{
    public static class SettingsHelper
    {

        public static string GetServiceSettings( IConfiguration config, string serviceName, string attribute )
        {
            return config[ $"CustomOptions:Services:{serviceName}:{attribute}" ];
        }

    }
}
