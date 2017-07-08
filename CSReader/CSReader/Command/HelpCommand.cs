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
        /// <returns>コマンドの結果コード</returns>
		public ResultCode Execute()
		{
			string helpMessage = @"usage: csr [command_name] [command_args] ...";
			System.Console.WriteLine(helpMessage);

			return ResultCode.Sucess;
		}
    }
}
