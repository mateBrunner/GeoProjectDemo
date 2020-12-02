using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoProjectServiceTeszt;
using System.Security.Claims;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace GeoProjectDemo.Services
{
    public class SessionService
    {

        private Dictionary<string, SessionAdatok> m_SessionAdatok = new Dictionary<string, SessionAdatok>( );
        

        public SessionAdatok GetSessionAdatok( string hash )
        {
            if ( m_SessionAdatok.ContainsKey( hash ) )
                return m_SessionAdatok[ hash ];

            m_SessionAdatok.Add( hash, new SessionAdatok
            {
                Nev = hash.Split( "||" )[ 0 ],
                ClaimsPrincipal = new ClaimsPrincipal( 
                    new ClaimsIdentity( new[]
                    {
                        new Claim(ClaimTypes.Name, hash.Split("||")[0]),
                        new Claim(ClaimTypes.Role, "admin"),
                        new Claim(ClaimTypes.Role, "user228")
                    }, "authentication type" ) )
            } );

            return m_SessionAdatok[ hash ];

        }

        public async Task<string> GetHash( string windowsUser, string browser )
        {
            return $"{windowsUser}||{browser}";
        }


    }

    public class SessionAdatok
    {

        public string Nev { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

    }

}
