using BaseClasses;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoProjectDemo.Services
{
    public class GeoProjectService : IGeoProjectService
    {

        private GeoProjectServiceTeszt.GeoProjectServiceClient m_client;

        public GeoProjectService( IConfiguration config )
        {
            var options = GetServiceOptions( config );
            m_client = new GeoProjectServiceTeszt.GeoProjectServiceClient( options );

        }

        private ServiceOptions GetServiceOptions( IConfiguration config )
        {
            ServiceOptions options = new ServiceOptions( )
            {
                AuthType        = config[ "CustomOptions:Services:GeoProjectService:AuthType" ],
                URL             = config[ "CustomOptions:Services:GeoProjectService:URL" ],
                Username        = config[ "CustomOptions:Services:GeoProjectService:Username" ],
                Password        = config[ "CustomOptions:Services:GeoProjectService:Password" ],
                ProxyAddress    = config[ "CustomOptions:Services:GeoProjectService:ProxyAddress" ],
                ProxyPort       = Convert.ToInt32( config[ "CustomOptions:Services:GeoProjectService:ProxyPort" ] ),
                ProxyUsername   = config[ "CustomOptions:Services:GeoProjectService:ProxyUsername" ],
                ProxyPassword   = config[ "CustomOptions:Services:GeoProjectService:ProxyPassword" ]
            };

            return options;
        }

    }
}
