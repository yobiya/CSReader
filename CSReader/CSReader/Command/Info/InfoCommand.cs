using CSReader.DB;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Command.Info
{
    /// <summary>
    /// 解析した情報を表示するコマンド
    /// </summary>
    public class InfoCommand
    {
        public const string COMMAND_NAME = "info";

        public static ICommand Create(DataBaseBase dataBase, IEnumerable<string> args)
        {
            if (args.Count() == 0)
            {
                // 引数がなければ、ヘルプコマンドを返す
                return new InfoHelpCommand();
            }

            dataBase.Connect(System.Environment.CurrentDirectory, true);

            string categoryOption = args.Take(1).Single();
            args = args.Skip(1);
            string name = args.Take(1).Single();

            ICommand command = null;
            switch (categoryOption)
            {
            case "-t":
                command = new TypeInfoCommand(dataBase, name);
                break;

            default:
                command = new InfoHelpCommand();
                break;
            }

            return command;
        }
    }
}
