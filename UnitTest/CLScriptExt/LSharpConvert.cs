using System;
using System.Collections.Generic;

using System.Text;
using UnitTest;

namespace CSEvilTestor.testfunc
{
    public class LSharpConvert
    {
        public static CLRSharp.ICLRSharp_Environment env;
        public static object FromJson(object ScriptType, MyJson.JsonNode_Object data)
        {
            CLRSharp.ICLRType_Sharp type = ScriptType as CLRSharp.ICLRType_Sharp;
            CLRSharp.CLRSharp_Instance s = new CLRSharp.CLRSharp_Instance(type);
            type.GetMethod(".ctor", CLRSharp.MethodParamList.constEmpty()).Invoke(null, s, new object[] { });


            foreach (var m in type.GetFieldNames())
            {
                var field = type.GetField(m);
                if (field.isStatic) continue;
                //s.Fields[m] = field.DefValue;

                //        s.member[m.Key] = new CSLE.CLS_Content.Value();
                //        s.member[m.Key].type = m.Value.type.type;
                //        s.member[m.Key].value = m.Value.type.DefValue;//先填默认值

                if (data.ContainsKey(m) == false)//json中没有的部分填默认值
                    continue;
                if (data[m].IsNull())//json中没有的部分填默认值
                    continue;

                {

                    if ((Type)field.FieldType.TypeForSystem == typeof(int))
                    {
                        s.Fields[m] = data[m].AsInt();
                    }
                    else if ((Type)field.FieldType.TypeForSystem == typeof(float))
                    {
                        s.Fields[m] = (float)data[m].AsDouble();
                    }
                    else if ((Type)field.FieldType.TypeForSystem == typeof(double))
                    {
                        s.Fields[m] = data[m].AsDouble();
                    }
                    else if ((Type)field.FieldType.TypeForSystem == typeof(string))
                    {
                        s.Fields[m] = data[m].AsString();
                    }
                    else if ((Type)field.FieldType.TypeForSystem == typeof(List<CLRSharp.CLRSharp_Instance>))
                    {
                        //处理List
                        List<CLRSharp.CLRSharp_Instance> list = new List<CLRSharp.CLRSharp_Instance>();
                        s.Fields[m] = list;
                        var subtype = field.FieldType.SubTypes[0];
                        foreach (MyJson.JsonNode_Object item in data[m].AsList())
                        {
                            list.Add(FromJson(subtype, item) as CLRSharp.CLRSharp_Instance);
                        }
                    }
                    else if ((Type)field.FieldType.TypeForSystem == typeof(List<string>))
                    {
                        List<string> list = new List<string>();
                        s.Fields[m] = list;
                        foreach (MyJson.JsonNode_ValueString item in data[m].AsList())
                        {
                            list.Add(item);
                        }
                    }
                    else if ((Type)field.FieldType.TypeForSystem == typeof(List<int>))
                    {
                        List<int> list = new List<int>();
                        s.Fields[m] = list;
                        foreach (MyJson.JsonNode_ValueNumber item in data[m].AsList())
                        {
                            list.Add(item);
                        }
                    }
                    else if ((Type)field.FieldType.TypeForSystem == typeof(List<float>))
                    {
                        List<float> list = new List<float>();
                        s.Fields[m] = list;
                        foreach (MyJson.JsonNode_ValueNumber item in data[m].AsList())
                        {
                            list.Add(item);
                        }
                    }
                    else if ((Type)field.FieldType.TypeForSystem == typeof(List<double>))
                    {
                        List<double> list = new List<double>();
                        s.Fields[m] = list;
                        foreach (MyJson.JsonNode_ValueNumber item in data[m].AsList())
                        {
                            list.Add(item);
                        }
                    }
                    else if (field.FieldType.TypeForSystem == typeof(CLRSharp.CLRSharp_Instance))//其他嵌套脚本类型
                    {
                        s.Fields[m] = FromJson(field.FieldType, data[m] as MyJson.JsonNode_Object);
                    }
                    else
                    {
                        Logger.Log("发现不能处理的类型:" + type + ":" + m + ":" + type.GetField(m).FieldType);
                    }
                }

            }


