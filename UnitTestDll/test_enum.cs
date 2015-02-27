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

        static void UnitTest_03(EUIPanelID id)
        {
            Type type = typeof(EUIPanelID);
        }
        static void UnitTest_04(EUIPanelID id)
        {
            string name = typeof(EUIPanelID).GetEnumName(id);
        }

    }


            
}
