using System;
using System.Collections.Generic;
using System.Linq;
using Features.CustomLinq;

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


            //manual foreach
            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }


            // using extension method linke linq
            Console.WriteLine(developers.CustomCount());

            //Named method 
            foreach (var employee in developers.Where(NameStartWithS))
            {
                Console.WriteLine(employee.Name);
            }

            //Anonymous Method
            foreach (var employee in developers.Where(
                delegate (Employee emp)
                {
                    return emp.Name.StartsWith("S");
                }))
            {
                Console.WriteLine(employee.Name);
            }

            //Lamda Expression
            foreach (var employee in developers.Where(e=>e.Name.StartsWith("S")))
            {
                Console.WriteLine(employee.Name);
            }

            //func type deligate with lamda, last param is return type
            Func<Double> pi = () => 3.1416; // use ()=> ,when no pram is passed
            Func<int, int> Square = x=>x*x; // use just param x=> , when only one param
            Func<int, int, int> Area = (x, y) =>
             {
                 var temp = x * y;
                 return temp;
             };                             //use ()=> , when two or more param and also can use method sbody{}

            Console.WriteLine(pi());
            Console.WriteLine(Square(3));
            Console.WriteLine(Area(3,8));


            //Action type deligate
            Action<string> Write = x => Console.WriteLine(x);
            Write("Action Executed");



        }

      

        private static bool NameStartWithS(Employee employee)
        {
            return employee.Name.StartsWith("S");
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
