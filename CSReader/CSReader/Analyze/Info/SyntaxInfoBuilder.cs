using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using CSReader.DB;

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

        public void BuildTypeInfo()
        {
            foreach (var syntax in _syntaxWalker.ClassDeclarationSyntaxList)
            {
                var info
                    = new TypeInfo
                        {
                            UniqueId = GetUniqueId(),
                            Name = syntax.Identifier.Text
                        };

                _dataBase.InsertInfo(info);
            }
        }

        public void BuildMethodInfo()
        {
            foreach (var syntax in _syntaxWalker.MethodDeclarationSyntaxList)
            {
                var info
                    = new MethodInfo
                        {
                            UniqueId = GetUniqueId(),
                            Name = syntax.Identifier.Text
                        };

                _dataBase.InsertInfo(info);
            }
        }

        private int GetUniqueId()
        {
            _uniqueId++;
            return _uniqueId;
        }
    }
}
