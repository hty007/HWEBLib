using htyWEBlib.Geo;
using htyWEBlib.HelpersTag;
using System;

namespace htyWEBlib.Tag
{
    public class SvgTag : SvgContent
    {
        #region Конструкторы и поля
        public string Height { get => this["height"]; set => this["height"] = value; }
        public string Width { get => this["width"]; set => this["width"] = value; }

        public SvgTag() : base(TypeTAG.svg)
        {
        }
        public SvgTag(string name) : this()
        {
            this["name"] = name;
        }
        public SvgTag( double width, double height, string name = null) : this()
        {
            if (name != null)
                this["name"] = name;
            Height = height.ToString();
            Width = width.ToString();
        }

        
        #endregion
        #region Функционал


        #endregion
    }

    public class SvgContent : BuilderTag
    {
        public SvgContent(TypeTAG tag) : base(tag)
        {
        }
        internal SvgContent Arrow(Geo.HPoint o0, Geo.HPoint oX, double length = -1)
        {
               Line line = new Line(o0, oX);
               var tag = line.Arrow(0.4, (length == -1)?line.Length * 0.1:length);
               Add(tag);
               return tag;
        }
        internal new SvgContent Text(Geo.HPoint pos, string text, double length = -1)
        {
            var tag = new SvgContent(TypeTAG.text);
            tag["x"] = ((int)pos.X).ToString();
            tag["y"] = ((int)pos.Y).ToString();
            var t = (BuilderTag)tag;
            t.Text =text;
            Add(tag);
            return tag;
        }

        public SvgContent Rect(double x, double y, double width, double height)
        {
            var tag = new SvgContent(TypeTAG.rect);
            tag["x"] = x.ToString();
            tag["y"] = y.ToString();
            tag["width"] = width.ToString();
            tag["height"] = height.ToString();
            Add(tag);
            return tag;
        }
        public SvgContent Line(double x1, double y1, double x2, double y2, string stroke = "black")
        {
            var tag = new SvgContent(TypeTAG.line);
            tag["x1"] = (Math.Round(x1)).ToString();
            tag["y1"] = (Math.Round(y1)).ToString();
            tag["x2"] = (Math.Round(x2)).ToString();
            tag["y2"] = (Math.Round(y2)).ToString();
            if (stroke !=  null)
                tag["stroke"] = stroke;
            this.Add(tag);
            return tag;
        }
        public SvgContent Circle(Geo.HPoint M, double r,
            string stroke = "black", string strokeWidth = "1",
            string fill = null)
        {
            return Circle(M.X, M.Y, r, stroke, strokeWidth, fill);
        }

        public SvgContent Circle(double cx, double cy, double r, 
            string stroke = "black", string strokeWidth = "1", 
            string fill = null)
        {
            var tag = new SvgContent(TypeTAG.circle);
            tag["cx"] = (Math.Round(cx)).ToString();
            tag["cy"] = (Math.Round(cy)).ToString();
            tag["r"] = (Math.Round(r)).ToString();

            if (stroke != null)
                tag["stroke"] = stroke;

            tag["stroke-width"] = strokeWidth;

            if (fill != null)
                tag["fill"] = fill;

            this.Add(tag);
            return tag;
        }

        public SvgContent AddG()
        {
            var tag = new SvgContent(TypeTAG.g);            
            this.Add(tag);
            return tag;
        }

    }
}
