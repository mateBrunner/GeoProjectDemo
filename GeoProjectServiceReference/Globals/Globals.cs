namespace GeoProjectServiceReference.Globals
{
    public static class Globals
    {

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
