using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoProjectDemo.Models
{
    public class Kompetencia : IComparable<Kompetencia>
    {

        public long Azonosito { get; set; }
        public string Nev { get; set; }
        public long? CsoportAzonosito { get; set; }
        public long Szint { get; set; }
        public string SzintNev { get; set; }

        public int CompareTo( Kompetencia other )
        {
            if ( this.Szint < other.Szint )
            {
                return 1;
            }
            else if ( this.Szint > other.Szint )
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }


    }
}
