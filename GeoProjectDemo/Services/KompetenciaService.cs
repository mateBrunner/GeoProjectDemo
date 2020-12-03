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
            szintDict.Add( 0, "NA" );

            result.Szintek = szintDict.Values.ToList( );

            //Kompetenciacsoportok összegyûjtése
            var csoportok = kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA_CSOPORT" );
            result.Kategoriak = csoportok.Select( c => new KompetenciaKategoria( )
            {
                Azonosito = c.Azonosito,
                Nev = c.Ertek
            } ).ToList( );

            //kompetenciák összegyûjtése
            List<Kompetencia> komps = kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA" )
                .OrderBy( k => k.SzuloKodAzonosito )
                .Select( k => new Kompetencia( )
                {
                    Azonosito = k.Azonosito,
                    Nev = k.Ertek,
                    KategoriaAzonosito = k.SzuloKodAzonosito
                } ).ToList( );
            komps[ 0 ].IsFirstInGroup = true;
            komps[ 0 ].PropertyNev = "KompProp0";
            for (int i = 1; i < komps.Count; i++ )
            {
                komps[ i ].PropertyNev = $"KompProp{i}";
                if ( komps[ i ].KategoriaAzonosito != komps[ i - 1 ].KategoriaAzonosito )
                    komps[ i ].IsFirstInGroup = true;
            }

            Dictionary<string, Kompetencia> kompDict = new Dictionary<string, Kompetencia>( );
            komps.ForEach( k => kompDict.Add( k.PropertyNev, k ) );
            result.Kompetenciak = komps;

            //Dolgozók összegyûjtése
            foreach ( Dolgozok dolg in dolgozokRes.Dolgozok )
            {
                var current = new ExpandoObject( );// as IDictionary<string, object>;
                
                current.TryAdd( "Nev", dolg.Nev );
                current.TryAdd( "Azonosito", dolg.Azonosito );
                foreach ( Kompetencia komp in komps )
                {
                    int szint = dolg.Kompetenciak.Where( k => k.Key == komp.Azonosito ).FirstOrDefault( ).Value;

                    current.TryAdd( komp.PropertyNev, szintDict[ szint ] );
                }

                result.Dolgozok.Add( current as ExpandoObject );
            }

            return result;
        }

        
    }
}
