﻿using CSReader.Analyze;
using CSReader.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CSReader.Command
{
    /// <summary>
    /// プロジェクトの解析を行うコマンド
    /// </summary>
    public class AnalyzeCommand : ICommand
    {
        public const string COMMAND_NAME = "analyze";

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
                    var solutionDirectoryPath = Path.GetDirectoryName(_solutionPath);
                    dataBase.Connect(solutionDirectoryPath, false);

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
