using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    public class Type_Common_CLRSharp : ICLRType_Sharp
    {
        public System.Type TypeForSystem
        {
            get
            {
                return typeof(CLRSharp_Instance);
            }
        }
        public Mono.Cecil.TypeDefinition type_CLRSharp;
        public ICLRSharp_Environment env
        {
            get;
            private set;
        }
        public ICLRType[] SubTypes
        {
            get;
            private set;
        }
        public Type_Common_CLRSharp(ICLRSharp_Environment env, Mono.Cecil.TypeDefinition type)
        {
            this.env = env;
            this.type_CLRSharp = type;
            foreach (var m in this.type_CLRSharp.Methods)
            {
                if (m.Name == ".cctor")
                {
                    NeedCCtor = true;
                    break;
                }
            }

        }
        public void ResetStaticInstace()
        {
            this._staticInstance = null;
            foreach (var m in this.type_CLRSharp.Methods)
            {
                if (m.Name == ".cctor")
                {
                    NeedCCtor = true;
                    break;
                }
            }

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
                            return new Method_Common_CLRSharp(this, m);
                    }
                }
            }
            return null;
        }
        public object InitObj()
        {
            return new CLRSharp_Instance(this);
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
                    return new Field_Common_CLRSharp(this, f);
                }
            }
            return null;
        }
        public bool IsInst(object obj)
        {
            if (obj is CLRSharp_Instance)
            {
                CLRSharp_Instance ins = obj as CLRSharp_Instance;
                if (ins.type == this)
                {
                    return true;
                }
                //这里还要实现继承关系
            }
            return false;

        }

        public ICLRType GetNestType(ICLRSharp_Environment env, string fullname)
        {
            foreach (var stype in type_CLRSharp.NestedTypes)
            {
                if (stype.Name == fullname)
                {
                    var itype = new Type_Common_CLRSharp(env, stype);
                    env.RegType(itype);
                    return itype;
                }
            }
            return null;
        }

        CLRSharp_Instance _staticInstance = null;
        public CLRSharp_Instance staticInstance
        {
            get
            {
                if (_staticInstance == null)
                    _staticInstance = new CLRSharp_Instance(this);
                return _staticInstance;
            }
        }

        public bool NeedCCtor
        {
            get;
            private set;
        }
        public void InvokeCCtor(ThreadContext context)
        {
            NeedCCtor = false;
            this.GetMethod(".cctor", null).Invoke(context, this.staticInstance, new object[] { });

        }


        public string[] GetFieldNames()
        {
            string[] abc = new string[type_CLRSharp.Fields.Count];
            for (int i = 0; i < type_CLRSharp.Fields.Count; i++)
            {
                abc[i] = type_CLRSharp.Fields[i].Name;
            }
            return abc;
        }
    }
    public class Method_Common_CLRSharp : IMethod_Sharp
    {
        Type_Common_CLRSharp _DeclaringType;

        public Method_Common_CLRSharp(Type_Common_CLRSharp type, Mono.Cecil.MethodDefinition method)
        {

            if (method == null)
                throw new Exception("not allow null method.");
            this._DeclaringType = type;

            method_CLRSharp = method;
            ReturnType = type.env.GetType(method.ReturnType.FullName);
            ParamList = new MethodParamList(type.env, method);
        }
        public string Name
        {
            get
            {
                return method_CLRSharp.Name;

            }
        }

        public bool isStatic
        {
            get
            {
                return method_CLRSharp.IsStatic;
            }
        }
        public ICLRType DeclaringType
        {
            get
            {
                return _DeclaringType;
            }
        }
        public ICLRType ReturnType
        {
            get;
            private set;


        }
        public MethodParamList ParamList
        {
            get;
            private set;
        }
        public Mono.Cecil.MethodDefinition method_CLRSharp;

        public object Invoke(ThreadContext context, object _this, object[] _params)
        {
            if (context == null)
                context = ThreadContext.activeContext;
            if (context == null)
                throw new Exception("这个线程上没有CLRSharp:ThreadContext");
            if (method_CLRSharp.Name == ".ctor")
            {

                CLRSharp_Instance inst = new CLRSharp_Instance(_DeclaringType);

                context.ExecuteFunc(this, inst, _params);
                return inst;
            }
            return context.ExecuteFunc(this, _this, _params);
        }

        CodeBody _body = null;
        public CodeBody body
        {
            get
            {
                if (_body == null)
                {
                    if (!method_CLRSharp.HasBody)
                        return null;
                    _body = new CodeBody(this.DeclaringType.env, method_CLRSharp);
                }
                return _body;
            }

        }
    }

    public class Field_Common_CLRSharp : IField
    {
        public Type_Common_CLRSharp _DeclaringType;
        public Mono.Cecil.FieldDefinition field;
        public Field_Common_CLRSharp(Type_Common_CLRSharp type, Mono.Cecil.FieldDefinition field)
        {
            this.field = field;
            this.FieldType = type.env.GetType(field.FieldType.FullName);
            this._DeclaringType = type;

        }
        public ICLRType FieldType
        {
            get;
            private set;
        }
        public ICLRType DeclaringType
        {
            get
            {
                return _DeclaringType;
            }
        }
        public void Set(object _this, object value)
        {
            CLRSharp_Instance sins = null;
            if (_this == null)
            {
                sins = _DeclaringType.staticInstance;
            }
            else
            {
                sins = _this as CLRSharp_Instance;
            }


            sins.Fields[field.Name] = value;
        }

        public object Get(object _this)
        {
            CLRSharp_Instance sins = null;
            if (_this == null)
            {
                sins = _DeclaringType.staticInstance;
            }
            else
            {
                sins = _this as CLRSharp_Instance;
            }
            object v = null;
            sins.Fields.TryGetValue(field.Name, out v);
            return v;
        }

        public bool isStatic
        {
            get { return this.field.IsStatic; }
        }
    }
}
