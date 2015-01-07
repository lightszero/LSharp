using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t01
{
    public class Class1
    {
        void Do001()
        {
            byte t1 = 1;
            sbyte t2 = 3;
            UInt16 t3 = 4;
            for(int i=0;i<10;i++)
            {
                Console.WriteLine("hi there." + i);
            }
            Test(t3);
        }

        static void Test(int fuck)
        {
            Test2(fuck);
        }

        static void Test2(int i)
        {
            string abc = "abc";
            Console.WriteLine(abc);
        }
        void abcd(aaa aa)
        {
            int a = aa.ab;
        }
    }
    public class aaa
    {
        public int ab;
    }
}
