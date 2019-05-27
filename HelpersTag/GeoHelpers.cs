using htyWEBlib.Geo;
using htyWEBlib.Tag;
using System;

namespace htyWEBlib.HelpersTag
{
    public static class GeoHelpers
    {
        public static SvgContent Arrow(this Line l,  double angel, double lengt)
        {
            SvgContent tag = new SvgContent(TypeTAG.g);
            tag.Line(l.Begin.X, l.Begin.Y, l.End.X, l.End.Y);
            double ugol = Math.Atan2(l.Begin.Y - l.End.Y, l.Begin.X - l.End.X);
            //double ugol = Math.Atan((Begin.X - End.X)/(Begin.Y - End.Y));
            tag.Line(l.End.X, l.End.Y, l.End.X + lengt * Math.Cos(ugol + angel), l.End.Y + lengt * Math.Sin(ugol + angel));
            tag.Line(l.End.X, l.End.Y, l.End.X + lengt * Math.Cos(ugol - angel), l.End.Y + lengt * Math.Sin(ugol - angel));
            tag["stroke"] = "black";
            return tag;
        }
    }
}
