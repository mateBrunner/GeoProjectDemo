using BaseClasses;
using Geometria.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace GeoProjectServiceTeszt
{
    public partial class GeoProjectServiceClient : System.ServiceModel.ClientBase<GeoProjectServiceTeszt.GeoProjectService>, GeoProjectServiceTeszt.GeoProjectService
    {



        public GeoProjectServiceClient( ServiceOptions options ) :
                base( GetCustomBinding( ), GetDefaultEndpointAddress( ) )
        {

            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_GeoProjectService.ToString( );

            var webProxy = new WebProxy( $"{options.ProxyAddress}:{options.ProxyPort}", true );
            webProxy.Credentials = new NetworkCredential(
                options.ProxyUsername,
                Cryptor.Decrypt( options.ProxyPassword ) );
            WebRequest.DefaultWebProxy = webProxy;

            ClientCredentials.UserName.UserName = options.Username;
            ClientCredentials.UserName.Password = Cryptor.Decrypt( options.Password );

            ConfigureEndpoint( this.Endpoint, this.ClientCredentials );

        }

        private static System.ServiceModel.Channels.Binding GetCustomBinding( )
        {
            var endpointConfiguration = EndpointConfiguration.BasicHttpBinding_GeoProjectService;

            if ( ( endpointConfiguration == EndpointConfiguration.BasicHttpBinding_GeoProjectService ) )
            {
                System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding( );
                binding.MaxBufferSize = int.MaxValue;
                binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                binding.MaxReceivedMessageSize = int.MaxValue;
                binding.AllowCookies = true;
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;

                var customBinding = new CustomBinding( binding );
                var htbe = customBinding.Elements.Find<HttpTransportBindingElement>( );

                htbe.ProxyAuthenticationScheme = AuthenticationSchemes.Basic;

                htbe.UseDefaultWebProxy = false;


                return customBinding;
            }
            throw new System.InvalidOperationException( string.Format( "Could not find endpoint with name \'{0}\'.", endpointConfiguration ) );
        }
    }
}
