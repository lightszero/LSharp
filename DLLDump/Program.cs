using System;
using System.Collections.Generic;
using System.Text;

namespace DLLDump
{
    class Program
    {
        static void Main(string[] args)
        {
            if (System.IO.Directory.Exists("dlls") == false)
            {
                System.IO.Directory.CreateDirectory("dlls");
            }
            Console.WriteLine("把需要dump的dll文件和配对的pdb或者mdb，放在这个exe同层的dlls 目录中");
            string[] file = System.IO.Directory.GetFiles("dlls", "*.dll", System.IO.SearchOption.AllDirectories);


            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("------ Menu ------");
                Console.WriteLine("Type Dll index to Dump it.");
                Console.WriteLine("Type Q to quit this.");
                Console.ForegroundColor = ConsoleColor.Gray;

                for (int i = 0; i < file.Length; i++)
                {
                    string outtext = "(" + (i + 1).ToString("D03") + ") " + file[i];
                    if (System.IO.File.Exists(file[i] + ".mdb"))
                        outtext += "(with mdb)";

                    string fpdb = file[i].Substring(0, file[i].Length - 4) + ".pdb";
                    if (System.IO.File.Exists(fpdb))
                        outtext += "(with pdb)";
                    Console.WriteLine(outtext);
                }
                string line = Console.ReadLine();
                if (line.ToLower() == "q")
                    return;

                int ind = 0;
                if (int.TryParse(line, out ind))
                {
                    if (ind < 0 || ind > file.Length)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Fail Number。");
                        continue;
                    }
                    Dump(file[ind - 1]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fail Input。");
                }
            }

        }
        class Logger : CLRSharp.ICLRSharp_Logger
        {

            public void Log(string str)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(str);
            }

            public void Log_Warning(string str)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(str);
            }

            public void Log_Error(string str)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(str);
            }
        }

        static void Dump(string file)
        {

            bool bMDB = System.IO.File.Exists(file + ".mdb");
            string fpdb = file.Substring(0, file.Length - 4) + ".pdb";
            bool bPDB = System.IO.File.Exists(fpdb);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Begin Dump " + file);

            System.IO.Stream s = null;
            System.IO.Stream sp = null;

            try
            {
                var env = new CLRSharp.CLRSharp_Environment(new Logger());
                if (bPDB)
                {
                    Console.WriteLine("Load　dll　and  pdb");

                    s = System.IO.File.OpenRead(file);
                    sp = System.IO.File.OpenRead(fpdb);

                    env.LoadModule(s, sp, new Mono.Cecil.Pdb.PdbReaderProvider());

                }
                else if (bMDB)
                {
                    Console.WriteLine("Load　dll　and  mdb");

                    s = System.IO.File.OpenRead(file);
                    sp = System.IO.File.OpenRead(file + ".mdb");
                    {
                        env.LoadModule(s, sp, new Mono.Cecil.Mdb.MdbReaderProvider());
                    }
                }
                else
                {
                    Console.WriteLine("Load　dll");
                    s = System.IO.File.OpenRead(file);
                    {
                        env.LoadModule(s);
                    }
                }

                var types = env.GetAllTypes();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Got Type:" + types.Length);

                foreach (var t in types)
                {
                    var st = env.GetType(t) as CLRSharp.ICLRType_Sharp;
                    if (st == null) continue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("dump type:" + st.FullName);
                    Console.ForegroundColor = ConsoleColor.Gray;

                    var ms = st.type_CLRSharp.Methods;
                    if (ms == null || ms.Count == 0) continue;
                    foreach (var m in ms)
                    {
                        Console.WriteLine("==dump method:" + m);
                        if (m.HasBody)
                        {
                            foreach (Mono.Cecil.Cil.Instruction i in m.Body.Instructions)
                            {
                                string line = i.ToString();

                                //i.Offset
                                if (i.SequencePoint != null)
                                {
                                    line += "(" + i.SequencePoint.Document.Url + ")";
                                    line += " |(" + i.SequencePoint.StartLine + "," + i.SequencePoint.StartColumn + ")";

                                    //if (codes[i.SequencePoint.Document.Url] != null)
                                    //{
                                    //    int lines = i.SequencePoint.StartLine;
                                    //    if (lines - 1 < codes[i.SequencePoint.Document.Url].Length)
                                    //        line += "| " + codes[i.SequencePoint.Document.Url][i.SequencePoint.StartLine - 1];
                                    //}
                                }
                                Console.WriteLine(line);
                            }
                        }
                    }

                }

            }
            catch (Exception err)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("error:" + err.ToString());

            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
                if (sp != null)
                {
                    sp.Close();
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("End Dump " + file);
        }
    }
}
