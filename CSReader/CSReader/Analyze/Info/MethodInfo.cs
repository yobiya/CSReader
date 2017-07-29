using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Info
{
    public class MethodInfo
    {
        /// <summary>
        /// メソッド情報を格納するテーブルの作成SQL文
        /// </summary>
        public static string CreateTableCommandText
        {
            get
            {
                return "CREATE TABLE method_info(name NVARCHAR NOT NULL PRIMARY KEY);";
            }
        }

        public string Name;

        public MethodInfoTableColumn CreateTableColumn()
        {
            return
                new MethodInfoTableColumn
                {
                    Name = Name
                };
        }
    }

    [Table(Name="method_info")]
    public class MethodInfoTableColumn
    {
        [Column(Name="name", DbType="NVARCHAR", CanBeNull=false, IsPrimaryKey = true)]
        public string Name;
    }
}
