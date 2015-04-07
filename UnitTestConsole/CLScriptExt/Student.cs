using System;
using System.Collections.Generic;
using System.Text;

namespace CLScriptExt
{
    public class P1
    {
        public static void t1()
        {

        }
        public static int t2;
        public static int t3
        {
            get;
            set;
        }
    }
    public class P2 : P1
    {
        public static void TestS<T>()
        {
            Console.WriteLine(typeof(T).ToString());
        }
        public static void TestS2<T1,T2>()
        {
            Console.WriteLine(typeof(T1).ToString() + "," + typeof(T2).ToString());
        }
    }
    public class MyClass2
    {
        public static MyClass2 instance = new MyClass2();

        public T GetComponent<T>(MyClass2 obj, int deep)
        {
            return default(T);
        }
    }


    public class Student : P2
    {
        public void Test(string str,int i=0)
        {
            Console.WriteLine("str=" + i);
        }
        public class StudentAss
        {
            public int size = 10;

        }
        public string name
        {
            get;
            set;
        }
        public static int page;
        public int age;
        public class S1
        {
            public int v;
        }
        public List<int> vs = new List<int>();
        public int[] vs2 = new int[] { 1, 2, 3, 4 };

        public void ToString2()
        {

        }
        public void ToString2<T>(T obj)
        {
            Console.WriteLine(obj.ToString());
        }



    }

}
