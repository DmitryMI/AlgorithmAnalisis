namespace Lab03.Matrixes
{
    abstract class MatrixMultiplier
    {
        public abstract Matrix Multiply(Matrix a, Matrix b);
        public abstract long Ticks { get; }
        public static MatrixMultiplier GetMatrixMultiplier(MultiplicationMethod method)
        {
            MatrixMultiplier multiplier = null;
            switch (method)
            {
                case MultiplicationMethod.Simple:
                    multiplier = new SimpleMultiplier();
                    break;

                case MultiplicationMethod.Winograd:
                    break;
                case MultiplicationMethod.EnhancedWinograd:
                    break;
            }

            return multiplier;
        }
    }
}
