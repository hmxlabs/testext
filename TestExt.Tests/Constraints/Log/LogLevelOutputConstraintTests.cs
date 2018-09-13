using HmxLabs.Core.Log;
using HmxLabs.TestExt.SyntaxHelpers.Log;
using NUnit.Framework;

namespace HmxLabs.TestExt.Tests.Constraints.Log
{
    [TestFixture]
    public class LogLevelOutputConstraintTests
    {
        [Test]
        public void TestPassesWhenConstraintShouldMatch()
        {
            var logger = new DiscreteMemoryLogger("test");
            logger.Info("hello");
            logger.Debug("hello");

            Assert.That(logger.LogMessages[LogLevel.Debug], Has.Count.GreaterThan(0));
            Assert.That(logger.LogMessages[LogLevel.Information], Has.Count.GreaterThan(0));
            
            Assert.That(logger, HasNotLogged.Above(LogLevel.Information));
        }

        [Test]
        public void TestFailsWhenConstraintIsBreached()
        {
            var logger = new DiscreteMemoryLogger("test");
            logger.Info("hello");
            logger.Debug("hello");

            Assert.That(logger.LogMessages[LogLevel.Debug], Has.Count.GreaterThan(0));
            Assert.That(logger.LogMessages[LogLevel.Information], Has.Count.GreaterThan(0));

            Assert.Throws<AssertionException>(() => Assert.That(logger, HasNotLogged.Above(LogLevel.Debug)));
        }
    }
}