using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
namespace UnitTestDll
{
    public class Class1
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
            int p = 1;
            float n = 5.0f;
            var i =1*p+54/n+3334*p+54;
            Logger.Log("calc:" + i);
        }
    }
}
