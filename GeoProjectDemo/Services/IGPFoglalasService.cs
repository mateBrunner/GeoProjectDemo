using GeoProjectServiceTeszt;
using System.Threading.Tasks;

namespace GeoProjectDemo.Services
{
    public interface IGPFoglalasService
    {

        Task<CallResultGetWindowsAuthenticatedUserId> GetWindowsAuthenticatedUserIdAsync( );

        Task<CallResultLogin> LoginAsync( string appName, string appVersion, int userId, int roleUserId );

        Task<CallResultSelectDolgozokRecords> SelectDolgozokRecordsAsync( string sessionId, bool mindenDolgozo );

        Task<CallResultGetAllPublicFoglalasok> GetAllPublicFoglalasokAsync( string sessionId );

        Task<CallResultSelectProjektRecords> SelectProjektRecordsAsync( string sessionId, bool archivaltakIs, bool nemSajatokIs );

    }
}
