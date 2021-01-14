using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoProjectServiceTeszt;
using System.Dynamic;
using BaseClasses;


namespace GeoProjectDemo.Services
{
    public class FoglalasService
    {

        private IGPFoglalasService m_GPService;

        public FoglalasService( IGPFoglalasService service )
        {
            m_GPService = service;
        }

        public async Task<FoglalasAdatok> GetAdatok( )
        {
            FoglalasAdatok result = new FoglalasAdatok( );

            var res1 = await m_GPService.GetWindowsAuthenticatedUserIdAsync( );
            var res2 = await m_GPService.LoginAsync( "asdf", "asdf", res1.WindowsUserId, res1.WindowsUserId );

            CallResultGetAllPublicFoglalasok foglalasok =
                await m_GPService.GetAllPublicFoglalasokAsync( res2.Session.SessionId );

            Dictionary<int, Dolgozok> dolgozokDict =
                ( await m_GPService.SelectDolgozokRecordsAsync( res2.Session.SessionId, false ) )
                .Dolgozok.ToDictionary( x => x.Azonosito, x => x );

            Dictionary<int, Projektek> projektekDict =
                ( await m_GPService.SelectProjektRecordsAsync( res2.Session.SessionId, true, true ) )
                .Projektek.ToDictionary( x => x.Azonosito, x => x );

            result.Napok = GetVisibleDays( );

            foreach ( var dolg in foglalasok.Foglalasok.DolgozoFoglalasok )
            {
                var current = new ExpandoObject( );

                current.TryAdd( "Azonosito", dolg.Key );
                current.TryAdd( "Nev", dolgozokDict[ dolg.Key ].Nev );

                var dolgozoFoglalasok = dolg.Value.NapiFoglalasok;

                foreach ( Nap nap in result.Napok )
                {

                    NapiFoglalas napiFoglalas = new NapiFoglalas( );

                    NapiPublikaltFoglalasok dbFoglalasok;

                    //kiszedjük az adott napra vonatkozó foglalásokat
                    if ( !dolgozoFoglalasok.TryGetValue( nap.Date, out dbFoglalasok ) )
                    {
                        current.TryAdd( nap.PropertyNev, napiFoglalas );
                        continue;
                    }

                    napiFoglalas.Foglalasok = dbFoglalasok.Foglalasok.Select(
                        f => new Foglalas( )
                        {
                            TeljesIdotartam = f.Idotartam,
                            Tulfoglalas = f.Tulfoglalas,
                            ProjektAzonosito = f.ProjektAzonosito,
                            ProjektNev = projektekDict[ f.ProjektAzonosito ].RovidNev,
                            ProjektVezeto = dolgozokDict[ (int)projektekDict[ f.ProjektAzonosito ].ProjektVezeto ].Nev,
                            ProjektVezetoAzon = dolgozokDict[ (int)projektekDict[ f.ProjektAzonosito ].ProjektVezeto ].Azonosito,
                            ProjektSzam = Convert.ToInt32( projektekDict[ f.ProjektAzonosito ].Szama ),
                            TevekenysegAzon = f.TevekenysegAzonosito,
                            TevekenysegSorszam = f.TevekenysegSorszam
                        }
                        ).ToList( );

                    napiFoglalas.CreateToolTipSzoveg( );
                    current.TryAdd( nap.PropertyNev, napiFoglalas );

                }

                result.Dolgozok.Add( current );
            }

            return result;
        }

        private List<Nap> GetVisibleDays()
        {
            var firstDay = DateTime.Now.Subtract( new TimeSpan( 14, 0, 0, 0 ) );
            List<Nap> res = new List<Nap>( );

            for ( int i = 0; i < 60; i++ )
                res.Add( new Nap( firstDay.AddDays( i ) ) );

            res[ 0 ].IsFirstInGroup = true;

            return res;
        }

    }

}
