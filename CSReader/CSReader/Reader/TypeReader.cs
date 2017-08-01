using System.Linq;
using CSReader.Analyze.Info;
using CSReader.Command.Print;
using CSReader.DB;
using CSReader.Reader.FindKey;

namespace CSReader.Reader
{
    public class TypeReader
    {
        public class Info
        {
            [Value("name")]
            public string Name;

            [Value("namespace")]
            public string NameSpace;

            [Value("methods")]
            public string[] Methods;
        }

        private readonly DataBase _dataBase;

        public TypeReader(DataBase dataBase)
        {
            _dataBase = dataBase;
        }

        public Info Read(string name)
        {
            var typeInfo = _dataBase.SelectInfo<TypeInfo>(new NameFindKey(name));
            if (typeInfo == null)
            {
                return null;
            }

            var namespaceInfo = _dataBase.SelectInfo<NamespaceInfo>(new IdFindKey(typeInfo.NamespaceId));
            var methodInfos = _dataBase.SelectInfos<MethodInfo>(i => i.ParentTypeId == typeInfo.Id);

            return
                new Info
                {
                    Name = typeInfo.Name,
                    NameSpace = namespaceInfo.Name,
                    Methods = methodInfos.Select(i => i.Name).ToArray()
                };
        }
    }
}
