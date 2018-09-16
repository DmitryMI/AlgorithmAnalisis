﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01.StringDistanceMeasure
{
    class LevenshteinDistance : StringDistance
    {
        private string _first, _second;
        private int _result;
        private LetterMatrix _matrix;
        public LevenshteinDistance(string a, string b)
        {
            _first = a;
            _second = b;
            Calculate();
        }

        public override int GetDistance()
        {
            return  _result;
        }

        public override LetterMatrix GetLetterMatrix()
        {
            return _matrix;
        }

        private void Calculate()
        {
            _matrix = new LetterMatrix(_first, _second);
            int s1Len = _first.Length;
            int s2Len = _second.Length;

            for (int i = 0; i <= s1Len; i++)
            {
                for (int j = 0; j <= s2Len; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        _matrix[i, j] = 0;
                    }
                    else if (j == 0 && i > 0)
                    {
                        _matrix[i, j] = i;
                    }
                    else if (i == 0 && j > 0)
                    {
                        _matrix[i, j] = j;
                    }
                    else
                    {

                        var diff = 0;
                        if (_first[i - 1] == _second[j - 1])
                            diff = 0;
                        else
                            diff = 1;

                        int insertion = _matrix[i - 1, j] + 1;
                        int deletion = _matrix[i, j - 1] + 1;
                        int substitution = _matrix[i - 1, j - 1] + diff;
                        _matrix[i, j] = Min(insertion, deletion, substitution);
                    }
                }
            }

            _result = _matrix[s1Len, s2Len];
        }

        public override string MethodName => "Расстояние Левенштейна";
    }
}
