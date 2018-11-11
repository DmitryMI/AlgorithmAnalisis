using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab05.Matrixes.MatrixesParallel
{
    class WinogradMultiplierParallel
    {
        private long _ticks;
        private Matrix _result;


        private int _totalThreads;

        private int _lineCount;

        private Matrix _g, _h;
        private int _a, _b, _c, _d;




        // Part one values
        private int[] rowFactors, colFactors;


        public Matrix Result => _result;
        public long Ticks => _ticks;


        public void Process(int threadCount, Matrix a, Matrix b)
        {
            _totalThreads = threadCount;
            _g = a;
            _h = b;
            _a = _g.Rows;
            _b = _g.Cols;
            _c = _h.Cols;
            _d = _b / 2;

            _result = new Matrix(_a, _c);

            rowFactors = new int[_a];
            colFactors = new int[_b];

            List<Thread> threadPool = new List<Thread>();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            // Processing row factors

            for (int i = 0; i < _totalThreads; i++)
            {
                Thread thread = new Thread(GetRowFactors);
                thread.Start(i);
                threadPool.Add(thread);
            }

            WaitForAll(threadPool);
            threadPool.Clear();


            // Processing col factors

            for (int i = 0; i < _totalThreads; i++)
            {
                Thread thread = new Thread(GetColFactors);
                thread.Start(i);
                threadPool.Add(thread);
            }

            WaitForAll(threadPool);
            threadPool.Clear();


            // Calculating matrix
            for (int i = 0; i < _totalThreads; i++)
            {
                Thread thread = new Thread(MatrixCalculation);
                thread.Start(i);
                threadPool.Add(thread);
            }

            WaitForAll(threadPool);
            threadPool.Clear();

            // Adding remainders
            if (_b % 2 == 1)
            {
                for (int i = 0; i < _totalThreads; i++)
                {
                    Thread thread = new Thread(AddRemainders);
                    thread.Start(i);
                    threadPool.Add(thread);
                }

                WaitForAll(threadPool);
                threadPool.Clear();
            }


            stopwatch.Stop();
            _ticks = stopwatch.ElapsedTicks;
        }

        private void WaitForAll(IEnumerable<Thread> theads)
        {
            foreach (var thread in theads)
            {
                thread.Join();
            }
        }



        private void GetRowFactors(object threadNumObj)
        {
            int threadNum = (int) threadNumObj;

            int range = (int)Math.Ceiling((double)_a / _totalThreads);

            int startRow = threadNum * range;
            int endRow = startRow + range;
            if (endRow > _a)
                endRow = _a;

            for (int i = startRow; i < endRow; i++)
            {
                rowFactors[i] = _g[i, 0] * _g[i, 1];
                for (int j = 2; j <= _d; j++)
                {
                    rowFactors[i] = rowFactors[i] + _g[i, 2 * j - 2] * _g[i, 2 * j - 1];
                }
            }
        }

        private void GetColFactors(object threadNumObj)
        {
            int threadNum = (int) threadNumObj;

            int range = (int)Math.Ceiling((double)_c / _totalThreads);

            int startRow = threadNum * range;
            int endRow = startRow + range;
            if (endRow > _c)
                endRow = _c;

            for (int i = startRow; i < endRow; i++)
            {
                colFactors[i] = _h[0, i] * _h[1, i];
                for (int j = 2; j <= _d; j++)
                {
                    colFactors[i] = colFactors[i] + _h[2 * j - 2, i] * _h[2 * j - 1, i];
                }
            }
        }

        private void MatrixCalculation(object threadNumObj)
        {
            int threadNum = (int) threadNumObj;

            int range = (int)Math.Ceiling((double)_a / _totalThreads);

            int startRow = threadNum * range;
            int endRow = startRow + range;
            if (endRow > _a)
                endRow = _a;

            for (int i = startRow; i < endRow; i++)
            {
                for (int j = 0; j < _c; j++)
                {
                    _result[i, j] = -rowFactors[i] - colFactors[j];
                    for (int k = 1; k <= _d; k++)
                    {
                        _result[i, j] = _result[i, j] +
                                        (_g[i, 2 * k - 2] + _h[2 * k - 1, j]) *
                                        (_g[i, 2 * k - 1] + _h[2 * k - 2, j]);
                    }
                }
            }
        }

        private void AddRemainders(object threadNumObj)
        {
            int threadNum = (int) threadNumObj;

            int range = (int)Math.Ceiling((double)_a / _totalThreads);

            int startRow = threadNum * range;
            int endRow = startRow + range;
            if (endRow > _a)
                endRow = _a;

            for (int i = startRow; i < endRow; i++)
            {
                for (int j = 0; j < _c; j++)
                {
                    _result[i, j] = _result[i, j] + _g[i, _b - 1] * _h[_b - 1, j];
                }
            }
        }
    }
}