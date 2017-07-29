using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Info
{
    [Table(Name="type_info")]
    public class TypeInfo
    {
        [Column(Name="unique_id", DbType="INT", CanBeNull=false, IsPrimaryKey = true)]
        public int UniqueId;

        [Column(Name="name", DbType="NVARCHAR", CanBeNull=false)]
        public string Name;
    }
}
