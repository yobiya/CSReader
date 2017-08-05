using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Analyze.Row
{
    class SyntaxInfoBuilder
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
                BuildNamespaceInfo(syntax);
            }
        }

        public void BuildTypeInfos()
        {
            foreach (var syntax in _syntaxWalker.ClassDeclarationSyntaxList)
            {
                BuildTypeInfo(syntax);
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
                            ParentTypeId = BuildTypeInfo(syntax.Parent).Id,
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
        /// ネームスペースの情報を構築して保存する
        /// </summary>
        /// <param name="syntax">構文</param>
        /// <returns>ネームスペースの情報</returns>
        private NamespaceDeclarationRow BuildNamespaceInfo(NamespaceDeclarationSyntax syntax)
        {
            string name = ((IdentifierNameSyntax)syntax.Name).Identifier.ValueText;
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
        /// クラス、構造体などの型情報を構築して保存する
        /// </summary>
        /// <param name="syntax">構文</param>
        /// <returns>型情報</returns>
        private TypeDeclarationRow BuildTypeInfo(SyntaxNode syntax)
        {
            var classSyntax = syntax as ClassDeclarationSyntax;

            string name = classSyntax.Identifier.Text;
            var typeInfo = _dataBase.SelectInfo<TypeDeclarationRow>(i => i.Name == name);
            if (typeInfo != null)
            {
                // 既に保存されているので、そのまま値を返す
                return typeInfo;
            }

            var info
                = new TypeDeclarationRow
                    {
                        Id = _idGenerator.Generate(),
                        Name = name,
                        NamespaceId = BuildNamespaceInfo(((NamespaceDeclarationSyntax)syntax.Parent)).Id
                    };

            _dataBase.InsertInfo(info);

            return info;
        }
    }
}
