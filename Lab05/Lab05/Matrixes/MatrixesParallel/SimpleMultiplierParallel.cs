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

            Thread[] threads = new Thread[threadCount];

            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Task scheduling here
            int oneThreadRange = _lineCount / threadCount;
            int remainder = _lineCount % threadCount;
            List<JobPart> jobParts = new List<JobPart>(remainder);

            int last = 0;
            for (int i = 0; i < threadCount; i++)
            {
                int start = i * oneThreadRange;
                int end = start + oneThreadRange;
                JobPart jobPart = new JobPart(start, end);
                threads[i] = new Thread(Multiply);
                threads[i].Start(jobPart);
                last = end;
            }

            // Adding remainders
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
                        threads[i] = new Thread(Multiply);
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

            // end of scheduling

                timer.Stop();
            _ticks = timer.ElapsedTicks;
        }

        public Matrix Result => _result;
        public long Ticks => _ticks;

        private void Multiply(object jobPartObj)
        {
            JobPart jobPart = (JobPart)jobPartObj;
            int startRow = jobPart.Start;
            int endRow = jobPart.End;
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
