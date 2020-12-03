using BaseClasses;
using System.Collections.Generic;

namespace BaseClasses
{
    public class Dolgozo
    {

        public long Azonosito { get; set; }
        public string Nev { get; set; }
        public List<Kompetencia> Kompetenciak { get; set; } = new List<Kompetencia>( );

    }


}
