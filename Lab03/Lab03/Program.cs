using System;
using Lab03.Matrixes;

namespace Lab03
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestEnhancedWinograd();
            Analize();

            Console.ReadKey();
        }

        static void Analize()
        {
            // Test multiplication with sizes from 100 to 1000 with step 100

            Array methods = Enum.GetValues(typeof(MultiplicationMethod));

            for (int i = methods.Length - 1; i >= 0; i--)
            //for (int i = 0; i < methods.Length; i++)
            {
                MultiplicationMethod method = (MultiplicationMethod)methods.GetValue(i);

                Console.WriteLine("\nMethod: " + method);

                MatrixMultiplier multiplier = MatrixMultiplier.GetMatrixMultiplier(method);

                for (int size = 100; size <= 1000; size += 100)
                {
                    Matrix left = new Matrix(size, size);
                    Matrix right = new Matrix(size, size);
                    Matrix.GenerateRandom(left, 0, size);
                    Matrix.GenerateRandom(right, 0, size);

                    long ticksAvg = Test100Times(multiplier, left, right);

                    Console.WriteLine("Size: " + size + ". Ticks: " + ticksAvg);
                }
            }
        }

        static long Test100Times(MatrixMultiplier multiplier, Matrix a, Matrix b)
        {
            long ticks = 0;

            for (int i = 0; i < 100; i++)
            {
                multiplier.Multiply(a, b);
                ticks += multiplier.Ticks;
            }

            return ticks / 100;
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
            Matrix.GenerateRandom(m1, 0, 16);
            Matrix.GenerateRandom(m2, 0, 16);
            //FillSequentially(m1, 1, 1);
            //FillSequentially(m2, 16, -1);

            Matrix correctResult = new SimpleMultiplier().Multiply(m1, m2);

            Matrix testResult = new WinogradMultiplier().Multiply(m1, m2);

            Console.WriteLine("Testing even-sized matrixes");
            Console.WriteLine("\nFirst matrix: ");
            Console.WriteLine(m1);
            Console.WriteLine("\nSecond matrix: ");
            Console.WriteLine(m2);
            
            Console.WriteLine("\nTest result: ");
            Console.WriteLine(testResult);

            bool correct = correctResult.Equals(testResult);
            Console.WriteLine(correct ? "CORRECT" : "WRONG RESULT!");
            if (!correct)
            {
                Console.WriteLine("\nEstimated result: ");
                Console.WriteLine(correctResult);
            }

            // Testing odd size
            m1 = new Matrix(3, 3);
            m2 = new Matrix(3, 3);
            Matrix.GenerateRandom(m1, 0, 9);
            Matrix.GenerateRandom(m2, 0, 9);
            //FillSequentially(m1, 1, 1);
            //FillSequentially(m2, 9, -1);

            correctResult = new SimpleMultiplier().Multiply(m1, m2);

            testResult = new WinogradMultiplier().Multiply(m1, m2);

            Console.WriteLine("\nTesting odd-sized matrixes");
            Console.WriteLine("\nFirst matrix: ");
            Console.WriteLine(m1);
            Console.WriteLine("\nSecond matrix: ");
            Console.WriteLine(m2);
            Console.WriteLine("\nTest result: ");
            Console.WriteLine(testResult);

            correct = correctResult.Equals(testResult);
            Console.WriteLine(correct ? "CORRECT" : "WRONG RESULT!");
            if (!correct)
            {
                Console.WriteLine("\nEstimated result: ");
                Console.WriteLine(correctResult);
            }

        }

        static void TestEnhancedWinograd()
        {
            // Testing even size
            Matrix m1 = new Matrix(4, 4);
            Matrix m2 = new Matrix(4, 4);
            Matrix.GenerateRandom(m1, 0, 16);
            Matrix.GenerateRandom(m2, 0, 16);
            //FillSequentially(m1, 1, 1);
            //FillSequentially(m2, 16, -1);

            Matrix correctResult = new SimpleMultiplier().Multiply(m1, m2);

            Matrix testResult = new EnhancedWinogradMultiplier().Multiply(m1, m2);

            Console.WriteLine("Testing even-sized matrixes");
            Console.WriteLine("\nFirst matrix: ");
            Console.WriteLine(m1);
            Console.WriteLine("\nSecond matrix: ");
            Console.WriteLine(m2);

            Console.WriteLine("\nTest result: ");
            Console.WriteLine(testResult);

            bool correct = correctResult.Equals(testResult);
            Console.WriteLine(correct ? "CORRECT" : "WRONG RESULT!");
            if (!correct)
            {
                Console.WriteLine("\nEstimated result: ");
                Console.WriteLine(correctResult);
            }

            // Testing odd size
            m1 = new Matrix(3, 3);
            m2 = new Matrix(3, 3);
            Matrix.GenerateRandom(m1, 0, 9);
            Matrix.GenerateRandom(m2, 0, 9);
            //FillSequentially(m1, 1, 1);
            //FillSequentially(m2, 9, -1);

            correctResult = new SimpleMultiplier().Multiply(m1, m2);

            EnhancedWinogradMultiplier enhancedWinogradMultiplier = new EnhancedWinogradMultiplier();
            enhancedWinogradMultiplier.SetBufferSize(3);

            testResult = enhancedWinogradMultiplier.Multiply(m1, m2);

            Console.WriteLine("\nTesting odd-sized matrixes");
            Console.WriteLine("\nFirst matrix: ");
            Console.WriteLine(m1);
            Console.WriteLine("\nSecond matrix: ");
            Console.WriteLine(m2);
            Console.WriteLine("\nTest result: ");
            Console.WriteLine(testResult);

            correct = correctResult.Equals(testResult);
            Console.WriteLine(correct ? "CORRECT" : "WRONG RESULT!");
            if (!correct)
            {
                Console.WriteLine("\nEstimated result: ");
                Console.WriteLine(correctResult);
            }

        }
    }
}
