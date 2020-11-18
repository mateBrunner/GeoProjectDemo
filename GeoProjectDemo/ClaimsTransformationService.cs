using GeoProjectDemo.Data;
using GeoProjectDemo.Globals;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace GeoProjectDemo
{
    public class ClaimsTransformationService : IClaimsTransformation
    {

        private WindowsAuthentication m_winAuth;

        #region Constructor
        public ClaimsTransformationService( WindowsAuthentication winAuth )
        {
            m_winAuth = winAuth;
        }
        #endregion


        #region IClaimsTransformation implementation
        public Task<ClaimsPrincipal> TransformAsync( ClaimsPrincipal principal )
        {

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)principal.Identity;

            var windowsUser = (WindowsIdentity)principal.Identity;
            string userName = windowsUser.Name.Split( '\\' )[ 1 ];

            var user = m_winAuth.GetLoggedInUser( userName );

            if ( user == null || !IsInGroup( windowsUser, "GEO\\Domain Users" ) )
                return Task.FromResult( principal );

            foreach ( Claim claim in user.Claims )
                if ( !claimsIdentity.Claims.Contains( claim ) )
                    claimsIdentity.AddClaim( claim );

            return Task.FromResult( principal );

        }
        #endregion

        #region Private functions
        private bool IsInGroup( WindowsIdentity windowsUser, string groupName )
        {
            if ( windowsUser.Groups != null )
            {
                foreach ( var group in windowsUser.Groups )
                {
                    try
                    {
                        if ( group.Translate( typeof( NTAccount ) ).ToString( ) == groupName )
                            return true;
                    }
                    catch ( Exception )
                    {
                        // ignored
                    }
                }
            }
            return false;
        }
        #endregion

    }
}
