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
        public static MethodParamList OneParam_Int
        {
            get
            {
                if (_OneParam_Int==null)
                {
                    _OneParam_Int = new MethodParamList();
                    _OneParam_Int.Add(new CLRSharp.Type_Common_System(typeof(int)));
                }
                return _OneParam_Int;
            }
        }
        public MethodParamList(CLRSharp_Environment env, Mono.Cecil.MethodReference method)
        {
            if (method.HasParameters)
            {
                foreach (var p in method.Parameters)
                {
                    string paramname = p.ParameterType.FullName;
                    if (p.ParameterType.IsGenericParameter)
                    {

                        var typegen = method.DeclaringType as Mono.Cecil.GenericInstanceType;
                        if (p.ParameterType.Name[0] == '!')
                        {
                            int index = int.Parse(p.ParameterType.Name.Substring(1));
                            paramname = typegen.GenericArguments[index].FullName;
                        }
                    }
                    this.Add(env.GetType(paramname, method.Module));
                }
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
