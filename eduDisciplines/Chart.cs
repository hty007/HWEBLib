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
        private List<Session> Data { get; set; }

        public Session this[int index] { get => Data[index]; }
        public Axis XAxis { get; set; }
        public Axis YAxis { get; set; }
        private readonly static double EPSILON = 1e-6;

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
            private bool railes = false;

            public int CountStep => (int)Math.Ceiling((Max - Min) / Step);
            public int CountSingle => (int)Math.Ceiling((Max - Min) / SingleSegment);
            public int CountSinglePositive => (int)Math.Ceiling((Max) / SingleSegment);
            /// <summary>
            /// Число шагов в отрицательную сторону (значение отрицательое ) 
            /// </summary>
            public int CountSingleNegative => (int)Math.Ceiling((Min) / SingleSegment);
            /// <summary>Метка, надпись</summary>
            public string Label
            {
                get => label;
                set
                {
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

            public bool Railes { get => railes; set => railes = value; }

            public void Load(BinaryReader reader)
            {
                var data = new Pairs(reader);
                var count = reader.ReadInt32();                
                foreach (var p in data)
                //for (int i = 0; i < count; i++)
                {
                    //var p = new Pair();
                    //p.Load(reader);
                    switch (p.Key)
                    {
                        case "l": Label = (string)p.Value; break;
                        case "mx": Max = (double)p.Value; break;
                        case "mn": Min = (double)p.Value; break;
                        case "st": Step = (double)p.Value; break;
                        case "sn": SingleSegment = (double)p.Value; break;
                        case "r": railes = (bool)p.Value; break;
                    }
                }
            }
            public void Save(BinaryWriter writer)
            {
                var data = new Pairs();
                if (label != null)
                    data.Add(new Pair("l", label));
                if (Math.Abs(Max - Min) > EPSILON)
                {
                    data.Add(new Pair("mx", Max));
                    data.Add(new Pair("mn", Min));
                }
                if (Math.Abs(step) > EPSILON)
                    data.Add(new Pair("st", step));

                if (Math.Abs(singleSegment) > EPSILON)
                    data.Add(new Pair("sn", singleSegment));
                if (railes) data.Add("r", railes);
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
            if (XAxis.Max < maxX) XAxis.Max = maxX;
            if (XAxis.Min > minX) XAxis.Min = minX;
            if (YAxis.Max < maxY) YAxis.Max = maxY;
            if (YAxis.Min > minY) YAxis.Min = minY;
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

            data.PicSession(svg, Data[0]);


            //svg["stroke"] = "#765373";
            return tag;
        }



        /// <summary>
        /// Добавить новую сесию
        /// </summary>
        /// <param name="points">Points.</param>
        public void AddSession(params HPoint[] points)
        {
            Session s = new Session(Data.Count);
            s.Add(points);
            Data.Add(s);
        }
        /// <summary>
        /// Добавить точки в последную  сесию
        /// </summary>
        /// <param name="points">Points.</param>
        public void Add(params HPoint[] points)
        {
            if (Data.Count == 0) Data.Add(new Session(0));
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


        #endregion
        #region класс Tdata 
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
            //internal int beginX;
            internal int endX;
            internal int stepX;
            //internal int beginY;
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
            public HPoint OX1 { get; private set; }
            public HPoint OY1 { get; private set; }

            internal void Calculate(Chart ch, int m, int p)
            {
                margin = m; padding = p;
                XAxis = ch.XAxis; YAxis = ch.YAxis;
                //if(XAxis.Min<0)
                O0 = new HPoint(x: margin + 2 * padding, y: Heigth - margin - padding);
                OX = new HPoint(x: Width - margin, y: O0.Y);
                OY = new HPoint(x: O0.X, y: margin + padding);
                OX1 = O0.Delta(0,0);
                OY1 = O0.Delta(0,0); 
                endX = (int)OX.X - padding; 
                endY = (int)OY.Y + padding;

                if (XAxis.Min < 0)
                {
                    var steps = XAxis.CountSinglePositive - XAxis.CountSingleNegative ;
                    stepX = ((int)(endX - OX1.X ) / steps);
                    O0 = OX1.Delta(- XAxis.CountSingleNegative * stepX, 0);
                    OY.X = OY1.X = O0.X;
                }
                else
                    stepX = (int)((endX - OX1.X) / (XAxis.CountSingle));

                if (YAxis.Min<0)
                {
                    var steps = YAxis.CountSinglePositive - YAxis.CountSingleNegative;
                    stepY = ((int)(OY1.Y - endY ) / steps);
                    O0 = OY1.Delta(0, YAxis.CountSingleNegative * stepY);
                    OX.Y = OX1.Y = O0.Y;
                }
                else
                    stepY = (int)(Math.Abs(endY - OY1.Y) / (YAxis.CountSingle));
            }
            /// <summary> Рисуем направляющие</summary>
            internal void Rails(SvgTag svg)
            {
                var g = svg.AddG();
                if (XAxis.Railes)
                    for (int x = (int)OX1.X; x <= endX; x += stepX)
                        if (Math.Abs(x - O0.X) > EPSILON)
                        g.Line(x, OY1.Y, x, endY, null);


                if (YAxis.Railes)
                    for (int y = (int)OY1.Y; y >= endY; y -= stepY)
                        if (Math.Abs(y - O0.Y) > EPSILON)
                            g.Line(OX1.X, y, endX, y, null);

                g["stroke-dasharray"] = "10,5";
                g["stroke"] = "black";
            }
            /// <summary> Рисуем оси и подписываем их</summary>
            internal void PicAxis(SvgTag svg)
            {
                var arX = svg.Arrow(OX1, OX, padding);
                arX["stroke-width"] = "2";
                var arY = svg.Arrow(OY1, OY, padding);
                arY["stroke-width"] = "2";
                if (XAxis.Label != null)
                    svg.Text(OX.Delta(- 0.5* padding,  padding), XAxis.Label);
                if (YAxis.Label != null)
                    svg.Text(OY.Delta(-padding, 0), YAxis.Label);
            }
            /// <summary> Надписи</summary>
            internal void Labels(SvgTag svg)
            {
                var g = svg.AddG();
                int l = 0;
                for (int x = (int)OX1.X; x <= endX; x += stepX)
                {
                    double lab = XAxis.Min + l*XAxis.SingleSegment;
                    l++;
                    if (Math.Abs(lab % XAxis.Step) < EPSILON)
                    {
                        var label = g.Text(new HPoint(x, O0.Y + padding), lab.ToString());
                        label["text-anchor"] = "middle";
                    }

                }
                l = 0;
                for (int y = (int)OY1.Y; y >= endY; y -= stepY)
                {
                    double lab = YAxis.Min + l * YAxis.SingleSegment;
                    l++;
                    if (Math.Abs(lab % YAxis.Step) < EPSILON)
                    {
                        var label = g.Text(new HPoint(OX1.X - 2 * padding, y), lab.ToString());
                        label["dominant-baseline"] = "middle";
                    }

                }
            }

            internal HPoint ConvertPoint(HPoint p)
            {
                int newX = (int)(O0.X + p.X * stepX / XAxis.SingleSegment);
                int newY = (int)(O0.Y - p.Y * stepY / YAxis.SingleSegment);
                return new HPoint(newX, newY);
            }

            internal void PicSession(SvgTag svg, Session session)
            {
                SvgContent g = svg.AddG(id: $"session{session.ID}");
                switch (session.Type)
                {
                    case SessionType.Line:
                        SvgContent line = g.AddPolyline(session.GetPoints(this.ConvertPoint));
                        break;
                    case SessionType.Point: break;
                    case SessionType.Square: break;
                }
            }
        }
        #endregion
    }


}
