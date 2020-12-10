using System.Collections.Generic;
using System.Dynamic;

namespace BaseClasses
{
    public class FoglalasAdatok
    {
        public List<ExpandoObject> Dolgozok { get; set; } = new List<ExpandoObject>( );
        public List<Nap> Napok { get; set; } = new List<Nap>( );
    }
}
