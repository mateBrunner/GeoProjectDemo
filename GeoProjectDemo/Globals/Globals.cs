using BaseClasses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

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


                var options = m_ServiceOptions.FirstOrDefault( s => s.ServiceName == "GeoProjectService" );

                m_ProjectServiceTeszt = new GeoProjectServiceTeszt.GeoProjectServiceClient( null );



                //if ( options != null )
                //{
                //    m_ProjectServiceTeszt.ClientCredentials.UserName.UserName = options.Username;
                //    m_ProjectServiceTeszt.ClientCredentials.UserName.Password = options.Password;
                //} else
                ////az első hívásnál eldöntjük, hogy azért nincs kitöltve, mert nincs authentikáció
                //{
                //    m_ProjectServiceTeszt.ClientCredentials.UserName.UserName = Environment.UserName;
                //}


#region Version1
                ////var b_custom = m_ProjectServiceTeszt.Endpoint.Binding as System.ServiceModel.Channels.CustomBinding;

                ////if ( b_custom != null )
                ////{
                ////    var htbe = b_custom.Elements.Find<HttpTransportBindingElement>( );
                ////    htbe.ProxyAddress = new Uri( string.Format( "http://{0}:{1}", "localhost", "8080" ) );
                ////    htbe.ProxyAuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous; // Or whatever authentication mechanism your proxy server uses
                ////    htbe.UseDefaultWebProxy = false;

                ////}

                //var binding = new BasicHttpBinding( );

                ////binding.Name = "DEEWR_Customer_OUTBinding";
                ////binding.AllowCookies = false;
                ////binding.SendTimeout = new TimeSpan( 0, 10, 0 );
                ////binding.ReceiveTimeout = new TimeSpan( 0, 10, 0 );
                ////binding.OpenTimeout = new TimeSpan( 0, 10, 0 );
                ////binding.CloseTimeout = new TimeSpan( 0, 10, 0 );
                ////binding.MaxBufferPoolSize = 2147483647;
                ////binding.MaxReceivedMessageSize = 2147483647;
                ////binding.TextEncoding = Encoding.UTF8;
                ////binding.TransferMode = TransferMode.Buffered;
                ////binding.BypassProxyOnLocal = false;
                ////binding.UseDefaultWebProxy = false;

                ////binding.ReaderQuotas.MaxDepth = 32;
                ////binding.ReaderQuotas.MaxStringContentLength = 5242880;
                ////binding.ReaderQuotas.MaxArrayLength = 16384;
                ////binding.ReaderQuotas.MaxBytesPerRead = 4096;
                ////binding.ReaderQuotas.MaxNameTableCharCount = 16384;

                ////binding.Security.Mode = BasicHttpSecurityMode.Transport;
                ////binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

                //var customBinding = new CustomBinding( binding );
                //var htbe = customBinding.Elements.Find<HttpTransportBindingElement>( );

                ////var webProxy = new WebProxy( $"{options.ProxyAddress}/{options.ProxyPort}", true );
                ////webProxy.Credentials = new NetworkCredential( options.ProxyUsername, options.ProxyPassword );
                ////WebRequest.DefaultWebProxy = webProxy;


                ////htbe.ProxyAddress = new Uri( "http://localhost:8080" );
                ////htbe.ProxyAuthenticationScheme = AuthenticationSchemes.Basic;

                ////htbe.UseDefaultWebProxy = false;

                //m_ProjectServiceTeszt.Endpoint.Binding = customBinding;
#endregion

#region Version2

                //var binding2 = new CustomBinding( );

                //TransportSecurityBindingElement securityBindingElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement( );

                //var secureTransport = new HttpsTransportBindingElement( );
                //secureTransport.UseDefaultWebProxy = false;
                //secureTransport.ProxyAddress = new Uri( $"{options.ProxyAddress}/{options.ProxyPort}" );
                //secureTransport.ProxyAuthenticationScheme = AuthenticationSchemes.Basic;

                //binding2.Elements.Add( securityBindingElement );
                //binding2.Elements.Add( secureTransport );

                //var endpointAddress = new EndpointAddress( options.URL );

                //var factory = new ChannelFactory<GeoProjectServiceTeszt>( binding2, endpointAddress );

                //// Credentials for authentication against the remote service
                //factory.Credentials.UserName.UserName = options.Username;
                //factory.Credentials.UserName.Password = options.Password;

                //// Credentials for authentication against the proxy server
                //factory.Credentials.Windows.ClientCredential.UserName = options.ProxyUsername;
                //factory.Credentials.Windows.ClientCredential.Password = options.ProxyPassword;

                //var client = factory.CreateChannel( );

#endregion


                return m_ProjectServiceTeszt;
            }
        }


    }
}
