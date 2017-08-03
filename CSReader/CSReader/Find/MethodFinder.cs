﻿using CSReader.Analyze.Info;
using CSReader.DB;
using System.Collections.Generic;

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
            public bool? IsStatic;
            public bool? IsVirtual;
        }

        private readonly DataBaseBase _dataBase;
        private readonly Condition _condition;

        public MethodFinder(DataBaseBase dataBase, Condition condition)
        {
            _dataBase = dataBase;
            _condition = condition;
        }

        /// <summary>
        /// 検索を行う
        /// </summary>
        /// <returns>見つかったメソッド情報のコレクション</returns>
        public IEnumerable<MethodInfo> Find()
        {
            IEnumerable<MethodInfo> methodInfos = null;
            if (_condition.MethodName != null)
            {
                methodInfos = _dataBase.SelectInfos<MethodInfo>(i => i.Name == _condition.MethodName);
            }

            if (methodInfos == null)
            {
                return new MethodInfo[0];
            }

            return methodInfos;
        }
    }
}
