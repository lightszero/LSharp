using CLScriptExt;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
namespace UnitTestDll
{
    public class ExpTest_60
    {
        //只要有一个静态函数包含UnitTest名称的，就作为单元测试
        public static object UnitTest_6001()
        {
            //6001 object
            Vector3 vec = new Vector3(1, 2, 3) * 0.5f;
            string str = "abc,aaa,cccc";
            string[] ss = str.Split(new char[] { ',' });


            Student s = new Student();
            s.Test("abc");



            Vector3 vec2 = new Vector3(1, 2, 3);
            Vector3 vec3 = vec + vec2;
            Logger.Log(vec + "," + vec2 + "," + vec3);
            bool b = vec == vec2;

            return "loading..." + b;

        }
        public static void UnitTest_6002()
        {
            //6002 object
            //member set and get

            Vector3 vec = new Vector3(1, 2, 3);

            vec.x = 15;

            float x1 = vec.x;
            Logger.Log(x1 + "," + vec);

            float x2 = vec.len;
            Logger.Log(x2.ToString());

            Vector3 v3 = vec.Normalized();
            Logger.Log(v3.ToString());

            return;

        }
        public static void UnitTest_6003()
        {
            //6003 object
            //static

            Vector3 vec = new Vector3(1, 0, 0);
            Vector3 vec2 = Vector3.One;

            Vector3 cross = Vector3.Cross(vec, vec2);

            Vector3.typetag = "fff";
            Logger.Log(cross.ToString());

            Logger.Log(Vector3.typetag);

            return;

        }
        public static object UnitTest_6004()
        {//6004 object
            //array

            List<float> list = new List<float>();

            Vector3 vec = new Vector3(1, 0, 0);
            Vector3 vec2 = Vector3.One;


            Vector3 cross = Vector3.Cross(vec, vec2);

            List<Vector3> vecList = new List<Vector3>();
            vecList.Add(vec);
            vecList.Add(vec2);
            vecList.Add(cross);
            vecList[1] = cross;
            Logger.Log( cross.ToString());

            var v = vecList[1].y;

            Logger.Log( v.ToString());

            return vecList.Count;

        }
        public static object UnitTest_6005()
        {//6005 object
            //enum

            Country c = Country.Chinese;
            Country c2 = Country.English;
            Country c3 = Country.Japnese;
            int i = (int)(c3);
            Logger.Log(c + "," + c2 + "," + c3);

            return c3;

        }
        public static object UnitTest_6006()
        {//6006 object
            UserData.Instance().HeroDataMap.Clear();
            UserData.Instance().HeroDataMap.Add("1", "111");
            UserData.Instance().HeroDataMap.Add("10", "1111");
            var data = UserData.Instance().HeroDataMap;
            string d = data["1"];

            string d2 = UserData.Instance().HeroDataMap["10"];

            return "d=" + d + ",d2=" + d2;


        }
        public static void UnitTest_6007()
        {//6007 object
            MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();
            obj["n"] = new MyJson.JsonNode_ValueString("fuck");
            //outicon2.spriteName = view.values[ii] as MyJson.JsonNode_ValueString

            Student s = new Student();
            if(s==null)
            {
                Logger.Log("Error out.");
            }
            else
            {
                Logger.Log("Right out.");
            }
            string key = "n";
            s.name = obj[key] as MyJson.JsonNode_ValueString;
            int i = s.vs2[1];

            s.ToString2<int>(22);

        }
        public static void UnitTest_6008()
        {//6008 object

            Student.TestS2<int, int>();
        }
    }
}
