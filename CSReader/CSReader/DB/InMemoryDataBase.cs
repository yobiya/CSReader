
namespace CSReader.DB
{
    public class InMemoryDataBase : DataBaseBase
    {
        public override void Connect(string solutionDirectoryPath, bool isRead)
        {
            if (_connection != null)
            {
                // 既に接続されているので、何も行わない
                return;
            }

            ConnectImpl(":memory:", isRead);
        }
    }
}
