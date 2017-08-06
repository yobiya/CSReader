using CSReader.Analyze.Row;

namespace CSReader.Find
{
    public interface ICondition
    {
        bool Match(MethodDeclarationRow row);
    }

    public class VirtualCondition : ICondition
    {
        public static ICondition Create(string arg) => arg == "virtual" ? new VirtualCondition() : null;

        public bool Match(MethodDeclarationRow row)
        {
            return row.QualifierValue == MethodDeclarationRow.Qualifier.Virtual;
        }
    }

    public class OverrideCondition : ICondition
    {
        public static ICondition Create(string arg) => arg == "override" ? new OverrideCondition() : null;

        public bool Match(MethodDeclarationRow row)
        {
            return row.QualifierValue == MethodDeclarationRow.Qualifier.Override;
        }
    }

    public class StaticCondition : ICondition
    {
        public static ICondition Create(string arg) => arg == "static" ? new StaticCondition() : null;

        public bool Match(MethodDeclarationRow row)
        {
            return row.QualifierValue == MethodDeclarationRow.Qualifier.Static;
        }
    }

    public class NameCondition : ICondition
    {
        private readonly string _name;

        public static ICondition Create(string arg)
        {
             return arg.StartsWith("name==") ? new NameCondition(arg) : null;
        }


        private NameCondition(string arg)
        {
            _name = arg.Substring("name==".Length);
        }

        public bool Match(MethodDeclarationRow row)
        {
            return row.Name == _name;
        }
    }
}
