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
                           equals new { Manufacturer = manufacturer.Name, manufacturer.Year }
                           orderby car.Combined descending, car.Name ascending
                           select new
                           {
                               manufacturer.Headquarters,
                               car.Name,
                               car.Combined
                           };

            //join in extension method syntax
            var query2 =
                cars.Join(manufacturers, c => c.Manufacturer, m => m.Name, (c, m) => new  //for composit key c=> new {c.Manufacturer, c.Year}, m=> new {Manufaturere= m.Name, m.Year}
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




            //Grouping in query syntax
            var queryGroup = from car in cars
                             group car by car.Manufacturer.ToUpper() into m
                             orderby m.Key
                             select m;

            //Grouping in Method Expression
            var queryGroup2 = cars.GroupBy(c => c.Manufacturer.ToUpper())
                .OrderBy(g => g.Key);

            foreach (var group in queryGroup)
            {
                Console.WriteLine($"{group.Key}");
                foreach (var car in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : { car.Combined}");

                }
            }

            //Group Join in Query Syntax
            var queryGroupJoin =
                from manufacture in manufacturers
                join car in cars on manufacture.Name equals car.Manufacturer
                into carGroup
                select new
                {

                    Manufacturer = manufacture,
                    Cars = carGroup

                };

            var queryGroupJoin2 = manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) => new { Manufacturer = m, Car = g }).OrderBy(m => m.Manufacturer.Name);

            foreach (var group in queryGroupJoin)
            {
                Console.WriteLine($"{ group.Manufacturer.Name} : {group.Manufacturer.Headquarters}");

                foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name}:{car.Combined}");
                }
            }



            //Aggregation 

            var queryAgrr = from car in cars
                            group car by car.Manufacturer into carGroup
                            select new
                            {
                                Name = carGroup.Key,
                                Max = carGroup.Max(c => c.Combined),
                                Min = carGroup.Min(c => c.Combined),
                                Avg = carGroup.Average(c => c.Combined)

                            }
                            //ordering
                            into result
                            orderby result.Max descending
                            select result;
            // Aggregation Method Syntax //efficient
            var queryAggrEff = cars.GroupBy(c => c.Manufacturer)
               .Select(g =>
               {

                   var result = g.Aggregate(new CarStats(), (acc, c) => acc.Accumulate(c), acc => acc.Compute());
                   return new
                   {

                       Name = g.Key,
                       Avg = result.Average,
                       Max = result.Max,
                       Min = result.Min

                   };





               }).OrderByDescending(r=>r.Max);

            foreach (var result in queryAgrr)
            {
                Console.WriteLine($"{result.Name}: Max{result.Max}, Min{result.Min}, Average{result.Avg}");
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

    public class CarStats
    {
        public CarStats()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;

        }
        public CarStats Accumulate(Car car)
        {
            Count += 1;
            Total += car.Combined;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Max(Min, car.Combined);
          

            return this;
        }

        public CarStats Compute()
        {
            Average = Total / Count;
            return this;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
        public double Average { get; set; }

    }
}

