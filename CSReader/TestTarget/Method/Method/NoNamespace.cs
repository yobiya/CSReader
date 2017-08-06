/// <summary>
/// ネームスペースを持たないクラス
/// </summary>
public class NoNamespace
{
    private int _value;

    public int Value => _value;

    public NoNamespace(int value)
    {
        _value = value;
    }

    public void AddValue(int add)
    {
        _value += add;
    }
}