            return s;
        }

        public static MyJson.JsonNode_Object ToJson(object ScriptObject)
        {
            CLRSharp.CLRSharp_Instance sobj = ScriptObject as CLRSharp.CLRSharp_Instance;
            if (sobj == null) return null;
            MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();
            var stype = sobj.type;
            //    CSLE.SType stype=sobj.type;
            var ms = stype.GetFieldNames();
            foreach (var m in ms)
            {
                var field = stype.GetField(m);
                if (field.isStatic) continue;
                if ((Type)field.FieldType.TypeForSystem == typeof(int))
                {
                    obj[m] = new MyJson.JsonNode_ValueNumber((int)sobj.Fields[m]);
                }
                else if ((Type)field.FieldType.TypeForSystem == typeof(float))
                {
                    obj[m] = new MyJson.JsonNode_ValueNumber((float)sobj.Fields[m]);
                }
                else if ((Type)field.FieldType.TypeForSystem == typeof(double))
                {
                    obj[m] = new MyJson.JsonNode_ValueNumber((double)sobj.Fields[m]);
                }
                else if ((Type)field.FieldType.TypeForSystem == typeof(string))
                {
                    obj[m] = new MyJson.JsonNode_ValueString((string)sobj.Fields[m]);
                }

        //        else if ((Type)m.Value.type.type == typeof(List<object>))
                else if ((Type)field.FieldType.TypeForSystem == typeof(List<CLRSharp.CLRSharp_Instance>))
                {
                    //处理List
                    List<CLRSharp.CLRSharp_Instance> slist = sobj.Fields[m] as List<CLRSharp.CLRSharp_Instance>;
                    var list = new MyJson.JsonNode_Array();
                    obj[m] = list;
                    foreach (var item in slist)
                    {
                        list.Add(ToJson(item));
                    }
                }
                else if ((Type)field.FieldType.TypeForSystem == typeof(List<string>))
                {
                    var list = new MyJson.JsonNode_Array();
                    obj[m] = list;

                    List<string> slist = sobj.Fields[m] as List<string>;

                    foreach (var item in slist)
                    {
                        list.Add(new MyJson.JsonNode_ValueString(item));
                    }
                }
                else if ((Type)field.FieldType.TypeForSystem == typeof(List<int>))
                {
                    var list = new MyJson.JsonNode_Array();
                    obj[m] = list;

                    List<int> slist = sobj.Fields[m] as List<int>;

                    foreach (var item in slist)
                    {
                        list.Add(new MyJson.JsonNode_ValueNumber(item));
                    }
                }
                else if ((Type)field.FieldType.TypeForSystem == typeof(List<float>))
                {
                    var list = new MyJson.JsonNode_Array();
                    obj[m] = list;

                    List<float> slist = sobj.Fields[m] as List<float>;

                    foreach (var item in slist)
                    {
                        list.Add(new MyJson.JsonNode_ValueNumber(item));
                    }
                }
                else if ((Type)field.FieldType.TypeForSystem == typeof(List<double>))
                {
                    var list = new MyJson.JsonNode_Array();
                    obj[m] = list;

                    List<double> slist = sobj.Fields[m] as List<double>;

                    foreach (var item in slist)
                    {
                        list.Add(new MyJson.JsonNode_ValueNumber(item));
                    }
                }
                else if (field.FieldType is CLRSharp.ICLRType_Sharp)
                //        else if ((CSLE.SType)m.Value.type.type != null)//其他嵌套脚本类型
                {
                    obj[m] = ToJson(sobj.Fields[m]);
                }
                else
                {
                    Logger.Log("发现不能处理的类型:" + stype.Name + ":" + m + ":" + field.FieldType.ToString());
                }
            }
            return obj;
        }
    }
}
