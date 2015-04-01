using System;
using UnitTest;
using System.Collections.Generic;

namespace UnitTestDll
{
	public class test_RefOut
	{
		private class UserDefClass {
			public int value01;
			public string value02 = "";

			public void StringToValue(ref string v, string str) {
				v = str;
			}
		}


		public static void UnitTest_Out() {
			Dictionary<int, UserDefClass> dict = new Dictionary<int,UserDefClass>();
			UserDefClass obj = new UserDefClass();
			obj.value01 = 888;
			dict.Add(0, obj);
			UserDefClass testObj;
			if(dict.ContainsKey(0)) {
				testObj = dict[0];
				Logger.Log(string.Format("Value01 : {0}", testObj.value01));
			}
			if(dict.TryGetValue(0, out testObj)) {
				Logger.Log("Test OK.");
			}
		}

		public static void UnitTest_Ref() {

			UserDefClass obj = new UserDefClass();
			obj.StringToValue(ref obj.value02, "test");
			Logger.Log(obj.value02);

		}

	}
}
