using System;
using System.Collections.Generic;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
           IEnumerable<Employee> developers = new Employee[] {

                new Employee {Id=1,Name="John"},
                new Employee {Id=2,Name="Susan"}
            
            };

            IEnumerable<Employee> sales = new List<Employee>() 
            {
                new Employee{Id = 3, Name = "Alex" }
             
            };

            string strExt ="3.2";
            _ = strExt.ToDouble();

            IEnumerator<Employee> enumerator = developers.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }

            
        }
    }

    public static class StringExtensionMethod //public static class
    {
        static public double ToDouble(this string data)//static mathod and (this T argument) paramters.
        {
            double result = double.Parse(data);
            return result;
        }
    }
}
