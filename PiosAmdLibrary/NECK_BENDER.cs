using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PiosAmdLibrary
{
    public class NECK_BENDER
    {
        

        public NECK_BENDER(string Path)
        {
            BinaryReader reader = new BinaryReader(File.Open(Path, FileMode.Open));

            int ChunkCount = reader.ReadInt32();
            List<NECK_CHUNK> Chunks = new List<NECK_CHUNK>();
            for (int i = 0; i < ChunkCount; i++)
                Chunks.Add(new NECK_CHUNK(reader));
                
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
        public NECK_CHUNK(BinaryReader reader)
        {
            Name = reader.ReadChars(32);
            int size = reader.ReadInt32();
            Data = reader.ReadBytes(size);
        }
    }
}
