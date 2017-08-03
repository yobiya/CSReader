using CSReader.Analyze.Info;
using CSReader.DB;
using CSReader.Find;
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
        private readonly DataBase _dataBase;
        private readonly MethodFinder _finder;

        public event Action OnExecuteEnd;

        private FindMethodCommand(DataBase dataBase, MethodFinder.Condition condition)
        {
            _dataBase = dataBase;
            _finder = new MethodFinder(dataBase, condition);
        }

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <returns>検索結果</returns>
        public string Execute()
        {
            var methodInfos = _finder.Find();

            string result = null;

            if (methodInfos.Any())
            {
                result
                    = methodInfos
                        .Select(i =>
                            {
                                var typeInfo = _dataBase.SelectInfo<TypeInfo>(t => t.Id == i.ParentTypeId);
                                var namespaceInfo = _dataBase.SelectInfo<NamespaceInfo>(n => n.Id == typeInfo.NamespaceId);

                                return $"{namespaceInfo.Name}.{typeInfo.Name}.{i.Name}";
                            })
                        .Aggregate((a, b) => a + Environment.NewLine + b);
            }

            OnExecuteEnd?.Invoke();

            return result;
        }

        public static FindMethodCommand Create(DataBase dataBase, IEnumerable<string> args)
        {
            if (args.Count() == 0)
            {
                throw new Exception("Find info is nothing.");
            }

            var condition = new MethodFinder.Condition();
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
