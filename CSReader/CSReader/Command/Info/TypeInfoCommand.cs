using CSReader.Analyze.Info;
using CSReader.DB;
using CSReader.Reader;
using CSReader.Reader.FindKey;

namespace CSReader.Command.Info
{
    public class TypeInfoCommand : ICommand
    {
        private readonly TypeReader _typeReader;
        private string _name;

        public TypeInfoCommand(DataBase dataBase, string name)
        {
            _typeReader = new TypeReader(dataBase);
            _name = name;
        }

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>コマンドの結果コード</returns>
        public ResultCode Execute()
        {
            var info = _typeReader.Read(_name);
            if (info == null)
            {
                return ResultCode.Error;
            }

            System.Console.WriteLine(info.Name);

            return ResultCode.Sucess;
        }
    }
}
