using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnitTest;

namespace UnitTestDll.SimU3D
{
    class TestYield
    {
        public static void UnitTest_01()
        {
            var vv = yoo();
            Logger.Log("got");
            foreach (var y in vv)
            {
                Logger.Log("y=" + y);
            }
            //var vv2 = yoo2("aa","bb");

        }
        static IEnumerable yoo()
        {
            yield return "a";
            yield return "b";
            yield return "c";
        }
        //static IEnumerator yoo2(string name,string name2)
        //{
        //    yield return "a";
        //    yield return "b";
        //    yield return "c";
        //}
    }
}
