using System;
using System.Collections;
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
        public List<Session> Data { get; set; }
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

            public int CountStep => (int)Math.Ceiling((Max - Min) / Step);
            public int CountSingle => (int)Math.Ceiling((Max - Min) / SingleSegment);
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
                Session s = new Session();
                s.Load(reader);
                Data.Add(s);
            }
        }
        public override void Save(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(Name);
            XAxis.Save(writer);
            YAxis.Save(writer);
            writer.Write(Data.Count);
            foreach (Session s in Data)
            {
                s.Save(writer);
            }
        }
        public Chart()
        {
            XAxis = new Axis();
            YAxis = new Axis();
            Data = new List<Session>();
        }
        #endregion
        #region Функционал
        public void MinMax()
        {
            double minX, minY, maxX, maxY;
            minX = minY = maxX = maxY = 0;
            foreach (var session in Data)
                foreach(HPoint point in session)
                {
                    if (minX > point.X) minX = point.X;
                    if (maxX < point.X) maxX = point.X;
                    if (minY > point.Y) minY = point.Y;
                    if (maxY < point.Y) maxY = point.Y;
                }
            XAxis.Max = maxX; XAxis.Min = minX;
            YAxis.Max = maxY; YAxis.Min = minY;
        }

        public override HTag ToTag()
        {
            TData data = new TData(width: 300,heigth: 200);
            MinMax();
            data.Calculate(this, m: 5, p: 15);
            var tag = HTag.Build(TypeTAG.div, nameID: $"chart{ID}");
            tag.AddP(Name);
            SvgTag svg = tag.AddSvg($"chart{ID}", data.Width, data.Heigth);
            // Рисуем оси и подписываем их
            data.PicAxis(svg);
            // Рисуем направляющие
            data.Rails(svg);
            // Подписываем еденичные отрезки
            data.Labels(svg);
            foreach (var p in Data[0])
            {
                //var p = Data[0];
                var np = data.ConvertPoint(p);
                svg.Circle(np, 2);
            }
            return tag;
        }



        /// <summary>
        /// Добавить новую сесию
        /// </summary>
        /// <param name="points">Points.</param>
        public void AddSession(params HPoint[] points)
        {
            Session s = new Session();
            s.Add(points);
            Data.Add(s);
        }
        /// <summary>
        /// Добавить точки в последную  сесию
        /// </summary>
        /// <param name="points">Points.</param>
        public void Add(params HPoint[] points)
        {
            if (Data.Count == 0) Data.Add(new Session());
            Data[Data.Count - 1].Add(points);
        }
        /// <summary>
        /// Добавить точки в определёную сесию
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public void Add(int session, double x, double y)
        {
            if (session >= Data.Count) throw new ArgumentException();
            Data[session].Add(x,y);
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
                stepX = (endX - beginX) / (XAxis.CountSingle);
                beginY = (int)O0.Y;
                endY = (int)OY.Y + padding;
                stepY = Math.Abs(endY - beginY) / (YAxis.CountSingle);
            }
            /// <summary> Рисуем направляющие</summary>
            internal void Rails(SvgTag svg)
            {
                    var g = svg.AddG();
                    for (int x = beginX; x <= endX; x += stepX)
                    {
                        g.Line(x, beginY, x, endY, null);
                        
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
                var g = svg.AddG();
                for (int i = 0; i<XAxis.CountStep; i++)
                {
                    int x = beginX + i * stepX;
                    int y = beginY + padding;
                    double lab = i * XAxis.Step;
                       
                    g.Text(new HPoint(x,y), lab.ToString());
                }

            }

            internal HPoint ConvertPoint(HPoint p)
            {
                    var newX = beginX + p.X * stepX / XAxis.SingleSegment;
                    var newY = beginY - p.Y * stepY / YAxis.SingleSegment;
                    return new HPoint(newX, newY);
            }
        }
        #endregion
        #region 
        #endregion
    }

    public class Session: IEnumerable<HPoint>, IHData
    {
        private List<HPoint> data;
        public HPoint this[int index] { get => data[index]; set => data[index] = value;  }
        public int Count { get => data.Count; }
        public Session()
        {
            data = new List<HPoint>();
        }
        public void Add(double x, double y)
        {
            Add(new HPoint(x, y));
        }
        public void Add(params HPoint[] points)
        {
            data.AddRange(points);
        }

        public IEnumerator<HPoint> GetEnumerator()
        {
            return ((IEnumerable<HPoint>)data).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<HPoint>)data).GetEnumerator();
        }
        public void Load(BinaryReader reader)
        {
            data.Clear();
            int c = reader.ReadInt32();
            for (int i = 0; i<c; i++)
            {
                HPoint p = new HPoint();
                p.Load(reader);
                data.Add(p);
            }
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(Count);
            foreach (var p in data)
            {
                p.Save(writer);
            }
        }
    }
}
