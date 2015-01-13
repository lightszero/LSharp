using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnitTest.Perform
{
    public delegate void Action();
    public class Perform
    {

        public static double PerformSystem01(int count)
        {
            DateTime t0 = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                GameObject go = new GameObject();
                UnityEngine.GameObject.Destory(go);
            }
            double o = (DateTime.Now - t0).TotalSeconds;
            Logger.Log("time=" + o);
            return o;
        }
        public static double PerformSystem02(int count)
        {
            DateTime t0 = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                Vector3 p;
                p.x = 1;
                p.y = 2;
                p.z = 3;
            }
            double o = (DateTime.Now - t0).TotalSeconds;
            Logger.Log("time=" + o);
            return o;
        }
        public static double PerformSystem03(int count)
        {
            DateTime t0 = DateTime.Now;
            for (int c = 0; c < count; c++)
            {
                double num = 1.11111f;
                for (int i = 0; i < 1000; i++)
                {
                    num *= 1.01f;
                }
            }
            double o = (DateTime.Now - t0).TotalSeconds;
            Logger.Log("time=" + o);
            return o;
        }
        public static double Call(Action act, int count)
        {
            DateTime t0 = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                act();
            }
            double o = (DateTime.Now - t0).TotalSeconds;
            Logger.Log("time=" + o);
            return o;
        }
    }
}
