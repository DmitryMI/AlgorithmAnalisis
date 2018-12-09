using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06.AcoAlgorithm
{
    class Bruteforcer
    {
        private double _aMin, _aMax, _c, _roMin, _roMax;
        private int _iterations, _tMin, _tMax;
        public Bruteforcer(double alphaMin, double alphaMax, double c, double roMin, double roMax, int tMin, int tMax, int iterations)
        {
            _aMin = alphaMin;
            _aMax = alphaMax;
            _c = c;
            _roMin = roMin;
            _roMax = roMax;
            _tMin = tMin;
            _tMax = tMax;
            _iterations = iterations;
        }

        public AcoProcessor.AcoParameters[] Iterate(AcoProcessor.AcoParameters paramBase)
        {
            List<AcoProcessor.AcoParameters> result = new List<AcoProcessor.AcoParameters>();

            double alpha = _aMin;
            double alphaStep = (_aMax - _aMin) / _iterations;

            double roStep = (_roMax - _roMin) / _iterations;

            for (int i = 0; i < _iterations; i++)
            {
                double beta = _c - alpha;
                double ro = _roMin;
                for (int j = 0; j < _iterations; j++)
                {
                    for (int t = _tMin; t <= _tMax; t++)
                    {
                        AcoProcessor.AcoParameters iteration = paramBase;
                        iteration.alpha = alpha;
                        iteration.beta = beta;
                        iteration.p = ro;
                        iteration.tMax = t;

                        result.Add(iteration);
                    }
                    ro += roStep;
                }
                alpha += alphaStep;
                if (alpha > _aMax)
                    alpha = _aMax;
            }

            return result.ToArray();
        }
    }
}
