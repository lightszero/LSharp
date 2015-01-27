using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTest;
using System.Collections;

namespace UnitTestDll
{
    class Test_ByLynn
    {
        public static void UnitTest_01()
        {
            int[] arr = new int[10];
            int[] arr2 = new int[11];

            if (arr[0] > arr2[0])
            {
                Logger.Log("1");
            }
            else
            {
                Logger.Log("2");
            }
        }

        public static void UnitTest_02()
        {
            int[] arr = new int[] { 2 };

            int t = arr[0];
            Logger.Log(t.ToString());
            if (arr[0] == 2)
            {
                Logger.Log("ok");
            }
            else
            {
                Logger.Log("err");
            }
        }

        public static void UnitTest_03()
        {
            Dictionary<string, object> dir = new Dictionary<string, object>();
            dir["test"] = "test";
            foreach (string item in dir.Values)
            {
                Logger.Log(item);
            }
        }
    }
}