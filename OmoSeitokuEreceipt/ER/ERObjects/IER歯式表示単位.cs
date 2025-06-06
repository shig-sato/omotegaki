namespace OmoEReceLib.ERObjects
{
    public interface IER歯式表示単位
    {
        bool Is乳歯 { get; }
        ER_状態 状態 { get; }
        string 歯席表示 { get; }
    }
}
