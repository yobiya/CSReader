using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Row
{
    [Table(Name="type_info")]
    public class TypeDeclarationRow
    {
        public enum Category
        {
            Class,
            Interface,
            Struct,
            Enum
        }

        [Column(Name="id", DbType="INT", CanBeNull=false, IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(Name="name", DbType="NVARCHAR", CanBeNull=false)]
        public string Name { get; set; }

        [Column(Name="category", DbType="INT", CanBeNull=false)]
        public Category CategoryValue { get; set; }

        [Column(Name="parent_id", DbType="INT", CanBeNull=false)]
        public int ParentId { get; set; }
    }
}
