using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    class Type_Common_System : ICLRType
    {
        public System.Type TypeForSystem
        {
            get;
            private set;
        }
        public Type_Common_System(System.Type type, string aname)
        {
            this.TypeForSystem = type;
            FullNameWithAssembly = aname;
        }
        public string Name
        {
            get { return TypeForSystem.Name; }
        }

        public string FullName
        {
            get { return TypeForSystem.FullName; }
        }
        public string FullNameWithAssembly
        {
            get;
            private set;

            //{
            //    string aname = TypeForSystem.AssemblyQualifiedName;
            //    int i = aname.IndexOf(',');
            //    i = aname.IndexOf(',', i + 1);
            //    return aname.Substring(0, i);
            //}
        }
        public IMethod GetMethod(string funcname, MethodParamList types)
        {
            if (funcname == ".ctor")
            {
                var con = TypeForSystem.GetConstructor(types.ToArraySystem());
                return new Method_Common_System(con);
            }
            var method = TypeForSystem.GetMethod(funcname, types.ToArraySystem());
            return new Method_Common_System(method);
        }
        public IMethod GetMethodT(string funcname, MethodParamList ttypes, MethodParamList types)
        {
            //这个实现还不完全
            //有个别重构下，判定比这个要复杂
            System.Reflection.MethodInfo _method = null;
            var ms = TypeForSystem.GetMethods();
            foreach (var m in ms)
            {
                if (m.Name == funcname && m.IsGenericMethodDefinition)
                {
                    var ts = m.GetGenericArguments();
                    var ps = m.GetParameters();
                    if (ts.Length == ttypes.Count && ps.Length == types.Count)
                    {
                        _method = m;
                        break;
                    }

                }
            }

            // _method = TypeForSystem.GetMethod(funcname, types.ToArraySystem());

            return new Method_Common_System(_method.MakeGenericMethod(ttypes.ToArraySystem()));
        }
        public IField GetField(string name)
        {
            return new Field_Common_System(TypeForSystem.GetField(name));
        }
        public bool IsInst(object obj)
        {
            return TypeForSystem.IsInstanceOfType(obj);

        }


        public ICLRType GetNestType(ICLRSharp_Environment env, string fullname)
        {
            throw new NotImplementedException();
        }
    }
    class Field_Common_System : IField
    {
        public System.Reflection.FieldInfo info;
        public Field_Common_System(System.Reflection.FieldInfo field)
        {
            info = field;
        }

        public void Set(object _this, object value)
        {
            info.SetValue(_this, value);
        }

        public object Get(object _this)
        {
            return info.GetValue(_this);
        }

        public bool isStatic
        {
            get { return info.IsStatic; }
        }
    }

    class Method_Common_System : IMethod
    {

        public Method_Common_System(System.Reflection.MethodBase method)
        {
            if (method == null)
                throw new Exception("not allow null method.");
            method_System = method;
        }
        public bool isStatic
        {
            get { return method_System.IsStatic; }
        }

        public System.Reflection.MethodBase method_System;

        public object Invoke(ThreadContext context, object _this, object[] _params)
        {

            if (method_System is System.Reflection.ConstructorInfo)
            {
                var newobj = (method_System as System.Reflection.ConstructorInfo).Invoke(_params);
                return newobj;
            }
            else
            {
                return method_System.Invoke(_this, _params);
            }

        }



    }

}
