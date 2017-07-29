using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Info
{
    public class TypeInfo
    {
        public string Name;

        public TypeInfoTableColumn CreateTableColumn()
        {
            return
                new TypeInfoTableColumn
                {
                    Name = Name
                };
        }
    }

    [Table(Name="type_info")]
    public class TypeInfoTableColumn
    {
        [Column(Name="name", DbType="NVARCHAR", CanBeNull=false, IsPrimaryKey = true)]
        public string Name;
    }
}
