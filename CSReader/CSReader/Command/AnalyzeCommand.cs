using System.Collections.Generic;
using System.Linq;

namespace CSReader.Command
{
    /// <summary>
    /// プロジェクトの解析を行うコマンド
    /// </summary>
    public class AnalyzeCommand : ICommand
    {
		public const string COMMAND_NAME = "analyze";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="args">コマンド引数</param>
		private AnalyzeCommand(IEnumerable<string> args)
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
			if (args.Count() == 0)
			{
				// 引数がなければ、ヘルプコマンドを返す
				return new AnalyzeHelpCommand();
			}

			return new AnalyzeCommand(args);
		}
    }
}
