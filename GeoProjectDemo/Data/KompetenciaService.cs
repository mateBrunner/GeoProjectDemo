using GeoProjectDemo.Models;
using GeoProjectServiceTeszt;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeoProjectDemo.Data
{
    public class KompetenciaService
    {

        public async Task<List<Dolgozo>> GetDolgozokAsync( )
        {
            
            var res1 = await Globals.ProjectServiceTeszt.GetWindowsAuthenticatedUserIdAsync( );
            var res2 = await Globals.ProjectServiceTeszt.LoginAsync( "asdf", "asdf", res1.WindowsUserId, res1.WindowsUserId );
            CallResultGetAllDolgozokKompetencia kompetenciak = 
                await Globals.ProjectServiceTeszt.GetAllDolgozokKompetenciaAsync( res2.Session.SessionId );
            CallResultGetKodtablaAdatok kodtablaAdatok = 
                await Globals.ProjectServiceTeszt.GetKodtablaAdatokAsync( res2.Session.SessionId );
            CallResultSelectDolgozokRecords dolgozokRes = 
                await Globals.ProjectServiceTeszt.SelectDolgozokRecordsAsync( res2.Session.SessionId, false );

            //TODO - osztályok
            dolgozokRes.Dolgozok = dolgozokRes.Dolgozok.Where( d => 
             d.Osztaly >= 11100 && d.Osztaly < 11200 && d.ErvenyessegKezdete < DateTime.Now && (
             d.ErvenyessegVege > DateTime.Now || d.ErvenyessegVege == null ) ).ToArray( );

            var v1 = kodtablaAdatok.Adatok.Where( x => x.Azonosito == 8 ).FirstOrDefault( );
            var v2 = kodtablaAdatok.Adatok.Where( x => x.Azonosito == 19004 ).FirstOrDefault( );
            var v3 = kodtablaAdatok.Adatok.Where( x => x.Azonosito == 20055 ).FirstOrDefault( );
            var v4 = kodtablaAdatok.Adatok.Where( x => x.Azonosito == 21857 ).FirstOrDefault( );
            var v5 = kodtablaAdatok.Adatok.Where( x => x.Azonosito == 21020 ).FirstOrDefault( );

            //Kompetenciaszintek összegyûjtése
            var szintek = kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA_SZINT" );
            Dictionary<long, string> szintDict = new Dictionary<long, string>( );
            foreach ( KodtablaAdatok szint in szintek )
                szintDict.Add( szint.Azonosito, szint.Ertek );

            //Dolgozók összegyûjtése
            Dictionary<long, Dolgozo> dolgozoDict = new Dictionary<long, Dolgozo>( );
            foreach ( Dolgozok dolg in dolgozokRes.Dolgozok )
                dolgozoDict.Add( dolg.Azonosito, new Dolgozo( ) { Azonosito = dolg.Azonosito, Nev = dolg.Nev } );

            List<Dolgozo> dolgozok = dolgozokRes.Dolgozok.Select(
                d => new Dolgozo( ) { Azonosito = d.Azonosito, Nev = d.Nev,
                    Kompetenciak = d.Kompetenciak.Select( k => new Kompetencia( ) { 
                        Azonosito = k.Key, 
                        Szint = k.Value,
                        SzintNev = szintDict[k.Value]
                    } ).ToList( )
                } ).ToList( );

            return dolgozok;
        }

        public async Task<List<Kompetencia>> GetKompetenciakAsync()
        {
            var res1 = await Globals.ProjectServiceTeszt.GetWindowsAuthenticatedUserIdAsync( );
            var res2 = await Globals.ProjectServiceTeszt.LoginAsync( "asdf", "asdf", res1.WindowsUserId, res1.WindowsUserId );
            CallResultGetKodtablaAdatok kodtablaAdatok =
                await Globals.ProjectServiceTeszt.GetKodtablaAdatokAsync( res2.Session.SessionId );
            return kodtablaAdatok.Adatok.Where(k => k.KodTipus == "KOMPETENCIA")
                .Select(k => new Kompetencia( ) {
                    Azonosito = k.Azonosito,
                    Nev = k.Ertek,
                    CsoportAzonosito = k.SzuloKodAzonosito
                } ).ToList();
        }

        public async Task<List<ExpandoObject>> GetDynamics( )
        {

            var res1 = await Globals.ProjectServiceTeszt.GetWindowsAuthenticatedUserIdAsync( );
            var res2 = await Globals.ProjectServiceTeszt.LoginAsync( "asdf", "asdf", res1.WindowsUserId, res1.WindowsUserId );
            CallResultGetAllDolgozokKompetencia kompetenciak =
                await Globals.ProjectServiceTeszt.GetAllDolgozokKompetenciaAsync( res2.Session.SessionId );
            CallResultGetKodtablaAdatok kodtablaAdatok =
                await Globals.ProjectServiceTeszt.GetKodtablaAdatokAsync( res2.Session.SessionId );
            CallResultSelectDolgozokRecords dolgozokRes =
                await Globals.ProjectServiceTeszt.SelectDolgozokRecordsAsync( res2.Session.SessionId, false );

            //TODO - osztályok
            dolgozokRes.Dolgozok = dolgozokRes.Dolgozok.Where( d =>
             d.Osztaly >= 11100 && d.Osztaly < 11200 && d.ErvenyessegKezdete < DateTime.Now && (
             d.ErvenyessegVege > DateTime.Now || d.ErvenyessegVege == null ) ).ToArray( );

            //Kompetenciaszintek összegyûjtése
            var szintek = kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA_SZINT" );
            Dictionary<int, string> szintDict = new Dictionary<int, string>( );
            foreach ( KodtablaAdatok szint in szintek )
                szintDict.Add( szint.Azonosito, szint.Ertek );

            var a = "2. kezdõ" == szintDict[ 21020 ];

            szintDict.Add( 0, "nincs" );
            //szintDict[ 21000 ] = "0. nem releváns";
            //szintDict[ 21010 ] = "1. nincs";
            szintDict[ 21020 ] = "2. kezdõ";
            //szintDict[ 21030 ] = "3. gyakorlott";
            //szintDict[ 21040 ] = "4. szakértõ";
            //szintDict[ 21050 ] = "5. jedi";

            //kompetenciák összegyûjtése
            var komps = kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA" );
            Dictionary<int, string> kompDict = new Dictionary<int, string>( );
            foreach ( KodtablaAdatok komp in komps )
                kompDict.Add( komp.Azonosito, komp.Ertek );

            kompDict.Remove( 20090 );
            kompDict.Remove( 20091 );

            //Dolgozók összegyûjtése
            List<ExpandoObject> dolgozok = new List<ExpandoObject>( );
            foreach ( Dolgozok dolg in dolgozokRes.Dolgozok )
            {
                var current = new ExpandoObject( ) as IDictionary<string, object>;
                current.Add( "Nev", dolg.Nev );
                current.Add( "Azonosito", dolg.Azonosito );
                foreach ( int kompAzon in kompDict.Keys )
                {
                    int szint = dolg.Kompetenciak.Where( k => k.Key == kompAzon ).FirstOrDefault( ).Value;

                    current.Add( kompDict[ kompAzon ], szintDict[ szint ] );
                }

                dolgozok.Add( current as ExpandoObject );
            }

            return dolgozok;
        }

        public async Task<List<string>> GetKompetenciaNevek( )
        {
            var res1 = await Globals.ProjectServiceTeszt.GetWindowsAuthenticatedUserIdAsync( );
            var res2 = await Globals.ProjectServiceTeszt.LoginAsync( "asdf", "asdf", res1.WindowsUserId, res1.WindowsUserId );
            CallResultGetKodtablaAdatok kodtablaAdatok =
                await Globals.ProjectServiceTeszt.GetKodtablaAdatokAsync( res2.Session.SessionId );
            return kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA" )
                .Select( k => k.Ertek ).Distinct().ToList( );
        }


    }
}
