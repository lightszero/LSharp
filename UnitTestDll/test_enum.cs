using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTest;
using System.Reflection;

namespace UnitTestDll
{
    class test_enum
    {
        public enum EUIPanelID
        {
            NULL=0,
            INT1=1,
            INT2,
        };

        public static void UnitTest_01()
        {
            Dictionary<EUIPanelID, string> dict = new Dictionary<EUIPanelID, string>();

            bool bo = dict.ContainsKey(EUIPanelID.INT1);
        }
        
        public static void UnitTest_02()
        {
            Test_02(EUIPanelID.INT1);
        }
        static void Test_02(EUIPanelID id)
        {
            Logger.Log("test_enum.UnitTest_02");
        }

        static void UnitTest_03()
        {
            //system type  å’?l# type  ä¸èƒ½ä¸€èµ·æž
            //Type type = typeof(EUIPanelID);
        }


        static void UnitTest_07()
        {
            LinkedList<EUIPanelID> ll = new LinkedList<EUIPanelID>();
            bool b = ll.Contains(EUIPanelID.INT1);
        }
        
        static void UnitTest_09()
        {
            bool[] m_IsDataReady = new bool[4] { false, false, false, false };
            int len = m_IsDataReady.Length;
            bool tmp = m_IsDataReady[0];
            //tmp = m_IsDataReady[1];
            //tmp = m_IsDataReady[2];
            //tmp = m_IsDataReady[3];

        }
        static void UnitTest_10()
        {
            int[] m_IsDataReady = new int[4] { 1, 1, 1, 1 };
            int len = m_IsDataReady.Length;
            int tmp = m_IsDataReady[0];
            //tmp = m_IsDataReady[1];
            //tmp = m_IsDataReady[2];
            //tmp = m_IsDataReady[3];

        }
        static void UnitTest_11()
        {
            float[] m_IsDataReady = new float[4] { 1f, 1f, 1f, 1f };
            int len = m_IsDataReady.Length;
            float tmp = m_IsDataReady[0];
            //tmp = m_IsDataReady[1];
            //tmp = m_IsDataReady[2];
            //tmp = m_IsDataReady[3];

        }
        static void UnitTest_12()
        {
            string[] m_IsDataReady = new string[4] { "1", "1", "1", "1" };
            int len = m_IsDataReady.Length;
            string tmp = m_IsDataReady[0];
            //tmp = m_IsDataReady[1];
            //tmp = m_IsDataReady[2];
            //tmp = m_IsDataReady[3];

        }
        
        static void UnitTest_13()
        {
            bool[] data = new bool[4] { true,true,true,true};
            data[0] = false;
        }

    }


            
}
