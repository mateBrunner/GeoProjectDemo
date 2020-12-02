using System.Dynamic;
using System.Collections.Generic;

namespace GeoProjectDemo.Helpers
{
    public static class CopyHelper
    {

        public static ExpandoObject ShallowCopyExpando( ExpandoObject original )
        {
            var clone = new ExpandoObject( );

            var _original = (IDictionary<string, object>)original;
            var _clone = (IDictionary<string, object>)clone;

            foreach ( var kvp in _original )
                _clone.Add( kvp );

            return clone;
        }

        public static ExpandoObject DeepCopyExpando( ExpandoObject original )
        {
            var clone = new ExpandoObject( );

            var _original = (IDictionary<string, object>)original;
            var _clone = (IDictionary<string, object>)clone;

            foreach ( var kvp in _original )
                _clone.Add( kvp.Key, kvp.Value is ExpandoObject ? DeepCopyExpando( (ExpandoObject)kvp.Value ) : kvp.Value );

            return clone;
        }

    }
}
