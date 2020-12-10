using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace BaseClasses
{
    public class NapiFoglalas
    {
        public List<Foglalas> Foglalasok { get; set; } = new List<Foglalas>( );
        public MarkupString ToolTipSzoveg { get; set; }
        public int GetOsszIdotartam
        {
            get
            {
                return Foglalasok.Sum( f => f.TeljesIdotartam ) - GetTulfoglalas;
            }
        }
        public int GetTulfoglalas
        {
            get
            {
                return Foglalasok.Sum( f => f.Tulfoglalas );
            }
        }

        public void CreateToolTipSzoveg( )
        {

            StringBuilder normalFoglalasok = new StringBuilder( "Normál foglalások" );
            StringBuilder tulFoglalasok = new StringBuilder( $"<br>Túlfoglalások" );

            foreach ( Foglalas f in Foglalasok )
            {
                if ( f.TeljesIdotartam > f.Tulfoglalas )
                    normalFoglalasok.Append( new StringBuilder(
                        $"<br>&nbsp;&nbsp;&nbsp;{f.ProjektSzam} / {f.ProjektNev} ({f.ProjektVezeto}) - {f.TeljesIdotartam - f.Tulfoglalas} óra" ) );
                else if ( f.Tulfoglalas > 0 )
                    tulFoglalasok.Append( new StringBuilder(
                         $"<br>&nbsp;&nbsp;&nbsp;{f.ProjektSzam} / {f.ProjektNev} ({f.ProjektVezeto}) - {f.Tulfoglalas} óra" ) );
            }


            ToolTipSzoveg = new MarkupString( normalFoglalasok.Append( tulFoglalasok.ToString( ) ).ToString( ) );

        }

    }
}
