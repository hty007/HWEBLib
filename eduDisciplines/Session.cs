using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using htyWEBlib.data;
using htyWEBlib.Geo;

namespace htyWEBlib.eduDisciplines
{
    public class Session : IEnumerable<HPoint>, IHData
    {
        private List<HPoint> data;
        public HPoint this[int index] { get => data[index]; set => data[index] = value; }
        public int Count { get => data.Count; }
        public SessionType Type { get; set; }
        public int ID { get; internal set; }

        public Session(int id = 0)
        {
            data = new List<HPoint>();
            Type = SessionType.Point;
            ID = id;
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
            Type = (htyWEBlib.eduDisciplines.SessionType)reader.ReadInt32();
            for (int i = 0; i < c; i++)
            {
                HPoint p = new HPoint();
                p.Load(reader);
                data.Add(p);
            }
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(Count);
            writer.Write((int)Type);
            foreach (var p in data)
            {
                p.Save(writer);
            }
        }

        internal HPoint[] GetPoints()
        {
            return data.ToArray();
        }

        internal HPoint[] GetPoints(Func<HPoint, HPoint> convertPoint)
        {
            List<HPoint> points = new List<HPoint>();
            foreach (HPoint p in data)
            {
                points.Add(convertPoint(p));
            }
            return points.ToArray();
        }
    }
    public enum SessionType
    {
        Line,
        Point,
        Square
    }

}
