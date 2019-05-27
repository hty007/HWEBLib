using System;

namespace htyWEBlib.Geo
{
    public class Line
    {
        private static double Pow2(double x) => x * x;
        public Line(double x1, double y1, double x2, double y2)
        {
            Begin = new HPoint(x1,y1);
            End = new HPoint(x2,y2);
        }

        public Line()
        {
            Begin = new HPoint();
            End = new HPoint();
        }

        public Line(HPoint begin, HPoint end)
        {
            Begin = begin;
            End = end;
        }

        public HPoint Begin { get; set; }
        public HPoint End { get; set; }

        public double AngleHorisontal { get => Math.Atan2(Begin.Y - End.Y, Begin.X - End.X); }
        public double AngleVertical { get => Math.PI / 2 - Math.Atan2(Begin.Y - End.Y, Begin.X - End.X); }
        public double Length { get=> }
    }

}
