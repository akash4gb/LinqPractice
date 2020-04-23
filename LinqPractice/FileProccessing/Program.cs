using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileProccessing
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProccessFile("Fuel.csv");
            var manufacturers = ProccessManufacturer("manufacturers.csv");

            //var query = cars.OrderByDescending(c => c.Combined).ThenBy(c=>c.Name) ;


            //join in query syntax
            var query1 = from car in cars
                         join manufacturer in manufacturers on car.Manufacturer equals manufacturer.Name
                         orderby car.Combined descending, car.Name ascending
                         select new
                         {
                             manufacturer.Headquarters,
                             car.Name,
                             car.Combined
                         };
            
            //join with multiple key
            var query1v2 = from car in cars
                         join manufacturer in manufacturers
                         on new { car.Manufacturer, car.Year } 
                         equals new { Manufacturer = manufacturer.Name, manufacturer.Year}
                         orderby car.Combined descending, car.Name ascending
                         select new
                         {
                             manufacturer.Headquarters,
                             car.Name,
                             car.Combined
                         };

            //join in extension method syntax
            var query2 =
                cars.Join(manufacturers, c => c.Manufacturer, m => m.Name, (c, m) => new
                {
                    //c.Name,
                    //c.Combined,
                    //m.Headquarters,
                    Car = c,
                    Manufacturer = m

                }).OrderByDescending(c => c.Car.Combined).ThenBy(c => c.Car.Name).Select(
                    c => new
                    {
                        c.Car.Name,
                        c.Car.Combined,
                        c.Manufacturer.Headquarters

                    }
                    );



            foreach (var car in query2.Take(10))
            {
                Console.WriteLine($"{car.Headquarters} {car.Name} : {car.Combined}");
            }
        }

        private static List<Manufacturer> ProccessManufacturer(string path)
        {
            var query = File.ReadAllLines(path)
                 .Skip(1)
                 .Where(l => l.Length > 0)
                 .Select(selector: Manufacturer.ParseFromCsv)
                 ;
            return query.ToList();
        }

        private static List<Car> ProccessFile(string path)
        {

            var query = from line in File.ReadAllLines(path).Skip(1)
                        where line.Length > 1
                        select Car.ParseFromCsv(line);

            return query.ToList();
            //File.ReadAllLines(path)
            //    .Skip(1)
            //    .Where(line => line.Length > 1)
            //    .Select(selector: Car.ParseFromCsv)
            //    .ToList();

        }


    }
}
