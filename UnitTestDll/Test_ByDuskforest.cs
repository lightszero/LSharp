using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTest;
using System.Collections;

namespace UnitTestDll
{
    class Test_ByDuskforest
    {
        public static void UnitTest_Bool2Object()
        {
            TestClass.TestObjectArg(true);
        }

        public static void UnitTest_Int2Byte()
        {
            TestClass.TestByteArg(32);
        }

        public static void UnitTest_CtorWithNoArg()
        {
            //这行代码放在Program.cs里面执行是不会报错的
            System.Activator.CreateInstance(Type.GetType("TestClass"));
        }

        public static void UnitTest_CtorWithByteArg()
        {
            //这行代码放在Program.cs里面执行是不会报错的
            System.Activator.CreateInstance(Type.GetType("TestClass"), new Object[] { (byte)32 });
        }
    }
}
