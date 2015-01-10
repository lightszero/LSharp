

using System;
using System.Collections;
using System.Collections.Generic;
using UnitTest;

public class Script_TestConstructor
{
    string abc = ("dd" as string).ToString();
    private int[] a = new int[] { 1, 3, 4 };
    public static int[] b = new int[234];
    public static Dictionary<string, object> objs = new Dictionary<string, object>();
    public static Dictionary<int, int> dicHasBlock = new Dictionary<int, int>();

    public static List<object> objs2 = new List<object>();
    public Script_TestConstructor()
    {
        Dictionary<string, object> objs = new Dictionary<string, object>();
        //Test();
        ////Debug.Log(null);
        //Test2();
        //Test3(22, 33, 44);
        objs.Add("abb",this);
        objs2.Add(this);
        Script_TestConstructor thisislist= objs["abb"] as Script_TestConstructor;
        thisislist.LogtT();
    }
    public int[] TTT(int[] ttt)
    {
        return ttt;
    }
    public void LogtT()
    {
        Logger.Log("LogtT");
    }
    public static void Test3(int a, int b, int c)
    {
        ////// 这个是正确的.
        int v = (a * (b + 1)) * (c + 2);
        int value1 = config.Cell(v);
        Logger.Log("value1=>" + value1);

        ////// 发生运行时错误 => 找不到 CeilToInt 方法.
        int value2 = config.Cell((a * (b + 1)) * (c + 2));
        Logger.Log("value2=>" + value2);
    }
    public bool Test()
    {
        return false;
    }

    public static void Test2()
    {
        for (int i = 0; i < 10; i++)
        {
            TestClass.instance.Log();
        }
    }
}
