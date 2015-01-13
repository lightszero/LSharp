using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
namespace UnitTestDll
{
    //随手写的测试用例，目标是啥也不知道
    public class Test0
    {
        //只要有一个static void UnitTest() 函数的，就是单元测试
        public static void UnitTest_01()
        {
            Logger.Log("abc");
            Console.WriteLine("aaaa");
        }
        public static void UnitTest_02()
        {
            for (int i = 0; i < 10; i++)
            {
                Logger.Log("abc");
            }

        }
        public static void UnitTest_03()
        {
            bool b = 3==3;
            bool b2 = 4 == 3;
            Logger.Log(TF(b2) + "," + TestClass.TF (b));
            int p = 1;
            float n = 5.0f;
            var i = 1 * p + 54 / n + 3334 * p + 54;
            Logger.Log("calc:" + i);
        }
        public static bool TF(bool v)
        {
            return v;
        }
  
    }
}
