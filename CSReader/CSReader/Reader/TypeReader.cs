using CSReader.Analyze.Info;
using CSReader.DB;
using CSReader.Reader.FindKey;

namespace CSReader.Reader
{
    public class TypeReader
    {
        public class Info
        {
            public string Name;
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

            return
                new Info
                {
                    Name = typeInfo.Name,
                    NameSpace = _dataBase.SelectInfo<NamespaceInfo>(new IdFindKey(typeInfo.NamespaceId)).Name
                };
        }
    }
}
