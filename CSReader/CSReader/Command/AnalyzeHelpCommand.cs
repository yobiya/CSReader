namespace CSReader.Command
{
    /// <summary>
    /// Analyzeコマンドのヘルプコマンド
    /// </summary>
    public class AnalyzeHelpCommand : ICommand
    {
        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>コマンドの結果コード</returns>
		public ResultCode Execute()
		{
			System.Console.WriteLine("Usage: csr analyze [target solution file path]");

			return ResultCode.Sucess;
		}
    }
}
