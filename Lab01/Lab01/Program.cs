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
                
                int result = distance.GetDistance();
                LetterMatrix matrix = distance.GetLetterMatrix();
                Console.WriteLine("Метод: " + distance.MethodName);
                Console.WriteLine("Значение: " + result);
                Console.WriteLine("Матрица: \n");
                Console.Write(matrix.ToString());
                Console.WriteLine("\n");
            }

            Console.ReadKey();
        }
    }
}
