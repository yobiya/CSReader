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

            return
                new Info
                {
                    Name = typeInfo.Name,
                    NameSpace = namespaceInfo.Name
                };
        }
    }
}
