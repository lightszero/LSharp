using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
using UnityEngine;

namespace UnitTestDll.SimU3D
{
    class TestU3D
    {
        public static void UnitTest_01()
        {
            GameObject obj = new GameObject();
            Logger.Log("c1=" + (obj == null));
            Logger.Log("c2=" + (obj.transform == null));
            TT t = new TT();
            for (int i = 0; i < 10; i++)
            {
                t.Test();
            }
        }
    }
    class TT
    {
        Transform tra = null;
        public void Test()
        {
            if (tra == null)
            {
                Logger.Log("c1=" + (tra == null));
                GameObject obj = new GameObject();
                tra = obj.transform;
            }
            else
            {
                Logger.Log("c1=" + (tra == null));
            }
        }

    }
}
