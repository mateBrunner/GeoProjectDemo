using System.Collections.Generic;
using System.Dynamic;

namespace BaseClasses
{
    public class KompetenciaAdatok
    {

        public List<ExpandoObject> Dolgozok { get; set; } = new List<ExpandoObject>( );
        public List<Kompetencia> Kompetenciak { get; set; } = new List<Kompetencia>( );
        public List<string> Szintek { get; set; } = new List<string>( );
        public List<KompetenciaKategoria> Kategoriak { get; set; } = new List<KompetenciaKategoria>( );

    }
}
