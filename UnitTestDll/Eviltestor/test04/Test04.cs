//using CSEvilTestor;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;

class Test04
{
    public static Dictionary<string, object> objs = new Dictionary<string, object>();
    public static void Run()
    {
        IT4_Impl impl1 = new IT4_Impl();
        
        impl1.Call1();
        impl2 = new IT4_Impl();
        Test04.impl2 = new IT4_Impl2();
        Test04.impl2.Call1();
        Test04 t4 = new Test04();
        t4.Run1();
        objs.Add("t1", impl1);
        objs.Add("t4", t4);
        Test04.IT4();

    }
    static void IT4()
    {

    }
    void Run1()
    {
        
        impl1 = new IT4_Impl();

        impl1.Call1();
        this.impl1 = new IT4_Impl2();
        this.impl1.Call1();
    }
    static IT4 impl2;
    IT4 impl1;

}
interface IT4
{
    void Call1();
    void Call2(int i, string n);

    string name
    {
        get;
        set;
    }
    string readonlyname
    {
        get;
    }

}
class IT4_Impl : IT4
{
    public IT4_Impl()
    {
        this.name = "IT4_Impl";
    }

    public void Call1()
    {
        Logger.Log("IT4_Impl.Call1");
    }

    public void Call2(int i, string n)
    {
        Logger.Log("IT4_Impl.Call2(" + i + "," + n + ")");
    }

    public string name
    {
        get;
        set;
    }

    public string readonlyname
    {
        get;
        private set;
    }

}

class IT4_Impl2 : IT4
{
    public IT4_Impl2()
    {
        this.name = "IT4_Impl2";
    }

    public void Call1()
    {
        Logger.Log("IT4_Impl2.Call1");
    }

    public void Call2(int i, string n)
    {
        Logger.Log("IT4_Impl2.Call2(" + i + "," + n + ")");
    }

    public string name
    {
        get;
        set;
    }

    public string readonlyname
    {
        get;
        private set;
    }
}