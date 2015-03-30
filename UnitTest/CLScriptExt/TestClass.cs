
using System;
using System.Collections.Generic;

using System.Text;
using UnitTest;


    public class TestClass
    {
        public TestClass()
        {
        }

        public TestClass(byte b)
        {
        }

        public string name
        {
            get
            {
                return "hi";
            }
            set
            {
                Logger.Log(value);
            }
        }
        public bool b;
        public string ttc = "123";

        private static TestClass g_this = null;
        public static TestClass instance
        {
            get
            {
                if(g_this==null)
                {
                    g_this = new TestClass();
                }
                return g_this;
            }
        }
        int i = 0;
        public void Log()
        {
            i++;
            Logger.Log("i=" + i);
        }
        public static bool TF(bool b)
        {
            return b;
        }

        public static void TestObjectArg(Object o)
        {
            bool b = (bool)o;
        }

        public static void TestByteArg(byte b)
        {
        }
    }
