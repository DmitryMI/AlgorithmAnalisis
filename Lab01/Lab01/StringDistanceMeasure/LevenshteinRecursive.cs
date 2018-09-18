using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab01.StringDistanceMeasure
{
    unsafe class LevenshteinRecursive : StringDistance
    {
        private string _wordA, _wordB;
        private int _result;
        private LetterMatrix _matrix;

        private bool _calculated = false;

        //private IntPtr _tablePtr;
        private int* _tableIntPtr;


        public LevenshteinRecursive(string a, string b)
        {
            _wordA = a;
            _wordB = b;
            _matrix = new LetterMatrix(a, b);

            var tablePtr = Marshal.AllocHGlobal((a.Length + 1) * (b.Length + 1) * sizeof(int));
            _tableIntPtr = (int*)(tablePtr.ToPointer());
        }

        public sealed override int GetDistance()
        {
            if (!_calculated)
            {
                //_result = D(_wordA.Length, _wordB.Length);
                _result = D_ptr(_wordA.Length, _wordB.Length);
                _calculated = true;
            }
            return _result;
        }

        public override LetterMatrix GetLetterMatrix()
        {
            if (!_calculated)
            {
                //_result = D(_wordA.Length, _wordB.Length);
                _result = D_ptr(_wordA.Length, _wordB.Length);
                _calculated = true;
            }
            else
            {
                for(int i = 0; i < _wordA.Length; i++)
                {
                    for(int j = 0; j < _wordB.Length; j++)
                    {
                        int index = _wordA.Length * i + j;
                        int value = Marshal.ReadInt32(new IntPtr(_tableIntPtr) + index);
                        _matrix[i, j] = value;
                    }
                }
            }
            return _matrix;
        }

        private int D(int i, int j)
        {
            if (i == 0 && j == 0)
            {
                _matrix[i, j] = 0;
                return 0;
            }
            if (j == 0 && i > 0)
            {
                _matrix[i, j] = i;
                return i;
            }
            if (i == 0 && j > 0)
            {
                _matrix[i, j] = j;
                return j;
            }

            int I = D(i, j - 1) + 1;
            int R = D(i - 1, j) + 1;
            int M = D(i - 1, j - 1) + Match(_wordA[i - 1], _wordB[j - 1]);
            int min = Min(I, R, M);

            _matrix[i, j] = min;

            return min;
        }

        private int D_ptr(int i, int j)
        {
            if (i == 0 && j == 0)
            {
                //_matrix[i, j] = 0;
                //Marshal.WriteInt32(_tablePtr + i * _wordA.Length + j, 0);
                int* ptr = _tableIntPtr + (i * _wordA.Length) + j;
                *ptr = 0;
                return 0;
            }
            if (j == 0 && i > 0)
            {
                //_matrix[i, j] = i;
                //Marshal.WriteInt32(_tablePtr + i * _wordA.Length + j, i);
                int* ptr = _tableIntPtr + (i * _wordA.Length) + j;
                *ptr = i;
                return i;
            }
            if (i == 0 && j > 0)
            {
                //_matrix[i, j] = j;
                //Marshal.WriteInt32(_tablePtr + i * _wordA.Length + j, j);
                int* ptr = _tableIntPtr + (i * _wordA.Length) + j;
                *ptr = j;
                return j;
            }

            int I = D(i, j - 1) + 1;
            int R = D(i - 1, j) + 1;
            int M = D(i - 1, j - 1) + Match(_wordA[i - 1], _wordB[j - 1]);
            int min = Min(I, R, M);

            //_matrix[i, j] = min;
            //Marshal.WriteInt32(_tablePtr + i * _wordA.Length + j, min);
            int* ptr0 = _tableIntPtr + (i * _wordA.Length) + j;
            *ptr0 = min;

            return min;
        }

        public override string MethodName => "Расстояние Левенштейна (рекурсивная реализация)";

        
    }
}
