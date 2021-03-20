using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace PerspectiveTransform
{
    class Program
    {
        static void Main(string[] args)
        {
            Sample1();
            Sample2();
        }

        static void Sample1()
        {
            var topLeft = new Point(169, 109);
            var topRight = new Point(939, 96);
            var bottomRight = new Point(1072, 659); 
            var bottomLeft = new Point(31, 657);
            
            PerformTransformation("Images/1.jpg", topLeft, topRight, bottomRight, bottomLeft);  
        }
        
        static void Sample2()
        {
            var topLeft = new Point(337, 71);
            var topRight = new Point(1029, 234);
            var bottomRight = new Point(930, 790); 
            var bottomLeft = new Point(117, 524);

            PerformTransformation("Images/2.jpg", topLeft, topRight, bottomRight, bottomLeft);            
        }

        static void PerformTransformation(
            string imageFileName,
            Point topLeft, Point topRight, Point bottomRight, Point bottomLeft)
        {
            double left = (topLeft.X + bottomLeft.X) / 2.0;
            double right = (topRight.X + bottomRight.X) / 2.0;
            double top = (topLeft.Y + topRight.Y) / 2.0;
            double bottom = (bottomLeft.Y + bottomRight.Y) / 2.0;
            int width = (int)(right - left);
            int height = (int)(bottom - top);
            
            var matrix = ImageProjectionHelper.CalculateProjectiveTransformationMatrix(
                width: width,
                height: height,
                fromTopLeft: topLeft,
                fromTopRight: topRight,
                fromBottomRight: bottomRight, 
                fromBottomLeft: bottomLeft);
            
            using var img = Image.Load(imageFileName);
            img.Mutate(context =>{ 
                context.Transform(new ProjectiveTransformBuilder().AppendMatrix(matrix));
                context.Crop(width, height);
            });

            var dstFileName = $"dst_{Path.GetFileNameWithoutExtension(imageFileName)}.png";
            img.SaveAsPng(dstFileName);
        }
    }
}
