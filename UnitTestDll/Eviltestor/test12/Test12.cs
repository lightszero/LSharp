
using UnitTest;

/// <summary>
/// 测试覆盖系统方法
/// </summary>
public class Test12
{
    public string Name;
    public int Age;

    public override string ToString()
    {
        return string.Format("Name:{0}, Age:{1}", Name, Age);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Test12))
        {
            return false;
        }
        var o = (Test12) obj;
        return Name == o.Name && Age == o.Age;
    }

    public override int GetHashCode()
    {
        //for test
        return 123456789;
    }

    public static void TestOverrideObjectMethod()
    {
        Test12 t1 = new Test12 {Name = "Test1", Age = 18};
        Test12 t2 = new Test12 {Name = "Test1", Age = 18};

        Logger.Log("t1.ToString(): " + t1);
        Logger.Log("t1.GetHashCode(): " + t1.GetHashCode());
        Logger.Log("t1.Equals(t2): " + t1.Equals(t2));
    }
}
