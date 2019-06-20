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
            /// <summary>Метка, надпись</summary>
            public string Label
            {
                get => label;
                set
                {
                    TryLabel = true;
                    label = value;
                }
            }
            /// <summary>Eдиничный отрезок</summary>
            public double SingleSegment
            {
                get
                {
                    if (TrySegment)
                        if (Math.Abs(singleSegment) < 1e-6) return step;
                    return singleSegment;
                }
                set
                {
                    TrySegment = true;
                    singleSegment = value;
                }
            }
            /// <summary>Шаг нумерования</summary>
            public double Step
            {
                get
                {
                    if (TrySegment)
                        if (Math.Abs(step) < 1e-6) return singleSegment;
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
            Data = new List<HPoint>();
        }
        #endregion
        #region Функционал
        public override HTag ToTag()
        {
            TData data = new TData(width: 300,heigth: 200);
            data.Calculate(this, m: 5, p: 15);
            var tag = HTag.Build(TypeTAG.div, nameID: $"chart{ID}");
            tag.AddP(Name);
            var svg = tag.AddSvg();
            svg.Width = data.Width.ToString();
            svg.Height = data.Heigth.ToString();
            // Рисуем оси и подписываем их
            data.PicAxis(svg);
            // Рисуем направляющие
            data.Rails(svg);
            // Подписываем еденичные отрезки
            data.Labels(svg);
            foreach (var p in Data)
            {
                //var p = Data[0];
                var np = ConvertPoint(p, beginX, stepX, beginY, stepY);
                svg.Circle(np, 2);
            }
            return tag;
        }

        private HPoint ConvertPoint(HPoint p, int beginX, int stepX, int beginY, int stepY )
        {
            var newX = beginX + p.X * stepX / XAxis.SingleSegment;
            var newY = beginY - p.Y * stepY / YAxis.SingleSegment;
            return new HPoint(newX, newY);
        }



        public void Add(params HPoint[] points)
        {
            Data.AddRange(points);/*
            foreach (HPoint point in points)
            {
                Data.Add(point);
            }/**/
        }
        public void Add(double x, double y)
        {
            Data.Add(new HPoint(x,y));
        }

        private class TData
        {
            public TData(int width, int heigth)
            {
                Width = width;
                Heigth = heigth;
            }
            internal int Width;
            internal int Heigth;
            internal int margin;
            internal int padding;
            internal int beginX;
            internal int endX;
            internal int stepX;
            internal int beginY;
            internal int endY;
            internal int stepY;
            
            /// <summary> Начало координат </summary>
            public HPoint O0 { get; internal set; }
            /// <summary> Нахождения оси Х </summary>
            public HPoint OX { get; internal set; }
            /// <summary> Нахождение оси Y </summary>
            public HPoint OY { get; internal set; }
            public Axis XAxis { get; private set; }
            public Axis YAxis { get; private set; }

            internal void Calculate(Chart ch , int m, int p)
            {
                margin = m; padding = p;
                XAxis = ch.XAxis; YAxis = ch.YAxis;
                O0 = new HPoint(x: margin + padding, y: Heigth - margin - padding);
                OX = new HPoint(x: Width - margin, y: O0.Y);
                OY = new HPoint(x: O0.X, y: margin + padding);
                beginX = (int)O0.X;
                endX = (int)OX.X - padding;
                stepX = (endX - beginX) / (XAxis.CountStep);
                beginY = (int)O0.Y;
                endY = (int)OY.Y + padding;
                stepY = Math.Abs(endY - beginY) / (YAxis.CountStep);
            }
            /// <summary> Рисуем направляющие</summary>
            internal void Rails(SvgTag svg)
            {
                    var g = svg.AddG();
                    for (int x = beginX; x <= endX; x += stepX)
                    {
                        g.Line(x, beginY, x, endY, null);
                        g.Text(new HPoint(x, beginY + padding), (-beginX + x / stepX * XAxis.SingleSegment).ToString());
                    }
                    for (int y = beginY; y >= endY; y -= stepY)
                        g.Line(beginX, y, endX, y, null);
                    g["stroke-dasharray"] = "10,5";
                    g["stroke"] = "black";
            }
            /// <summary> Рисуем оси и подписываем их</summary>
            internal void PicAxis(SvgTag svg)
            {
                    svg.Arrow(O0, OX, padding);
                    svg.Arrow(O0, OY, padding);
                    if (XAxis.Label != null)
                        svg.Text(OX.Delta(-20, 15), XAxis.Label);
                    if (YAxis.Label != null)
                        svg.Text(OY.Delta(-padding, 0), YAxis.Label);                
            }

            internal void Labels(SvgTag svg)
            {
                XAxis.Step;
            }
        }
        #endregion
        #region 
        #endregion
    }

}
