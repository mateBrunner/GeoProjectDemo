using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoProjectDemo.Services;
using System.Dynamic;
using BaseClasses;
using GeoProjectDemo.Helpers;
using System.IO;
using System.Globalization;
using System;
using Microsoft.JSInterop;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace GeoProjectDemo.Pages
{
    public partial class Kompetenciak
    {
        [Inject]
        KompetenciaService m_Service { get; set; }

        [Inject]
        IJSRuntime jsRuntime { get; set; }

        public Dictionary<long?, string> KategoriaDict { get; set; } = new Dictionary<long?, string>( );
        public List<string> Szintek { get; set; }
        private List<ExpandoObject> Dolgozok { get; set; }
        public List<Kompetencia> KompetenciaList { get; set; }  = new List<Kompetencia>( );
        public List<KompetenciaKategoria> Kategoriak { get; set; } = new List<KompetenciaKategoria>( );
        public List<long> SelectedKategoriak { get; set; }

        public bool EditWindowIsVisible { get; set; } = false;
        public ExpandoObject EditedDolgozo { get; set; }

        protected override async Task OnAfterRenderAsync( bool first )
        {
            if ( !first )
                return;

            var res = await m_Service.GetAdatok( );
            Szintek = res.Szintek;
            KompetenciaList = res.Kompetenciak;
            Dolgozok = res.Dolgozok;
            Kategoriak = res.Kategoriak;

            Kategoriak.ForEach( k => KategoriaDict.Add( k.Azonosito, k.Nev ) );
            SelectedKategoriak = Kategoriak.Select( k => k.Azonosito ).ToList( );

            SetFilterContext( KompetenciaList );


            StateHasChanged( );
        }

        void SelectedKategoriakChanged( List<long> values )
        {
            SelectedKategoriak = values;
            KompetenciaList.ForEach( k => k.IsVisible = SelectedKategoriak.Contains( (long)k.KategoriaAzonosito ) );
        }

        void Edit( ExpandoObject dolgozoKompetenciak )
        {
            var cloned = CopyHelper.ShallowCopyExpando( dolgozoKompetenciak );
            EditedDolgozo = cloned;
            EditWindowIsVisible = true;
        }

        public void SaveEdit( )
        {
            var dolgozo = EditedDolgozo as IDictionary<string, object>;
            var ind = Dolgozok
                .FindIndex( d => ( d as IDictionary<string, object> )[ "Azonosito" ] == dolgozo[ "Azonosito" ] );

            var dolgozoDict = Dolgozok[ ind ] as IDictionary<string, object>;
            foreach ( Kompetencia komp in KompetenciaList )
                dolgozoDict[ komp.PropertyNev ] = dolgozo[ komp.PropertyNev ];

            EditWindowIsVisible = false;
            StateHasChanged( );
        }

        public async void ExcelExport()
        {
            if ( Dolgozok == null )
                return;

            Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider( );

            Workbook wb = ExcelHelper.ExportKompetenciakToExcel( Dolgozok, KompetenciaList, KategoriaDict );

            byte[] bytes;
            using ( MemoryStream output = new MemoryStream( ) )
            {
                formatProvider.Export( wb, output );
                bytes = output.ToArray( );
            }

            await jsRuntime.InvokeAsync<object>( "saveAsFile", "Kompetenciak.xlsx", Convert.ToBase64String( bytes ) );

        }

    }


}
