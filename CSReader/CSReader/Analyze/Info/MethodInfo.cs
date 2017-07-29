using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Info
{
    [Table(Name="method_info")]
    public class MethodInfo
    {
        [Column(Name="name", DbType="NVARCHAR", CanBeNull=false, IsPrimaryKey = true)]
        public string Name;
    }
}
