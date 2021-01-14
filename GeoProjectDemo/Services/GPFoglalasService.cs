using GeoProjectServiceTeszt;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace GeoProjectDemo.Services
{
    public class GPFoglalasService : IGPFoglalasService
    {

        private GeoProjectServiceClient m_client;

        public GPFoglalasService( IConfiguration config )
        {
            m_client = new GeoProjectServiceClient( config );
        }

        public async Task<CallResultGetWindowsAuthenticatedUserId> GetWindowsAuthenticatedUserIdAsync( )
        {
            return await m_client.GetWindowsAuthenticatedUserIdAsync( );
        }

        public async Task<CallResultLogin> LoginAsync( string appName, string appVersion, int userId, int roleUserId)
        {
            return await m_client.LoginAsync( appName, appVersion, userId, roleUserId );
        }

        public async Task<CallResultSelectDolgozokRecords> SelectDolgozokRecordsAsync( string sessionId, bool mindenDolgozo)
        {
            return await m_client.SelectDolgozokRecordsAsync( sessionId, mindenDolgozo );
        }

        public async Task<CallResultGetAllPublicFoglalasok> GetAllPublicFoglalasokAsync( string sessionId )
        {
            return await m_client.GetAllPublicFoglalasokAsync( sessionId );
        }

        public async Task<CallResultSelectProjektRecords> SelectProjektRecordsAsync( string sessionId, bool archivaltakIs, bool nemSajatokIs )
        {
            return await m_client.SelectProjektRecordsAsync( sessionId, archivaltakIs, nemSajatokIs );
        }

    }
}
