using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab01.StringDistanceMeasure;

namespace Lab01
{
    class Program
    {
        private static Random _rnd = new Random();

        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "-t")
            {
                TestTime();
                Console.WriteLine("TESTING COMPLETED");
                Console.ReadKey();
                return;
            }

            if (args.Length > 0 && args[0] == "-rt")
            {
                TestTree();
                Console.WriteLine("TESTING COMPLETED");
                Console.ReadKey();
                return;
            }

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

        static char GetRandomLetter()
        {
            int distance = 'я' - 'a';
            int val = _rnd.Next(0, distance);
            return (char) ('a' + val);
        }

        static string GetRandomWord(int lenght)
        {
            string result = "";

            for (int i = 0; i < lenght; i++)
            {
                result += GetRandomLetter();
            }

            return result;
        }

        static void TestTime()
        {
            int repeatCount = 1000;
            int currentWordLength = 0;
            int finalWordLength = 16;

            for (; currentWordLength < finalWordLength; currentWordLength++)
            {
                Console.WriteLine("Word length: " + currentWordLength);

                foreach (StringDistance.Measure method in Enum.GetValues(typeof(StringDistance.Measure)))
                {
                    string a = GetRandomWord(currentWordLength);
                    string b = GetRandomWord(currentWordLength);

                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    for (int i = 0; i < repeatCount; i++)
                    {
                        StringDistance measureMethod = StringDistance.StringDistanceBuilder.GetInstance(method, a, b);
                        int result = measureMethod.GetDistance();
                    }
                    watch.Stop();

                    long ticks = watch.ElapsedTicks;
                    double avgTicks = (double)ticks / repeatCount;
                    Console.WriteLine("Method: " + method.ToString() + "; Ticks average: " + avgTicks);
                }

                Console.WriteLine();
            }
        }

        static void TestTree()
        {
            LevenshteinRecursiveTest test = new LevenshteinRecursiveTest("Дверь", "Двор");
            test.GetDistance();
        }
    }
}
