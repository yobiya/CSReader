using CSReader.Analyze.Row;
using CSReader.DB;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Find
{
    /// <summary>
    /// メソッドの検索を行う
    /// </summary>
    public class MethodFinder
    {
        private readonly DataBaseBase _dataBase;
        private readonly IEnumerable<ICondition> _conditions;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataBase">データベース</param>
        /// <param name="conditions">条件のコレクション</param>
        public MethodFinder(DataBaseBase dataBase, IEnumerable<ICondition> conditions)
        {
            _dataBase = dataBase;
            _conditions = conditions;
        }

        /// <summary>
        /// 検索を行う
        /// </summary>
        /// <returns>見つかったメソッド情報のコレクション</returns>
        public IEnumerable<MethodDeclarationRow> Find()
        {
            var rows = _dataBase.GetRows<MethodDeclarationRow>();

            foreach (var row in rows)
            {
                if (_conditions.All(c => c.Match(row)))
                {
                    yield return row;
                }
            }
        }
    }
}
