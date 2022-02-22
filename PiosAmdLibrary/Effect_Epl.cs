using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using static PiosAmdLibrary.Functions;

namespace PiosAmdLibrary
{
    public class Effect_Epl
    {
        public ushort Id { get; set; }
        public ushort Id2 { get; set; }

        public byte[] Data { get; set; }

        public ulong EplSize
        {
            get { return (ulong)Data.Length; }
        }

        public ulong Effect_eplSize
        {
            get
            {
                ulong size = 20;
                size += EplSize;
                size += CalculatePadding(size);
                return size;
            }
        }
        public Effect_Epl()
        {

        }
        public Effect_Epl(BinaryReader reader)
        {
            ReadData(reader);
        }
        public Effect_Epl(byte[] Bytes)
        {
            using (BinaryReader reader = new BinaryReader(new MemoryStream(Bytes)))
                ReadData(reader);
        }
        public Effect_Epl(string path)
        {
            using (BinaryReader reader = new BinaryReader(System.IO.File.Open(path, FileMode.Open)))
            {
                ReadData(reader);
            }

                
        }
        public void Save(string path)
        {
            using (BinaryWriter writer = new BinaryWriter(System.IO.File.Open(path, FileMode.Create)))
            {
                WriteData(writer);

            }

        }
        public void Save(BinaryWriter writer)
        {
            WriteData(writer);
        }

        private void WriteData(BinaryWriter writer)
        {
            long Offset = writer.BaseStream.Position;
            writer.Write(Id);
            writer.Write(Id2);
            writer.Write((uint)Data.Length);
            for (int i = 0; i < 12; i++)
                writer.Write((byte)0);

            writer.Write(Data);

            long h = CalculatePadding(writer.BaseStream.Position);
            for (int i = 0; i < h; i++)
                writer.Write((byte)0);
        }

        private void ReadData(BinaryReader reader)
        {
            long Offset = reader.BaseStream.Position;

            Id = reader.ReadUInt16();
            Id2 = reader.ReadUInt16();
            int Eplsize = reader.ReadInt32();
            reader.ReadBytes(12);
            Data = reader.ReadBytes(Eplsize);
            reader.ReadBytes((int)CalculatePadding(reader.BaseStream.Position - Offset));
        }
    }
}
