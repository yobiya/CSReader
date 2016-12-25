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
		public void Execute()
		{
			System.Console.Error.WriteLine("'" + _commandName + "' is unknown command name.");
		}
    }
}
