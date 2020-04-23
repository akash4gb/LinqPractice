using System;
using System.Collections.Generic;
using System.Text;

namespace FileProccessing
{
    class Manufacturer
    {
        public string Name { get; set; }
        public string Headquarters { get; set; }
        public int Year { get; set; }

       

        internal static Manufacturer ParseFromCsv(string line)
        {
            var values = line.Split(',');
            return new Manufacturer
            {
                Name = values[0],
                Headquarters = values[1],
                Year = Int32.Parse(values[2])

            };
        }
    }

}
