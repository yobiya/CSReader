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
		public void Execute()
		{
			System.Console.WriteLine("Usage: csr analyze [target solution file path]");
		}
    }
}
