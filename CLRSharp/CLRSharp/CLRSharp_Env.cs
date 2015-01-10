using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{

    public class CLRSharp_Environment : ICLRSharp_Environment
    {
        public string version
        {
            get
            {
                return "0.10Alpha";
            }
        }
        public ICLRSharp_Logger logger
        {
            get;
            private set;
        }
        public CLRSharp_Environment(ICLRSharp_Logger logger)
        {
            this.logger = logger;
            logger.Log_Warning("CLR# Ver:" + version + " Inited.");
        }
        Dictionary<string, ICLRType> mapType = new Dictionary<string, ICLRType>();
        //public Dictionary<string, Mono.Cecil.ModuleDefinition> mapModule = new Dictionary<string, Mono.Cecil.ModuleDefinition>();

        public void LoadModule(System.IO.Stream dllStream, System.IO.Stream pdbStream)
        {
            var module = Mono.Cecil.ModuleDefinition.ReadModule(dllStream);
            if (pdbStream != null)
            {
                module.ReadSymbols(new Mono.Cecil.Pdb.PdbReaderProvider().GetSymbolReader(module, pdbStream));
            }
            //mapModule[module.Name] = module;
            if (module.HasTypes)
            {
                foreach (var t in module.Types)
                {
                    mapType[t.FullName] = new Type_Common_CLRSharp(this,t);
                }
            }

        }
        public string[] GetAllTypes()
        {
            string[] array = new string[mapType.Count];
            mapType.Keys.CopyTo(array, 0);
            return array;
        }
        //得到类型的时候应该得到模块内Type或者真实Type
        //一个统一的Type,然后根据具体情况调用两边

        public ICLRType GetType(string fullname, Mono.Cecil.ModuleDefinition module)
        {
            ICLRType type = null;
            bool b = mapType.TryGetValue(fullname, out type);
            if (!b)
            {
                if (fullname.Contains("<>"))//匿名类型
                {
                    string[] subts = fullname.Split('/');
                    ICLRType ft = GetType(subts[0], module);
                    for (int i = 1; i < subts.Length; i++)
                    {
                        ft = ft.GetNestType(this, subts[i]);
                    }
                    return ft;
                }
                string fullnameT = fullname.Replace('/', '+');

                if (fullnameT.Contains("<"))
                {
                    string outname = "";
                    int depth = 0;
                    int lastsplitpos = 0;
                    for (int i = 0; i < fullname.Length; i++)
                    {
                        string checkname = null;
                        if (fullname[i] == '/')
                        {

                        }
                        else if (fullname[i] == '<')
                        {
                            depth++;
                            if (depth == 1)//
                            {
                                lastsplitpos = i;
                                outname += "[";
                                continue;
                            }

                        }
                        else if (fullname[i] == '>')
                        {
                            if (depth == 1)
                            {
                                checkname = fullnameT.Substring(lastsplitpos + 1, i - lastsplitpos - 1);
                                var subtype = GetType(checkname, module);
                                outname += "[" + subtype.FullNameWithAssembly + "]";
                                lastsplitpos = i;
                            }
                            depth--;
                            if (depth == 0)
                            {
                                outname += "]";
                                continue;
                            }
                        }
                        else if (fullname[i] == ',')
                        {
                            if (depth == 1)
                            {
                                checkname = fullnameT.Substring(lastsplitpos + 1, i - lastsplitpos - 1);
                                var subtype = GetType(checkname, module);
                                outname += "[" + subtype.FullNameWithAssembly + "],";
                                lastsplitpos = i;
                            }
                        }
                        if (depth == 0)
                        {
                            outname += fullnameT[i];
                        }
                    }
                    fullnameT = outname;
                    //    fullnameT = fullnameT.Replace('<', '[');
                    //fullnameT = fullnameT.Replace('>', ']');


                }

                System.Type t = System.Type.GetType(fullnameT);

                if (t == null && module != null && module.HasAssemblyReferences)
                {

                    foreach (var rm in module.AssemblyReferences)
                    {
                        t = System.Type.GetType(fullnameT + "," + rm.Name);
                        if (t != null)
                        {
                            fullnameT = fullnameT + "," + rm.Name;
                            break;
                        }
                    }

                }
                if (t != null)
                {
                    type = new Type_Common_System(this, t, fullnameT);
                }
                mapType[fullname] = type;
            }
            return type;
        }


        public ICLRType GetType(System.Type systemType)
        {
            ICLRType type = null;
            bool b = mapType.TryGetValue(systemType.FullName, out type);
            if (!b)
            {
                type = new Type_Common_System(this, systemType, systemType.FullName);
            }
            return type;
        }
        public void RegType(ICLRType type)
        {
            mapType[type.FullName] = type;
        }
    }
}
