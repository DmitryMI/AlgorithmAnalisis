using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04.Sorting
{
    abstract class Sorter
    {
        private long _ticks;
        public long LastOperationTicks => _ticks;
        public void Sort(int[] collection)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Process(collection);
            watch.Stop();
            _ticks = watch.ElapsedTicks;

            AssertSort(collection);
        }

        private void AssertSort(int[] collection)
        {
            for (int i = 1; i < collection.Length; i++)
            {
                if (collection[i] < collection[i - 1])
                    throw new InvalidRealizationException(collection, i);
            }
        }

        protected abstract void Process(int[] collection);

        public class InvalidRealizationException : Exception
        {
            private string _info;
            public string Info => _info;
            public InvalidRealizationException(int[] arr, int checkIndex)
            {
                string msg = "Array is not sorted!\n";
                if (arr.Length <= 20)
                {
                    // Print error pointer
                    for (int i = 0; i < +"Array: ".Length; i++)
                    {
                        msg += ' ';
                    }
                    
                    for (int i = 0; i < checkIndex; i++)
                    {
                        msg += ' ';
                        msg += ' ';
                    }
                    msg += "|\n";

                    msg += "Array: ";
                    for (int i = 0; i < arr.Length; i++)
                    {
                        msg += arr[i] + " ";
                    }
                }

                _info = msg;
            }
        }
    }
}
