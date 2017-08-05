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
        public class Condition
        {
            public string MethodName;
            public MethodDeclarationRow.Qualifier Qualifier;
        }

        private readonly DataBaseBase _dataBase;
        private readonly Condition _condition;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataBase">データベース</param>
        /// <param name="condition">条件</param>
        public MethodFinder(DataBaseBase dataBase, Condition condition)
        {
            _dataBase = dataBase;
            _condition = condition;
        }

        /// <summary>
        /// 検索を行う
        /// </summary>
        /// <returns>見つかったメソッド情報のコレクション</returns>
        public IEnumerable<MethodDeclarationRow> Find()
        {
            var rows = _dataBase.GetRows<MethodDeclarationRow>();
            if (_condition.MethodName != null)
            {
                rows = rows.Where(i => i.Name == _condition.MethodName);
            }

            if (_condition.Qualifier != MethodDeclarationRow.Qualifier.None)
            {
                rows = rows.Where(i => i.QualifierValue == (int)_condition.Qualifier);
            }

            return rows;
        }
    }
}
