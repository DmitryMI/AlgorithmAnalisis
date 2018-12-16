using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConveyerLab.ConveyerProcessing
{
    class Conveyer : IDisposable
    {
        public delegate object Job(object previous);
        private readonly Queue<object>[] _dataQueues;
        private bool _stop;
        private readonly Queue<object> _outputObjects = new Queue<object>();

        struct ThreadJobArgument
        {
            public ThreadJobArgument(int index, Job job)
            {
                JobIndex = index;
                Job = job;
            }
            public int JobIndex;
            public Job Job;
        }

        private void ThreadJob(object argument)
        {
            ThreadJobArgument arg = (ThreadJobArgument) argument;
            int jobIndex = arg.JobIndex;
            Job job = arg.Job;

            while (!_stop)
            {
                object nextData;
                lock (_dataQueues[jobIndex])
                {
                    if(_dataQueues[jobIndex].Count != 0)
                        nextData = _dataQueues[jobIndex].Dequeue();
                    else
                        nextData = null;
                }

                if(nextData == null)
                    continue;

                object result = job(nextData);

                lock (_dataQueues[jobIndex + 1])
                {
                    _dataQueues[jobIndex + 1].Enqueue(result);
                }
            }

            Debug.WriteLine("Thread " + jobIndex + " stopped");
        }

        public void StopConveyer()
        {
            _stop = true;
        }

        public Conveyer(Job[] jobs)
        {
            //var threads = new Thread[jobs.Length];
            _dataQueues = new Queue<object>[jobs.Length + 1];

            for (int i = 0; i < jobs.Length; i++)
            {
                Thread thread = new Thread(ThreadJob);
                thread.Start(new ThreadJobArgument(i, jobs[i]));
                _dataQueues[i] = new Queue<object>();
            }

            _dataQueues[jobs.Length] = _outputObjects;
        }

        public void WriteNext(object nextData)
        {
            lock (_dataQueues[0])
            {
                _dataQueues[0].Enqueue(nextData);
            }
        }

        public bool HasNextResult()
        {
            bool flag;
            lock (_outputObjects)
            {
                flag = _outputObjects.Count != 0;
            }
            return flag;
        }

        public object ReadNextResult()
        {
            object result;
            lock (_outputObjects)
            {
                result = _outputObjects.Dequeue();
            }

            return result;
        }

        public void Dispose()
        {
            StopConveyer();
        }
    }
}
