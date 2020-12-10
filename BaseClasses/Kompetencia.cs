
namespace BaseClasses
{
    public class Kompetencia
    {

        public long Azonosito { get; set; }
        public string PropertyNev { get; set; }
        public string Nev { get; set; }
        public long? KategoriaAzonosito { get; set; }
        public long Szint { get; set; }
        public string SzintNev { get; set; }
        public bool IsFirstInGroup { get; set; } = false;
        public bool IsVisible { get; set; } = true;


    }
}
