using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;

namespace UnitTestDll.Eviltestor
{
    public class EvilTestor
    {
        public static void UnitTest_01()
        {
            //这个测试完成Helloworld.


            Script_TestConstructor tc = new Script_TestConstructor();
        }
        public static void UnitTest_02()
        {

            Test02.Run();
        }
        public static void UnitTest_03()
        {
            //这个测试测试一些奇特的类型名
            //List<string>
            //List<List<int>>
            //List<List<List<double>>>

            Test03.Run();
        }
        public static void UnitTest_04()
        {
            //这个测试测试接口继承

            Test04.Run();
        }
        public static void UnitTest_05()
        {
            //这个测试测试try catch throw
            Test05.Run();
        }
        public static void UnitTest_06()
        {
            //这个测试测试


            Dictionary<string, object> dicNameToBlock = new Dictionary<string, object>();
            Block b = new Block("gg1", 2, 3, true, "2");
            dicNameToBlock.Add("1", b);

            foreach (var item in dicNameToBlock)
            {
                Logger.Log("key=" + item.Key);
                Block bb = (Block)item.Value;// as Block;
                Logger.Log("bb.name=" + bb.blockName);
            }

        }
        public static void UnitTest_07()
        {
            //这个测试测试try catch throw
            Test07 t = new Test07();
            t.Run();
        }
        public static void UnitTest_08()
        {
            //这个测试测试try catch throw


            Test08.F();

        }
        public static void UnitTest_09()
        {
            //这个测试测试try catch throw


            Test09.Test();

        }
        public static void UnitTest_10()
        {
            //这个测试测试try catch throw
            Test10 t = new Test10();
            t.Debug();


        }

        public static void UnitTest_11()
        {
            Test10.TestInnerClass();
        }
    }
}
