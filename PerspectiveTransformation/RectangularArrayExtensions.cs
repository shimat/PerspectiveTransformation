using System;

namespace PerspectiveTransform
{
    public static class RectangularArrayExtensions
    {
        /// <summary>
        /// Matrix multiplication
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double[,] Multiply(this double[,] a, double[,] b)
        {
            int rowsA = a.GetLength(0);
            int colsA = a.GetLength(1);
            int rowsB = b.GetLength(0);
            int colsB = b.GetLength(1);
            if (colsA != rowsB)
                throw new ArgumentException("matrices cannot be multiplied");

            var result = new double[rowsA, colsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < colsA; k++)
                    {
                        sum += a[i, k] * b[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        /// <summary>
        /// Matrix / scalar
        /// </summary>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double[,] Divide(this double[,] m, double d)
        {
            if (Math.Abs(d) < 1e-9)
                throw new ArgumentException("d == 0", nameof(d));

            int rows = m.GetLength(0);
            int cols = m.GetLength(1);

            var result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = m[i, j] / d;
                }
            }

            return result;
        }
    }
}
