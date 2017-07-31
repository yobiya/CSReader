namespace CSReader.Command
{
    /// <summary>
    /// サポートされていないコマンド
    /// </summary>
    public class UnsupportedCommand : ICommand
    {
        private readonly string _commandName;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandName">コマンド名</param>
        public UnsupportedCommand(string commandName)
        {
            _commandName = commandName;
        }

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>なし</returns>
        /// <exception cref="Exception">エラーメッセージを含む例外</exception>  
        public string Execute()
        {
            throw new System.Exception($"'{_commandName}' is unknown command name.");
        }
    }
}
