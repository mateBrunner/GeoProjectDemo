using BaseClasses;
using GeoProjectDemo.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoProjectDemo.Pages
{
    public partial class Foglalasok
    {

        [Inject]
        FoglalasService m_Service { get; set; }

        List<ExpandoObject> Dolgozok { get; set; }
        List<Nap> NapList { get; set; }

        protected override async Task OnAfterRenderAsync( bool first )
        {
            if ( !first )
                return;

            var res = await m_Service.GetAdatok( );
            Dolgozok = res.Dolgozok;
            NapList = res.Napok;

            StateHasChanged( );

        }
        
    }
    
}
