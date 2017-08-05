using CSReader.Analyze.Row;
using System;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;

namespace CSReader.DB
{
    public abstract class DataBaseBase
    {
        protected SQLiteConnection _connection;
        private DataContext _context;

        public abstract void Connect(string solutionDirectoryPath, bool isRead);

        protected void ConnectImpl(string dbFilePath, bool isRead)
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

            _context = new DataContext(_connection);
        }

        private void SetUpTables()
        {
            var tableTypes
                = new []
                {
                    typeof(NamespaceDeclarationRow),
                    typeof(MethodDeclarationRow),
                    typeof(TypeDeclarationRow),
                    typeof(MethodInvocationRow)
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
            _context?.Dispose();
            _connection?.Close();
        }

        /// <summary>
        /// 対応するテーブルに情報を挿入する
        /// </summary>
        /// <param name="info">情報</param>
        public void InsertInfo<T>(T info) where T : class
        {
            var table = _context.GetTable<T>();
            table.InsertOnSubmit(info);
            _context.SubmitChanges();
        }

        public T SelectInfo<T>(Func<T, bool> condition) where T : class
        {
            return SelectInfos<T>(condition).SingleOrDefault();
        }

        public T[] SelectInfos<T>(Func<T, bool> condition) where T : class
        {
            var table = _context.GetTable<T>();
            return table.Where(condition).ToArray();
        }
    }
}