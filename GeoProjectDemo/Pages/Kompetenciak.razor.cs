using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoProjectDemo.Services;
using System.Dynamic;
using BaseClasses;
using GeoProjectDemo.Helpers;

namespace GeoProjectDemo.Pages
{
    public partial class Kompetenciak
    {
        [Inject]
        KompetenciaService m_Service { get; set; }

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

    }


}
