using System;
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
            
        }

        public override string MethodName => "Расстояние Левенштейна";
    }
}
