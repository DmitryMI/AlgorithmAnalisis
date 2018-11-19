using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab03.Matrixes
{
    class EnhancedWinogradMultiplier : MatrixMultiplier
    {
        private long _ticks;

        private int[] _buffer1, _buffer2;
        private int[,] _matrixBuffer;

        public override long Ticks => _ticks;

        public override Matrix Multiply(Matrix a, Matrix b)
        {
            if (_matrixBuffer == null)
                _matrixBuffer = new int[a.Cols, b.Rows];


            if (a.Cols != _matrixBuffer.GetLength(0) || b.Rows != _matrixBuffer.GetLength(1))
                _matrixBuffer = new int[a.Cols, b.Rows];

            Stopwatch timer = new Stopwatch();
            timer.Start();
            Matrix result = Process(a, b);
            timer.Stop();

            _ticks = timer.ElapsedTicks;

            return result;
        }

        public void SetBufferSize(int matrixSize)
        {
            _buffer1 = new int[matrixSize];
            _buffer2 = new int[matrixSize];
        }

        private void ClearBuffers()
        {
            for (int i = 0; i < _buffer1.Length; i++)
            {
                _buffer1[i] = 0;
            }
            for (int i = 0; i < _buffer2.Length; i++)
            {
                _buffer2[i] = 0;
            }
        }


        // Enhancement 1: replace x = x + y with x += y

        // Enhancement 2: replace 2 * (b / 2) with precalculated division
        // line int d = b / 2 uses division of b, we can use this value for two times

        // Enhancement 3: replace multiplication and subtraction of block-const values with precalculated values
        private Matrix Process(Matrix g, Matrix h)
        {
            int a = g.Rows;
            int b = g.Cols;
            int c = h.Cols;

            int d = b / 2;
            int remainderPart = b - d;

            int[] rowFactors, colFactors;

            if (_buffer1 != null && _buffer1.Length >= a)
            {
                rowFactors = _buffer1;
            }
            else
            {
                rowFactors = new int[a];
                _buffer1 = rowFactors;
            }

            if (_buffer2 != null && _buffer2.Length >= c)
                colFactors = _buffer2;
            else
            {
                colFactors = new int[c];
                _buffer2 = colFactors;
            }

            //ClearBuffers();

            Matrix r = new Matrix(a, c, _matrixBuffer);

            // Вычисление коэффициентов строк для первой матрицы
            for (int i = 0; i < a; i++)
            {
                rowFactors[i] = g[i, 0] * g[i, 1];
                for (int j = 2; j <= d; j++)
                {
                    int dj = 2 * j;
                    rowFactors[i] += g[i, dj - 2] * g[i, dj - 1];
                }
            }

            // Вычисление коэффициентов столбцов для второй матрицы
            for (int i = 0; i < c; i++)
            {
                colFactors[i] = h[0, i] * h[1, i];
                for (int j = 2; j <= d; j++)
                {
                    int dj = 2 * j;
                    colFactors[i] += h[dj - 2, i] * h[dj - 1, i];
                }
            }

            // Вычисление матрицы R
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    r[i, j] = -rowFactors[i] - colFactors[j];
                    for (int k = 1; k <= d; k++)
                    {
                        int dk = 2 * k;
                        int dkSub1 = dk - 1;
                        int dkSub2 = dk - 2;
                        r[i, j] += (g[i, dkSub2] + h[dkSub1, j]) *
                                   (g[i, dkSub1] + h[dkSub2, j]);
                    }
                }
            }

            // Прибавление членов в случае нечетной общей размерности
            if (remainderPart != d)
            {
                int bSub1 = b - 1;
                for (int i = 0; i < a; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        r[i, j] += g[i, bSub1] * h[bSub1, j];
                    }
                }
            }

            return r;
        }
    }
}
