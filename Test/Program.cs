using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var empService = new EmployeeService();

            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            var list = empService.GetAll();
            stopwatch1.Stop();
            Console.WriteLine("With Generic Time {0} :", stopwatch1.Elapsed);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = empService.GetDataTable();
            stopwatch.Stop();
            Console.WriteLine("Without Generic Time {0} :", stopwatch.Elapsed);



            Console.ReadKey();
        }
    }
}
