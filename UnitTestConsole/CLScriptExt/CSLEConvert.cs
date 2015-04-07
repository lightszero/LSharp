using System;
using System.Collections.Generic;

using System.Text;

namespace CSEvilTestor.testfunc
{
    class CSLEConvert
    {
        //public static CSLE.ICLS_Environment env;
        //public static object FromJson(string ScriptTypeName, MyJson.JsonNode_Object data)
        //{
        //    var type = env.GetTypeByKeywordQuiet(ScriptTypeName);
        //    CSLE.SType stype = type.function as CSLE.SType;
        //    CSLE.SInstance s = new CSLE.SInstance();
        //    s.type = stype;

        //    foreach (var m in stype.members)
        //    {
        //        if (m.Value.bStatic) continue;

        //        s.member[m.Key] = new CSLE.CLS_Content.Value();
        //        s.member[m.Key].type = m.Value.type.type;
        //        s.member[m.Key].value = m.Value.type.DefValue;//先填默认值

        //        if (data.ContainsKey(m.Key) == false)//json中没有的部分填默认值
        //            continue;
        //        if (data[m.Key].IsNull())//json中没有的部分填默认值
        //            continue;

        //        {

        //            if ((Type)m.Value.type.type == typeof(int))
        //            {
        //                s.member[m.Key].value = data[m.Key].AsInt();
        //            }
        //            else if ((Type)m.Value.type.type == typeof(float))
        //            {
        //                s.member[m.Key].value = (float)data[m.Key].AsDouble();
        //            }
        //            else if ((Type)m.Value.type.type == typeof(double))
        //            {
        //                s.member[m.Key].value = data[m.Key].AsDouble();
        //            }
        //            else if ((Type)m.Value.type.type == typeof(string))
        //            {
        //                s.member[m.Key].value = data[m.Key].AsString();
        //            }
        //            else if ((Type)m.Value.type.type == typeof(List<object>))
        //            {
        //                //处理List
        //                List<object> list = new List<object>();
        //                s.member[m.Key].value = list;
        //                int i = m.Value.type.keyword.IndexOf('<');

        //                string subtype = m.Value.type.keyword.Substring(i + 1);
        //                subtype = subtype.Remove(subtype.Length - 1);
        //                foreach (MyJson.JsonNode_Object item in data[m.Key].AsList())
        //                {
        //                    list.Add(FromJson(subtype, item));
        //                }
        //            }
        //            else if ((Type)m.Value.type.type == typeof(List<string>))
        //            {
        //                List<string> list = new List<string>();
        //                s.member[m.Key].value = list;
        //                foreach (MyJson.JsonNode_ValueString item in data[m.Key].AsList())
        //                {
        //                    list.Add(item);
        //                }
        //            }
        //            else if ((Type)m.Value.type.type == typeof(List<int>))
        //            {
        //                List<int> list = new List<int>();
        //                s.member[m.Key].value = list;
        //                foreach (MyJson.JsonNode_ValueNumber item in data[m.Key].AsList())
        //                {
        //                    list.Add(item);
        //                }
        //            }
        //            else if ((Type)m.Value.type.type == typeof(List<float>))
        //            {
        //                List<float> list = new List<float>();
        //                s.member[m.Key].value = list;
        //                foreach (MyJson.JsonNode_ValueNumber item in data[m.Key].AsList())
        //                {
        //                    list.Add(item);
        //                }
        //            }
        //            else if ((Type)m.Value.type.type == typeof(List<double>))
        //            {
        //                List<double> list = new List<double>();
        //                s.member[m.Key].value = list;
        //                foreach (MyJson.JsonNode_ValueNumber item in data[m.Key].AsList())
        //                {
        //                    list.Add(item);
        //                }
        //            }
        //            else if ((CSLE.SType)m.Value.type.type != null)//其他嵌套脚本类型
        //            {
        //                CSLE.SType subtype = (CSLE.SType)m.Value.type.type;
        //                s.member[m.Key].value = FromJson(m.Value.type.keyword, data[m.Key] as MyJson.JsonNode_Object);
        //            }
        //            else
        //            {
        //                Debug.Log("发现不能处理的类型:" + ScriptTypeName + ":" + m.Key + ":" + m.Value.type.ToString());
        //            }
        //        }

