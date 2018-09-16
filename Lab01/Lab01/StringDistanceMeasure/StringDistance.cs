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
                        instance = new LevenshteinDistance(a, b);
                        break;
                    case Measure.DamerauLevenshtein:
                        instance = new DamerauLevenshtein(a, b);
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

        protected int Min(int a, int b, int c)
        {
            if (a <= b && a <= c)
                return a;
            if (b <= a && b <= c)
                return b;
            return c;
        }

        protected int Min(int a, int b, int c, int d)
        {
            int min3 = Min(a, b, c);
            if (d < min3)
                return d;
            return min3;
        }

        protected int Match(char a, char b)
        {
            if (a == b)
                return 0;
            return 1;
        }
    }
}
