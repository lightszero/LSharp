
using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;

class T04
{
    public void Test()
    {
        Logger.Log("tcall:" + name);
    }
    public string name;
}
class Test08
{
    public int i;
    public Test08()
    {
        i = 5;
    }
    public Color32 vv;
    public void Call()
    {
        Logger.Log("i=" + i);
    }
    public static void F()
    {
        Test08 tt = new Test08();
        tt.Call();
        tt.vv = new Color32(1,1,1,1);//.FromArgb(1);
        
        tlist = new List<T04>();
        tlist.Add(new T04());
        tlist[0].name="cool";
        tlist.Add(new T04());
        tlist[1].name = "fuck";
        Logger.Log("tt:" + tlist[0].name);
        tlist[1].Test();
    }
    /// <summary>
    /// T04 
    /// </summary>
    static List<T04> tlist;
}
