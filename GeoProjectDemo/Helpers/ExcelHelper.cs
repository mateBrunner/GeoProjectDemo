using BaseClasses;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Telerik.Documents.Common.Model;
using Telerik.Documents.SpreadsheetStreaming;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace GeoProjectDemo.Helpers
{
    public static class ExcelHelper
    {

        public static Workbook ExportKompetenciakToExcel( 
            List<ExpandoObject> expandok, 
            List<Kompetencia> kompetenciak,
            Dictionary<long?, string> kategoriak
        )
        {

            var workbook = new Workbook( );
            var ws = workbook.Worksheets.Add( );
            ws.Name = "Kompetenciák";

            ThemableColor black = new ThemableColor( Telerik.Documents.Media.Color.FromRgb(0, 0, 0) );
            ThemableColor green = new ThemableColor( Telerik.Documents.Media.Color.FromRgb( 147, 220, 105 ) );

            GradientFill greenGradientFill = new GradientFill( 
                GradientType.Horizontal,
                Telerik.Documents.Media.Color.FromRgb( 147, 220, 105 ),
                Telerik.Documents.Media.Color.FromRgb( 100, 180, 76 ) );

            //első oszlop és első két sor bold-ra állítása
            ws.Columns.GetColumnSelection( 0 ).SetWidth( new ColumnWidth( 160, true ) );
            var firstColumn = ws.Cells[ new CellRange(0, 0, expandok.Count + 1, 0) ];
            firstColumn.SetIsBold( true );
            var headerRows = ws.Cells[ new CellRange( 0, 0, 1, kompetenciak.Count ) ];
            headerRows.SetIsBold( true );
            headerRows.SetFill( greenGradientFill );
            ws.Cells[ new CellRange( 0, 0, 1, 0 ) ].Merge( );
            ws.Cells[ 0, 0 ].SetValue( "Név" );
            ws.Cells[ 0, 0 ].SetHorizontalAlignment( RadHorizontalAlignment.Center );

            //első sorok szövege
            List<int> groups = kompetenciak
                .Where( i => i.IsFirstInGroup )
                .Select( i => kompetenciak.IndexOf( i ) )
                .ToList( );
            groups.Add( kompetenciak.Count );

            for ( int i = 0; i < groups.Count - 1; i++ )
            {
                ws.Cells[ 0, groups[ i ] + 1, 0, groups[ i + 1 ] ].Merge( );
                ws.Cells[ 0, groups[ i ] + 1 ].SetHorizontalAlignment( RadHorizontalAlignment.Left );
                ws.Cells[ 0, groups[ i ] + 1 ].SetValue( kategoriak[ kompetenciak[ groups[ i ] ].KategoriaAzonosito ] ); 
            }

            for ( int i = 0; i < kompetenciak.Count; i++ )
            {
                ws.Cells[ 1, i + 1 ].SetValue( kompetenciak[ i ].Nev );
                ws.Columns.GetColumnSelection( i + 1 ).SetWidth( new ColumnWidth( 120, true ) );
            }

            //sorok szövegei
            for ( int i = 0; i < expandok.Count; i++ )
            {
                var dolgozo = expandok[ i ] as IDictionary<string, object>;

                ws.Cells[ i + 2, 0 ].SetValue( dolgozo[ "Nev" ].ToString() );

                for ( int j = 0; j < kompetenciak.Count; j++ )
                    ws.Cells[ 2 + i, 1 + j ].SetValue( dolgozo[ kompetenciak[ j ].PropertyNev ].ToString() );
            }

            //borderek
            CellBorders border = new CellBorders(
            new CellBorder( CellBorderStyle.Thin, black ),   // Left border 
            new CellBorder( CellBorderStyle.None, black ),   // Top border 
            new CellBorder( CellBorderStyle.Thin, black ),   // Right border 
            new CellBorder( CellBorderStyle.None, black ),   // Bottom border 
            new CellBorder( CellBorderStyle.None, black ),   // Inside horizontal border 
            new CellBorder( CellBorderStyle.None, black ),   // Inside vertical border 
            new CellBorder( CellBorderStyle.None, black ),   // Diagonal up border 
            new CellBorder( CellBorderStyle.None, black ) ); // Diagonal down border 

            for ( int i = 0; i < kompetenciak.Count; i++ )
            {
                ws.Cells[ 0, i + 1, expandok.Count + 1, i + 1 ].SetBorders( border );
            }

            border.Right = new CellBorder( CellBorderStyle.Medium, black );

            for ( int i = 0; i < groups.Count; i++ )
            {
                ws.Cells[ 0, groups[ i ], expandok.Count + 1, groups[ i ] ].SetBorders( border );
            }


            return workbook;

        }

    }
}
