using CSReader.Analyze.Info;
using System;
using System.Data.Linq;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace CSReader.DB
{
    public class DataBase : IDisposable
    {
        private SQLiteConnection _connection;

        public void Connect(string directoryPath, bool isRead)
        {
            var dbFilePath = Path.Combine(directoryPath, ".csreader.db");

            if (!isRead)
            {
                // 既にファイルがある場合は削除する
                File.Delete(dbFilePath);
            }

            ConnectImpl(dbFilePath, isRead);
        }

        public void ConnectInMemory(bool isRead)
        {
            ConnectImpl(":memory:", isRead);
        }

        private void ConnectImpl(string dbFilePath, bool isRead)
        {
            var connectionString = new SQLiteConnectionStringBuilder { DataSource = dbFilePath }.ToString();

            if (isRead)
            {
                _connection = new SQLiteConnection(connectionString, true);
            }
            else
            {
                _connection = new SQLiteConnection(connectionString);
            }

            _connection.Open();

            if (!isRead)
            {
                // 必要なテーブルを全て作成する
                SetUpTables();
            }
        }

        private void SetUpTables()
        {
            var tableTypes
                = new []
                {
                    typeof(NamespaceInfo),
                    typeof(MethodInfo),
                    typeof(TypeInfo)
                };

            foreach (var type in tableTypes)
            {
                using (var createTableCommand = _connection.CreateCommand())
                {
                    createTableCommand.CommandText = SQLCreator.CreateCreateTableSQL(type);
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }

        public void Disconnect()
        {
            _connection.Close();
       }

        void IDisposable.Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// 対応するテーブルに情報を挿入する
        /// </summary>
        /// <param name="info">情報</param>
        public void InsertInfo<T>(T info) where T : class
        {
            using (var context = new DataContext(_connection))
            {
                var table = context.GetTable<T>();
                table.InsertOnSubmit(info);
                context.SubmitChanges();
            }
        }

        public T SelectInfo<T>(Func<T, bool> condition) where T : class, IInfo
        {
            return SelectInfos<T>(condition).SingleOrDefault();
        }

        public T[] SelectInfos<T>(Func<T, bool> condition) where T : class, IInfo
        {
            using (var context = new DataContext(_connection))
            {
                var table = context.GetTable<T>();
                return table.Where(condition).ToArray();
            }
        }
    }
}