using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PiosAmdLibrary;

namespace EffectEplEditor
{
    class Ids
    {
        public ushort Id { get; set; }
        public ushort Id2 { get; set; }
    }
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
                Console.WriteLine($"EffectEplEditor v{Version.Major}.{Version.Minor}.{Version.Build}\n" +
                    $"Extracts and Repacks contents of p4g effect_epl files contained inside of amd archives\n" +
                    $"Usage:\n" +
                    $"       EffectEplEditor.exe InputFile (optional)OutputFolder\n" +
                    $"       EffectEplEditor.exe InputFolder (optional)OutputFile");
                Exit();
            }
            string path = @$"{Path.GetDirectoryName(InputFile)}\{Path.GetFileNameWithoutExtension(InputFile)}_extracted"; // deletes the extension from the filename and adds _extracted

            if (args.Length > 1)
                path = args[1];


            if (File.Exists(InputFile))
            {
                Console.WriteLine($"Loading: {InputFile}...");

                Effect_Epl EEpl = new Effect_Epl(InputFile);

                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                Directory.CreateDirectory(path);

                var Json = new Ids { Id = EEpl.Id, Id2 = EEpl.Id2 };

                var JsonToWrite = JsonConvert.SerializeObject(Json, Formatting.Indented);

                using (var writer = new StreamWriter(path + @"\id.json"))
                {
                    writer.Write(JsonToWrite);
                }

                File.WriteAllBytes(path + @"\data.epl",EEpl.Data);

                Console.WriteLine($"Effect_epl File extracted to: {path}");
                return;
            }
            else if (!Directory.Exists(InputFile))
            {
                Console.WriteLine($"\n{InputFile} does not exist");
                Exit();
            }

            if (args.Length == 1)
                path = InputFile + ".Eepl";

            Console.WriteLine($"Reading the contents of: {InputFile}...");

            if (!File.Exists(InputFile + @"\id.json"))
            {
                Console.WriteLine("\nid.txt does not exist");
                Exit();
            }

            if (!File.Exists(InputFile + @"\data.epl"))
            {
                Console.WriteLine("\ndata.epl does not exist");
                Exit();
            }

            string JsonFromFile;
            using (var reader = new StreamReader(InputFile + @"\id.json"))
                JsonFromFile = reader.ReadToEnd();

            Ids NewIds = JsonConvert.DeserializeObject<Ids>(JsonFromFile);

            Effect_Epl NewEEpl = new Effect_Epl();


            NewEEpl.Id = NewIds.Id;
            NewEEpl.Id2 = NewIds.Id2;
            NewEEpl.Data = File.ReadAllBytes(InputFile + @"\data.epl");
            NewEEpl.Save(path);

            Console.WriteLine($"\neffect_epl File Saved to: {path}");
        }
    }
}
