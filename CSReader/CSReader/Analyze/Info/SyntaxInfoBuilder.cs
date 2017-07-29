using CSReader.DB;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSReader.Analyze.Info
{
    class SyntaxInfoBuilder
    {
        private readonly SyntaxWalker _syntaxWalker;
        private readonly DataBase _dataBase;

        private int _uniqueId;

        public SyntaxInfoBuilder(SyntaxWalker syntaxWalker, DataBase dataBase)
        {
            _syntaxWalker = syntaxWalker;
            _dataBase = dataBase;
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
                    = new MethodInfo
                        {
                            UniqueId = GetUniqueId(),
                            Name = syntax.Identifier.Text,
                            ParentTypeId = BuildTypeInfo(syntax.Parent).UniqueId
                        };

                _dataBase.InsertInfo(info);
            }
        }

        private int GetUniqueId()
        {
            _uniqueId++;
            return _uniqueId;
        }

        /// <summary>
        /// クラス、構造体などの型情報を構築して保存する
        /// </summary>
        /// <param name="syntax">構文</param>
        /// <returns>型情報</returns>
        private TypeInfo BuildTypeInfo(SyntaxNode syntax)
        {
            var classSyntax = syntax as ClassDeclarationSyntax;

            string name = classSyntax.Identifier.Text;
            var typeInfo = _dataBase.SelectTypeInfo(name);
            if (typeInfo != null)
            {
                // 既に保存されているので、そのまま値を返す
                return typeInfo;
            }

            var info
                = new TypeInfo
                    {
                        UniqueId = GetUniqueId(),
                        Name = name
                    };

            _dataBase.InsertInfo(info);

            return info;
        }
    }
}
