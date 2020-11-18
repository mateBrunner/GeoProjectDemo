using GeoProjectDemo.Models;
using System.Collections.Generic;
using System.Dynamic;

namespace GeoProjectDemo.Data
{
    public class KompetenciaAdatok
    {

        public List<ExpandoObject> Dolgozok { get; set; } = new List<ExpandoObject>( );
        public Dictionary<string, Kompetencia> KompetenciaByProp { get; set; } = new Dictionary<string, Kompetencia>( );
        public List<string> Szintek { get; set; } = new List<string>( );

    }
}
