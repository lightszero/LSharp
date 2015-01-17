using CLRSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test01
{
    class MyCrossBind : CLRSharp.ICrossBind
    {
        public Type Type
        {
            get
            {
                return typeof(Interface.IMyType);
            }
        }

        public object CreateBind(CLRSharp.CLRSharp_Instance inst)
        {
            return new Base_IMyType(inst);
        }
        public class Base_IMyType : Interface.IMyType
        {
            CLRSharp_Instance inst;
            public Base_IMyType(CLRSharp.CLRSharp_Instance inst)
            {
                var context = ThreadContext.activeContext;
                this.inst = inst;
                var ms = this.inst.type.GetMethodNames();
                foreach (string name in ms)
                {
                    if (name.Contains("GetName"))
                        _GetName = this.inst.type.GetMethod(name, MethodParamList.MakeEmpty());
                    if (name.Contains("GetDesc"))
                        _GetDesc = this.inst.type.GetMethod(name, MethodParamList.MakeEmpty());
                }
            }
            IMethod _GetName;
            IMethod _GetDesc;


            public string GetName()
            {
                var context = ThreadContext.activeContext;
                var obj = _GetName.Invoke(context, inst, null);

                return obj as string;
            }

            public string GetDesc()
            {
                var context = ThreadContext.activeContext;
                var obj = _GetDesc.Invoke(context, inst, null);

                return obj as string;
            }
        }
    }
}
