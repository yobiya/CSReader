using System;

namespace CSReader.Command.Info
{
    public class InfoHelpCommand : ICommand
    {
        public event Action OnExecuteEnd;

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>ヘルプの文字列</returns>
        public string Execute()
        {
            OnExecuteEnd?.Invoke();

            return string.Empty;
        }
    }
}
