
using CLScriptExt;
using CSEvilTestor.testfunc;
//using CSEvilTestor;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;


class Like
{
    public string name;
    public string desc;
}
class Love
{
    public int id;
}
class TSave
{
    public string name;
    public int age;
    public Love love;
    public List<Like> like;
    public List<string> strs;
}
class Test09
{
    public Color32 vv;

    public static void Test()
    {
        MyJson.JsonNode_Object objst = MyJson.Parse
            ("{\"love\":{\"id\":12345}\"name\":\"aname\",\"age\":123,\"like\":[{\"name\":\"aaa\",\"desc\":\"aaaaaa\"},{\"name\":\"bbb\",\"desc\":\"bbbbbb\"}],\"strs\":[\"aa\",\"bb\"]}")
                as MyJson.JsonNode_Object;

        //throw new NotImplementedException("仍然未改写");
        TSave read = LSharpConvert.FromJson(typeof(TSave), objst) as TSave;

        Logger.Log("read.name=" + read.name);
        Logger.Log("read.like[0].name=" + read.like[0].name);
        Like l = read.like[0];
        Logger.Log(l.name);

        Logger.Log("read.strs[1]=" + read.strs[1]);
        Logger.Log("read.love.id=" + read.love.id);
        Logger.Log("write" + LSharpConvert.ToJson(read).ToString());
    }

}

