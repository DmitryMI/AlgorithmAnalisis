using System;

namespace Lab01.StringDistanceMeasure
{
    public abstract class StringDistance
    {
        public enum Measure
        {
            Levenshtein, DamerauLevenshtein, LevenshteinRecursive
        }

        public static class StringDistanceBuilder
        {
            public static StringDistance GetInstance(Measure measure, string a, string b)
            {
                StringDistance instance = null;
                switch (measure)
                {
                    case Measure.Levenshtein:
                        //instance = new LevenshteinDistance(a, b);
                        break;
                    case Measure.DamerauLevenshtein:
                        //instance = new LevenshteinDistance(a, b);
                        break;
                    case Measure.LevenshteinRecursive:
                        instance = new LevenshteinRecursive(a, b);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(measure), measure, null);
                }

                return instance;
            }
        }

        public abstract int GetDistance();

        public abstract LetterMatrix GetLetterMatrix();
        public abstract string MethodName { get; }
    }
}
