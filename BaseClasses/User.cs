using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BaseClasses
{
    public class User
    {

        public string Nev { get; set; }
        public List<Claim> Claims { get; set; } = new List<Claim>( );

    }
}
