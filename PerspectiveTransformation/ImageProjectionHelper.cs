using SixLabors.ImageSharp;
using System;
using System.Numerics;

namespace PerspectiveTransform
{
    /// <summary>
    /// https://github.com/SixLabors/ImageSharp/issues/787
    /// </summary>
    public static class ImageProjectionHelper
    {
        public static Matrix4x4 CalculateProjectiveTransformationMatrix(
            int width, int height,
            Point fromTopLeft, Point fromTopRight, Point fromBottomLeft, Point fromBottomRight)
        {
            var d = MapBasisToPoints(
                new Point(0, 0),
                new Point(width, 0),
                new Point(0, height),
                new Point(width, height)
            );
            var s = MapBasisToPoints(fromTopLeft, fromTopRight, fromBottomLeft, fromBottomRight);
            var result = d.Multiply(AdjugateMatrix(s));
            var normalized = result.Divide(result[2, 2]);
            return new Matrix4x4(
                (float)normalized[0, 0], (float)normalized[1, 0], 0, (float)normalized[2, 0],
                (float)normalized[0, 1], (float)normalized[1, 1], 0, (float)normalized[2, 1],
                0, 0, 1, 0,
                (float)normalized[0, 2], (float)normalized[1, 2], 0, (float)normalized[2, 2]
            );
        }

        private static double[,] AdjugateMatrix(double[,] matrix)
        {
            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3)
                throw new ArgumentException("Must provide a 3x3 matrix.");

            var ret = new double[3, 3];
            ret[0, 0] = matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1];
            ret[0, 1] = matrix[0, 2] * matrix[2, 1] - matrix[0, 1] * matrix[2, 2];
            ret[0, 2] = matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1];
            ret[1, 0] = matrix[1, 2] * matrix[2, 0] - matrix[1, 0] * matrix[2, 2];
            ret[1, 1] = matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0];
            ret[1, 2] = matrix[0, 2] * matrix[1, 0] - matrix[0, 0] * matrix[1, 2];
            ret[2, 0] = matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0];
            ret[2, 1] = matrix[0, 1] * matrix[2, 0] - matrix[0, 0] * matrix[2, 1];
            ret[2, 2] = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            return ret;
        }

        private static double[,] MapBasisToPoints(Point p1, Point p2, Point p3, Point p4)
        {
            var a = new double[3,3]
            {
                {p1.X, p2.X, p3.X},
                {p1.Y, p2.Y, p3.Y},
                {1, 1, 1}
            };
            var b = new double[3,1] {
                { p4.X },
                { p4.Y },
                { 1 }
            };
            var aj = AdjugateMatrix(a);
            var v = aj.Multiply(b);
            var m = new double[3,3]
            {
                {v[0,0], 0,      0 },
                {0,      v[1,0], 0 },
                {0,      0,      v[2,0] }
            };
            return a.Multiply(m);
        }
    }
}
