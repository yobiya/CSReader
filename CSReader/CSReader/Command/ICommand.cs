namespace CSReader.Command
{
	/// <summary>
	/// コマンドのインターフェイス
	/// </summary>
    public interface ICommand
    {
        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>コマンドの結果コード</returns>
		ResultCode Execute();
    }
}
