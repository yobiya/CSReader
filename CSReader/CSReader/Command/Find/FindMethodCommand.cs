using CSReader.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Command.Find
{
    /// <summary>
    /// メソッドを検索するコマンド
    /// </summary>
    public class FindMethodCommand : ICommand
    {
        private class FindCondition
        {
            public string MethodName;
            public bool? IsStatic;
            public bool? IsVirtual;
        }

        private readonly DataBase _dataBase;
        private readonly FindCondition _condition;

        public event Action OnExecuteEnd;

        private FindMethodCommand(DataBase dataBase, FindCondition condition)
        {
            _dataBase = dataBase;
            _condition = condition;
        }

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>検索結果</returns>
        public string Execute()
        {
            OnExecuteEnd?.Invoke();
            return null;
        }

        public static FindMethodCommand Create(DataBase dataBase, IEnumerable<string> args)
        {
            if (args.Count() == 0)
            {
                throw new Exception("Find info is nothing.");
            }

            var condition = new FindCondition();
            foreach (var arg in args)
            {
                switch (arg)
                {
                case "static": condition.IsStatic = true; break;
                case "virtual": condition.IsVirtual = true; break;
                default: condition.MethodName = arg;  break;
                }
            }

            return new FindMethodCommand(dataBase, condition);
        }
    }
}
