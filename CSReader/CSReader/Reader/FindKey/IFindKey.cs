using CSReader.Analyze.Info;

namespace CSReader.Reader.FindKey
{
    public interface IFindKey
    {
        bool Match(IInfo info);
    }
}
