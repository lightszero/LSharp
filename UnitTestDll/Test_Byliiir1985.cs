using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTest;
using System.Collections;

namespace UnitTestDll
{
    class Test_Byliiir1985
    {
        /// <summary>
        /// 性能测试
        /// </summary>
        /// <returns></returns>
        public static void UnitTest_Performance()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            int cnt = 0;
            for (int i = 0; i < 100000; i++)
            {
                cnt += i;
            }
            sw.Stop();

            Logger.Log(string.Format("Elapsed time:{0:0}ms, result = {1}", sw.ElapsedMilliseconds, cnt));

            sw.Reset();
            sw.Start();
            cnt = 0;
            for (int i = 0; i < 100000; i++)
            {
                FuncCallResult(ref cnt, i);
            }
            sw.Stop();

            Logger.Log(string.Format("Elapsed time:{0:0}ms, result = {1}", sw.ElapsedMilliseconds, cnt));
        }

        public static void UnitTest_Cls()
        {
            Test1098Cls cls = new Test1098Cls();
            Test1098Sub(cls);

            Logger.Log(string.Format("A={0} B={1}", cls.A, cls.B));
        }

        public static void UnitTest_Generics()
        {
            //如果一个类继承一个泛型参数为这个类本身的泛型类，就没法正确找到该类型了
            Logger.Log(SingletonTest.Inst.foo());
        }

        public static void UnitTest_NestedGenerics()
        {
            //如果一个嵌套的类是泛型类参数，则这个类无法被找到
            Logger.Log(new NestedTestBase<NestedTest>().ToString());
        }

        class Test1098Cls
        {
            public int A { get; set; }
            public string B { get; set; }
        }

        static void Test1098Sub(Test1098Cls cls)
        {
            cls.A = 2;
            cls.B = "ok";
        }

        static void FuncCallResult(ref int cnt, int i)
        {
            cnt++;
        }

        class NestedTest
        {

        }

        class NestedTestBase<T>
        {

        }
    }

    class SingletonTest : Singleton<SingletonTest>
    {
        public string foo()
        {
            return "bar";
        }
    }
    class Singleton<T> where T : new()
    {
        private static T _inst;

        protected Singleton()
        {
        }

        public static T Inst
        {
            get
            {
                if (Singleton<T>._inst == null)
                {
                    Singleton<T>._inst = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
                }
                return Singleton<T>._inst;
            }
        }
    }
}
