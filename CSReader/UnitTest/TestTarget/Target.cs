namespace UnitTest.TestTarget
{
    public class Target
    {
        public static string GetSolutionPath(string name)
        {
            return "../../../TestTarget/" + name + "/" + name + ".sln";
        }
    }
}
