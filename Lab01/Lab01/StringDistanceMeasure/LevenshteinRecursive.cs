using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab01.StringDistanceMeasure
{
    unsafe class LevenshteinRecursive : StringDistance, IDisposable
    {
        private string _wordA, _wordB;
        private int _result;
        private LetterMatrix _matrix;

        private bool _calculated = false;

        private int* _tableIntPtr;

        public LevenshteinRecursive(string a, string b)
        {
            _wordA = a;
            _wordB = b;
            _matrix = new LetterMatrix(a, b);

            int arrLength = (a.Length + 1) * (b.Length + 1);

            var tablePtr = Marshal.AllocHGlobal(arrLength * sizeof(int));            

            _tableIntPtr = (int*)(tablePtr.ToPointer());

            for (int i = 0; i < arrLength; i++)
            {
                *(_tableIntPtr + i) = 0;
            }
        }

        public sealed override int GetDistance()
        {
            if (!_calculated)
            {
                _result = D_ptr(_wordA.Length, _wordB.Length);
                _calculated = true;
            }
            return _result;
        }

        public override LetterMatrix GetLetterMatrix()
        {
            if (!_calculated)
            {
                _result = D_ptr(_wordA.Length, _wordB.Length);
                _calculated = true;
            }

            for (int i = 0; i < _wordA.Length + 1; i++)
            {
                for (int j = 0; j < _wordB.Length + 1; j++)
                {
                    int index = _wordB.Length * i + j;
                    int value = *(_tableIntPtr + index);
                    _matrix[i, j] = value;
                }
            }

            return _matrix;
        }

        private int D_ptr(int i, int j)
        {
            if (i == 0 && j == 0)
            {
                int* ptr = _tableIntPtr + (i * _wordB.Length) + j;
                *ptr = 0;
                return 0;
            }
            if (j == 0 && i > 0)
            {
                int* ptr = _tableIntPtr + (i * _wordB.Length) + j;
                *ptr = i;
                return i;
            }
            if (i == 0 && j > 0)
            {
                int* ptr = _tableIntPtr + (i * _wordB.Length) + j;
                *ptr = j;
                return j;
            }

            int I = D_ptr(i, j - 1) + 1;
            int R = D_ptr(i - 1, j) + 1;
            int M = D_ptr(i - 1, j - 1) + Match(_wordA[i - 1], _wordB[j - 1]);
            int min = Min(I, R, M);

            int* ptr0 = _tableIntPtr + (i * _wordB.Length) + j;
            *ptr0 = min;

            return min;
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(new IntPtr(_tableIntPtr));
        }

        public override string MethodName => "Расстояние Левенштейна (рекурсивная реализация)";

        
    }
}