        //    }


        //    return s;
        //}

        //public static MyJson.JsonNode_Object ToJson(object ScriptObject)
        //{
        //    CSLE.SInstance sobj = ScriptObject as CSLE.SInstance;
        //    if (sobj == null) return null;
        //    MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();
        //    CSLE.SType stype=sobj.type;
        //    foreach (var m in stype.members)
        //    {
        //        if ((Type)m.Value.type.type == typeof(int))
        //        {
        //            obj[m.Key] = new MyJson.JsonNode_ValueNumber((int)sobj.member[m.Key].value);
        //        }
        //        else if ((Type)m.Value.type.type == typeof(float))
        //        {
        //            obj[m.Key] = new MyJson.JsonNode_ValueNumber((float)sobj.member[m.Key].value);
        //        }
        //        else if ((Type)m.Value.type.type == typeof(double))
        //        {
        //            obj[m.Key] = new MyJson.JsonNode_ValueNumber((double)sobj.member[m.Key].value);
        //        }
        //        else if ((Type)m.Value.type.type == typeof(string))
        //        {
        //            obj[m.Key] = new MyJson.JsonNode_ValueString((string)sobj.member[m.Key].value);
        //        }
        //        else if ((Type)m.Value.type.type == typeof(List<object>))
        //        {
        //            //处理List
        //            List<object> slist = sobj.member[m.Key].value as List<object>;
        //            var list = new MyJson.JsonNode_Array();
        //            obj[m.Key] = list;
        //            foreach(var item in slist)
        //            {
        //                list.Add(ToJson(item));
        //            }
        //        }
        //        else if ((Type)m.Value.type.type == typeof(List<string>))
        //        {
        //            var list =new MyJson.JsonNode_Array();
        //            obj[m.Key] = list;

        //            List<string> slist = sobj.member[m.Key].value as List<string>;
                  
        //            foreach (var item in slist)
        //            {
        //                list.Add(new MyJson.JsonNode_ValueString(item));
        //            }
        //        }
        //        else if ((Type)m.Value.type.type == typeof(List<int>))
        //        {
        //            var list = new MyJson.JsonNode_Array();
        //            obj[m.Key] = list;

        //            List<int> slist = sobj.member[m.Key].value as List<int>;

        //            foreach (var item in slist)
        //            {
        //                list.Add(new MyJson.JsonNode_ValueNumber(item));
        //            }
        //        }
        //        else if ((Type)m.Value.type.type == typeof(List<float>))
        //        {
        //            var list = new MyJson.JsonNode_Array();
        //            obj[m.Key] = list;

        //            List<float> slist = sobj.member[m.Key].value as List<float>;

        //            foreach (var item in slist)
        //            {
        //                list.Add(new MyJson.JsonNode_ValueNumber(item));
        //            }
        //        }
        //        else if ((Type)m.Value.type.type == typeof(List<double>))
        //        {
        //            var list = new MyJson.JsonNode_Array();
        //            obj[m.Key] = list;

        //            List<double> slist = sobj.member[m.Key].value as List<double>;

        //            foreach (var item in slist)
        //            {
        //                list.Add(new MyJson.JsonNode_ValueNumber(item));
        //            }
        //        }
        //        else if ((CSLE.SType)m.Value.type.type != null)//其他嵌套脚本类型
        //        {
        //            obj[m.Key] = ToJson(sobj.member[m.Key].value);
        //        }
        //        else
        //        {
        //            Debug.Log("发现不能处理的类型:" + stype.Name + ":" + m.Key + ":" + m.Value.type.ToString());
        //        }
        //    }
        //    return obj;
        //}
    }
}
