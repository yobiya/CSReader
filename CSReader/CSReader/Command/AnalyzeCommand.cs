using CSReader.Analyze;
using CSReader.DB;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSReader.Command
{
    /// <summary>
    /// プロジェクトの解析を行うコマンド
    /// </summary>
    public class AnalyzeCommand : ICommand
    {
        public const string COMMAND_NAME = "analyze";

        private readonly DataBaseBase _dataBase;
        private readonly string _solutionPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataBase">未接続のデータベース</param>
        /// <param name="solutionPath">解析するソリューションのパス</param>
        private AnalyzeCommand(DataBaseBase dataBase, string solutionPath)
        {
            _dataBase = dataBase;
            _solutionPath = solutionPath;
        }

        /// <summary>
        /// 解析を実行する
        /// </summary>
        /// <returns>表示する文字列は無いのでnull</returns>
        public string Execute()
        {
            var solutionDirectoryPath = Path.GetDirectoryName(_solutionPath);
            _dataBase.Connect(solutionDirectoryPath, false);

            var analyzer = new Analyzer(_dataBase, _solutionPath);
            analyzer.Analyze();

            // 表示する文字列は無い
            return null;
        }

        public static ICommand Create(DataBaseBase dataBase, IEnumerable<string> args)
        {
            if (args.Count() == 0)
            {
                // 引数がなければ、ヘルプコマンドを返す
                return new AnalyzeHelpCommand();
            }

            var solutionPath = args.First();
            return new AnalyzeCommand(dataBase, solutionPath);
        }
    }
}
