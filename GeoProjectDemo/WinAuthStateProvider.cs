using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace GeoProjectDemo
{
        public class WinAuthStateProvider : AuthenticationStateProvider
        {
            public async override Task<AuthenticationState> GetAuthenticationStateAsync( )
            {
                //Getting logged in windows user name
                var user = System.Security.Principal.WindowsIdentity.GetCurrent( ).Name;
                //Logic for fetching role from DB — Here you can get other details abou user from DB also
                string role = "super-user2";
                //Generating new Authentication state with claims if user have any role
                if ( role != "" && role != null )
                {
                    var WindowsAuth = new ClaimsIdentity( new List<Claim>( ) {
                    new Claim(ClaimTypes.Name,user.Split('\\')[1]),
                    new Claim(ClaimTypes.Role,role)}, "windows" );
                    return await Task.FromResult( new AuthenticationState( new ClaimsPrincipal( WindowsAuth ) ) );
                }
                else
                {
                    var anonymous = new ClaimsIdentity( );
                    return await Task.FromResult( new AuthenticationState( new ClaimsPrincipal( anonymous ) ) );
                }
            }

            //public async Task Login( )
            //{
            //    var state = await GetAuthenticationStateAsync( );
            //    NotifyAuthenticationStateChanged( Task.FromResult( state ) );
            //}

        }

}
