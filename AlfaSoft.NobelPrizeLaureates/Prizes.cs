using System;
using System.Collections.Generic;
using System.Text;

namespace AlfaSoft.NobelPrizeLaureates
{
    class Prizes
    {
        public string Name { get; set; }
        public DateTime Year { get; set; }

        public Prizes(string name, DateTime year)
        {
            Name = name;
            Year = year;
        }
    }
}
