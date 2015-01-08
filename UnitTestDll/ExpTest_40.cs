using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
namespace UnitTestDll
{
    public class ExpTest_40
    {
        //只要有一个静态函数包含UnitTest名称的，就作为单元测试
        public static object UnitTest_4001()
        {
            //4001 loops

            int b = 50;
            for (; b > 0; )
            {
                b -= 3;
            }
            for (int i = 0; i < 100; i++)
            {
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
            List<int> ints = new List<int>();
            ints.Add(1);
            ints.Add(2);
            ints.Add(3);
            for (int j = 0; j < ints.Count; j++)
            {
                Logger.Log(j+","+ 0);
            }

            foreach (int k in ints)
            {
                Logger.Log(k+","+ 1);
            }
            return b;
        }
        public static object UnitTest_4002()
        {
            //4002 loops

            List<int> ints = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                ints.Add(i);
            }
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
    }
}
