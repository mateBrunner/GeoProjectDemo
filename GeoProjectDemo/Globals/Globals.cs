using BaseClasses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoProjectDemo.Globals
{
    public static class Globals
    {

        private static List<ServiceOptions> m_ServiceOptions;

        private static GeoProjectServiceTeszt.GeoProjectServiceClient m_ProjectServiceTeszt;

        public static GeoProjectServiceTeszt.GeoProjectServiceClient ProjectServiceTeszt
        {
            get
            {
                if ( m_ProjectServiceTeszt != null )
                    return m_ProjectServiceTeszt;

                m_ProjectServiceTeszt = new GeoProjectServiceTeszt.GeoProjectServiceClient( );

                var appSettingsOptions = m_ServiceOptions.FirstOrDefault( s => s.ServiceName == "GeoProjectService" );

                if ( appSettingsOptions != null )
                {
                    m_ProjectServiceTeszt.ClientCredentials.UserName.UserName = appSettingsOptions.Username;
                    m_ProjectServiceTeszt.ClientCredentials.UserName.Password = appSettingsOptions.Password;
                } else
                {
                    m_ProjectServiceTeszt.ClientCredentials.UserName.UserName = Environment.UserName;
                }

                return m_ProjectServiceTeszt;
            }
        }


        public static void SaveServiceOptions(string appSettings)
        {
            JObject jAppSettings = JObject.Parse( appSettings );

            List<JToken> results = jAppSettings[ "CustomOptions" ][ "Services" ].Children( ).ToList( );

            List<ServiceOptions> serviceOptions = new List<ServiceOptions>( );
            foreach ( JToken result in results )
            {
                ServiceOptions searchResult = result.ToObject<ServiceOptions>( );
                serviceOptions.Add( searchResult );
            }

            m_ServiceOptions = serviceOptions;
        }

    }
}
