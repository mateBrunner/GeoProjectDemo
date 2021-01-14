using GeoProjectServiceTeszt;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace GeoProjectDemo.Services
{
    public class GPKompetenciaService : IGPKompetenciaService
    {

        private GeoProjectServiceClient m_client;

        public GPKompetenciaService( IConfiguration config )
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

        public async Task<CallResultGetAllDolgozokKompetencia> GetAllDolgozokKompetenciaAsync( string sessionId )
        {
            return await m_client.GetAllDolgozokKompetenciaAsync( sessionId );
        }

        public async Task<CallResultGetKodtablaAdatok> GetKodtablaAdatokAsync( string sessionId )
        {
            return await m_client.GetKodtablaAdatokAsync( sessionId );
        }

        public async Task<CallResultSelectDolgozokRecords> SelectDolgozokRecordsAsync( string sessionId, bool mindenDolgozo)
        {
            return await m_client.SelectDolgozokRecordsAsync( sessionId, mindenDolgozo );
        }

    }
}
