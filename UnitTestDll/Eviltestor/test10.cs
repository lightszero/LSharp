using UnitTest;
class Test10
{
    private int i = 0;

    public Test10()
    {
        i = 10;
    }

    public void Debug()
    {
        Logger.Log(i.ToString());
    }
}