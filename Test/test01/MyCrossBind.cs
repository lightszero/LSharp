using CLRSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test01
{
    class MyCrossBind : CLRSharp.ICrossBind
    {
        //CrossBind 是提供一种模拟的方式，让脚本看上去好像继承了程序
        //脚本内部的实例类型均为CLRSharp.CLRSharp_Instance
        //CrossBind首先有一个Type接口，指定CrossBind作用于哪个真实类型
        //到脚本继承到CrossBind指定的Type时，其实脚本并不能真的继承他。
        //就调用对应的ICrossBind::CreateBind(脚本实例)
        public Type Type
        {
            get
            {
                return typeof(Interface.IMyType);
            }
        }

        /// <summary>
        /// 创建真正的绑定实例
        /// </summary>
        /// <param name="inst">传入的脚本实例</param>
        /// <returns></returns>
        public object CreateBind(CLRSharp.CLRSharp_Instance inst)
        {
            return new Base_IMyType(inst);
        }
        /// <summary>
        /// 绑定实例继承自真正想要继承的类型
        /// 实现其所有的方法，在其方法内通过脚本的实例调用对应的脚本方法
        /// </summary>
        public class Base_IMyType : Interface.IMyType
        {
            //脚本实例肯定要保存，当从程序中调用方法时，要通过他去调用脚本的
            CLRSharp_Instance inst;

            /// <summary>
            /// 构造函数只是为了把脚本实例传进来，用其他的形式也可以
            /// </summary>
            /// <param name="inst"></param>
            public Base_IMyType(CLRSharp.CLRSharp_Instance inst)
            {
                var context = ThreadContext.activeContext;
                this.inst = inst;
                //这里先把需要的脚本方法记下来,只是为了后面调用起来方便
                var ms = this.inst.type.GetMethodNames();
                foreach (string name in ms)
                {
                    if (name.Contains("GetName"))
                        _GetName = this.inst.type.GetMethod(name, MethodParamList.constEmpty());
                    if (name.Contains("GetDesc"))
                        _GetDesc = this.inst.type.GetMethod(name, MethodParamList.constEmpty());
                    if (name.Contains("SetName"))
                        _SetName = this.inst.type.GetMethod(name, MethodParamList.Make(context.environment.GetType(typeof(string))));
                }
            }
            IMethod _GetName;
            IMethod _GetDesc;
            IMethod _SetName;


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

            public void SetName(string name)
            {
                var context = ThreadContext.activeContext;
                var obj = _SetName.Invoke(context, inst, new object[]{name});
            }
        }
    }
}
