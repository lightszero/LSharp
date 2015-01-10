
//using CSEvilTestor;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;

class Test05
{
    public static void Run()
    {
        if (c5 == null)
        {
            c5 = new C5();

        }
        try
        {


            throw new NotImplementedException("E2");
        }
        catch (NotSupportedException err)
        {
            Logger.Log("not here.");
        }
        catch (NotImplementedException err)
        {
            Logger.Log("Got.");
        }
        catch (Exception err)
        {
            Logger.Log("Got 2.");
        }
    }
    static C5 c5 = null;
}

class C5
{

}
