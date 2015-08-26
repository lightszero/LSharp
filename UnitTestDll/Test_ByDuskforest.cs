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
        public interface itest
        {
            System.Object testfunc();
        }

        public class testclass : itest
        {
            public System.Object testfunc()
            {
                return this;
            }
        }

        public static void UnitTest_interfacetest()
        {
            itest a = new testclass();
            itest b = a.testfunc() as itest;
            if (b == null)
            {
                throw new Exception("b = null");
            }
        }
        public static void UnitTest_casttest()
        {
            string str = "1";
            int a;
            int.TryParse(str, out a);
            int b = 1;
            Console.WriteLine("a=" + a + ",b=" + b);
            if (a == b)
            {
                Console.WriteLine("smm");
                //UnityEngine.Debug.Log(1);
            }
        }
        public static void UnitTest_Whilefloat()
        {
            System.Object a = 5.0f;

            while ((float)a > 0)
            {
                Console.WriteLine(a);
                break;
            }
        }
        public static void UnitTest_ByteCast()
        {
            Hashtable hash = new Hashtable();
            byte a = 2;
            hash.Add("state", a);
            byte index = (byte)hash["state"];

            switch (index)
            {
                case 1:
                    break;
                case 2:
                    Console.WriteLine("a=" + a);
                    break;
                default:
                    break;
            }
        }
        public static void UnitTest_String()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>()
           {
               {0, "今天天气不错风和日丽鸟语花香"}
           };

            int a = 5;
            float b = 5.01f;
            byte c = 5;
            string d = "5s";

            string test = dic[0] + "-" + b + "(" + dic[0];
            string test1 = dic[0] + "-" + a + "(" + dic[0];
            string test2 = dic[0] + "-" + b + "(" + dic[0];
            string test3 = dic[0] + "-" + c + "(" + dic[0];
            string test4 = dic[0] + "-" + d + "(" + dic[0];

            Console.WriteLine(test);
            Console.WriteLine(test1);
            Console.WriteLine(test2);
            Console.WriteLine(test3);
            Console.WriteLine(test4);
        }
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
            int[,] a = new int[1, 1];
            a[0, 0] = 4;
            Logger.Log("abc=" + a[0, 0]);
        }

        private static Object ReturnObject()
        {
            return 100;
        }

        public static void UnitTest_ConvertReturnObject2Float()
        {
            //在.net下不会报错，但是在unity3d下会报错，先把用例提交了
            if ((float)ReturnObject() != 0)
            {
                Logger.Log("100 != 0");
            }
        }
        public static void UnitTest_ConvertReturnObject2Double()
        {
            //在.net下不会报错，但是在unity3d下会报错，先把用例提交了
            if ((double)ReturnObject() != 0)
            {
                Logger.Log("100 != 0");
            }
        }
    }
}
