using CSReader.Analyze.Row;
using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Analyze
{
    public class SyntaxAnalyzer
    {
        private readonly DataBaseBase _dataBase;
        private readonly SyntaxWalker _syntaxWalker;
        private readonly UniqueIdGenerator _idGenerator;

        public SyntaxAnalyzer(DataBaseBase dataBase, SyntaxWalker syntaxWalker, UniqueIdGenerator idGenerator)
        {
            _dataBase = dataBase;
            _syntaxWalker = syntaxWalker;
            _idGenerator = idGenerator;
        }

        public void BuildNamespaceDeclarations()
        {
            foreach (var syntax in _syntaxWalker.NamespaceDeclarationSyntaxList)
            {
                BuildNamespaceDeclarationRow(syntax);
            }
        }

        public void BuildTypeDeclarations()
        {
            foreach (var syntax in _syntaxWalker.ClassDeclarationSyntaxList)
            {
                BuildTypeDeclarationRow(syntax);
            }

            foreach (var syntax in _syntaxWalker.StructDeclarationSyntaxList)
            {
                BuildTypeDeclarationRow(syntax);
            }
        }

        public void BuildMethodDeclarations()
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

                _dataBase.Insert(info);
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

            _dataBase.Insert(info);

            return info;
        }

        /// <summary>
        /// クラス、構造体などの型定義を構築して保存する
        /// </summary>
        /// <param name="syntax">構文</param>
        /// <returns>型定義</returns>
        private TypeDeclarationRow BuildTypeDeclarationRow(BaseTypeDeclarationSyntax syntax)
        {
            string name = syntax.Identifier.Text;
            var typeInfo = _dataBase.SelectInfo<TypeDeclarationRow>(i => i.Name == name);
            if (typeInfo != null)
            {
                // 既に保存されているので、そのまま値を返す
                return typeInfo;
            }

            var category = TypeDeclarationRow.Category.Class;
            if (syntax is ClassDeclarationSyntax) { category = TypeDeclarationRow.Category.Class; }
            else if (syntax is StructDeclarationSyntax) { category = TypeDeclarationRow.Category.Struct; }
            else if (syntax is InterfaceDeclarationSyntax) { category = TypeDeclarationRow.Category.Interface; }
            else if (syntax is EnumDeclarationSyntax) { category = TypeDeclarationRow.Category.Enum; }

            var row
                = new TypeDeclarationRow
                    {
                        Id = _idGenerator.Generate(),
                        Name = name,
                        CategoryValue = category,
                        ParentId = GetSyntaxDeclarationId((dynamic)syntax.Parent)
                    };

            _dataBase.Insert(row);

            return row;
        }

        private int GetSyntaxDeclarationId(NamespaceDeclarationSyntax syntax)
        {
            return BuildNamespaceDeclarationRow(syntax).Id;
        }

        private int GetSyntaxDeclarationId(BaseTypeDeclarationSyntax syntax)
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
