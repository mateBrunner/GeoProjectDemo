using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.JSInterop;
using GeoProjectDemo.Services;

namespace GeoProjectDemo
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private IJSRuntime m_JSRuntime;
        private SessionService m_SessionService;

        public CustomAuthStateProvider( IJSRuntime jsRuntime, SessionService sessionService )
        {
            m_JSRuntime = jsRuntime;
            m_SessionService = sessionService;
        }
            
        public override async Task<AuthenticationState> GetAuthenticationStateAsync( )
        {
            SessionAdatok userAdatok = await GetSessionAdatok( );

            return await Task.FromResult( new AuthenticationState( userAdatok.ClaimsPrincipal ) );
        }

        public async void LogInUser(string username)
        {
            SessionAdatok userAdatok = await GetSessionAdatok( );

            ( userAdatok.ClaimsPrincipal.Identity as ClaimsIdentity ).AddClaim( new Claim( ClaimTypes.Role, "role2" ) );
            NotifyAuthenticationStateChanged( Task.FromResult( new AuthenticationState( userAdatok.ClaimsPrincipal ) ) );
        }

        private async Task<SessionAdatok> GetSessionAdatok( )
        {
            string browser = await m_JSRuntime.InvokeAsync<string>( "getBrowserInfo" );
            var machineName = System.Environment.MachineName;
            string hash = await m_SessionService.GetHash( machineName, browser );

            return m_SessionService.GetSessionAdatok( hash );
        }


    }


}
