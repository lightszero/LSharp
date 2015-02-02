using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;

namespace CLScriptExt
{
    public enum Country:ushort
    {
        Chinese,
        English,
        Japnese,
    };
    public class UserData
    {
        public static void ShowContry(Country c)
        {
            Logger.Log(c.ToString());
        }
        public static UserData g_this = new UserData();
        public static UserData Instance()
        {
            return g_this;
        }
        public Dictionary<string, string> HeroDataMap = new Dictionary<string, string>();
    }
}
