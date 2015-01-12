using UnitTest;
class Test10
{
    private int i = 0;
    short j = 0;
    long k = 0;
    public Test10()
    {
        i = 10;
        j = 35;
        k = 58;
    }

    public void Debug()
    {
        Logger.Log(i.ToString());
    }

    public static void TestInnerClass()
    {
        new InnerClass();
    }

    public class InnerClass
    {
        public InnerClass()
        {

        }
    }
}