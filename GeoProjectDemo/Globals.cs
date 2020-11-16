using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoProjectDemo
{
    public class Globals
    {

        private static GeoProjectServiceReference.GeoProjectServiceClient m_ProjectService =
            new GeoProjectServiceReference.GeoProjectServiceClient( );
        public static GeoProjectServiceReference.GeoProjectServiceClient ProjectService
        {
            get
            {
                return m_ProjectService;
            }
        }

        private static GeoProjectServiceTeszt.GeoProjectServiceClient m_ProjectServiceTeszt =
            new GeoProjectServiceTeszt.GeoProjectServiceClient( );
        public static GeoProjectServiceTeszt.GeoProjectServiceClient ProjectServiceTeszt
        {
            get
            {
                return m_ProjectServiceTeszt;
            }
        }

    }
}
