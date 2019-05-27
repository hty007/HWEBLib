using System;
using System.Collections.Generic;
using System.IO;
using htyWEBlib.data;
using htyWEBlib.Geo;
using htyWEBlib.Tag;

namespace htyWEBlib.eduDisciplines
{
    public class Chart : Fragment
    {        
        #region Поля, свойства
        public List<HPoint> Data { get; set; }
        public Axis XAxis { get; set; }
        public Axis YAxis { get; set; }

        public class Axis : ICoder
        {
            /// <summary>Метка</summary>
            public string Label;
            /// <summary>Еденицы измерения</summary>
            public string Units;
            /// <summary>Максимальное значение</summary>
            public double Max;
            /// <summary>Минимальное значение</summary>
            public double Min;
            /// <summary>Шаг нумерования</summary>
            public double Step;
            /// <summary>Eдиничный отрезок</summary>
            public double SingleSegment;
            public bool TrySegment;

            public int CountStep => (int)Math.Ceiling((Max - Min) / SingleSegment);

            const string sep = "@#$";
            public string Code()
            {
                return string.Join(sep, Label, Units, Max.ToString(), Min.ToString(), Step.ToString(), SingleSegment.ToString());
            }
            public void Decode(string code)
            {
                var obj = code.Split(new[] { sep }, StringSplitOptions.None);
                Label = obj[0];
                Units = obj[1];
                Max = double.Parse(obj[2]);
                Min = double.Parse(obj[3]);
                Step = double.Parse(obj[4]);
                SingleSegment = double.Parse(obj[5]);
            }
        }
        #endregion
        #region Констркуторы и переопределения        
        public override void Load(BinaryReader reader)
        {
            ID = reader.ReadInt32();
            Name = reader.ReadString();
            XAxis.Decode(reader.ReadString());
            YAxis.Decode(reader.ReadString());
            int cD = reader.ReadInt32();
            for (int i = 0; i < cD; i++)
            {
                HPoint p = new HPoint();
                p.Load(reader);
                Data.Add(p);
            }
        }
        public override void Save(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(Name);
            writer.Write(XAxis.Code());
            writer.Write(YAxis.Code());
            foreach (HPoint p in Data)
            {
                p.Save(writer);
            }
        }
        public Chart()
        {
            XAxis = new Axis();
            YAxis = new Axis();
        }
        #endregion
        #region Функционал
        public override HTag ToTag()
        {
            int w = 300;
            int h = 200;
            int margin = 5;
            int padding = 10;
            var tag = HTag.Build(TypeTAG.div, nameID:$"chart{ID}");
            tag.AddP(Name);
            var svg =  tag.AddSvg();
            svg.Width = w.ToString();
            svg.Height = h.ToString();
            var O0 = new HPoint(x: margin + padding, y: h - margin - padding);
            var OX = new HPoint(x: w-margin, y: O0.Y);
            var OY = new HPoint(x: O0.X, y: margin + padding);

            svg.Arrow(O0, OX, padding);
            svg.Arrow(O0, OY, padding);
            if (XAxis.Label != null)
                svg.Text(OX.Delta(-20, 10), XAxis.Label);
            if (YAxis.Label != null)
                svg.Text(OY.Delta(-padding,0), YAxis.Label);

            int beginX = (int)O0.X;
            int endX = (int)OX.X - padding;
            int step = (endX - beginX) / (XAxis.CountStep);
            int beginY = (int)O0.Y;
            int endY = (int)OY.Y + padding;
            var g = svg.AddG();
            for (int x = beginX; x <= endX; x += step)
                g.Line(x, beginY, x, endY);


            return tag;
        }
        #endregion
        #region 
        #endregion
    }

}
