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

        private static readonly Func<string, DataBaseBase, ICondition>[] _createCondtions =
        {
            VirtualCondition.Create,
            OverrideCondition.Create,
            StaticCondition.Create,
            NameCondition.Create,
            CallCountCondition.Create
        };

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
            var methodRows = _finder.Find();

            if (!methodRows.Any())
            {
                return null;
            }

            return
                methodRows
                    .Select(i =>
                        {
                            var parentName = GetAllParentName(i.ParentTypeId);
                            return (parentName == null) ? i.Name : $"{parentName}.{i.Name}";
                        })
                    .Aggregate((a, b) => a + Environment.NewLine + b);
        }

        private IEnumerable<ICondition> CreateConditions(IEnumerable<string> args)
        {
            if (args.Count() == 0)
            {
                throw new Exception("'find' method command arguments are empty.");
            }

            foreach (var arg in args)
            {
                var condition = _createCondtions.Select(c => c(arg, _dataBase)).FirstOrDefault(c => c != null);

                if (condition == null)
                {
                    throw new Exception($"'find' method command argument '{arg}' is not supported.");
                }

                yield return condition;
            }
        }

        private string GetAllParentName(int parentId)
        {
            var parentTypeRow = _dataBase.GetRows<TypeDeclarationRow>().SingleOrDefault(t => t.Id == parentId);
            if (parentTypeRow == null)
            {
                // 親は型の定義ではなかった
                var namespaceRow = _dataBase.GetRows<NamespaceDeclarationRow>().SingleOrDefault(n => n.Id == parentId);
                if (namespaceRow == null)
                {
                    // 親はネームスペースではなかった
                    return null;
                }

                return namespaceRow.Name;
            }

            var parentName = GetAllParentName(parentTypeRow.ParentId);
            if (parentName == null)
            {
                // 親の名前は無かった
                return parentTypeRow.Name;
            }

            return $"{parentName}.{parentTypeRow.Name}";
        }
    }
}
