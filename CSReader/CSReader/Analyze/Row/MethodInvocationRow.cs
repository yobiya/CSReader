﻿using System.Data.Linq.Mapping;

namespace CSReader.Analyze.Row
{
    [Table(Name="method_invocation")]
    public class MethodInvocationRow
    {
        [Column(Name="id", DbType="INT", CanBeNull=false, IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(Name="name", DbType="NVARCHAR", CanBeNull=false)]
        public string Name { get; set; }

        [Column(Name="method_declaration_id", DbType="INT", CanBeNull=false)]
        public int MethodDeclarationId { get; set; }
    }
}
