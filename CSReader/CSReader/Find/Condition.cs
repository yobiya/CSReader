using CSReader.Analyze.Row;
using CSReader.DB;
using System.Linq;

namespace CSReader.Find
{
    public interface ICondition
    {
        bool Match(MethodDeclarationRow row);
    }

    public class VirtualCondition : ICondition
    {
        public static ICondition Create(string arg, DataBaseBase dataBase) => arg == "virtual" ? new VirtualCondition() : null;

        public bool Match(MethodDeclarationRow row)
        {
            return row.QualifierValue == MethodDeclarationRow.Qualifier.Virtual;
        }
    }

    public class OverrideCondition : ICondition
    {
        public static ICondition Create(string arg, DataBaseBase dataBase) => arg == "override" ? new OverrideCondition() : null;

        public bool Match(MethodDeclarationRow row)
        {
            return row.QualifierValue == MethodDeclarationRow.Qualifier.Override;
        }
    }

    public class StaticCondition : ICondition
    {
        public static ICondition Create(string arg, DataBaseBase dataBase) => arg == "static" ? new StaticCondition() : null;

        public bool Match(MethodDeclarationRow row)
        {
            return row.QualifierValue == MethodDeclarationRow.Qualifier.Static;
        }
    }

    public class NameCondition : ICondition
    {
        private readonly string _name;

        public static ICondition Create(string arg, DataBaseBase dataBase)
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

    public class CallCountCondition : ICondition
    {
        private readonly DataBaseBase _dataBase;
        private readonly int _count;

        public static ICondition Create(string arg, DataBaseBase dataBase)
        {
             return arg.StartsWith("call_count==") ? new CallCountCondition(arg, dataBase) : null;
        }

        public CallCountCondition(string arg, DataBaseBase dataBase)
        {
            _dataBase = dataBase;

            var countText = arg.Substring("call_count==".Length);
            _count = System.Int32.Parse(countText);
        }

        public bool Match(MethodDeclarationRow row)
        {
            int declarationId = row.Id;
            int callCount = _dataBase.GetRows<MethodInvocationRow>().Count(r => r.MethodDeclarationId == declarationId);

            return callCount == _count;
        }
    }
}
