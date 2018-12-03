using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04.Sorting
{
    class MergeSorter : Sorter
    {

        protected override void Process(int[] colletion)
        {
            int[] b = new int[colletion.Length];
            TopDownMergeSort(colletion, b);
        }

        void TopDownMergeSort(int[] a, int[] b)
        {
            a.CopyTo(b, 0);
            TopDownSplitMerge(b, 0, a.Length, a);
        }

        void TopDownSplitMerge(int[] b, int iBegin, int iEnd, int[] a)
        {
            if(iEnd - iBegin < 2)
                return;

            int iMiddle = (iEnd + iBegin) / 2;
            TopDownSplitMerge(a, iBegin, iMiddle, b);
            TopDownSplitMerge(a, iMiddle, iEnd, b);

            TopDownMerge(b, iBegin, iMiddle, iEnd, a);
        }

        void TopDownMerge(int[] a, int iBegin, int iMiddle, int iEnd, int[] b)
        {
            int i = iBegin;
            int j = iMiddle;

            for (int k = iBegin; k < iEnd; k++)
            {
                if (i < iMiddle && (j >= iEnd || a[i] <= a[j]))
                {
                    b[k] = a[i];
                    i++;
                }
                else
                {
                    b[k] = a[j];
                    j++;
                }
            }
        }
    }
}
