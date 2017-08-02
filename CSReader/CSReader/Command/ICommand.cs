using System;

namespace CSReader.Command
{
    /// <summary>
    /// コマンドのインターフェイス
    /// </summary>
    public interface ICommand
    {
        event Action OnExecuteEnd;

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>
        /// 出力する文字列
        /// 出力するものが無ければnull
        /// </returns>
        /// <exception cref="Exception">エラーが発生した場合は例外が投げられる</exception>  
        string Execute();
    }
}
