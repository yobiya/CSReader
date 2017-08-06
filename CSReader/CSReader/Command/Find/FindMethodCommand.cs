using CSReader.Analyze.Row;
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
        private readonly DataBaseBase _dataBase;
        private readonly MethodFinder _finder;

        public FindMethodCommand(DataBaseBase dataBase, IEnumerable<string> args)
        {
            var condition = CreateConditions(args);    
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
                                var typeInfo = _dataBase.SelectInfo<TypeDeclarationRow>(t => t.Id == i.ParentTypeId);
                                var namespaceInfo = _dataBase.SelectInfo<NamespaceDeclarationRow>(n => n.Id == typeInfo.ParentId);

                                return $"{namespaceInfo.Name}.{typeInfo.Name}.{i.Name}";
                            })
                        .Aggregate((a, b) => a + Environment.NewLine + b);
            }

            return result;
        }

        private static IEnumerable<ICondition> CreateConditions(IEnumerable<string> args)
        {
            if (args.Count() == 0)
            {
                throw new Exception("'find' method command arguments are empty.");
            }

            foreach (var arg in args)
            {
                var condition = _createCondtions.Select(c => c(arg)).FirstOrDefault(c => c != null);

                if (condition == null)
                {
                    throw new Exception($"'find' method command argument '{arg}' is not supported.");
                }

                yield return condition;
            }
        }

        private static Func<string, ICondition>[] _createCondtions =
        {
            VirtualCondition.Create,
            OverrideCondition.Create,
            StaticCondition.Create,
            NameCondition.Create
        };
    }
}
