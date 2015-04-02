using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
using UnityEngine;
namespace UnitTestDll
{
    public class UlngTest
    {
        public ulong aaa = 15645613;

        public static void UnitTest_TestThread()
        {
            Logger.Log("TestThread");

            TestClass.StartThread(() => { Logger.Log("In thread."); });
        }

        public static void DoUINt64(ulong a)
        {
            UnitTest.Logger.Log("a=" + a);
        }
        public static void UnitTest_UInt64()
        {
            UlngTest tt = new UlngTest();
            UlngTest.DoUINt64(tt.aaa);
        }

        private UlngTest()
        {

        }
        public static UlngTest g_this = null;
        public static UlngTest Instance
        {
            get
            {
                if (g_this == null)
                {
                    g_this = new UlngTest();
                }
                return g_this;
            }
        }
        public void Test(int abc)
        {
            abc = 3;
            Logger.Log("abc=" + abc);
        }
        public void Test2()
        {
            Logger.Log("c=3");
        }
    }
    //随手写的测试用例，目标是啥也不知道
    public class Test0
    {
        public static int[] arrI = new int[] { 1, 2, 3 };

        public static void UnitTest_StaticArr()
        {
            int i = 0;
            while (true)
            {
                if (arrI[i] != 1)
                {
                    throw new Exception("UnitTest_StaticArr");
                }
                break;
            }
            Logger.Log("OK");
        }



        public static void UnitTest_Instance()
        {
            UlngTest t = UlngTest.Instance;
            Logger.Log("c1");
            t.Test2();
            t.Test(5);
        }
        public static void UnitTest_passEnum()
        {
            UserData.ShowContry(Country.Chinese);
        }
        public static void UnitTest_delegate()
        {
            TestDele.myup calBak = new TestDele.myup(target1);
            calBak += target2;
        }

        static void target1()
        {
        }
        static void target2()
        {
        }
        public static void UnitTest_Type()
        {
            string str = "abcd";
            char ccc = str[0];
            Color32 c = new Color32(2, 2, 3, 5);
            c.TestType(typeof(Color32));
        }
        public struct MyS
        {
            public int i;
        }
        public enum EUIPanelID
        {
            NULL=0,
            INT1=1,
            INT2,
        };
        public static void UnitTest_Out()
        {
            EUIPanelID id = EUIPanelID.INT1;
            Color32 c = new Color32(2, 2, 3, 5);
            byte b = 0;
            c.GetA(out b);
            Logger.Log("a=" + b);
            c.GetB(ref b);
            Logger.Log("b=" + b);

        }
        public static void UnitTest_Arr()
        {
            var vectors = new Vector3[] { 
                new Vector3(335, 253, 0),
                new Vector3(335, 2533, 0), 
                new Vector3(335, 25332, 0)};

            foreach(var v in vectors)
            {
                Logger.Log("v=" + v.y);
            }
        }

        public static void UnitTest_Float()
        {
            Test0 t = new Test0();
            Vector3 v3 = new Vector3();
            t.CalFloat(v3);
        }

        void CalFloat(Vector3 v3)
        {
            float midy = (1 - v3.y) / 1;
        }
        public static void UnitTest_ListExt2()
        {
            List<int> a = new List<int>();
            a.Add(1);
            a.Add(22);
            a.Add(3);
            a.Sort((l, r) =>
                {
                    return r - l;
                });
            foreach(var i in a)
            {
                Logger.Log("i=" + i);
            }
        }
        public static void UnitTest_ListExt()
        {
            //GameObject obj = new GameObject();
            //GameObject obj2 = new GameObject();
            //List<int> int1 = new List<int>();
            //int ii = 1;
            Test02 t = new Test02();
            t.LogOut2(0, 1, 2, 333, 444, 5);
        }
        
        public static void UnitTest_ListInit()
        {
            List<int> lst = new List<int>();
            List<int> lst2 = new List<int>(lst);
        }
        public static void UnitTest_FuncM()
        {
            FuncM(0, 1, 2, 3, 4, 5);
        }

        static void FuncM(int m0, int m1, int m2, int m3, int m4, int m5)
        {
            Logger.Log("" + m0);
            Logger.Log("" + m1);
            Logger.Log("" + m2);
            Logger.Log("" + m3);
            Logger.Log("" + m4);
            Logger.Log("" + m5);

        }
        public static void UnitTest_Int()
        {
            float f = -1.5f;
            int i = (int)(-(f));
            Logger.Log("i=" + i);
            if (i != 1)
            {
                Logger.Log("" + i);
                throw new Exception();
            }
        }
        public static void UnitTest_Change()
        {
            int i = 1;
            i = -i;
            if (i != -1)
            {
                Logger.Log("" + i);
                throw new Exception();
            }
        }
        public static void UnitTest_V3()
        {
            Vector3 v = new Vector3(45, 5, 0);
            PassV3(v);
        }

        static void PassV3(Vector3 v3)
        {
            Logger.Log("" + v3.y);
        }
        public static void UnitTest_Judge()
        {
            int i = -1;
            int j = 2;
            if (i >= 0 && j < 2)
                Logger.Log("true");
            else
                Logger.Log("false"); // 应该输出这个

            Logger.Log("j=" + j);
            int a = j / 2;
            Logger.Log("a=" + a);
            if (a != 1)
            {
                Logger.Log("" + j);
                throw new Exception();
            }

        }
        public static void UnitTest_Math()
        {
            int i = 1;
            Math(1);
        }

        static void Math(int j)
        {
            Logger.Log("" + j);
            int a = j / 2;
            if (j != 1)
            {
                Logger.Log("" + j);
                throw new Exception();
            }
        }
        //只要有一个static void UnitTest() 函数的，就是单元测试
        public static void UnitTest_01()
        {
            UnityEngine.GameObject o = new UnityEngine.GameObject(25);
            Logger.Log("abc" + o.GetId());
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
            TestClass.TF(true);
            TestClass.instance.b = true;
            bool b = 3 == 3;
            bool b2 = 4 == 3;
            Logger.Log(TF(b2) + "," + TestClass.TF(b));
            int p = 1;
            float n = 5.0f;
            var i = 1 * p + 54 / n + 3334 * p + 54;
            Logger.Log("calc:" + i);
        }
        public static bool TF(bool v)
        {
            return v;
        }

        public static void tttt(int abc)
        {
            Logger.Log("tttt." + abc.ToString());
        }
        public static void UnitTest_NoName()
        {
            Action<int> _abc = (abc) =>
            {
                Logger.Log("callv3=" + abc);

            };
            _abc(234);
            Test02 ttt = new Test02();
            ttt.testdele = tttt;
            ttt.testdele = (abc) =>
            {
                Logger.Log("callv1=" + abc);

            };

            ttt.testdele(4567);
            ttt.TestCall((abc) =>
            {
                Logger.Log("callv2=" + abc);

            });
        }
        public static void UnitTest_if()
        {
            string s = "123";

            if (s != null && s != "")
            {
                Logger.Log(s);
            }

            if (s != "" && s != null) // 对调一下不行
            {
                Logger.Log(s);
            }
        }
        public static void UnitTest_Action()
        {
            Dictionary<short, Action> dic = new Dictionary<short, Action>();
            //short k = 0;
            //dic[k] = () => { Logger.Log("0"); };
            dic[(short)0] = () => { Logger.Log("0"); };
            dic[(short)0]();
        }
    }
}
