using System;
using Lab03.Matrixes;

namespace Lab03
{
    class Program
    {
        static void Main(string[] args)
        {
            TestWinograd();

            Console.ReadKey();
        }

        static void FillSequentially(Matrix a, int initial, int shift)
        {
            int count = initial;
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Rows; j++)
                {
                    a[i, j] = count;
                    count += shift;
                }
            }
        }

        static void TestSimpleMethod()
        {
            Matrix first = new Matrix(2, 2);
            Matrix second = new Matrix(2, 2);

            Matrix correctResult = new Matrix(2, 2);

            correctResult[0, 0] = 1;
            correctResult[0, 1] = 0;
            correctResult[1, 0] = 9;
            correctResult[1, 1] = 4;

            first[0, 0] = 0;
            first[0, 1] = 1;
            first[1, 0] = 2;
            first[1, 1] = 3;

            second[0, 0] = 3;
            second[0, 1] = 2;
            second[1, 0] = 1;
            second[1, 1] = 0;

            Console.WriteLine("First matrix: ");
            Console.WriteLine(first.ToString());

            Console.WriteLine("\nSecond matrix: ");
            Console.WriteLine(second.ToString());


            MatrixMultiplier multiplier = MatrixMultiplier.GetMatrixMultiplier(MultiplicationMethod.Simple);

            if (multiplier != null)
            {
                Matrix result = multiplier.Multiply(first, second);
                Console.WriteLine("Result: ");
                Console.WriteLine(result);

                Console.WriteLine(correctResult.Equals(result) ? "CORRECT" : "WRONG RESULT!");
            }



            Console.WriteLine();
        }

        static void TestWinograd()
        {
            // Testing even size
            Matrix m1 = new Matrix(4, 4);
            Matrix m2 = new Matrix(4, 4);
            //Matrix.GenerateRandom(m1, 0, 16);
            //Matrix.GenerateRandom(m2, 0, 16);
            FillSequentially(m1, 1, 1);
            FillSequentially(m2, 16, -1);

            Matrix correctResult = new SimpleMultiplier().Multiply(m1, m2);

            Matrix testResult = new WinogradMultiplier().Multiply(m1, m2);

            Console.WriteLine("Testing even-sized matrixes");
            Console.WriteLine("\nFirst matrix: ");
            Console.WriteLine(m1);
            Console.WriteLine("\nSecond matrix: ");
            Console.WriteLine(m2);
            Console.WriteLine("\nEstimated result: ");
            Console.WriteLine(correctResult);
            Console.WriteLine("\nTest result: ");
            Console.WriteLine(testResult);
            Console.WriteLine(correctResult.Equals(testResult) ? "CORRECT" : "WRONG RESULT!");

            // Testing odd size
            m1 = new Matrix(3, 3);
            m2 = new Matrix(3, 3);
            //Matrix.GenerateRandom(m1, 0, 9);
            //Matrix.GenerateRandom(m2, 0, 9);
            FillSequentially(m1, 1, 1);
            FillSequentially(m2, 9, -1);

            correctResult = new SimpleMultiplier().Multiply(m1, m2);

            testResult = new WinogradMultiplier().Multiply(m1, m2);

            Console.WriteLine("\nTesting odd-sized matrixes");
            Console.WriteLine("\nFirst matrix: ");
            Console.WriteLine(m1);
            Console.WriteLine("\nSecond matrix: ");
            Console.WriteLine(m2);
            Console.WriteLine("\nEstimated result: ");
            Console.WriteLine(correctResult);
            Console.WriteLine("\nTest result: ");
            Console.WriteLine(testResult);
            Console.WriteLine(correctResult.Equals(testResult) ? "CORRECT" : "WRONG RESULT!");
        }
    }
}
