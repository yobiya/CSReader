namespace UnitTest.TestTarget
{
    public class Target
    {
        public static string GetSolutionDirectoryPath(string name)
        {
            return "../../../TestTarget/" + name;
        }

        public static string GetSolutionPath(string name)
        {
            return GetSolutionDirectoryPath(name) + "/" + name + ".sln";
        }
    }
}
