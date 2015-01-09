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
                            return new Method_Common_CLRSharp(m);
                    }
                }
            }
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
    }
    public class Method_Common_CLRSharp : IMethod
    {
        public Method_Common_CLRSharp(Mono.Cecil.MethodDefinition method)
        {
            if (method == null)
                throw new Exception("not allow null method.");
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
            return context.ExecuteFunc(method_CLRSharp, _this, _params);
        }
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
            throw new NotImplementedException();
        }

        public object Get(object _this)
        {
            throw new NotImplementedException();
        }

        public bool isStatic
        {
            get { return this.field.IsStatic; }
        }
    }
}
