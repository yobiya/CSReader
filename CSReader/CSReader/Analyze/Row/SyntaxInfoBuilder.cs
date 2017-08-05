using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Analyze.Row
{
    public class SyntaxInfoBuilder
    {
        private readonly DataBaseBase _dataBase;
        private readonly SyntaxWalker _syntaxWalker;
        private readonly UniqueIdGenerator _idGenerator;

        public SyntaxInfoBuilder(DataBaseBase dataBase, SyntaxWalker syntaxWalker, UniqueIdGenerator idGenerator)
        {
            _dataBase = dataBase;
            _syntaxWalker = syntaxWalker;
            _idGenerator = idGenerator;
        }

        public void BuildNamespaceInfos()
        {
            foreach (var syntax in _syntaxWalker.NamespaceDeclarationSyntaxList)
            {
                BuildNamespaceDeclarationRow(syntax);
            }
        }

        public void BuildTypeInfos()
        {
            foreach (var syntax in _syntaxWalker.ClassDeclarationSyntaxList)
            {
                BuildTypeDeclarationRow(syntax);
            }
        }

        public void BuildMethodInfos()
        {
            foreach (var syntax in _syntaxWalker.MethodDeclarationSyntaxList)
            {
                var info
                    = new MethodDeclarationRow
                        {
                            Id = _idGenerator.Generate(),
                            Name = syntax.Identifier.Text,
                            ParentTypeId = BuildTypeDeclarationRow((dynamic)syntax.Parent).Id,
                            UnieuqName = ConvertUniqueName(syntax),
                            QualifierValue = ConvertQualifierValue(syntax.Modifiers)
                        };

                _dataBase.InsertInfo(info);
            }
        }

        private string ConvertUniqueName(MethodDeclarationSyntax syntax)
        {
            string name = syntax.Identifier.Text + "(";

            var argTypeNameList = new List<string>();
            foreach (var param in syntax.ParameterList.Parameters)
            {
                var nodes = param.ChildNodes();
                var first = nodes.FirstOrDefault();
                if (first == null)
                {
                    break;
                }

                if (first.ToString() == "this")
                {
                    nodes = nodes.Skip(1);
                }

                argTypeNameList.Add(nodes.First().ToString());
            }

            if (argTypeNameList.Any())
            {
                name += argTypeNameList.Aggregate((a, b) => $"{a}, {b}");
            }

            name += ")";

            return name;
        }

        private int ConvertQualifierValue(SyntaxTokenList syntaxTokenList)
        {
            foreach (var token in syntaxTokenList)
            {
                switch (token.Text)
                {
                case "virtual":
                    return (int)MethodDeclarationRow.Qualifier.Virtual;

                case "override":
                    return (int)MethodDeclarationRow.Qualifier.Override;

                case "static":
                    return (int)MethodDeclarationRow.Qualifier.Static;
                }
            }

            return (int)MethodDeclarationRow.Qualifier.None;
        }

        /// <summary>
        /// ネームスペースの定義を構築して保存する
        /// </summary>
        /// <param name="syntax">構文</param>
        /// <returns>ネームスペースの定義</returns>
        private NamespaceDeclarationRow BuildNamespaceDeclarationRow(NamespaceDeclarationSyntax syntax)
        {
            string name = syntax.Name.ToString();
            var namespaceInfo = _dataBase.SelectInfo<NamespaceDeclarationRow>(i => i.Name == name);
            if (namespaceInfo != null)
            {
                // 既に保存されているので、そのまま値を返す
                return namespaceInfo;
            }

            var info
                = new NamespaceDeclarationRow
                    {
                        Id = _idGenerator.Generate(),
                        Name = name
                    };

            _dataBase.InsertInfo(info);

            return info;
        }

        /// <summary>
        /// クラス、構造体などの型定義を構築して保存する
        /// </summary>
        /// <param name="syntax">構文</param>
        /// <returns>型定義</returns>
        private TypeDeclarationRow BuildTypeDeclarationRow(ClassDeclarationSyntax syntax)
        {
            string name = syntax.Identifier.Text;
            var typeInfo = _dataBase.SelectInfo<TypeDeclarationRow>(i => i.Name == name);
            if (typeInfo != null)
            {
                // 既に保存されているので、そのまま値を返す
                return typeInfo;
            }

            var row
                = new TypeDeclarationRow
                    {
                        Id = _idGenerator.Generate(),
                        Name = name,
                        ParentId = GetSyntaxDeclarationId((dynamic)syntax.Parent)
                    };

            _dataBase.InsertInfo(row);

            return row;
        }

        private TypeDeclarationRow BuildTypeDeclarationRow(SyntaxNode syntax)
        {
            //todo 未実装
            return new TypeDeclarationRow { Id = 0 };
        }

        private int GetSyntaxDeclarationId(NamespaceDeclarationSyntax syntax)
        {
            return BuildNamespaceDeclarationRow(syntax).Id;
        }

        private int GetSyntaxDeclarationId(ClassDeclarationSyntax syntax)
        {
            return BuildTypeDeclarationRow(syntax).Id;
        }

        private int GetSyntaxDeclarationId(CompilationUnitSyntax syntax)
        {
            // namespaceなし
            return 0;
        }
    }
}
