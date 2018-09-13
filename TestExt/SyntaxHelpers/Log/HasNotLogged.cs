using HmxLabs.Core.Log;
using HmxLabs.TestExt.Constraints.Log;

namespace HmxLabs.TestExt.SyntaxHelpers.Log
{
    /// <summary>
    /// Syntax helper class designed to help write test cases like
    /// Assert.That(logger, HasNotLogged.Above(LogLevel.Info));
    /// </summary>
    public static class HasNotLogged
    {
        /// <summary>
        /// Utility function that allows specification of the highest log level output that should have occured during the test
        /// </summary>
        /// <param name="level_"></param>
        /// <returns></returns>
        public static LogOutputConstraint Above(LogLevel level_)
        {
            var logOutputConstraint = new LogOutputConstraint {ThresholdLogLevel = level_};
            return logOutputConstraint;
        }
    }
}
