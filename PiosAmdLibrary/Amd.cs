using System;
using System.IO;
using System.Collections.Generic;

namespace PiosAmdLibrary
{
    public class Amd
    {
        /// <summary>
        /// AmdChunks of an <see cref="Amd"/> file.
        /// </summary>
        public List<AmdChunk> Chunks;

        /// <summary>
        /// Creates a new empty <see cref="Amd"/>.
        /// </summary>
        public Amd()
        {
            
        }

        /// <summary>
        /// Reads an <see cref="Amd"/> from a file.
        /// </summary>
        public Amd(string Path)
        {
            BinaryReader reader = new BinaryReader(File.Open(Path, FileMode.Open));
            if (reader.ReadInt32() != 1263421507)
            {
                throw new Exception("Not a proper AMD File");
            }
            uint ChunkCount = reader.ReadUInt32();
            Chunks = new List<AmdChunk>();
            for (int i = 0; i < ChunkCount; i++)
                Chunks.Add(new AmdChunk(reader));
        }
        /// <summary>
        /// Reads an <see cref="Amd"/> from a <see cref="BinaryReader"/>.
        /// </summary>
        public Amd(BinaryReader reader)
        {
            if (reader.ReadInt32() != 1263421507)
            {
                throw new Exception("Not a proper AMD File");
            }
            uint ChunkCount = reader.ReadUInt32();
            Chunks = new List<AmdChunk>();
            for (int i = 0; i < ChunkCount; i++)
                Chunks.Add(new AmdChunk(reader));
        }
        /// <summary>
        /// Writes an <see cref="Amd"/> to a Path.
        /// </summary>
        public void Save(string Path)
        {
            BinaryWriter writer = new BinaryWriter(File.Open(Path,FileMode.Create));
            writer.Write(1263421507);
            writer.Write(Chunks.Count);
            foreach (AmdChunk chunk in Chunks)
                chunk.Save(writer);
        }

        /// <summary>
        /// Writes an <see cref="Amd"/> to a <see cref="BinaryWriter"/>.
        /// </summary>
        public void Save(BinaryWriter writer)
        {
            writer.Write(1263421507);
            writer.Write(Chunks.Count);
            foreach (AmdChunk chunk in Chunks)
                chunk.Save(writer);
        }
    }
    
    public class AmdChunk
    {
        public char[] FileType { get; set; }
        public int Size
        {
            get { return Data.Length; }
        }
        public byte[] Data { get; set; }
        public uint Flags { get; set; }

        public AmdChunk()
        {
        }
        public AmdChunk(string Path)
        {
            BinaryReader reader = new BinaryReader(File.Open(Path, FileMode.Open));
            FileType = reader.ReadChars(16);
            Flags = reader.ReadUInt32();
            int size = reader.ReadInt32();
            Data = reader.ReadBytes(size);
        }
        public AmdChunk(BinaryReader reader)
        {
            FileType = reader.ReadChars(16);
            Flags = reader.ReadUInt32();
            int size = reader.ReadInt32();
            Data = reader.ReadBytes(size);
        }

        public void Save(string Path)
        {
            BinaryWriter writer = new BinaryWriter(File.Open(Path, FileMode.Create));
            writer.Write(FileType);
            if (FileType.Length < 16)
                for (int i = 0; i < 16 - FileType.Length; i++)
                    writer.Write((byte)0);
            writer.Write(Flags);
            writer.Write(Data.Length);
            long pain = writer.BaseStream.Position; // if this long isn't created for some reason not all data gets written 
            writer.Write(Data);
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(FileType);
            if (FileType.Length < 16)
                for (int i = 0; i < 16 - FileType.Length; i++)
                    writer.Write((byte)0);
            writer.Write(Flags);
            writer.Write(Data.Length);
            long pain = writer.BaseStream.Position; // if this long isn't created for some reason not all data gets written 
            writer.Write(Data);
        }
    }
}
