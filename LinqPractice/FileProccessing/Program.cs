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

            var query = cars.OrderByDescending(c => c.Combined).ThenBy(c=>c.Name) ;

            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
            }
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
