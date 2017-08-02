using System;

namespace CSReader.Command
{
    /// <summary>
    /// Analyzeコマンドのヘルプコマンド
    /// </summary>
    public class AnalyzeHelpCommand : ICommand
    {
        public event Action OnExecuteEnd;

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>ヘルプ文字列</returns>
        public string Execute()
        {
            OnExecuteEnd?.Invoke();

            return "Usage: csr analyze [target solution file path]";
        }
    }
}
