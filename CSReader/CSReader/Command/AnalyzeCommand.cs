using System.Collections.Generic;

namespace CSReader.Command
{
    /// <summary>
    /// プロジェクトの解析を行うコマンド
    /// </summary>
    class AnalyzeCommand : ICommand
    {
		public const string COMMAND_NAME = "analyze";

		public AnalyzeCommand(IEnumerable<string> args)
		{
		}

        /// <summary>
        /// 解析を実行する
        /// </summary>
		public void Execute()
		{
		}

		public static ICommand Create(IEnumerable<string> args)
		{
			return new AnalyzeCommand(args);
		}
    }
}
