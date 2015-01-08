using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    public interface ICLRSharp_Logger
    {
        void Log(string str);
        void Log_Warning(string str);
        void Log_Error(string str);
    }
    public interface ICLRSharp_Environment
    {
        string version
        {
            get;
        }
        void LoadModule(System.IO.Stream dllStream);
        string[] GetAllTypes();
        Type_Common GetType(string name);
    }
    public class CLRSharp_Environment : ICLRSharp_Environment
    {
        public string version
        {
            get
            {
                return "0.02Alpha";
            }
        }
        Dictionary<string, Type_Common> mapType = new Dictionary<string, Type_Common>();
        Dictionary<string, Mono.Cecil.ModuleDefinition> mapModule = new Dictionary<string, Mono.Cecil.ModuleDefinition>();
        public void LoadModule(System.IO.Stream dllStream)
        {
            var module = Mono.Cecil.ModuleDefinition.ReadModule(dllStream);
            mapModule[module.Name] = module;
            if (module.HasTypes)
            {
                foreach (var t in module.Types)
                {
                    mapType[t.FullName] = new Type_Common(t);
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
        public Type_Common GetType(string name)
        {
            Type_Common type = null;
            mapType.TryGetValue(name, out type);
            return type;
        }
    }
}
