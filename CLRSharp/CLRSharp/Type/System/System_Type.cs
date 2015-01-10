using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    class Type_Common_System : ICLRType_System
    {
        public System.Type TypeForSystem
        {
            get;
            private set;
        }
        public ICLRSharp_Environment env
        {
            get;
            private set;
        }
        public Type_Common_System(ICLRSharp_Environment env, System.Type type, string aname)
        {
            this.env = env;
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
                return new Method_Common_System(this, con);
            }
            var method = TypeForSystem.GetMethod(funcname, types.ToArraySystem());
            return new Method_Common_System(this, method);
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

            return new Method_Common_System(this, _method.MakeGenericMethod(ttypes.ToArraySystem()));
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

        public Delegate CreateDelegate(Type deletype,object _this, IMethod_System _method)
        {
            return Delegate.CreateDelegate(deletype, _this, _method.method_System as System.Reflection.MethodInfo);
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

    class Method_Common_System : IMethod_System
    {

        public Method_Common_System(ICLRType DeclaringType, System.Reflection.MethodBase method)
        {
            if (method == null)
                throw new Exception("not allow null method.");
            method_System = method;
            this.DeclaringType = DeclaringType;
            if (method is System.Reflection.MethodInfo)
            {
                System.Reflection.MethodInfo info = method as System.Reflection.MethodInfo;
                ReturnType = DeclaringType.env.GetType(info.ReturnType);
            }
            ParamList =new MethodParamList(DeclaringType.env,method);
        }
        public bool isStatic
        {
            get { return method_System.IsStatic; }
        }
        public string Name
        {
            get
            {
                return method_System.Name;
            }
        }

        public ICLRType DeclaringType
        {
            get;
            private set;

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
        public System.Reflection.MethodBase method_System
        {
            get;
            private set;
        }

        public object Invoke(ThreadContext context, object _this, object[] _params)
        {
            //委托是很特殊的存在
            //if(this.DeclaringType.IsDelegate)
            //{

            //}
            if (method_System is System.Reflection.ConstructorInfo)
            {
                if (method_System.DeclaringType.IsSubclassOf(typeof(Delegate)))
                {//创建委托
                    object src = _params[0];
                    RefFunc fun = _params[1] as RefFunc;
                    ICLRType_Sharp clrtype = fun._method.DeclaringType as ICLRType_Sharp;
                    if(clrtype!=null)//onclr
                    {
                     
                        CLRSharp_Instance inst = src as CLRSharp_Instance;
                        if (fun._method.isStatic && clrtype != null)
                            inst = clrtype.staticInstance;
                        return inst.GetDelegate(context, method_System.DeclaringType,fun._method);
                    }
                    else//onsystem
                    {
                        ICLRType_System stype = fun._method.DeclaringType as ICLRType_System;
                        return stype.CreateDelegate(method_System.DeclaringType,src, fun._method as IMethod_System);
                    }
                }
                var newobj = (method_System as System.Reflection.ConstructorInfo).Invoke(_params);
                return newobj;
            }
            else
            {
                //if (method_System.DeclaringType.IsSubclassOf(typeof(Delegate)))//直接用Delegate.Invoke,会导致转到本机代码再回来
                ////会导致错误堆栈不方便观察,但是也没办法直接调用，只能手写一些常用类型
                //{
                //    //需要从Delegate转换成实际类型执行的帮助类
                //    Action<int> abc = _this as Action<int>;
                //    abc((int)_params[0]);
                //    return null;
                //}
                //else
                {
                    return method_System.Invoke(_this, _params);
                }
            }

        }



    }

}
