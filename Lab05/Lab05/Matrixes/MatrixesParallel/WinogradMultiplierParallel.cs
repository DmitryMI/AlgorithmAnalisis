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
        private int[] _rowFactors, _colFactors;


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

            _lineCount = _result.Rows;

            _rowFactors = new int[_a];
            _colFactors = new int[_b];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            DoParallel(GetRowFactors);

            DoParallel(GetColFactors);

            DoParallel(MatrixCalculation);

            // Adding remainders
            if (_b % 2 == 1)
            {
                DoParallel(AddRemainders);
            }


            stopwatch.Stop();
            _ticks = stopwatch.ElapsedTicks;
        }

        private void DoParallel(ParameterizedThreadStart job)
        {
            Thread[] threads = new Thread[_totalThreads];

            int threadRange = _lineCount / _totalThreads;
            int remainder = _lineCount % _totalThreads;

            int last = 0;

            for (int i = 0; i < _totalThreads; i++)
            {
                threads[i] = new Thread(job);

                int start = i * threadRange;
                int end = start + threadRange;
                JobPart jobPart = new JobPart(start, end);
                threads[i].Start(jobPart);
                last = end;
            }

            // Adding remainders
            List<JobPart> jobParts = new List<JobPart>(remainder);
            for (int i = 0; i < remainder; i++)
            {
                jobParts.Add(new JobPart(last, last + 1));
                last++;
            }

            // Searching for finished threads
            while (jobParts.Count > 0)
            {
                for (int i = 0; i < threads.Length; i++)
                {
                    if (!threads[i].IsAlive)
                    {
                        //threads[i].Start(jobParts[0]);
                        threads[i] = new Thread(job);
                        threads[i].Start(jobParts[0]);
                        break;
                    }
                }
                jobParts.RemoveAt(0);
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
        }

        private void WaitForAll(IEnumerable<Thread> theads)
        {
            foreach (var thread in theads)
            {
                thread.Join();
            }
        }



        private void GetRowFactors(object jobPartObject)
        {
            JobPart part = (JobPart)jobPartObject;
            int startRow = part.Start;
            int endRow = part.End;

            for (int i = startRow; i < endRow; i++)
            {
                _rowFactors[i] = _g[i, 0] * _g[i, 1];
                for (int j = 2; j <= _d; j++)
                {
                    _rowFactors[i] = _rowFactors[i] + _g[i, 2 * j - 2] * _g[i, 2 * j - 1];
                }
            }
        }

        private void GetColFactors(object jobPartObject)
        {
            JobPart part = (JobPart)jobPartObject;
            int startRow = part.Start;
            int endRow = part.End;

            for (int i = startRow; i < endRow; i++)
            {
                _colFactors[i] = _h[0, i] * _h[1, i];
                for (int j = 2; j <= _d; j++)
                {
                    _colFactors[i] = _colFactors[i] + _h[2 * j - 2, i] * _h[2 * j - 1, i];
                }
            }
        }

        private void MatrixCalculation(object jobPartObject)
        {
            JobPart part = (JobPart)jobPartObject;
            int startRow = part.Start;
            int endRow = part.End;

            for (int i = startRow; i < endRow; i++)
            {
                for (int j = 0; j < _c; j++)
                {
                    _result[i, j] = -_rowFactors[i] - _colFactors[j];
                    for (int k = 1; k <= _d; k++)
                    {
                        _result[i, j] = _result[i, j] +
                                        (_g[i, 2 * k - 2] + _h[2 * k - 1, j]) *
                                        (_g[i, 2 * k - 1] + _h[2 * k - 2, j]);
                    }
                }
            }
        }

        private void AddRemainders(object jobPartObject)
        {
            JobPart part = (JobPart) jobPartObject;
            int startRow = part.Start;
            int endRow = part.End;

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