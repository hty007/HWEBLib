using htyWEBlib.data;
using htyWEBlib.Tag;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htyWEBlib.Geo
{
    public class HPoint : IHData
    {
        public double X { get; set; }
        public double Y { get; set; }
        #region Конструкторы
        public HPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public HPoint(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public HPoint()
        {
        }
        #endregion
        #region Работа с точками
        /// <summary>
        /// Точка в прямоугольнике?
        /// </summary>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        /// <returns></returns>
        public bool RangeCheck(int maxX, int maxY) => RangeCheck(this, maxX, maxY);
        /// <summary>
        /// Дистанция до точки А 
        /// </summary>
        /// <returns>Растояние от этого экземпляра до точки А (double)</returns>
        /// <param name="A">A</param>
        public double Distance(HPoint A) => Distance(this, A);
        public static double Distance(HPoint instance, HPoint A)
        {
            var dx = A.X - instance.X;
            var dy = A.Y - instance.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        public static bool RangeCheck(HPoint instance, int maxX, int maxY)
        {
            return (instance.X >= 0 && instance.Y >= 0 && instance.X < maxX && instance.Y < maxY);
        }
        public HPoint Delta(double dx, double dy)
        {
            return new HPoint(X + dx, Y + dy);
        }
        #endregion
        #region Перегрузки, интерфейсы и другие форматы
        public override string ToString()
        {
            return $"({X:f},{Y:f})";
        }
        public void Load(BinaryReader reader)
        {
            X = reader.ReadDouble();
            Y = reader.ReadDouble();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
        }
        #endregion
    }

}
