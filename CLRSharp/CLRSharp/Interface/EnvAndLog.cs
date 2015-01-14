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

        void LoadModule(System.IO.Stream dllStream, System.IO.Stream pdbStream, Mono.Cecil.Cil.ISymbolReaderProvider debugInfoLoader);

        string[] GetAllTypes();
        ICLRType GetType(string name);

        string[] GetModuleRefNames();
        ICLRType GetType(System.Type systemType);

        void RegType(ICLRType type);
        ICLRSharp_Logger logger
        {
            get;
        }

        void RegCrossBind(ICrossBind bind);

        ICrossBind GetCrossBind(Type type);

    }
    public interface ICrossBind
    {
        Type Type
        { get; }
        object CreateBind(CLRSharp_Instance inst);
    }
}
