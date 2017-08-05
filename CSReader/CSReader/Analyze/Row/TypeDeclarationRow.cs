using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Row
{
    [Table(Name="type_info")]
    public class TypeDeclarationRow
    {
        [Column(Name="id", DbType="INT", CanBeNull=false, IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(Name="name", DbType="NVARCHAR", CanBeNull=false)]
        public string Name { get; set; }

        [Column(Name="namespace_id", DbType="INT", CanBeNull=false)]
        public int NamespaceId { get; set; }
    }
}
