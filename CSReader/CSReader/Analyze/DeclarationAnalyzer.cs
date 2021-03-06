﻿using CSReader.Analyze.Row;
using CSReader.DB;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

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

        public TypeDeclarationRow AnalyzeType(string name, int parentId, TypeKind typeKind)
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
                            TypeKind = typeKind,
                            ParentId = parentId
                        };

                _dataBase.Insert(row);
            }

            return row;
        }

        public MethodDeclarationRow AnalyzeMethod(string name, int parentId, string uniqueName, MethodDeclarationRow.Qualifier qualifier)
        {
            var row = _dataBase.GetRows<MethodDeclarationRow>().FirstOrDefault(i => i.UnieuqName == uniqueName && i.ParentTypeId == parentId);
            if (row == null)
            {
                // 保存されていないので、新しく作成して保存する
                row
                    = new MethodDeclarationRow
                        {
                            Id = _idGenerator.Generate(),
                            Name = name,
                            ParentTypeId = parentId,
                            UnieuqName = uniqueName,
                            QualifierValue = qualifier
                        };

                _dataBase.Insert(row);
            }

            return row;
        }
    }
}
