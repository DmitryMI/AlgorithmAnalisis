using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04.Sorting
{
    class RadixSorter : Sorter
    {
        protected override void Process(int[] collection)
        {
            RadixSort(collection);
        }

        int GetMax(int[] a)
        {
            int max = a[0];
            for(int i = 0; i < a.Length; i++)
                if (a[i] > max)
                    max = a[i];
            return max;
        }

        private void RadixSort(int[] arr)
        {
            int i = 0, digitPlace = 1;
            int[] result = new int[arr.Length];
            int largestNum = GetMax(arr);

            while (largestNum / digitPlace > 0)
            {
                int[] count = new int[10];

                for (i = 0; i < arr.Length; i++)
                    count[(arr[i] / digitPlace) % 10]++;

                for (i = 1; i < 10; i++)
                    count[i] += count[i - 1];

                for (i = arr.Length - 1; i >= 0; i--)
                {
                    result[count[(arr[i] / digitPlace) % 10] - 1] = arr[i];
                    count[(arr[i] / digitPlace) % 10]--;
                }

                for (i = 0; i < arr.Length; i++)
                    arr[i] = result[i];
                
                digitPlace *= 10;
            }
        }
    }
}
