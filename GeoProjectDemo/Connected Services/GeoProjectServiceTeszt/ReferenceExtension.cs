using BaseClasses;
using Geometria.Common;
using GeoProjectDemo.Helpers;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.ServiceModel.Channels;

namespace GeoProjectServiceTeszt
{
    public partial class GeoProjectServiceClient : System.ServiceModel.ClientBase<GeoProjectServiceTeszt.GeoProjectService>, GeoProjectServiceTeszt.GeoProjectService
    {



        public GeoProjectServiceClient( IConfiguration config ) :
                base( GetCustomBinding( ), GetDefaultEndpointAddress( ) )
        {

            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_GeoProjectService.ToString( );

            var proxyAddress = SettingsHelper.GetServiceSettings( 
                config, ServiceNames.GEOPROJECT, ServiceAttributes.PROXY_ADDRESS );

            var proxyPort = SettingsHelper.GetServiceSettings(
                config, ServiceNames.GEOPROJECT, ServiceAttributes.PROXY_PORT );

            var proxyUsername = SettingsHelper.GetServiceSettings(
                config, ServiceNames.GEOPROJECT, ServiceAttributes.PROXY_USER );

            var proxyPassword = SettingsHelper.GetServiceSettings(
                config, ServiceNames.GEOPROJECT, ServiceAttributes.PROXY_PWD );

            var username = SettingsHelper.GetServiceSettings(
                config, ServiceNames.GEOPROJECT, ServiceAttributes.USERNAME );

            var password = SettingsHelper.GetServiceSettings(
                config, ServiceNames.GEOPROJECT, ServiceAttributes.PASSWORD );

            var webProxy = new WebProxy( $"{proxyAddress}:{proxyPort}", true );
            webProxy.Credentials = new NetworkCredential(
                proxyUsername,
                Cryptor.Decrypt( proxyPassword ) );
            WebRequest.DefaultWebProxy = webProxy;

            ClientCredentials.UserName.UserName = username;
            ClientCredentials.UserName.Password = Cryptor.Decrypt( password );

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
