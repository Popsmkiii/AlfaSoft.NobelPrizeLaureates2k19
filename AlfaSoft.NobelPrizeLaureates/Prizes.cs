﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AlfaSoft.NobelPrizeLaureates
{
    class Prizes
    {
        public string Name { get; set; }
        public int Year { get; set; }

        public Prizes(string name, int year)
        {
            Name = name;
            Year = year;
        }
    }
}
