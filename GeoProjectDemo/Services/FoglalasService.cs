using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoProjectServiceTeszt;
using System.Dynamic;
using BaseClasses;

namespace GeoProjectDemo.Services
{
    public class FoglalasService
    {

        public async Task<FoglalasAdatok> GetAdatok( )
        {
            FoglalasAdatok result = new FoglalasAdatok( );

            var res1 = await Globals.Globals.ProjectServiceTeszt.GetWindowsAuthenticatedUserIdAsync( );
            var res2 = await Globals.Globals.ProjectServiceTeszt.LoginAsync( "asdf", "asdf", res1.WindowsUserId, res1.WindowsUserId );
            CallResultGetAllDolgozokKompetencia kompetenciak =
                await Globals.Globals.ProjectServiceTeszt.GetAllDolgozokKompetenciaAsync( res2.Session.SessionId );
            CallResultGetKodtablaAdatok kodtablaAdatok =
                await Globals.Globals.ProjectServiceTeszt.GetKodtablaAdatokAsync( res2.Session.SessionId );
            CallResultGetAllPublicFoglalasok foglalasok =
                await Globals.Globals.ProjectServiceTeszt.GetAllPublicFoglalasokAsync( res2.Session.SessionId );

            CallResultSelectDolgozokRecords dolgozokRes =
                await Globals.Globals.ProjectServiceTeszt.SelectDolgozokRecordsAsync( res2.Session.SessionId, false );

            var napok = GetVisibleDays( );
            napok[ 0 ].IsFirstInGroup = true;
            result.Napok = napok;

            //Dolgozók összegyûjtése
            foreach ( var dolg in foglalasok.Foglalasok.DolgozoFoglalasok )
            {
                var current = new ExpandoObject( );

                current.TryAdd( "Azonosito", dolg.Key );
                current.TryAdd( "Nev", dolgozokRes.Dolgozok.Where( k => k.Azonosito == dolg.Key ).FirstOrDefault( ).Nev );

                var dolgozoFoglalasok = dolg.Value.NapiFoglalasok;

                foreach (Nap nap in napok) {

                    NapiFoglalas napiFoglalas = new NapiFoglalas( );

                    NapiPublikaltFoglalasok dbFoglalasok;

                    if ( dolgozoFoglalasok.TryGetValue( nap.Date, out dbFoglalasok ) )
                    {
                        
                        for (int i = 0; i < dbFoglalasok.Foglalasok.Length; i++ )
                        {
                            napiFoglalas.Foglalasok.Add( new Foglalas( )
                            {
                                TeljesIdotartam = dbFoglalasok.Foglalasok[ i ].Idotartam,
                                Tulfoglalas = dbFoglalasok.Foglalasok[ i ].Tulfoglalas
                            } ); ;
                        }

                    }

                    current.TryAdd( nap.PropertyNev, napiFoglalas );

                }

                result.Dolgozok.Add( current as ExpandoObject );
            }

            return result;
        }

        private List<Nap> GetVisibleDays()
        {
            var firstDay = DateTime.Now.Subtract( new TimeSpan( 14, 0, 0, 0 ) );
            List<Nap> res = new List<Nap>( );

            for ( int i = 0; i < 60; i++ )
                res.Add( new Nap( firstDay.AddDays( i ) ) );
             
            return res;
        }

    }

    public class NapiFoglalas
    {
        public List<Foglalas> Foglalasok { get; set; } = new List<Foglalas>( );
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

    }

    public class Foglalas
    {
        public int TeljesIdotartam { get; set; }
        public int Tulfoglalas { get; set; }
    }

    public class FoglalasAdatok
    {
        public List<ExpandoObject> Dolgozok { get; set; } = new List<ExpandoObject>( );
        public List<Nap> Napok { get; set; } = new List<Nap>( );
    }

    public class Nap
    {

        public Nap(DateTime date)
        {
            Date = new DateTime( date.Year, date.Month, date.Day );
            IsMunkanap = date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
            IsFirstInGroup = date.Day == 1 || date.Day == 15;
        }

        public DateTime Date { get; set; }
        public bool IsMunkanap { get; set; }
        public bool IsFirstInGroup { get; set; }

        public string PropertyNev
        {
            get
            {
                return $"date_{Date.Year}_{Date.Month}_{Date.Day}";
            }
        }
    }


}
