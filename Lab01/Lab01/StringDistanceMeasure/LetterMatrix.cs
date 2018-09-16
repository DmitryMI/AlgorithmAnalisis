using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01.StringDistanceMeasure
{
    public class LetterMatrix
    {
        private const int MaxNumberLength = 3;

        private string _wordA, _wordB;
        private int[,] _matrix;

        public LetterMatrix(string wordA, string wordB)
        {
            _matrix = new int[wordA.Length + 1,wordB.Length + 1];
            _wordA = wordA;
            _wordB = wordB;
        }

        public int this[int i, int j]
        {
            get => _matrix[i, j];
            set => _matrix[i, j] = value;
        }

        public override string ToString()
        {
            return BuildString();
        }

        private string GetSeparator(int spaces)
        {
            string result = "";
            for (int i = 0; i < spaces; i++)
            {
                result += ' ';
            }
            return result;
        }

        private string BuildString()
        {
            // TODO Fix large numbers

            string result = "";
            string separator = GetSeparator(MaxNumberLength);

            result += ' ' + separator + '*' + separator;

            for (int i = 0; i < _wordA.Length; i++)
            {
                result += _wordA[i] + separator;
            }
            result += '\n';

            for (int j = 0; j <= _wordB.Length; j++)
            {
                if (j == 0)
                    result += '*' + separator;
                else
                    result += _wordB[j - 1] + separator;

                for (int i = 0; i <= _wordA.Length; i++)
                {
                    result += _matrix[i, j] + separator;
                }
                result += '\n';
            }

            return result;
        }
    }
}
