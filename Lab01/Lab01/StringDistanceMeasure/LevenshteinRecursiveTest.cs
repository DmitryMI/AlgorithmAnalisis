using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab01.StringDistanceMeasure
{
    class LevenshteinRecursiveTest : StringDistance
    {
        private string _wordA, _wordB;
        private int _result;
        private LetterMatrix _matrix;

        private bool _calculated = false;

        public LevenshteinRecursiveTest(string a, string b)
        {
            _wordA = a;
            _wordB = b;
            _matrix = new LetterMatrix(a, b);
        }

        public sealed override int GetDistance()
        {
            if (!_calculated)
            {
                _result = D(_wordA.Length, _wordB.Length);
                _calculated = true;
            }
            return _result;
        }

        public override LetterMatrix GetLetterMatrix()
        {
            if (!_calculated)
            {
                _result = D(_wordA.Length, _wordB.Length);
                _calculated = true;
            }
            return _matrix;
        }

        private int D(int i, int j)
        {
            Console.WriteLine($"(i: {i}, j: {j})");

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

            Console.WriteLine($"{i}, {j} calls: ");

            int I = D(i, j - 1) + 1;
            int R = D(i - 1, j) + 1;
            int M = D(i - 1, j - 1) + Match(_wordA[i - 1], _wordB[j - 1]);
            int min = Min(I, R, M);

            _matrix[i, j] = min;

            return min;
        }

        public override string MethodName => "Расстояние Левенштейна (рекурсивная реализация)";

    }
}
