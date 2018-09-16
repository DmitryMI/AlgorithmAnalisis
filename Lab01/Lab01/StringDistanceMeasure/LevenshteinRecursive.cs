using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01.StringDistanceMeasure
{
    class LevenshteinRecursive : StringDistance
    {
        private string _wordA, _wordB;
        private int _result;
        private LetterMatrix _matrix;
        public LevenshteinRecursive(string a, string b)
        {
            _wordA = a;
            _wordB = b;
            _matrix = new LetterMatrix(a, b);

            _result = D(_wordA.Length, _wordB.Length);
        }

        public sealed override int GetDistance()
        {
            return _result;
        }

        public override LetterMatrix GetLetterMatrix()
        {
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
            int M = D(i - 1, j - 1) + m(_wordA[i - 1], _wordB[j - 1]);
            int min = Min(I, R, M);

            _matrix[i, j] = min;

            return min;
        }

        private int m(char a, char b)
        {
            if (a == b)
                return 0;
            return 1;
        }

        private bool LesserThanOthers(int value, int a, int b)
        {
            return value <= a && value <= b;
        }

        private int Min(int a, int b, int c)
        {
            if (LesserThanOthers(a, b, c))
                return a;
            if (LesserThanOthers(b, a, c))
                return b;
            return c;
        }

        public override string MethodName => "Расстояние Левенштейна (рекурсивная реализация)";
    }
}
