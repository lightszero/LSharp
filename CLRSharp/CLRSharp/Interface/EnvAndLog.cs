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
        void LoadModule(System.IO.Stream dllStream, System.IO.Stream pdbStream);
        string[] GetAllTypes();
        ICLRType GetType(string name, Mono.Cecil.ModuleDefinition module);
        ICLRSharp_Logger logger
        {
            get;
        }
    }
}
