using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Row
{
    [Table(Name = "namespace_info")]
    public class NamespaceDeclarationRow
    {
        [Column(Name = "id", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(Name = "name", DbType = "NVARCHAR", CanBeNull = false)]
        public string Name { get; set; }
    }
}
