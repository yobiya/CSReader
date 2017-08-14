using System;
using System.Linq;
using CSReader.Analyze.Row;
using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSReader.Analyze
{
    /// <summary>
    /// 宣言の解析を行う
    /// </summary>
    public class DeclarationAnalyzer
    {
        private readonly DataBaseBase _dataBase;
        private readonly UniqueIdGenerator _idGenerator;

        public DeclarationAnalyzer(DataBaseBase dataBase, UniqueIdGenerator idGenerator)
        {
            _dataBase = dataBase;
            _idGenerator = idGenerator;
        }

        public NamespaceDeclarationRow AnalyzeNamespace(string name)
        {
            var namespaceDeclarationRow = _dataBase.GetRows<NamespaceDeclarationRow>().FirstOrDefault(i => i.Name == name);
            if (namespaceDeclarationRow == null)
            {
                // 保存されていないので、新しく作成して保存する
                namespaceDeclarationRow
                    = new NamespaceDeclarationRow
                        {
                            Id = _idGenerator.Generate(),
                            Name = name
                        };

                _dataBase.Insert(namespaceDeclarationRow);
            }

            return namespaceDeclarationRow;
        }

        public TypeDeclarationRow AnalyzeType(string name, int parentId, Func<TypeKind> getTypeKind)
        {
            var row = _dataBase.GetRows<TypeDeclarationRow>().FirstOrDefault(i => i.Name == name && i.ParentId == parentId);
            if (row == null)
            {
                // 保存されていないので、新しく作成して保存する
                row
                    = new TypeDeclarationRow
                        {
                            Id = _idGenerator.Generate(),
                            Name = name,
                            TypeKind = getTypeKind(),
                            ParentId = parentId
                        };

                _dataBase.Insert(row);
            }

            return row;
        }
    }
}
