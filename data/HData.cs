using System;
using System.IO;
using System.Text;

namespace htyWEBlib.data
{
    public abstract class HData : IHData
    {
        private const char Separator = '#';
        protected int[] data_I;
        protected double[] data_D;
        protected bool[] data_B;
        protected string[] data_S;

        /// <summary>
        /// Создает хранилища
        /// </summary>
        /// <param name="cI">Число ячеек int</param>
        /// <param name="cD">Число ячеек double</param>
        /// <param name="cB">Число ячеек bool</param>
        /// <param name="cS">Число ячеек string</param>
        public HData(int cI, int cD, int cB, int cS)
        {
            data_I = new int[cI];
            data_D = new double[cD];
            data_B = new bool[cB];
            data_S = new string[cS];
        }

        public void Load(BinaryReader reader)
        {
            int cI = reader.ReadInt32();
            int cD = reader.ReadInt32();
            int cB = reader.ReadInt32();
            int cS = reader.ReadInt32();

            if (data_I.Length <cI)
                data_I = new int[cI];
            if (data_D.Length < cD)
                data_D = new double[cD];
            if (data_B.Length < cB)
                data_B = new bool[cB];
            if (data_S.Length < cS)
                data_S = new string[cS];

            for (int i = 0; i < cI; i++)
                data_I[i] = reader.ReadInt32();
            for (int i = 0; i < cD; i++)
                data_D[i] = reader.ReadDouble();
            for (int i = 0; i < cB; i++)
                data_B[i] = reader.ReadBoolean();
            for (int i = 0; i < cS; i++)
                data_S[i] = reader.ReadString();

        }

        public void Save(BinaryWriter writer)
        {
            int cI = data_I.Length;
            int cD = data_D.Length;
            int cB = data_B.Length;
            int cS = data_S.Length;

            writer.Write(cI);
            writer.Write(cD);
            writer.Write(cB);
            writer.Write(cS);

            for (int i = 0; i < cI; i++)
                writer.Write(data_I[i]);
            for (int i = 0; i < cD; i++)
                writer.Write(data_D[i]);
            for (int i = 0; i < cB; i++)
                writer.Write(data_B[i]);
            for (int i = 0; i < cS; i++)
                writer.Write(data_S[i]);
        }

        
        
    }
}
