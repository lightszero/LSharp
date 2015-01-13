using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
using UnitTest.Perform;
using UnityEngine;

namespace UnitTestDll
{
    class Perform_01
    {
        public static void UnitTest01()
        {
            int count = 100;
            double v1 = Perform.PerformSystem01(count);
            double v2 = Perform.PerformSystem02(count);
            double v3 = Perform.PerformSystem03(count);
            Logger.Log("timeSys=" + v1 + "," + v2 + "," + v3);

            double sv1 = Perform.Call(() =>
            {
                GameObject go = new GameObject();
                UnityEngine.GameObject.Destory(go);
            }, count);
            double sv2 = Perform.Call(() =>
            {
                Vector3 p;
                p.x = 1;
                p.y = 2;
                p.z = 3;
            }, count);
            double sv3 = Perform.Call(() =>
            {
                double num = 1.11111f;
                for (int i = 0; i < 1000; i++)
                {
                    num *= 1.01f;
                }
            }, count);
            Logger.Log("timeSys=" + sv1 + "," + sv2 + "," + sv3);
        }

    }
}
