using GeoProjectServiceTeszt;
using System.Threading.Tasks;

namespace GeoProjectDemo.Services
{
    public interface IGPKompetenciaService
    {

        Task<CallResultGetWindowsAuthenticatedUserId> GetWindowsAuthenticatedUserIdAsync( );

        Task<CallResultLogin> LoginAsync( string appName, string appVersion, int userId, int roleUserId );

        Task<CallResultGetAllDolgozokKompetencia> GetAllDolgozokKompetenciaAsync( string sessionId );

        Task<CallResultGetKodtablaAdatok> GetKodtablaAdatokAsync( string sessionId );

        Task<CallResultSelectDolgozokRecords> SelectDolgozokRecordsAsync( string sessionId, bool mindenDolgozo );

    }
}
