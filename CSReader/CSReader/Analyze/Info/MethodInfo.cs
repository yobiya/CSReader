using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Info
{
    public class MethodInfo
    {
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
