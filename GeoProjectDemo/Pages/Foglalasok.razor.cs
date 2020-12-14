using BaseClasses;
using GeoProjectDemo.Helpers;
using GeoProjectDemo.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoProjectDemo.Pages
{
    public partial class Foglalasok
    {

        [Inject]
        FoglalasService m_FoglalasService { get; set; }
        
        [Inject]
        KompetenciaService m_KompetenciaService { get; set; }

        List<ExpandoObject> Dolgozok { get; set; }
        List<ExpandoObject> DolgozokCopy { get; set; }
        List<Nap> NapList { get; set; }
        KompetenciaAdatok KompetenciaAdatok { get; set; }

        public bool FilterWindowIsVisible { get; set; } = false;

        protected override async Task OnAfterRenderAsync( bool first )
        {
            if ( !first )
                return;

            var res = await m_FoglalasService.GetAdatok( );
            Dolgozok = res.Dolgozok;
            DolgozokCopy = Dolgozok.Select( d => CopyHelper.ShallowCopyExpando( d ) ).ToList( );

            NapList = res.Napok;
            KompetenciaAdatok = await m_KompetenciaService.GetAdatok( );            

            StateHasChanged( );

        }

        public void EditWindow()
        {
            FilterWindowIsVisible = true;
        }

        public void SaveFilter()
        {
            FilterWindowIsVisible = false;

            Dictionary<string, long> szintek = new Dictionary<string, long>( )
            {
                { "NA", 0 },
                { "0. nem releváns", 21000 },
                { "1. nincs", 21010 },
                { "2. kezdő", 21020 },
                { "3. gyakorlott", 21030 },
                { "4. szakértő", 21040 },
                { "5. jedi", 21050 }
            };

            Dolgozok = DolgozokCopy.Where( d =>
            {

                //if ( (d as IDictionary<string, object>)[ "Azonosito" ] == 1)
                //    var a = 0;

                IDictionary<string, object> dolgozoDict = KompetenciaAdatok.Dolgozok.Where( dd =>
                    (dd as IDictionary<string, object>)["Nev"].ToString() == (d as IDictionary<string, object>)["Nev"].ToString()
                ).FirstOrDefault( );

                if ( dolgozoDict == null )
                    return false;

                var a = dolgozoDict[ "Nev" ];

                foreach ( Kompetencia komp in KompetenciaAdatok.Kompetenciak )
                {

                    if ( szintek[ dolgozoDict[ komp.PropertyNev ].ToString( ) ] < komp.Szint )
                        return false;

                }
                


                return true;
            }

            ).ToList( );

            StateHasChanged( );
        }
        
    }
    
}
