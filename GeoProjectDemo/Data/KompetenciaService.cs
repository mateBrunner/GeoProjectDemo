using GeoProjectDemo.Models;
using GeoProjectDemo.Globals;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoProjectServiceTeszt;

namespace GeoProjectDemo.Data
{
    public class KompetenciaService
    {

        public async Task<KompetenciaAdatok> GetAdatok( )
        {
            KompetenciaAdatok result = new KompetenciaAdatok( );

            var res1 = await Globals.Globals.ProjectServiceTeszt.GetWindowsAuthenticatedUserIdAsync( );
            var res2 = await Globals.Globals.ProjectServiceTeszt.LoginAsync( "asdf", "asdf", res1.WindowsUserId, res1.WindowsUserId );
            CallResultGetAllDolgozokKompetencia kompetenciak =
                await Globals.Globals.ProjectServiceTeszt.GetAllDolgozokKompetenciaAsync( res2.Session.SessionId );
            CallResultGetKodtablaAdatok kodtablaAdatok =
                await Globals.Globals.ProjectServiceTeszt.GetKodtablaAdatokAsync( res2.Session.SessionId );
            CallResultSelectDolgozokRecords dolgozokRes =
                await Globals.Globals.ProjectServiceTeszt.SelectDolgozokRecordsAsync( res2.Session.SessionId, false );

            //TODO - osztályok
            dolgozokRes.Dolgozok = dolgozokRes.Dolgozok.Where( d =>
             d.Osztaly >= 11100 && d.Osztaly < 11200 && d.ErvenyessegKezdete < DateTime.Now && (
             d.ErvenyessegVege > DateTime.Now || d.ErvenyessegVege == null ) ).ToArray( );

            //Kompetenciaszintek összegyûjtése
            var szintek = kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA_SZINT" );
            Dictionary<int, string> szintDict = new Dictionary<int, string>( );
            foreach ( KodtablaAdatok szint in szintek )
                szintDict.Add( szint.Azonosito, Encoding.UTF8.GetString( Encoding.Default.GetBytes( szint.Ertek ) ) );
            szintDict.Add( 0, "nincs" );

            result.Szintek = szintDict.Values.ToList( );


            //kompetenciák összegyûjtése
            var komps = kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA" );
            Dictionary<int, string> kompDict = new Dictionary<int, string>( );
            int kompCounter = 0;
            foreach ( KodtablaAdatok komp in komps )
            {
                string propName = $"KompProp{kompCounter++}";
                kompDict.Add( komp.Azonosito, propName );
                result.KompetenciaByProp.Add( propName, new Kompetencia( ) { Azonosito = komp.Azonosito, Nev = komp.Ertek } );
            }

            //Dolgozók összegyûjtése
            foreach ( Dolgozok dolg in dolgozokRes.Dolgozok )
            {
                var current = new ExpandoObject( );// as IDictionary<string, object>;
                
                //current.Add( "Nev", dolg.Nev );
                //current.Add( "Azonosito", dolg.Azonosito );
                foreach ( int kompAzon in kompDict.Keys )
                {
                    int szint = dolg.Kompetenciak.Where( k => k.Key == kompAzon ).FirstOrDefault( ).Value;

                    current. TryAdd( kompDict[ kompAzon ], szintDict[ szint ] );
                }

                result.Dolgozok.Add( current as ExpandoObject );
            }

            return result;
        }

        
    }
}
