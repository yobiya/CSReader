using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CSReader.Analyze.Info
{
    class SyntaxInfoBuilder
    {
        private readonly SyntaxWalker _syntaxWalker;

        public SyntaxInfoBuilder(SyntaxWalker syntaxWalker)
        {
            _syntaxWalker = syntaxWalker;
        }

        public IEnumerable<TypeInfo> BuildTypeInfo()
        {
            return
                _syntaxWalker
                    .ClassDeclarationSyntaxList
                        .Select(syntax =>
                            {
                                return new TypeInfo
                                {
                                    Name = syntax.Identifier.Text
                                };
                            });
        }

        public IEnumerable<MethodInfo> BuildMethodInfo()
        {
            return
                _syntaxWalker
                    .MethodDeclarationSyntaxList
                        .Select(syntax =>
                            {
                                var parentDeclarationSyntax = syntax.Parent as ClassDeclarationSyntax;

                                return new MethodInfo
                                {
                                    Name = syntax.Identifier.Text
                                };
                            });
        }
    }
}
