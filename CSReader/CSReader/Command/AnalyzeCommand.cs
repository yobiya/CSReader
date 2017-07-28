using CSReader.Analyze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSReader.DB;

namespace CSReader.Command
{
    /// <summary>
    /// プロジェクトの解析を行うコマンド
    /// </summary>
    public class AnalyzeCommand : ICommand
    {
        public const string COMMAND_NAME = "analyze";
        private const string DB_FILE_PATH = ".csreader/analyze.db";

        private readonly string _solutionPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="solutionPath">解析するソリューションのパス</param>
        private AnalyzeCommand(string solutionPath)
        {
            _solutionPath = solutionPath;
        }

        /// <summary>
        /// 解析を実行する
        /// </summary>
        /// <returns>コマンドの結果コード</returns>
        public ResultCode Execute()
        {
            try
            {
                using (var dataBase = new DataBase())
                {
                    dataBase.Connect(_solutionPath);

                    var analyzer = new Analyzer(_solutionPath, dataBase);
                    analyzer.Analyze();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return ResultCode.Error;
            }

            return ResultCode.Sucess;
        }

        public static ICommand Create(IEnumerable<string> args)
        {
            if (args.Count() == 0)
            {
                // 引数がなければ、ヘルプコマンドを返す
                return new AnalyzeHelpCommand();
            }

            var solutionPath = args.First();
            return new AnalyzeCommand(solutionPath);
        }
    }
}
