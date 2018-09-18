using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab01.StringDistanceMeasure;

namespace Lab01
{
    class Program
    {
        static void Main(string[] args)
        {
            string a, b;

            Console.Write("Первая строка: ");
            a = Console.ReadLine();
            Console.Write("Вторая строка: ");
            b = Console.ReadLine();

            Console.WriteLine("\nРезультат: \n");

            foreach (StringDistance.Measure measure in Enum.GetValues(typeof(StringDistance.Measure)))
            {
                StringDistance distance = StringDistance.StringDistanceBuilder.GetInstance(measure, a, b);

                if(distance == null)
                    continue;

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();


                GC.Collect();

                stopwatch.Start();

                int result = distance.GetDistance();

                stopwatch.Stop();

                LetterMatrix matrix = distance.GetLetterMatrix();
                Console.WriteLine("Метод: " + distance.MethodName);
                Console.WriteLine("Значение: " + result);
                Console.WriteLine("Матрица: ");
                Console.Write(matrix.ToString());
                
                Console.WriteLine("Прошло времени (тиков): " + stopwatch.ElapsedTicks);
                Console.WriteLine("Прошло времени (секунд): " + stopwatch.ElapsedMilliseconds / 1000f);
                Console.WriteLine("\n");
            }

            Console.ReadKey();
        }
    }
}
