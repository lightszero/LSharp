using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
namespace UnitTestDll
{
    public class ExpTest_30
    {
        //只要有一个静态函数包含UnitTest名称的，就作为单元测试
        public static object UnitTest_3001()
        {
            TT t;
            t.abc = 1.0f;
            int i = 4;
            int c = 5;
            DateTime t1 = DateTime.Now;
            DateTime t2 = DateTime.Now;
            TimeSpan sp = t2 - t1;
            bool b = (i < 3 || c <= 5) && 1 < 2;
            return b;

        }
        public static object UnitTest_3002()
        {
            //3002

            byte b = 12;
            byte bt =(byte)( b + 3);
            Logger.Log ("b=" + b + " bt=" + bt);

            sbyte sb = -12;
            sbyte sb2 = (sbyte)(bt + b);
            Logger.Log("sb=" + sb + " sb2=" + sb2);

            short ssb = 123;
            short ssb2 =(short)( ssb + 4);
            Logger.Log("ssb=" + ssb + " ssb2=" + ssb2);

            Student s = new Student();
            for (int i = 0; i < 10; i++)
            {
                s.age++;
                Logger.Log(s.age.ToString());
            }
            for (int i = 0; i < 10; i++)
            {
                Student.page++;
                Logger.Log(Student.page.ToString());
            }

            return 0;
        }

    }
}
