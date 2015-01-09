using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    //一个ICLRType 是一个所有类型的抽象，无论是System.Type
    //还是CLRSharp的抽象，均可通过ICLRType进行调用
    public interface ICLRType
    {
        string Name
        {
            get;
        }
        string FullName
        {
            get;
        }
        string FullNameWithAssembly
        {
            get;
        }
        System.Type TypeForSystem
        {
            get;
        }
        //funcname==".ctor" 表示构造函数
        IMethod GetMethod(string funcname, MethodParamList types);
        IField GetField(string name);
    }
    public interface IMethod
    {
        object Invoke(ThreadContext context, object _this, object[] _params);
        bool isStatic
        {
            get;
        }
    }
    public interface IField
    {
        void Set(object _this, object value);
        object Get(object _this);
        bool isStatic
        {
            get;
        }

    }

}
