using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Info
{
    [Table(Name="method_info")]
    public class MethodInfo : IInfo
    {
        [Column(Name="id", DbType="INT", CanBeNull=false, IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(Name="name", DbType="NVARCHAR", CanBeNull=false)]
        public string Name { get; set; }

        [Column(Name="parent_type_id", DbType="INT", CanBeNull = false)]
        public int ParentTypeId { get; set; }

        [Column(Name="unique_name", DbType="NVARCHAR", CanBeNull=false)]
        public string UnieuqName { get; set; }
    }
}
