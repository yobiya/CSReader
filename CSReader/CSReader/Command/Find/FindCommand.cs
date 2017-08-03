using CSReader.DB;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Command.Find
{
    /// <summary>
    /// 解析した情報を検索するコマンド
    /// </summary>
    public class FindCommand
    {
        public const string COMMAND_NAME = "find";

        public static ICommand Create(DataBaseBase dataBase, IEnumerable<string> args)
        {
            if (args.Count() == 0)
            {
                // 引数がなければ、ヘルプコマンドを返す
                return new FindHelpCommand();
            }

            dataBase.Connect(System.Environment.CurrentDirectory, true);

            string categoryOption = args.Take(1).Single();
            args = args.Skip(1);

            ICommand command = null;
            switch (categoryOption)
            {
            case "-m":
                command = FindMethodCommand.Create(dataBase, args);
                break;

            default:
                command = new FindHelpCommand();
                break;
            }

            return command;
        }
    }
}
