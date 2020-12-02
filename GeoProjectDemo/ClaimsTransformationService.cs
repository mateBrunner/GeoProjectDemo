using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace GeoProjectDemo
{
    public class ClaimsTransformationService : IClaimsTransformation
    {

        #region Constructor
        public ClaimsTransformationService( )
        {
        }
        #endregion


        #region IClaimsTransformation implementation
        public Task<ClaimsPrincipal> TransformAsync( ClaimsPrincipal principal )
        {

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)principal.Identity;

            var windowsUser = (WindowsIdentity)principal.Identity;
            
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
