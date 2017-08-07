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
        private readonly UniqueIdGenerator _idGenerator;

        public SyntaxAnalyzer(DataBaseBase dataBase, UniqueIdGenerator idGenerator)
        {
            _dataBase = dataBase;
            _idGenerator = idGenerator;
        }

        public void Analyze(CompilationUnitSyntax rootSyntax)
        {
            AnalyzeChildNodes(rootSyntax, 0);
        }

        private void AnalyzeChildNodes(SyntaxNode syntaxNode, int parentId)
        {
            foreach (var node in syntaxNode.ChildNodes())
            {
                AnalyzeSyntax((dynamic)node, parentId);
            }
        }

        private void AnalyzeSyntax(NamespaceDeclarationSyntax syntax, int parentId)
        {
            string name = syntax.Name.ToString();
            var row = _dataBase.SelectInfo<NamespaceDeclarationRow>(i => i.Name == name);
            if (row == null)
            {
                // 保存されていないので、新しく作成して保存する
                row
                    = new NamespaceDeclarationRow
                        {
                            Id = _idGenerator.Generate(),
                            Name = name
                        };

                _dataBase.Insert(row);
            }

            AnalyzeChildNodes(syntax, row.Id);
        }

        private void AnalyzeSyntax(BaseTypeDeclarationSyntax syntax, int parentId)
        {
            string name = syntax.Identifier.Text;
            var row = _dataBase.SelectInfo<TypeDeclarationRow>(i => i.Name == name && i.ParentId == parentId);
            if (row == null)
            {
                // 保存されていないので、新しく作成して保存する
                var category = TypeDeclarationRow.Category.Class;
                if (syntax is ClassDeclarationSyntax) { category = TypeDeclarationRow.Category.Class; }
                else if (syntax is StructDeclarationSyntax) { category = TypeDeclarationRow.Category.Struct; }
                else if (syntax is InterfaceDeclarationSyntax) { category = TypeDeclarationRow.Category.Interface; }
                else if (syntax is EnumDeclarationSyntax) { category = TypeDeclarationRow.Category.Enum; }

                row
                    = new TypeDeclarationRow
                        {
                            Id = _idGenerator.Generate(),
                            Name = name,
                            CategoryValue = category,
                            ParentId = parentId
                        };

                _dataBase.Insert(row);
            }

            AnalyzeChildNodes(syntax, row.Id);
        }

        private void AnalyzeSyntax(MethodDeclarationSyntax syntax, int parentId)
        {
            var row
                = new MethodDeclarationRow
                    {
                        Id = _idGenerator.Generate(),
                        Name = syntax.Identifier.Text,
                        ParentTypeId = parentId,
                        UnieuqName = ConvertUniqueName(syntax),
                        QualifierValue = ConvertQualifier(syntax.Modifiers)
                    };

            _dataBase.Insert(row);
        }

        private void AnalyzeSyntax(SyntaxNode syntax, int parentId)
        {
            // 未対応の定義は無視する
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

        private MethodDeclarationRow.Qualifier ConvertQualifier(SyntaxTokenList syntaxTokenList)
        {
            foreach (var token in syntaxTokenList)
            {
                switch (token.Text)
                {
                case "virtual":
                    return MethodDeclarationRow.Qualifier.Virtual;

                case "override":
                    return MethodDeclarationRow.Qualifier.Override;

                case "static":
                    return MethodDeclarationRow.Qualifier.Static;
                }
            }

            return MethodDeclarationRow.Qualifier.None;
        }
    }
}
