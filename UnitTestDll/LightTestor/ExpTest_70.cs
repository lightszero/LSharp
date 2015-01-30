using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
namespace UnitTestDll
{
    public class ExpTest_70
    {
        //只要有一个静态函数包含UnitTest名称的，就作为单元测试
        public static void UnitTest_7001()
        {
            int[] ints3 = new int[] { 1, 2, 3 };
            int v = ints3[0];// = 44;
            Logger.Log("=" + v);
            //7001 array
            List<List<int>> list = new List<List<int>>();
            list.Add(new List<int>());
            list.Add(new List<int>());
            Logger.Log("listc=" + list.Count);
            list[0].Add(1);
            list[0].Add(2);
            list[0].Add(4);
            Logger.Log("i=" + list[0][1]);


            int[] ints2 = new int[3];
            ints2[1] = 44;
            Logger.Log("=" + ints2[1]);


        }
        public static void UnitTest_7002()
        {
            //7001 array

            string[] ints2 = new string[] { "55", "55", "55" };
            long i = 1;
            ints2[i] = "44";
            Logger.Log("=" + ints2[i]);

        }
        public static object UnitTest_7003()
        {
            //7003 array

            Dictionary<int, string> hip = new Dictionary<int, string>();
            hip[3] = "ff";
            hip[5] = "fff";
            hip[6] = "ffff";

            foreach (var i in hip)
            {
                Logger.Log("key=" + i.Key + ",v=" + i.Value);
            }

            return 0;
        }
    }
}
