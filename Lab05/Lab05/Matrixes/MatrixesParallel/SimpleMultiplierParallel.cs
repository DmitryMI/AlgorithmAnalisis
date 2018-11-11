using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab05.Matrixes.MatrixesParallel
{
    class SimpleMultiplierParallel
    {
        private Matrix _result;
        private int _totalThreads;
        private int _lineCount;
        private int _oneThreadRange;
        private Matrix _a, _b;

        private long _ticks;

        public void Process(int threadCount, Matrix a, Matrix b)
        {
            _totalThreads = threadCount;
            _a = a;
            _b = b;

            _result = new Matrix(a.Cols, b.Rows);
            _lineCount = _result.Rows;
            _oneThreadRange = (int)Math.Ceiling((double)_lineCount / _totalThreads);

            Thread[] threads = new Thread[threadCount];

            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(Multiply));
                thread.Start(i);
                threads[i] = thread;
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }

            timer.Stop();
            _ticks = timer.ElapsedTicks;
        }

        public Matrix Result => _result;
        public long Ticks => _ticks;

        private void Multiply(object threadNumObj)
        {
            int threadNum = (int) threadNumObj;
            int startRow = threadNum * _oneThreadRange;
            int endRow = startRow + _oneThreadRange;
            if (endRow > _lineCount)
                endRow = _lineCount;

            for (int i = startRow; i < endRow; i++)
            {
                for (int j = 0; j < _result.Cols; j++)
                {
                    _result[i, j] = 0;
                    for (int k = 0; k < _a.Rows; k++)
                    {
                        _result[i, j] = _result[i, j] + _a[i, k] * _b[k, j];
                    }
                }
            }
        }
    }
}
