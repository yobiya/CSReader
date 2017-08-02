using CSReader.Command.Print;
using CSReader.DB;
using CSReader.Reader;
using System;

namespace CSReader.Command.Info
{
    /// <summary>
    /// 型情報を取得するコマンド
    /// </summary>
    public class TypeInfoCommand : ICommand
    {
        private readonly TypeReader _typeReader;
        private string _name;

        public event Action OnExecuteEnd;

        public TypeInfoCommand(DataBase dataBase, string name)
        {
            _typeReader = new TypeReader(dataBase);
            _name = name;
        }

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>型情報の文字列</returns>
        public string Execute()
        {
            var info = _typeReader.Read(_name);

            OnExecuteEnd?.Invoke();
            return PrintConverter.Convert(info);
        }
    }
}
