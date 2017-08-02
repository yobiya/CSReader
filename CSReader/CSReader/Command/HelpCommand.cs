using System;

namespace CSReader.Command
{
    /// <summary>
    /// ヘルプを表示するコマンド
    /// </summary>
    public class HelpCommand : ICommand
    {
        public const string COMMAND_NAME = "help";

        public event Action OnExecuteEnd;

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>ヘルプ文字列</returns>
        public string Execute()
        {
            OnExecuteEnd?.Invoke();

            return @"usage: csr [command_name] [command_args] ...";
        }
    }
}
