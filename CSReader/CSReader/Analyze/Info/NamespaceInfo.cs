using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Info
{
    [Table(Name = "namespace_info")]
    public class NamespaceInfo : IInfo
    {
        [Column(Name = "id", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(Name = "name", DbType = "NVARCHAR", CanBeNull = false)]
        public string Name { get; set; }
    }
}
