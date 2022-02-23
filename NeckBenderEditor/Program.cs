using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            

            string InputFile = "";


            if (args.Length > 0)
                InputFile = args[0];
            else
            {
                Console.WriteLine($"NeckBenderEditor v{Version.Major}.{Version.Minor}.{Version.Build}\n" +
                    $"Extracts and Repacks contents of p4g NECK_BENDER Files\n" +
                    $"Usage:\n" +
                    $"       NeckBenderEditor.exe InputFile (optional)OutputFolder\n" +
                    $"       NeckBenderEditor.exe InputFolder (optional)OutputFile");
                Exit();
            }
            string path = $"{Path.GetDirectoryName(InputFile)}\\{Path.GetFileNameWithoutExtension(InputFile)}_extracted"; // deletes the extension from the filename and adds _extracted

            if (args.Length > 1)
                path = args[1];


            if (File.Exists(InputFile))
            {
                Console.WriteLine($"Loading: {InputFile}...");


                NECK_BENDER Bender = new NECK_BENDER(InputFile);




                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                Directory.CreateDirectory(path);



                for (int i = 0; i < Bender.Chunks.Count; i++)
                {
                    File.WriteAllBytes($@"{path}\{new string(Bender.Chunks[i].Name).Replace("\0","")}", Bender.Chunks[i].Data);
                }

                Console.WriteLine($"NECK_BENDER File extracted to: {path}");
                return;
            }
            else if (!Directory.Exists(InputFile))
            {
                Console.WriteLine($"\n{InputFile} does not exist");
                Exit();
            }

            if (args.Length == 1)
                path = InputFile + ".NECK_BENDER";

            Console.WriteLine($"Reading the contents of: {InputFile}...");

            string[] DatFiles = Directory.GetFiles(InputFile, "*.dat", SearchOption.TopDirectoryOnly);
            string[] OgFiles = Directory.GetFiles(InputFile, "*", SearchOption.TopDirectoryOnly);

            List<string> OgFilesList = OgFiles.ToList();
            Console.WriteLine(DatFiles.ToList().Except(OgFiles.ToList()).ToList().Count);
            foreach (string File in DatFiles) OgFilesList.Remove(File); // remove bin files from OgFiles

            OgFiles = OgFilesList.ToArray();


            List<NECK_CHUNK> NeckChunks = new List<NECK_CHUNK>();

            for (int i = 0; i < DatFiles.Length; i++) // add bin files
            {
                NECK_CHUNK Chunk = new NECK_CHUNK();
                Chunk.Data = File.ReadAllBytes(DatFiles[i]);
                Chunk.Name = Path.GetFileName(DatFiles[i]).ToCharArray();
                NeckChunks.Add(Chunk);
            }

            for (int i = 0; i < OgFiles.Length; i++) // add all other files
            {
                NECK_CHUNK Chunk = new NECK_CHUNK();
                Chunk.Data = File.ReadAllBytes(OgFiles[i]);
                Chunk.Name = Path.GetFileName(OgFiles[i]).ToCharArray();
                NeckChunks.Add(Chunk);
            }

            NECK_BENDER NewNeck = new NECK_BENDER();
            NewNeck.Chunks = NeckChunks;
            Console.WriteLine(NeckChunks.Count);
            NewNeck.Save(path);
            Console.WriteLine($"\nNECK_BENDER File Saved to: {path}");
        }
    }
}