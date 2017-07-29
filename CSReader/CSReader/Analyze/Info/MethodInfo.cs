using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Info
{
    [Table(Name="method_info")]
    public class MethodInfo
    {
        [Column(Name="unique_id", DbType="INT", CanBeNull=false, IsPrimaryKey = true)]
        public int UniqueId;

        [Column(Name="name", DbType="NVARCHAR", CanBeNull=false)]
        public string Name;

        [Column(Name="parent_type_id", DbType="INT")]
        public int ParentTypeId;
    }
}
