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
        public static void UnitTest_03()
        {
            uint[] Result = new uint[3] { 1, 1, 1 };

            //Result = new UInt32[3] { 1, 1, 1 };
            Result[1] = 100;

            char[] arr = new char[3] { '1', '2', '3' };
            arr[1] = '2';
            char b3 = arr[0];
            Logger.Log(b3.ToString());
        }
        public static void UnitTest_04()
        {
            uint[] arr = new uint[3] ;
            arr[1] = 1;
            uint b3 = arr[0];
            Logger.Log(b3.ToString());
        }
        public static void UnitTest_05()
        {
            ushort[] arr = new ushort[3] ;
            arr[1] = 1;
            ushort b3 = arr[0];
            Logger.Log(b3.ToString());
        }
        public static void UnitTest_06()
        {
            ulong[] arr = new ulong[3];
            arr[1] = 1;
            ulong b3 = arr[0];
            Logger.Log(b3.ToString());
        }
        public static void UnitTest_07()
        {
            byte[] arr = new byte[3];
            arr[1] = 1;
            byte b3 = arr[0];
            Logger.Log(b3.ToString());
        }
    }


            
}
