using System.Linq;

namespace CSReader.Command
{
    /// <summary>
    /// コマンドを生成する
    /// </summary>
    public static class CommandCreator
    {
        /// <summary>
        /// コンソールの引数から対応するコマンドを生成する
        /// </summary>
        /// <param name="args">コンソールの引数</param>
        /// <returns>コマンド</returns>
		public static ICommand Create(string[] args)
		{
			if (args.Length == 0)
			{
				// 引数がなければ、ヘルプコマンドを返す
				return new HelpCommand();
			}

			var commandName = args[0];
			var commandArgs = args.Skip(1);
			switch (commandName)
			{
			case HelpCommand.COMMAND_NAME:
				return new HelpCommand();

			case AnalyzeCommand.COMMAND_NAME:
				return AnalyzeCommand.Create(commandArgs);
			}

			return new UnsupportedCommand(commandName);
		}
    }
}
