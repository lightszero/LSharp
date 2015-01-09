using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    public class Type_Common_CLRSharp : ICLRType
    {
        public System.Type TypeForSystem
        {
            get
            {
                return typeof(ICLRType);
            }
        }
        public Mono.Cecil.TypeDefinition type_CLRSharp;
        public Type_Common_CLRSharp(Mono.Cecil.TypeDefinition type)
        {
            this.type_CLRSharp = type;

        }
        public string Name
        {
            get { return type_CLRSharp.Name; }
        }

        public string FullName
        {
            get { return type_CLRSharp.FullName; }
        }
        public string FullNameWithAssembly
        {
            get
            {
                return type_CLRSharp.FullName;// +"," + type_CLRSharp.Module.Name;
            }
        }
        public IMethod GetMethod(string funcname, MethodParamList types)
        {
            if (type_CLRSharp.HasMethods)
            {
                foreach (var m in type_CLRSharp.Methods)
                {
                    if (m.Name != funcname) continue;
                    if ((types == null) ? !m.HasParameters : (m.Parameters.Count == types.Count))
                    {
                        bool match = true;
                        for (int i = 0; i < ((types == null) ? 0 : types.Count); i++)
                        {
                            if (m.Parameters[i].ParameterType.FullName != types[i].FullName)
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                            return new Method_Common_CLRSharp(this,m);
                    }
                }
            }
            return null;
        }
        public IMethod GetMethodT(string funcname, MethodParamList ttypes, MethodParamList types)
        {
            return null;
        }
        public IField GetField(string name)
        {
            foreach (var f in type_CLRSharp.Fields)
            {
                if (f.Name == name)
                {
                    return new Field_Common_CLRSharp(f);
                }
            }
            return null;
        }
        public bool IsInst(object obj)
        {
            return false;

        }


        public ICLRType GetNestType(ICLRSharp_Environment env, string fullname)
        {
            foreach (var stype in type_CLRSharp.NestedTypes)
            {
                if (stype.Name == fullname)
                {
                    var itype = new Type_Common_CLRSharp(stype);
                    env.RegType(itype);
                    return itype;
                }
            }
            return null;
        }
    }
    public class Method_Common_CLRSharp : IMethod
    {
        Type_Common_CLRSharp type;
        public Method_Common_CLRSharp(Type_Common_CLRSharp type, Mono.Cecil.MethodDefinition method)
        {

            if (method == null)
                throw new Exception("not allow null method.");
            this.type = type;
            method_CLRSharp = method;
        }
        public bool isStatic
        {
            get
            {
                return method_CLRSharp.IsStatic;
            }
        }
        public Mono.Cecil.MethodDefinition method_CLRSharp;

        public object Invoke(ThreadContext context, object _this, object[] _params)
        {
            if (method_CLRSharp.Name == ".ctor")
            {
                SInstance inst = new SInstance(type);
                context.ExecuteFunc(method_CLRSharp, inst, _params);
                return inst;
            }
            return context.ExecuteFunc(method_CLRSharp, _this, _params);
        }
    }
    public class SInstance
    {
        Type_Common_CLRSharp type;
        public SInstance(Type_Common_CLRSharp type)
        {
            this.type = type;
        }
        public Dictionary<string, object> Fields = new Dictionary<string, object>();

    }
    public class Field_Common_CLRSharp : IField
    {
        public Mono.Cecil.FieldDefinition field;
        public Field_Common_CLRSharp(Mono.Cecil.FieldDefinition field)
        {
            this.field = field;
        }

        public void Set(object _this, object value)
        {
            SInstance sins = _this as SInstance;
            sins.Fields[field.Name] = value;
        }

        public object Get(object _this)
        {
            SInstance sins = _this as SInstance;
            return sins.Fields[field.Name];
        }

        public bool isStatic
        {
            get { return this.field.IsStatic; }
        }
    }
}
