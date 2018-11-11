using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab05.Matrixes;
using Lab05.Matrixes.MatrixesParallel;

namespace Lab05
{
    class Program
    {
        static bool TestMultipication(Matrix a, Matrix b, Matrix assertedResult)
        {
            Matrix correctResult = new SimpleMultiplier().Multiply(a, b);
            return correctResult.Equals(assertedResult);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Testing simple: ");
            TestSimpleParallel();


            Console.WriteLine("Testing winograd: ");
            TestWinogradParallel();

            Console.ReadKey();
        }

        static void TestSimpleParallel()
        {
            Matrix a = new Matrix(100, 100);
            Matrix.GenerateRandom(a, 0, 100 * 100);

            Matrix b = new Matrix(100, 100);
            Matrix.GenerateRandom(b, 0, 100 * 100);

            SimpleMultiplierParallel par = new SimpleMultiplierParallel();

            Console.WriteLine("TestSimpleParallel: Using " + Environment.ProcessorCount + " threads.");

            par.Process(Environment.ProcessorCount, a, b);

            bool ok = TestMultipication(a, b, par.Result);

            if (!ok)
            {
                Console.WriteLine("Calculations incorrect! \n" + par.Result.ToString());
            }
            else
            {
                Console.WriteLine("OK");
            }

        }

        static void TestWinogradParallel()
        {
            Matrix a = new Matrix(100, 100);
            Matrix.GenerateRandom(a, 0, 100 * 100);

            Matrix b = new Matrix(100, 100);
            Matrix.GenerateRandom(b, 0, 100 * 100);

            WinogradMultiplierParallel par = new WinogradMultiplierParallel();

            Console.WriteLine("TestSimpleParallel: Using " + Environment.ProcessorCount + " threads.");

            par.Process(Environment.ProcessorCount, a, b);

            bool ok = TestMultipication(a, b, par.Result);

            if (!ok)
            {
                Console.WriteLine("Calculations incorrect! \n" + par.Result.ToString());
            }
            else
            {
                Console.WriteLine("OK");
            }

        }
    }
}
