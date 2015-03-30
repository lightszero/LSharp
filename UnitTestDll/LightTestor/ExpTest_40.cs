using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
namespace UnitTestDll
{
    public class TestList
    {
        public TestList()
        {
            v = "bb1";
        }
        public string v;
    }
    public class ExpTest_40
    {
        //只要有一个静态函数包含UnitTest名称的，就作为单元测试
        public static object UnitTest_4001()
        {
            int ii = 0;
            int pp = ii++;
            Logger.Log("p=" + pp);
            LinkedList<int> ints2 = new LinkedList<int>();

            //4001 loops

            int b = 50;
            for (; b > 0; )
            {
                b -= 3;
                Logger.Log("for1");
            }
            for (int i = 0; i < 3; i++)
            {
                Logger.Log("for2");
                if (i % 2 == 0)
                {
                    continue;
                }
                Logger.Log("i=" + i);
                b++;
                if (b > 71)
                {
                    break;
                }
            }
            sbyte[] ints = new sbyte[3];
            ints[0] = 1;
            ints[1] = 2;
            ints[2] = 3;
            ints[1]++;
            ints[2]++;


            foreach (int k in ints)
            {
                Logger.Log(k + "," + 1);
            }
            return b;
        }

        public static object UnitTest_4002()
        {
            TestList[] tl = new TestList[3];
            tl[0]=new TestList();
            Logger.Log("v="+tl[0].v);
            List<TestList> tlist = new List<TestList>();

            List<IT4_Impl> it4p = new List<IT4_Impl>();
            it4p.Add(new IT4_Impl());
            //4002 loops
            int[] abc = new int[10];
            abc[0] = 1;
            abc[1] = 2;
            Logger.Log("abc.len=" + abc.Length + "," + abc[1]);
            Student[] st = new Student[10];
            st[1] = new Student();

            Logger.Log("st.len=" + st.Length + "," + st[1]);
            List<int> ints = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                ints.Add(i);
            }
            List<Student> st2 = new List<Student>();
            st2.Add(new Student());
            Logger.Log("ints.len=" + ints.Count + "," + ints[2]);
            Logger.Log("ints.len=" + st2.Count + "," + st2[0]);
            foreach (int k in ints)
            {
                if (k % 3 == 0)
                    Logger.Log(k + "," + 1);
                else
                    Logger.Log(k + "," + 3);
            }
            Logger.Log(99 + "," + 99);
            return "foreach";
        }
        public static object UnitTest_4003()
        {
            //4003 loops

            int i = 0;

            while (i < 10)
            {
                Logger.Log("i=" + i);
                i++;
            }

            do
            {
                Logger.Log("i=" + i);
                i++;
            } while (i < 10);

            return 0;
        }
        public static object UnitTest_4004()
        {
            int k = 2;
            int p = k - 1;
            //4003 loops
            //int i = 0;
            for (int i = 0; i < 100; i++)
            {
                switch (i % 7)
                {
                    case 1:
                        Logger.Log("got 7*n+1:" + i);
                        break;
                    case 2:
                        Logger.Log("got 7*n+2:" + i);
                        break;
                    default:
                        //Logger.Log("got other.");
                        break;
                }
            }
            return 0;
        }
    }
}
