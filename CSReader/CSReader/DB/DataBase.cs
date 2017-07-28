using System;
using System.Data.SQLite;
using System.IO;

namespace CSReader.DB
{
    public class DataBase : IDisposable
    {
        private SQLiteConnection _connection;

        public void Connect(string solutionPath)
        {
            var solutionDirectoryPath = Path.GetDirectoryName(solutionPath);
            var csreaderDirectoryPath = Path.Combine(solutionDirectoryPath, ".csreader");
            Directory.CreateDirectory(csreaderDirectoryPath);
            var path = Path.Combine(csreaderDirectoryPath, "analyze.db");

            var connectionString = new SQLiteConnectionStringBuilder { DataSource = path }.ToString();

            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
        }

        public void Disconnect()
        {
            _connection.Close();
        }

        void IDisposable.Dispose()
        {
            Disconnect();
        }
    }
}
