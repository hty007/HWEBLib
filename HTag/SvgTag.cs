using htyWEBlib.Geo;
using htyWEBlib.HelpersTag;
using System;
using System.Drawing;

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
        internal void Arrow(Geo.HPoint o0, Geo.HPoint oX)
        {
           Line line = new Line(o0, oX);
            this.Add(line.Arrow(0.4, line.Length*0.1));
        }
        public SvgContent Rect(double x, double y, double width, double height)
        {
            var tag = new SvgContent(TypeTAG.rect);
            tag["x"] = x.ToString();
            tag["y"] = y.ToString();
            tag["width"] = width.ToString();
            tag["height"] = height.ToString();
            this.Add(tag);
            return tag;
        }
        public SvgContent Line(double x1, double y1, double x2, double y2, string stroke = "black")
        {
            var tag = new SvgContent(TypeTAG.line);
            tag["x1"] = (Math.Round(x1)).ToString();
            tag["y1"] = (Math.Round(y1)).ToString();
            tag["x2"] = (Math.Round(x2)).ToString();
            tag["y2"] = (Math.Round(y2)).ToString();
            tag["stroke"] = stroke;
            this.Add(tag);
            return tag;
        }

    }
}
