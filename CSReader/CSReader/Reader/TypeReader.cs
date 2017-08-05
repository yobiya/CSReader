using CSReader.Analyze.Row;
using CSReader.Command.Print;
using CSReader.DB;
using System.Linq;

namespace CSReader.Reader
{
    public class TypeReader
    {
        public class Info
        {
            [Value("name")]
            public string Name;

            [Value("category")]
            public string Category;

            [Value("parent name")]
            public string ParentName;

            [Value("namespace")]
            public string Namespace;

            [Value("methods")]
            public string[] Methods;
        }

        private readonly DataBaseBase _dataBase;

        public TypeReader(DataBaseBase dataBase)
        {
            _dataBase = dataBase;
        }

        public Info Read(string name)
        {
            var typeDec = _dataBase.SelectInfo<TypeDeclarationRow>(i => i.Name == name);
            if (typeDec == null)
            {
                return null;
            }

            string parentName = "[none]";
            var namespaceDec = _dataBase.GetRows<NamespaceDeclarationRow>().SingleOrDefault(i => i.Id == typeDec.ParentId);
            if (namespaceDec == null)
            {
                // 親はネームスペースではなかったので、別の型とみなす
                var parentTypeDec = _dataBase.GetRows<TypeDeclarationRow>().Single(i => i.Id == typeDec.ParentId);
                parentName = parentTypeDec.Name;
                namespaceDec = GetNamespaceDeclarationRow(parentTypeDec.ParentId);
            }

            var methodInfos = _dataBase.SelectInfos<MethodDeclarationRow>(i => i.ParentTypeId == typeDec.Id);

            return
                new Info
                {
                    Name = typeDec.Name,
                    Category = typeDec.CategoryValue.ToString().ToLower(),
                    ParentName = parentName,
                    Namespace = namespaceDec.Name,
                    Methods = methodInfos.Select(i => i.Name).ToArray()
                };
        }

        /// <summary>
        /// 階層を遡り、ネームスペースの定義を取得する
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>ネームスペースの定義</returns>
        private NamespaceDeclarationRow GetNamespaceDeclarationRow(int id)
        {
            var row = _dataBase.GetRows<NamespaceDeclarationRow>().SingleOrDefault(i => i.Id == id);

            if (row == null)
            {
                var typeDeclarationRow = _dataBase.GetRows<TypeDeclarationRow>().Single(i => i.Id == id);
                return GetNamespaceDeclarationRow(typeDeclarationRow.ParentId);
            }

            return row;
        }
    }
}
