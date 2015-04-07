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
            System.Activator.CreateInstance(Type.GetType("TestClass,UnitTest"));
        }

        public static void UnitTest_CtorWithByteArg()
        {
            System.Activator.CreateInstance(Type.GetType("TestClass,UnitTest"), new Object[] { (byte)32 });
        }

        public static void UnitTest_2DimArray()
        {
            int[,] a = new int[1,1];
            a[0, 0] = 4;
            Logger.Log("abc=" + a[0, 0]);
        }

        private static Object ReturnObject()
        {
            return 100;
        }

        public static void UnitTest_ConvertReturnObject2Float()
        {
            if ((float)ReturnObject() != 0)
            {
                Logger.Log("100 != 0");
            }
        }
    }
}
