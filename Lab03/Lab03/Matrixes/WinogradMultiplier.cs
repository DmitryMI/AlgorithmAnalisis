using System;
using System.Diagnostics;

namespace Lab03.Matrixes
{
    class WinogradMultiplier : MatrixMultiplier
    {
        private long _ticks;

        public override long Ticks => _ticks;

        public override Matrix Multiply(Matrix a, Matrix b)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Matrix result = Process(a, b);
            timer.Stop();

            _ticks = timer.ElapsedTicks;

            return result;
        }

        private Matrix Process(Matrix g, Matrix h)
        {
            int a = g.Rows; // 1
            int b = g.Cols; // 1
            int c = h.Cols; // 1

            int d = b / 2; // чтение, деление, запись

            int[] rowFactors  = new int[a];
            int[] colFactors = new int[c];

            Matrix r = new Matrix(a, c);

            // Вычисление коэффициентов строк для первой матрицы
            for (int i = 0; i < a; i++) // 2
            {
                rowFactors[i] = g[i, 0] * g[i, 1]; // 4
                for (int j = 2; j <= d; j++)
                {
                    rowFactors[i] = rowFactors[i] + g[i, 2 * j - 2] * g[i, 2 * j - 1]; // 10
                }
            }

            // Вычисление коэффициентов столбцов для второй матрицы
            for (int i = 0; i < c; i++) // 2
            {
                colFactors[i] = h[0, i] * h[1, i]; // 4
                for (int j = 2; j <= d; j++) // 2
                {
                    colFactors[i] = colFactors[i] + h[2 * j - 2, i] * h[2 * j - 1, i]; // 10
                }
            }

            // Вычисление матрицы R
            for (int i = 0; i < a; i++) // 2
            {
                for (int j = 0; j < c; j++) // 2
                {
                    r[i, j] = -rowFactors[i] - colFactors[j]; // 5
                    for (int k = 1; k <= d ; k++) // 2
                    {
                        r[i, j] = r[i, j] +
                                  (g[i, 2 * k - 2] + h[2 * k - 1, j]) *
                                  (g[i, 2 * k - 1] + h[2 * k - 2, j]); // 14
                    }
                }
            }

            // Прибавление членов в случае нечетной общей размерности
            if (2 * (b / 2) != b)
            {
                for (int i = 0; i < a; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        r[i, j] = r[i, j] + g[i, b - 1] * h[b - 1, j];
                    }
                }
            }

            return r;
        }
    }
}
