using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PiosAmdLibrary
{
    public class NECK_BENDER
    {
        public List<NECK_CHUNK> Chunks { get; set; }

        public NECK_BENDER()
        {
            Chunks = new List<NECK_CHUNK>();
        }
        public NECK_BENDER(string Path)
        {

            BinaryReader reader = new BinaryReader(File.Open(Path, FileMode.Open));

            ReadData(reader);
                
        }

        public NECK_BENDER(BinaryReader reader)
        {
            ReadData(reader);

        }
        public void Save(string Path)
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Path)))
            {
                WriteData(writer);
                writer.Flush();
                writer.Close();
            }
        }
        public void Save(BinaryWriter writer)
        {
            WriteData(writer);
        }


        private void ReadData(BinaryReader reader)
        {
            int ChunkCount = reader.ReadInt32();
            Chunks = new List<NECK_CHUNK>();
            for (int i = 0; i < ChunkCount; i++)
                Chunks.Add(new NECK_CHUNK(reader));
        }

        private void WriteData(BinaryWriter writer)
        {
            writer.Write(Chunks.Count);
            foreach (NECK_CHUNK chunk in Chunks)
                chunk.Save(writer);
        }
    }

    public class NECK_CHUNK
    {
        public char[] Name;
        public int Size
        {
            get { return Data.Length; }
        }
        public byte[] Data { get; set; }
        public NECK_CHUNK()
        {
            
        }
        public NECK_CHUNK(byte[] Bytes)
        {
            using (BinaryReader reader = new BinaryReader(new MemoryStream(Bytes)))
                ReadData(reader);
        }
        public NECK_CHUNK(string Path)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Path)))
                ReadData(reader);
        }
        public NECK_CHUNK(BinaryReader reader)
        {
            ReadData(reader);
        }

        public void Save(string Path)
        {
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(Path));
            WriteData(writer);
        }
        public void Save(BinaryWriter writer)
        {
            WriteData(writer);
        }

        private void ReadData(BinaryReader reader)
        {
            Name = reader.ReadChars(32);
            int size = reader.ReadInt32();
            Data = reader.ReadBytes(size);
        }

        private void WriteData(BinaryWriter writer)
        {
            writer.Write(Name);
            if (Name.Length < 32)
            {
                for (int i = 0; i < 32 - Name.Length; i++)
                    writer.Write((byte)0);
            }
            writer.Write(Size);
            writer.Write(Data);
        }
    }
}
