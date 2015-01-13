using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
namespace UnitTestDll
{
    public class ExpTest_20
    {
        //只要有一个静态函数包含UnitTest名称的，就作为单元测试
        public static void UnitTest_2001()
        {
            //2001 define and set.

            int inta = 0;
            int intb = 0;
            int intc = inta;
            bool boola = false;
            bool boolb = true;
            bool boolc = boola;
            string stra = "";
            string strb = "oo中o";
            string strc = stra;
            char ca = 'd';

            return;

        }
        public static object UnitTest_2002()
        {
            int a;
            a = 5;

            return a;

        }
        public static object UnitTest_2003()
        {
            float b = (float)1;
            var c = b;

            return c;
        }

    }
}
