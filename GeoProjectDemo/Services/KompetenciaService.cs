using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoProjectServiceTeszt;
using System.Dynamic;
using BaseClasses;
using Microsoft.AspNetCore.Components;

namespace GeoProjectDemo.Services
{
    public class KompetenciaService
    {

        private IGPKompetenciaService m_GPService;

        public KompetenciaService( IGPKompetenciaService service )
        {
            m_GPService = service;
        }

        public async Task<KompetenciaAdatok> GetAdatok( )
        {
            KompetenciaAdatok result = new KompetenciaAdatok( );

            var res1 = await m_GPService.GetWindowsAuthenticatedUserIdAsync( );
            var res2 = await m_GPService.LoginAsync( "asdf", "asdf", res1.WindowsUserId, res1.WindowsUserId );
            CallResultGetAllDolgozokKompetencia kompetenciak =
                await m_GPService.GetAllDolgozokKompetenciaAsync( res2.Session.SessionId );
            CallResultGetKodtablaAdatok kodtablaAdatok =
                await m_GPService.GetKodtablaAdatokAsync( res2.Session.SessionId );
            CallResultSelectDolgozokRecords dolgozokRes =
                await m_GPService.SelectDolgozokRecordsAsync( res2.Session.SessionId, false );

            //TODO - oszt�lyok 
            dolgozokRes.Dolgozok = dolgozokRes.Dolgozok.Where( d =>
             d.Osztaly >= 11100 && d.Osztaly < 11200 && d.ErvenyessegKezdete < DateTime.Now && (
             d.ErvenyessegVege > DateTime.Now || d.ErvenyessegVege == null ) ).ToArray( );

            //Kompetenciaszintek �sszegy�jt�se
            var szintek = kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA_SZINT" );
            Dictionary<int, string> szintDict = new Dictionary<int, string>( );
            foreach ( KodtablaAdatok szint in szintek )
                szintDict.Add( szint.Azonosito, Encoding.UTF8.GetString( Encoding.Default.GetBytes( szint.Ertek ) ) );
            szintDict.Add( 0, "NA" );

            result.Szintek = szintDict.Values.ToList( );

            //Kompetenciacsoportok �sszegy�jt�se
            var csoportok = kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA_CSOPORT" );
            result.Kategoriak = csoportok.Select( c => new KompetenciaKategoria( )
            {
                Azonosito = c.Azonosito,
                Nev = c.Ertek
            } ).ToList( );

            //kompetenci�k �sszegy�jt�se
            List<Kompetencia> komps = kodtablaAdatok.Adatok.Where( k => k.KodTipus == "KOMPETENCIA" )
                .OrderBy( k => k.SzuloKodAzonosito )
                .Select( k => new Kompetencia( )
                {
                    Azonosito = k.Azonosito,
                    Nev = k.Ertek,
                    KategoriaAzonosito = k.SzuloKodAzonosito,
                    KategoriaNev = result.Kategoriak.Where( c => c.Azonosito == k.SzuloKodAzonosito ).FirstOrDefault( ).Nev
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

            //Dolgoz�k �sszegy�jt�se
            foreach ( Dolgozok dolg in dolgozokRes.Dolgozok )
            {
                var current = new ExpandoObject( );
                
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
