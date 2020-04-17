using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = new List < Movie >{
                new Movie { Title = "A Beautiful Mind", Rating = 8.9f, Year = 2002 },
                new Movie { Title = "Haksow Ridge", Rating = 8.0f, Year = 2010 },
                new Movie { Title = "Planet Apps", Rating = 7.9f, Year = 2008 },
                new Movie { Title = "Jurassic Park", Rating = 8.5f, Year = 2018 },
            };

            var query = movies.Filter(m => m.Year >= 2010);
            foreach (var movie in query)
            {
                Console.WriteLine(movie.Title);
            }

        }
    }
}
