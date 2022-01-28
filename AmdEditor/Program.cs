using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using PiosAmdLibrary;


namespace AmdEditor
{
    class Program
    {
        public static Assembly Assembly = Assembly.GetExecutingAssembly();
        public static AssemblyName AssemblyName = Assembly.GetName();
        public static Version Version = AssemblyName.Version;
        public static Type[] Types = Assembly.GetTypes();
        public static void Exit()
        {
            Console.WriteLine("\nPress Any Key to Quit");
            Console.ReadKey();
            System.Environment.Exit(0);
        }
        static void Main(string[] args)
        {
            //args = new string[] { @"C:\Users\piotr\Downloads\amdtoolsTest\es002_extracted" };

            string InputFile = "";


            if (args.Length > 0)
                InputFile = args[0];
            else
            {
                Console.WriteLine($"BedEditor v{Version.Major}.{Version.Minor}.{Version.Build}\n" +
                    $"Extracts and Repacks contents of p4g Amd Files\n" +
                    $"Usage:\n" +
                    $"       AmdEditor.exe InputFile (optional)OutputFolder\n" +
                    $"       AmdEditor.exe InputFolder (optional)OutputFile");
                Exit();
            }
            string path = $"{Path.GetDirectoryName(InputFile)}\\{Path.GetFileNameWithoutExtension(InputFile)}_extracted"; // deletes the extension from the filename and adds _extracted

            if (args.Length > 1)
                path = args[1];


            if (File.Exists(InputFile))
            {
                Console.WriteLine($"Loading: {InputFile}...");

                Amd AmdFile = new Amd(InputFile);
                



                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                Directory.CreateDirectory(path);

                

                string zeros = "";
                for (int i = 0; i < AmdFile.Chunks.Count.ToString().Length; i++)
                {
                    zeros += "0";
                }
                if (zeros == "0")
                    zeros = "00";

                List<uint> OgFlags = new List<uint>();

                for (int i = 0; i < AmdFile.Chunks.Count; i++)
                {
                    string extension = new string(AmdFile.Chunks[i].FileType).Replace("\0","");
                    OgFlags.Add(AmdFile.Chunks[i].Flags);
                    File.WriteAllBytes($@"{path}\{i.ToString(zeros)}.{extension}", AmdFile.Chunks[i].Data);
                }

                var json = OgFlags;

                var JsonToWrite = JsonConvert.SerializeObject(json, Formatting.Indented);

                using (var writer = new StreamWriter(path + @"\Flags.json"))
                    writer.Write(JsonToWrite);

                Console.WriteLine($"Amd File extracted to: {path}");
                return;
            }
            else if (!Directory.Exists(InputFile))
            {
                Console.WriteLine($"\n{InputFile} does not exist");
                Exit();
            }

            if (args.Length == 1)
                path = InputFile + ".amd";

            Console.WriteLine($"Reading the contents of: {InputFile}...");

            if (!File.Exists(InputFile + @"\Flags.json"))
            {
                Console.WriteLine("Flags.json doesn't exist");
                Exit();
            }

            string JsonFromFile;
            using (var reader = new StreamReader(InputFile + @"\Flags.json"))
                JsonFromFile = reader.ReadToEnd();

            List<uint> Flags = JsonConvert.DeserializeObject<List<uint>>(JsonFromFile);

            
            string[] OgFiles = Directory.GetFiles(InputFile, "*", SearchOption.TopDirectoryOnly);

            List<AmdChunk> AmdChunks = new List<AmdChunk>();

            if (Flags.Count < OgFiles.Length - 1)
            {
                Console.WriteLine("Flags.Json has too little values");
                Exit();
            }

            for (int i = 0; i < OgFiles.Length -1; i++)
            {
                AmdChunk Chunk = new AmdChunk();
                Chunk.Data = File.ReadAllBytes(OgFiles[i]);
                Chunk.FileType = OgFiles[i].Substring(OgFiles[i].IndexOf('.') + 1).ToUpper().ToCharArray();
                Chunk.Flags = Flags[i];
                AmdChunks.Add(Chunk);
            }
            
            Amd NewAmd = new Amd();
            NewAmd.Chunks = AmdChunks;
            NewAmd.Save(path);
            Console.WriteLine($"\nAmd File Saved to: {path}");
        }
    }
}