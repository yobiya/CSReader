using CSReader.DB;
using System;

namespace CSReader.Command.Find
{
    /// <summary>
    /// メソッドを検索するコマンド
    /// </summary>
    public class FindMethodCommand : ICommand
    {
        private readonly DataBase _dataBase;

        public event Action OnExecuteEnd;

        public FindMethodCommand(DataBase dataBase)
        {
            _dataBase = dataBase;
        }

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>検索結果</returns>
        public string Execute()
        {
            return null;
        }
    }
}
