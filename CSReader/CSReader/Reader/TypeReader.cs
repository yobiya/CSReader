using CSReader.DB;

namespace CSReader.Reader
{
    public class TypeReader
    {
        public class Info
        {
            public string Name;
        }

        private readonly DataBase _dataBase;

        public TypeReader(DataBase dataBase)
        {
            _dataBase = dataBase;
        }

        public Info Read(string name)
        {
            var typeInfo = _dataBase.SelectTypeInfo(name);

            return
                new Info
                {
                    Name = typeInfo.Name
                };
        }
    }
}
