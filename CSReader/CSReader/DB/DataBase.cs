using System.IO;

namespace CSReader.DB
{
    public class DataBase : DataBaseBase
    {
        public override void Connect(string solutionDirectoryPath, bool isRead)
        {
            var dbFilePath = Path.Combine(solutionDirectoryPath, ".csreader.db");

            if (!isRead)
            {
                // 既にファイルがある場合は削除する
                File.Delete(dbFilePath);
            }

            ConnectImpl(dbFilePath, isRead);
        }
    }
}