using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using GeoProjectDemo.Data;
using System.Linq;

namespace GeoProjectDemo.Globals
{
    public class WindowsAuthentication
    {

        List<User> m_LoggedInUsers = new List<User>( );

        public void LoginUser( User user )
        {
            if ( !m_LoggedInUsers.Any( u => u.Nev == user.Nev ) )
                m_LoggedInUsers.Add( user );
        }

        public User? GetLoggedInUser(string username)
        {
            return m_LoggedInUsers.FirstOrDefault( u => u.Nev == username );
        }


        [DllImport( "advapi32.dll", SetLastError = true )]
        public static extern bool LogonUser( string lpszUsername,
                string lpszDomain,
                string lpszPassword,
                int dwLogonType,
                int dwLogonProvider,
                ref IntPtr phToken
                );

        [DllImport( "kernel32.dll" )]
        public static extern int FormatMessage( int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId, ref String lpBuffer, int nSize, ref IntPtr Arguments );

        // closes open handes returned by LogonUser
        [DllImport( "kernel32.dll", CharSet = CharSet.Auto )]
        public extern static bool CloseHandle( IntPtr handle );




    }
}
