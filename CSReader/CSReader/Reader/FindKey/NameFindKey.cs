using CSReader.Analyze.Info;

namespace CSReader.Reader.FindKey
{
    public class NameFindKey : IFindKey
    {
        private readonly string _name;

        public NameFindKey(string name)
        {
            _name = name;
        }

        public bool Match(IInfo info)
        {
            return info.Name == _name;
        }
    }
}
