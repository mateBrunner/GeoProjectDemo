using GeoProjectDemo.Models;
using GeoProjectServiceTeszt;
using System;
using System.Collections.Generic;
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

    }
}
