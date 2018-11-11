using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab05.Matrixes;
using Lab05.Matrixes.MatrixesParallel;

namespace Lab05
{
    class Program
    {
        private static List<string> _logBuffer = new List<string>();

        static bool TestMultipication(Matrix a, Matrix b, Matrix assertedResult)
        {
            Matrix correctResult = new SimpleMultiplier().Multiply(a, b);
            return correctResult.Equals(assertedResult);
        }

        static void Main(string[] args)
        {
            //ExperimentSimple();
            ExperimentWinograd();

            Console.WriteLine("Programm finished...");
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

        static void ExperimentSimple()
        {
            int matrixSize = 600;

            int processorCount = Environment.ProcessorCount;

            int thresold = (int)Math.Pow(2, processorCount + 2);
            int currentThreadCount = 1;

            Matrix a = new Matrix(matrixSize, matrixSize);
            Matrix.GenerateRandom(a, 0, matrixSize * matrixSize);

            Matrix b = new Matrix(matrixSize, matrixSize);
            Matrix.GenerateRandom(b, 0, matrixSize * matrixSize);

            SimpleMultiplierParallel parallel = new SimpleMultiplierParallel();

            while (currentThreadCount <= thresold)
            {
                parallel.Process(currentThreadCount, a, b);
                WriteInfo(currentThreadCount, parallel.Ticks);
                currentThreadCount *= 2;
            }

            File.WriteAllLines("simple.txt", _logBuffer);
        }

        static void ExperimentWinograd()
        {
            int matrixSize = 600;

            int processorCount = Environment.ProcessorCount;

            int thresold = (int)Math.Pow(2, processorCount + 2);
            int currentThreadCount = 1;

            Matrix a = new Matrix(matrixSize, matrixSize);
            Matrix.GenerateRandom(a, 0, matrixSize * matrixSize);

            Matrix b = new Matrix(matrixSize, matrixSize);
            Matrix.GenerateRandom(b, 0, matrixSize * matrixSize);

            WinogradMultiplierParallel parallel = new WinogradMultiplierParallel();

            while (currentThreadCount <= thresold)
            {
                parallel.Process(currentThreadCount, a, b);
                WriteInfo(currentThreadCount, parallel.Ticks);
                currentThreadCount *= 2;
            }

            File.WriteAllLines("winograd.txt", _logBuffer);
        }

        static void WriteInfo(int threads, long ticks)
        {
            _logBuffer.Add(threads.ToString() + " " + ticks + " ");
            Console.WriteLine("Thread count: " + threads + ", ticks: " + ticks);
        }
    }
}
