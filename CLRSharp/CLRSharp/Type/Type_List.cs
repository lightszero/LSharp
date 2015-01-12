using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    /// <summary>
    /// 方法参数表
    /// </summary>
    public class MethodParamList : List<ICLRType>
    {
        private MethodParamList()
        {

        }
        static MethodParamList _OneParam_Int = null;
        public static MethodParamList MakeList_OneParam_Int(ICLRSharp_Environment env)
        {
            if (_OneParam_Int == null)
            {
                _OneParam_Int = new MethodParamList();
                _OneParam_Int.Add(env.GetType(typeof(int).FullName));
            }

            return _OneParam_Int;


        }
        static MethodParamList _ZeroParam = null;
        public static MethodParamList MakeEmpty()
        {
            if (_ZeroParam == null)
            {
                _ZeroParam = new MethodParamList();
            }
            return _ZeroParam;
        }
        public MethodParamList(ICLRSharp_Environment env, Mono.Cecil.MethodReference method)
        {
            if (method.HasParameters)
            {
                Mono.Cecil.GenericInstanceType _typegen = null;
                _typegen = method.DeclaringType as Mono.Cecil.GenericInstanceType;
                Mono.Cecil.GenericInstanceMethod gm = method as Mono.Cecil.GenericInstanceMethod;
                MethodParamList _methodgen = null;
                if (gm != null)
                    _methodgen = new MethodParamList(env, gm);
                foreach (var p in method.Parameters)
                {
                    string paramname = p.ParameterType.FullName;

                    if (p.ParameterType.IsGenericParameter)
                    {
                        if (p.ParameterType.Name.Contains("!!"))
                        {

                            int index = int.Parse(p.ParameterType.Name.Substring(2));
                            paramname = _methodgen[index].FullName;
                        }
                        else if (p.ParameterType.Name.Contains("!"))
                        {


                            int index = int.Parse(p.ParameterType.Name.Substring(1));
                            paramname = _typegen.GenericArguments[index].FullName;
                        }
                    }

                    if (paramname.Contains("!!"))
                    {
                        this.Add(GetTType(env, p,  _methodgen));
                    }
                    else
                    {
                        this.Add(env.GetType(paramname));
                    }
                }
            }
        }
        public MethodParamList(ICLRSharp_Environment env, Mono.Collections.Generic.Collection<Mono.Cecil.Cil.VariableDefinition> ps)
        {
            foreach (var p in ps)
            {
                string paramname = p.VariableType.FullName;

                this.Add(env.GetType(paramname));

            }
        }
        ICLRType GetTType(ICLRSharp_Environment env, Mono.Cecil.ParameterDefinition param,  MethodParamList _methodgen)
        {
            string typename = param.ParameterType.FullName;
            for (int i = 0; i < _methodgen.Count; i++)
            {
                string p = "!!" + i.ToString();
                typename = typename.Replace(p, _methodgen[i].FullName);
            }
            return env.GetType(typename);
        }
        public MethodParamList(ICLRSharp_Environment env, Mono.Cecil.GenericInstanceMethod method)
        {
            foreach (var p in method.GenericArguments)
            {
                string paramname = p.FullName;
                if (p.IsGenericParameter)
                {

                    var typegen = method.DeclaringType as Mono.Cecil.GenericInstanceType;
                    if (p.Name[0] == '!')
                    {
                        int index = int.Parse(p.Name.Substring(1));
                        paramname = typegen.GenericArguments[index].FullName;
                    }
                }
                this.Add(env.GetType(paramname));
            }
        }

        public MethodParamList(ICLRSharp_Environment env, System.Reflection.MethodBase method)
        {
            foreach (var p in method.GetParameters())
            {
                this.Add(env.GetType(p.ParameterType));
            }
        }
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

                    SystemType[i] = this[i].TypeForSystem;
                }
            }
            return SystemType;
        }
    }

}
