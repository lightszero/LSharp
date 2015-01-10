using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
namespace UnitTestDll
{
    public class ExpTest_50
    {
        //只要有一个静态函数包含UnitTest名称的，就作为单元测试
        public static object UnitTest_5001()
        {
            //5001 function
            //trace的两种用法
            int b = 50;
            Logger.Log("this is trace");//trace 只有一个参数时可以用空格;
            Logger.Log("test sth" + b);
            Logger.Log("test sth2" + "," + b);//trace 多个参数必须用函数调用语法

            return b;
        }
        public static void UnitTest_5002()
        {
            //5002 function
            //函数原型 testCallAdd(int a, int b)
            int b = 1 + 5;
            int c = (1 + (int)5.0f);
            int d = (1 + (int)5.0f);
            Logger.Log(b + "," + c + "," + d);


            return;
        }
        public static void UnitTest_5003()
        {
            //5003 function
            //函数原型  int testCallAdd4(int a, int b, int c = 0, int d = 0)
            int b = (1 + 5 + 1 + 1);
            int c = (int)(1 + 5.0f + 2);
            int d = (int)(1 + 5.0f);
            Logger.Log(b + "," + c + "," + d);

            Action<int> callnoname = (abc) =>
            {

                Logger.Log("nonamecall abc=" + abc + "d=" + d);

            };
            callnoname(44565);
            return;
        }
    }
}
