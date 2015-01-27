using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTest;
using System.Collections;

namespace UnitTestDll
{
    class Test_ByLynn
    {
        public static void UnitTest_01()
        {
            int[] arr = new int[10];
            int[] arr2 = new int[11];

            if ( arr[0] > arr2[0] )
            {
                Logger.Log("1");
            }
            else
            {
                Logger.Log("2");
            }
        }
    }
}
