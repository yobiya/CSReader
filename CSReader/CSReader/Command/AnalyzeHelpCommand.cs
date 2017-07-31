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
        /// <returns>ヘルプ文字列</returns>
        public string Execute()
        {
            return "Usage: csr analyze [target solution file path]";
        }
    }
}
