using System;

namespace CSReader.Command.Find
{
    public class FindHelpCommand : ICommand
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
