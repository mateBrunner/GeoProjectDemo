using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Blazored.SessionStorage;

namespace GeoProjectDemo
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private ISessionStorageService m_SessionStorage;

        public CustomAuthStateProvider( ISessionStorageService service )
        {
            m_SessionStorage = service;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync( )
        {

            var username = await m_SessionStorage.GetItemAsync<string>( "username" );

            ClaimsIdentity identity;

            if (username != null)
            {
                identity = new ClaimsIdentity( new[]
                {
                    new Claim(ClaimTypes.Name, "mate"),
                    new Claim(ClaimTypes.Role, "role1"),

                }, "apiauth_type" );
            } else
            {
                identity = new ClaimsIdentity( );
            }


            var user = new ClaimsPrincipal( identity );

            return await Task.FromResult( new AuthenticationState( user ) );

        }

        public void LogInUser(string username)
        {

            var identity = new ClaimsIdentity( new[]
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.Role, "role1"),

            }, "apiauth_type" );

            var user = new ClaimsPrincipal( identity );

            NotifyAuthenticationStateChanged( Task.FromResult( new AuthenticationState( user ) ) );

        }


    }


}
