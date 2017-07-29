using System.Collections.Generic;
using System.Linq;

namespace CSReader.Analyze.Info
{
    class MethodInfoBuilder
    {
        private readonly SyntaxWalker _syntaxWalker;

        public MethodInfoBuilder(SyntaxWalker syntaxWalker)
        {
            _syntaxWalker = syntaxWalker;
        }

        public IEnumerable<MethodInfo> Build()
        {
            return
                _syntaxWalker
                    .MethodDeclarationSyntaxList
                        .Select(syntax =>
                            {
                                var info = new MethodInfo();
                                info.Name = syntax.Identifier.ValueText;

                                return info;
                            });
        }
    }
}
