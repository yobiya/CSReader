using CSReader.Analyze.Info;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SQLite;
using System.IO;
using System.Linq;

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
            var dbFilePath = Path.Combine(csreaderDirectoryPath, "analyze.db");

            // 既にファイルがある場合は削除する
            File.Delete(dbFilePath);

            var connectionString = new SQLiteConnectionStringBuilder { DataSource = dbFilePath }.ToString();

            _connection = new SQLiteConnection(connectionString);
            _connection.Open();

            // 必要なテーブルを全て作成する
            SetUpTables();
        }

        private void SetUpTables()
        {
            var tableTypes
                = new []
                {
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

        public TypeInfo SelectTypeInfo(string name)
        {
            using (var context = new DataContext(_connection))
            {
                var table = context.GetTable<TypeInfo>();
                return table.Where(i => i.Name == name).SingleOrDefault();
            }
        }
    }
}