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
		void Execute();
    }
}
