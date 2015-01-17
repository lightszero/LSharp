
//using CSEvilTestor;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;

class Test02
{
    public static void Run()
    
    {

        Action<int> t = (int a) =>
        {
            //Debug.Log("a=" + a);
        };
        
        TestDele.instance.onUpdateD = Test;
        Action<int> deleTest = Test2;

        deleTest(13333);
        Test02.deleTest2 = deleTest;
        Test02.deleTest2(333);

        int config_citygrade = 0;
        TestDele.instance.ClearDele();
        TestDele.instance.onUpdateD = () =>
        {
            Logger.Log("direct.");
        };
        TestDele.instance.onUpdateD = Test;
        //直接注册回调的用法,+=,-=
        TestDele.instance.onUpdate += Test;

        //用Delegate类型指向函数的语法
        TestDele.instance.onUpdate2 += deleTest;
        TestDele.instance.onUpdate3 += Test3;

        //函数作为参数的用法
        TestDele.instance.AddDele(Test2);
        //Action<int> abc = Test2;
        TestDele.instance.AddDeleT3<int,string>(Test4);


        TestDele.instance.AddDele(null);

        TestDele.instance.AddDele((ii) =>
        {
            Logger.Log("Test lambda.");
        });
        //TestDele.instance.AddDele(deleTest);

        Test02 ttt = new Test02();
        ttt.deleTest3 = deleTest;
        ttt.deleTest3(3334);

        TestDele.instance.onUpdate2 += t;

        TestDele.instance.onUpdate2 += (b) =>
        {
            Logger.Log("b=" + b);
            //throw new Exception("ff");
        };


        TestDele.instance.Run();
    }
    static int i = 0;
    static Action<int> deleTest2 = null;

    Action<int> deleTest3; 
    static void Test()
    {

    }
    static void Test2(int v)
    {
        Logger.Log("Test2 i=" + v);
        i--;
    }
    static void Test4(int v,string str)
    {
        Logger.Log("Test4 i=" + v + ", s=" + str);
        i--;
    }

    static void Test3(int v, string str)
    {
        Logger.Log("test3:" + v + ":" + str);
    }
}

