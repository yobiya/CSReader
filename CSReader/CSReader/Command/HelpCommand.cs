namespace CSReader.Command
{
    /// <summary>
    /// ヘルプを表示するコマンド
    /// </summary>
    public class HelpCommand : ICommand
    {
        public const string COMMAND_NAME = "help";

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>ヘルプ文字列</returns>
        public string Execute()
        {
            return @"usage: csr [command_name] [command_args] ...";
        }
    }
}
