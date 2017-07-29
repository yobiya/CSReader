using CSReader.Analyze.Info;

namespace CSReader.Reader.FindKey
{
    public class IdFindKey : IFindKey
    {
        private readonly int _id;

        public IdFindKey(int id)
        {
            _id = id;
        }

        public bool Match(IInfo info)
        {
            return info.Id == _id;
        }
    }
}
