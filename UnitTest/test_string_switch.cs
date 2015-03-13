using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTest;

namespace UnitTestDll
{
    class test_string_switch
    {
        //test string switch
        public static void UnitTest_01()
        {
            string a = "121212";
            switch (a) { 
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "6":
                    break;
                case "7":
                    break;
                case "121212":
                    Logger.Log("test_string_switch");
                    break;
            }
        }
        //test ref and out
        public static void UnitTest_02()
        {
            int a = 1;
            Logger.Log(a.ToString());
        }
        private static void Ref(ref int i) {
            i += 1;
        }
        //test char[]
        public static void UnitTest_02()
        {
            char[] arr = new char[3] { 'a', 'b', 'c' };
            Logger.Log(arr[0]);
        }
        static void Test_02(EUIPanelID id)
        {
            Logger.Log("test_enum.UnitTest_02");
        }
    }


            
}
