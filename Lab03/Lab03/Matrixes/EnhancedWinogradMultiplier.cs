using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab03.Matrixes
{
    class EnhancedWinogradMultiplier : MatrixMultiplier
    {
        private long _ticks;

        private IntPtr _buffer1 = Marshal.AllocHGlobal(4000);
        private IntPtr _buffer2 = Marshal.AllocHGlobal(4000);


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


        // Enhancement 1: replace x = x + y with x += y

        // Enhancement 2: replace 2 * (b / 2) with precalculated division
        // line int d = b / 2 uses division of b, we can use this value for two times

        
        // Enhancement 3: replace multiplication and subtraction of block-const values with precalculated values

        unsafe private Matrix Process(Matrix g, Matrix h)

        {
            int a = g.Rows;
            int b = g.Cols;
            int c = h.Cols;

            int d = b / 2;

            /*int[] rowFactors = _buffer1;//new int[a];
            int[] colFactors = _buffer2;//new int[c];*/

            IntPtr rowFactors = _buffer1;
            IntPtr colFactors = _buffer2;

            Matrix r = new Matrix(a, c);

            int barrier = d + 1;

            // Вычисление коэффициентов строк для первой матрицы
            for (int i = 0; i < a; i++)
            {
                //rowFactors[i] = g[i, 0] * g[i, 1];
                IntPtr arrPointerIndex = rowFactors + i;
                int* arrPointer = (int*) (arrPointerIndex.ToPointer());
                //Marshal.WriteInt32(arrPointerIndex, g[i, 0] * g[i, 1]);
                *arrPointer = g[i, 0] * g[i, 1];
                for (int j = 2; j < barrier; j++)
                {
                    int dj = 2 * j - 2;
                    //int value = Marshal.ReadInt32(arrPointerIndex) + g[i, dj] * g[i, dj + 1];
                    //Marshal.WriteInt32(arrPointerIndex, value);
                    *arrPointer += g[i, dj] * g[i, dj + 1];
                    //rowFactors[i] += g[i, dj] * g[i, dj + 1];
                }
            }

            // Вычисление коэффициентов столбцов для второй матрицы
            for (int i = 0; i < c; i++)
            {
                //colFactors[i] = h[0, i] * h[1, i];
                //IntPtr arrPointerIndex = rowFactors + i;
                //Marshal.WriteInt32(arrPointerIndex, h[0, i] * h[1, i]);
                IntPtr arrPointerIndex = colFactors + i;
                int* arrPointer = (int*)(arrPointerIndex.ToPointer());
                for (int j = 2; j < barrier; j++)
                {
                    int dj = 2 * j - 2;
                    /*int value = Marshal.ReadInt32(arrPointerIndex) + h[dj, i] * h[dj + 1, i];
                    Marshal.WriteInt32(arrPointerIndex, value);*/
                    *arrPointer += h[dj, i] * h[dj + 1, i];
                    //colFactors[i] += h[dj, i] * h[dj + 1, i];
                }
            }

            // Вычисление матрицы R
            
            for (int i = 0; i < a; i++)
            {
                //IntPtr arrI = rowFactors + i;
                int* arrI = (int*)(rowFactors + i).ToPointer();
                for (int j = 0; j < c; j++)
                {
                    //IntPtr arrJ = colFactors + j;
                    int* arrJ = (int*)(colFactors + j).ToPointer();
                    r[i, j] = -*arrI - *arrJ;
                    for (int k = 1; k < barrier; k++)
                    {
                        int dkSub1 = 2 * k;
                        dkSub1--;
                        int dkSub2 = dkSub1;
                        dkSub2--;
                        r[i, j] += (g[i, dkSub2] + h[dkSub1, j]) * (g[i, dkSub1] + h[dkSub2, j]);
                    }
                }
            }

            // Прибавление членов в случае нечетной общей размерности
            int bSub1 = b - 1;
            if (b - d != d)
            {
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
