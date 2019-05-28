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

        public class Axis : IHData
        {
            /// <summary>Метка</summary>
            private string label;            
            /// <summary>Максимальное значение</summary>
            public double Max;
            /// <summary>Минимальное значение</summary>
            public double Min;
            /// <summary>Шаг нумерования</summary>
            private double step;
            /// <summary>Eдиничный отрезок</summary>
            private double singleSegment;
            private bool TrySegment;
            private bool TryLabel;

            public int CountStep => (int)Math.Ceiling((Max - Min) / SingleSegment);
            public string Label
            {
                get => label;
                set
                {
                    TryLabel = true;
                    label = value;
                }
            }
            public double SingleSegment
            {
                get
                {
                    if (TrySegment)
                        if (singleSegment == 0) return step;
                    return singleSegment;
                }
                set
                {
                    TrySegment = true;
                    singleSegment = value;
                }
            }
            public double Step
            {
                get
                {
                    if (TrySegment)
                        if (step == 0) return singleSegment;
                    return step;
                }

                set
                {
                    TrySegment = true;
                    step = value;
                }
            }

            public void Load(BinaryReader reader)
            {
                var data = new Pairs(reader);
                var count = reader.ReadInt32();                
                for (int i = 0; i < count; i++)
                {
                    var p = new Pair();
                    p.Load(reader);
                    switch (p.Key)
                    {
                        case "l": Label = (string)p.Value; break;
                        case "mx": Max = (double)p.Value; break;
                        case "mn": Min = (double)p.Value; break;
                        case "st": Step = (double)p.Value; break;
                        case "sn": SingleSegment = (double)p.Value; break;
                    }
                }
            }
            public void Save(BinaryWriter writer)
            {
                var data = new Pairs();
                if (label != null)
                    data.Add(new Pair("l", label));
                if (Max != Min)
                {
                    data.Add(new Pair("mx", Max));
                    data.Add(new Pair("mn", Min));
                }
                if (step != 0)
                    data.Add(new Pair("st", step));
                if (singleSegment != 0)
                    data.Add(new Pair("sn", singleSegment));
                data.Save(writer);
            }
        }
        #endregion
        #region Констркуторы и переопределения        
        public override void Load(BinaryReader reader)
        {
            ID = reader.ReadInt32();
            Name = reader.ReadString();
            XAxis.Load(reader);
            YAxis.Load(reader);
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
            XAxis.Save(writer);
            YAxis.Save(writer);
            writer.Write(Data.Count);
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
            // Рисуем оси и подписываем их
            svg.Arrow(O0, OX, padding);
            svg.Arrow(O0, OY, padding);
            if (XAxis.Label != null)
                svg.Text(OX.Delta(-20, 10), XAxis.Label);
            if (YAxis.Label != null)
                svg.Text(OY.Delta(-padding,0), YAxis.Label);
            // Рисуем направляющие
            int beginX = (int)O0.X;
            int endX = (int)OX.X - padding;
            int stepX = (endX - beginX) / (XAxis.CountStep);
            int beginY = (int)O0.Y;
            int endY = (int)OY.Y + padding;
            int stepY = Math.Abs(endY - beginY) / (YAxis.CountStep);
            var g = svg.AddG();
            for (int x = beginX; x <= endX; x += stepX)
                g.Line(x, beginY, x, endY);
            for (int y = beginY; y >= endY; y -= stepY)
                g.Line(beginX, y, endX, y);
            g["stroke-dasharray"] = "10,5";

            return tag;
        }
        #endregion
        #region 
        #endregion
    }

}
