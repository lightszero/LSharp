using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            InitTest();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("(A) 测试所有用例");
                Console.WriteLine("(L) 列出所有用例");
                Console.WriteLine("(S <用例ID>) 测试单个用例");
                Console.WriteLine("(D <用例ID>) 测试单个用例 详细log");
                Console.WriteLine("(F <用例ID>) 测试单个用例 no try");
                Console.WriteLine("(Q) 退出测试");

                string text = Console.ReadLine();
                if (text.Length > 0)
                {
                    if (text.ToLower()[0] == 'q')
                    {
                        return; ;
                    }
                    else if (text.ToLower()[0] == 'l')
                    {
                        ListAll();
                    }
                    else if (text.ToLower()[0] == 'a')
                    {
                        TestAll();
                    }
                    else if (text.ToLower()[0] == 's' || text.ToLower()[0] == 'd' || text.ToLower()[0] == 'f')
                    {
                        int id = 0;
                        CLRSharp.IMethod call;
                        try
                        {
                            id = int.Parse(text.Substring(2));
                            call = tests[id];
                        }
                        catch
                        {
                            logger.Log_Error("参数错误");
                            continue;
                        }
                        if (text.ToLower()[0] == 'd')
                            TestOne(call, true, false);
                        else if (text.ToLower()[0] == 'f')
                            TestOne(call, true, true);
                        else
                        {
                            try
                            {
                                TestOne(call, false, false);
                            }
                            catch(Exception err)
                            {
                                logger.Log_Error("dump:\n" + CLRSharp.ThreadContext.activeContext.Dump());
                                logger.Log_Error("excep:\n" + err.ToString());
                            }
                        }
                    }
                }

            }
        }
        public static CLRSharp.CLRSharp_Environment env = null;
        class Logger : CLRSharp.ICLRSharp_Logger
        {

            public void Log(string str)
            {
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine(str);
            }

            public void Log_Warning(string str)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("<W>" + str);
            }

            public void Log_Error(string str)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("<E>" + str);
            }
        }
        static Logger logger = new Logger();
        static List<CLRSharp.IMethod> tests = new List<CLRSharp.IMethod>();
        static void InitTest()
        {
            tests.Clear();

            var bytes = System.IO.File.ReadAllBytes("UnitTestDll.dll");
            var bytespdb = System.IO.File.ReadAllBytes("UnitTestDll.pdb");
            //var bytesmdb = System.IO.File.ReadAllBytes("UnitTestDll.dll.mdb");//现在支持mdb了
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            System.IO.MemoryStream mspdb = new System.IO.MemoryStream(bytespdb);
            //System.IO.MemoryStream msmdb = new System.IO.MemoryStream(bytesmdb);

            env = new CLRSharp.CLRSharp_Environment(new Logger());
            logger.Log(" L# Ver:" + env.version);
            try
            {
                env.LoadModule(ms, mspdb, new Mono.Cecil.Pdb.PdbReaderProvider());
                //env.LoadModule(ms, msmdb, new Mono.Cecil.Mdb.MdbReaderProvider());
            }
            catch (Exception err)
            {
                logger.Log_Error(err.ToString());
                logger.Log_Error("模块未加载完成，请检查错误");
            }
            var types = env.GetAllTypes();
            foreach (var t in types)
            {
                var tclr = env.GetType(t) as CLRSharp.Type_Common_CLRSharp;
                if (tclr != null && tclr.type_CLRSharp.HasMethods)
                {

                    foreach (var m in tclr.type_CLRSharp.Methods)
                    {
                        var mm = tclr.GetMethod(m.Name, CLRSharp.MethodParamList.constEmpty());
                        if (mm != null)
                        {
                            if (mm.Name.Contains("UnitTest"))
                                tests.Add(mm);
                        }
                    }
                    if (tclr.type_CLRSharp.HasNestedTypes)
                    {

                        foreach (var ttt in tclr.type_CLRSharp.NestedTypes)
                        {
                            var tclr2 = env.GetType(ttt.FullName) as CLRSharp.Type_Common_CLRSharp;

                            foreach (var m in tclr2.GetAllMethods())
                            {
                                if (m.ParamList.Count == 0)
                                {
                                    if (m.Name.Contains("UnitTest"))
                                        tests.Add(m);
                                }
                            }
                        }
                    }

                }

            }
            logger.Log(" Got Test:" + tests.Count);
        }
        static void ListAll()
        {
            logger.Log_Warning("ListALL.");

            for (int i = 0; i < tests.Count; i++)
            {
                logger.Log(i.ToString("D03") + " " + tests[i].DeclaringType.Name + "|" + tests[i].Name);
            }
            logger.Log_Warning("ListEnd. Count:" + tests.Count);
        }
        static void TestAll()
        {
            logger.Log_Warning("TestAll.");
            //reset
            var types = env.GetAllTypes();
            foreach (var t in types)
            {
                CLRSharp.ICLRType_Sharp type = env.GetType(t) as CLRSharp.ICLRType_Sharp;
                if (type != null)
                    type.ResetStaticInstace();
            }
            int finish = 0;
            foreach (var m in tests)
            {
                try
                {
                    TestOne(m, false, false);
                    finish++;
                }
                catch (Exception err)
                {

                }
            }
            logger.Log_Warning("TestAllEnd. Count:" + finish + "/" + tests.Count);
        }


        static object TestOne(CLRSharp.IMethod method, bool LogStep = false, bool notry = false)
        {


            int debug = LogStep ? 9 : 0;
            if (CLRSharp.ThreadContext.activeContext == null)
            {
                CLRSharp.ThreadContext context = new CLRSharp.ThreadContext(env, debug);
            }
            CLRSharp.ThreadContext.activeContext.SetNoTry = notry;
            return method.Invoke(CLRSharp.ThreadContext.activeContext, null, null);
        }
    }

}
namespace UnitTest
{
    public class Logger
    {
        public static void Log(string str)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(str);

        }
    }
}