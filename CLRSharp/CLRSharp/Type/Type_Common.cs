using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    public class TypeList : List<Type_Common>
    {
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public override string ToString()
        {
            if (name == null)
            {
                name = "";
                foreach (var t in this)
                {
                    name += t.ToString() + ";";
                }
            }
            return name;
        }
        string name = null;
        System.Type[] SystemType = null;
        public System.Type[] ToArraySystem()
        {
            if (SystemType == null)
            {
                SystemType = new System.Type[this.Count];
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].type_System != null)
                    {
                        SystemType[i] = this[i].type_System;
                    }
                    else
                    {
                        SystemType[i] = typeof(object);
                    }
                }
            }
            return SystemType;
        }
    }
    public class Type_Common
    {
        public override string ToString()
        {
            return (type_System != null) ? ("S:" + type_System.FullName) : ("V:" + type_CLRSharp.FullName);
        }
        public string FullName
        {
            get
            {
                return (type_System != null) ? type_System.FullName : type_CLRSharp.FullName;
            }
        }
        public Type_Common(Mono.Cecil.TypeDefinition type)
        {
            if (type == null)
                throw new Exception("not allow null type.");
            type_CLRSharp = type;
        }
        public Type_Common(System.Type type)
        {
            if (type == null)
                throw new Exception("not allow null type.");
            type_System = type;
        }
        public Mono.Cecil.TypeDefinition type_CLRSharp
        {
            get;
            private set;
        }
        public System.Type type_System
        {
            get;
            private set;
        }
        Dictionary<string, Mono.Cecil.MethodDefinition> mapMethod = null;
        void InitMethods()
        {
            mapMethod = new Dictionary<string, Mono.Cecil.MethodDefinition>();
            if (type_CLRSharp.HasMethods)
            {
                foreach (var m in type_CLRSharp.Methods)
                {
                    mapMethod[m.Name] = m;
                }
            }
        }
        public Method_Common GetMethod(string funcname, TypeList types)
        {
            if (type_System != null)
            {
                
                if(funcname==".ctor")
                {
                    var con = type_System.GetConstructor(types.ToArraySystem());
                    return new Method_Common(con);
                }
                var method = type_System.GetMethod(funcname, types.ToArraySystem());
                return new Method_Common(method);
            }
            else
            {
                if (type_CLRSharp.HasMethods)
                {
                    foreach (var m in type_CLRSharp.Methods)
                    {
                        if (m.Name != funcname) continue;
                        if ((types==null)?!m.HasParameters:(m.Parameters.Count == types.Count))
                        {
                            bool match = true;
                            for (int i = 0; i <((types==null)?0: types.Count); i++)
                            {
                                if (m.Parameters[i].ParameterType.FullName != types[i].FullName)
                                {
                                    match = false;
                                    break;
                                }
                            }
                            if (match)
                                return new Method_Common(m);
                        }
                    }
                }

            }
            return null;
        }

    }
    public class Method_Common
    {
        public Method_Common(Mono.Cecil.MethodDefinition method)
        {
            if (method == null)
                throw new Exception("not allow null method.");
            method_CLRSharp = method;
        }
        public Method_Common(System.Reflection.MethodBase method)
        {
            if (method == null)
                throw new Exception("not allow null method.");
            method_System = method;
        }
        public Mono.Cecil.MethodDefinition method_CLRSharp
        {
            get;
            private set;
        }
        public System.Reflection.MethodBase method_System
        {
            get;
            private set;
        }

        public object Invoke(Context context, object _this, object[] _params)
        {
            if(method_System!=null)
            {
                if (method_System is System.Reflection.ConstructorInfo)
                {
                    StackFrame.RefObj obj = _this as StackFrame.RefObj;
                    var newobj = (method_System as System.Reflection.ConstructorInfo).Invoke(_params);
                    if(obj!=null)
                        obj.Set(newobj);
                    return newobj;
                }
                else
                {
                    return method_System.Invoke(_this, _params);
                }
            }
            else
            {
                return context.ExecuteFunc(method_CLRSharp, _this, _params);
            }
        }
    }
}
