using System;

namespace BaseClasses
{
    public class Nap
    {

        public Nap( DateTime date )
        {
            Date = new DateTime( date.Year, date.Month, date.Day );
            IsMunkanap = date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
            IsFirstInGroup = date.Day == 1 || date.Day == 15;
        }

        public DateTime Date { get; set; }
        public bool IsMunkanap { get; set; }
        public bool IsFirstInGroup { get; set; }

        public string PropertyNev
        {
            get
            {
                return $"date_{Date.Year}_{Date.Month}_{Date.Day}";
            }
        }
    }
}
