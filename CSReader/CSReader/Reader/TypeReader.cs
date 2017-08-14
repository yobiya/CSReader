using CSReader.Analyze.Row;
using CSReader.Command.Print;
using CSReader.DB;
using System;
using System.Linq;

namespace CSReader.Reader
{
    public class TypeReader
    {
        public class Info
        {
            [Value("name")]
            public string Name;

            [Value("kind")]
            public string Kind;

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

        /// <summary>
        /// 型の情報を読み取る
        /// </summary>
        /// <param name="name">型の名前</param>
        /// <returns>型の情報</returns>
        /// <exception cref="Exception">対応する型が見つからなかった</exception>  
        public Info Read(string name)
        {
            var typeDec = _dataBase.SelectInfo<TypeDeclarationRow>(i => i.Name == name);
            if (typeDec == null)
            {
                throw new Exception($"Type name '{name}' is not found.");
            }

            string parentName = "[none]";
            string namespaceName = "[none]";
            if (typeDec.ParentId != 0)
            {
                // 親のIDを持っている
                var namespaceDec = _dataBase.GetRows<NamespaceDeclarationRow>().SingleOrDefault(i => i.Id == typeDec.ParentId);
                if (namespaceDec == null)
                {
                    // 親はネームスペースではなかったので、別の型とみなす
                    var parentTypeDec = _dataBase.GetRows<TypeDeclarationRow>().Single(i => i.Id == typeDec.ParentId);
                    parentName = parentTypeDec.Name;
                    namespaceDec = GetNamespaceDeclarationRow(parentTypeDec.ParentId);
                }

                if (namespaceDec != null)
                {
                    namespaceName = namespaceDec.Name;
                }
            }

            var methodInfos = _dataBase.SelectInfos<MethodDeclarationRow>(i => i.ParentTypeId == typeDec.Id);

            return
                new Info
                {
                    Name = typeDec.Name,
                    Kind = typeDec.TypeKind.ToString().ToLower(),
                    ParentName = parentName,
                    Namespace = namespaceName,
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
            if (id == 0)
            {
                return null;
            }

            var row = _dataBase.GetRows<NamespaceDeclarationRow>().SingleOrDefault(i => i.Id == id);

            if (row == null)
            {
                var typeDeclarationRow = _dataBase.GetRows<TypeDeclarationRow>().SingleOrDefault(i => i.Id == id);
                return GetNamespaceDeclarationRow(typeDeclarationRow.ParentId);
            }

            return row;
        }
    }
}
