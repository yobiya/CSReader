using System;
using System.Data.Linq.Mapping;
using System.Linq;

namespace CSReader.DB
{
    public class SQLCreator
    {
        public static string CreateCreateTableSQL(Type type)
        {
            string command = "CREATE TABLE ";

            var tableAttribute = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), false).First();
            command += tableAttribute.Name + "(";

            command
                += type
                    .GetFields()
                    .Select(t => (ColumnAttribute)t.GetCustomAttributes(typeof(ColumnAttribute), false).First())
                    .Select(CreateTableRowText)
                    .Aggregate((a, b) => $"{a}, {b}");

            command += ");";

            return command;
        }

        private static string CreateTableRowText(ColumnAttribute attribute)
        {
            string text = $"{attribute.Name} {attribute.DbType}";

            if (!attribute.CanBeNull)
            {
                text += " NOT NULL";
            }

            if (attribute.IsPrimaryKey)
            {
                text += " PRIMARY KEY";
            }

            return text;
        }
    }
}
