using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoProjectDemo.Models
{
    public class Dolgozo
    {

        public long Azonosito { get; set; }
        public string Nev { get; set; }
        public List<Kompetencia> Kompetenciak { get; set; } = new List<Kompetencia>( );

    }
}
