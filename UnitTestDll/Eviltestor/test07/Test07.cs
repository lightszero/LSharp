
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;

class Test07
{
    public void Run()
    {
        Logger.Log("===-----------===");
        Fun1();
    }
    //static C5 c5 = null;

    public void Fun1()
    {
        Logger.Log("Fun1 in");
        Fun2();
        Logger.Log("Fun1 out");
    }


    int _pix = int.MinValue;
    int _piy = int.MinValue;
    public void Fun2()
    {
        Logger.Log("Fun3 in"); // !!!!!!!!!!!!!!没有了
        int pix = 0;
        int piy = 0;
        if (pix == _pix && piy == _piy)
        {
            return;
        }

        Logger.Log("Fun3");
    }
}
