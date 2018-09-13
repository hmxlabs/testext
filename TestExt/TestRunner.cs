using System.Reflection;
using NUnit.ConsoleRunner;

namespace HmxLabs.TestExt
{
    /// <summary>
    /// Helper class. If the test assembly is complied as a .exe this class can be used
    /// to have the binary run all of the tests contained within the assembly as its
    /// runtime execution.
    /// 
    /// Uses the NUnit console runner to acheive this.
    /// </summary>
    public class TestRunner
    {
        /// <summary>
        /// Run all unit tests in the calling assembly
        /// </summary>
        /// <returns></returns>
        public static int RunTestsInConsole()
        {
            var testAssembly = Assembly.GetCallingAssembly();
            return RunTestsInConsole(testAssembly);
        }

        /// <summary>
        /// Run all unit tests in the specified assembly
        /// </summary>
        /// <param name="path_">The path of the assembly that contains the unit tests to run</param>
        /// <returns></returns>
        public static int RunTestsInConsole(string path_)
        {
            return Runner.Main(new[] {path_});
        }

        /// <summary>
        /// Run all unit tests in the specified assembly
        /// </summary>
        /// <param name="assembly_"></param>
        /// <returns></returns>
        public static int RunTestsInConsole(Assembly assembly_)
        {
            return Runner.Main(new[] {assembly_.Location});
        }
    }
}
