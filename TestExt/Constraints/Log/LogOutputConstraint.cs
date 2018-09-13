using System;
using System.Linq;
using System.Text;
using HmxLabs.Core.Log;
using NUnit.Framework.Constraints;

namespace HmxLabs.TestExt.Constraints.Log
{
    /// <summary>
    /// An NUnit contraint that enables test constructs such as:
    /// Assert.That(logger, HasNotLogged.Above(LogLevel.Info));
    /// to be written. 
    /// 
    /// The intention here being that for larger system tests that system under test
    /// may not actually output any observable erroneous behaviour to the client 
    /// because this is handled internally and the details just logged. It
    /// might therefore be desirable to check the log outputs to ensure
    /// that nothing untoward has happened.
    /// </summary>
    public class LogOutputConstraint : Constraint
    {
        /// <summary>
        /// The maximum log level to permit output for
        /// </summary>
        public LogLevel ThresholdLogLevel { get; set; }

        /// <summary>
        /// Implementation of the Contraint.Matches method. See NUnit documentation for further details
        /// </summary>
        /// <param name="actual_"></param>
        /// <returns></returns>
        public override bool Matches(object actual_)
        {
            actual = actual_;
            var logOutput = actual_ as ILogOutput;
            if (null != logOutput)
                return Matches(logOutput);

            _errorMessage.Append("Log output check failed. The provided object was of type [");
            _errorMessage.Append(actual_.GetType());
            _errorMessage.AppendLine("]. Expected an implementation of ILogOutput");
            return false;
        }

        /// <summary>
        /// Underlying method actually called through to by the Matches(Object) method
        /// 
        /// Simply tries to check if anything has been logged at a level above the minimum threshold specified
        /// </summary>
        /// <param name="logOutput_"></param>
        /// <returns></returns>
        public bool Matches(ILogOutput logOutput_)
        {
            var logLevelNames = Enum.GetNames(typeof (LogLevel));
            foreach (var logLevelName in logLevelNames)
            {
                var logLevel = LogLevelExtensions.Parse(logLevelName);

                if (ThresholdLogLevel >= logLevel)
                    continue;

                if (!logOutput_.LogMessages[logLevel].Any()) 
                    continue;

                _errorMessage.Append("Log output contains messages logged at level [");
                _errorMessage.Append(logLevel.ToLogString());
                _errorMessage.Append("] when no messages above level [");
                _errorMessage.Append(ThresholdLogLevel.ToLogString());
                _errorMessage.Append("] were expected");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Writes out an error message if the test has failed. See NUnit documentation for more details
        /// </summary>
        /// <param name="writer_"></param>
        public override void WriteMessageTo(MessageWriter writer_)
        {
            writer_.WriteLine(_errorMessage.ToString());
        }

        /// <summary>
        /// Writes out an error message if the test has failed. See NUnit documentation for more details.
        /// 
        /// This should never be called in this implementation
        /// </summary>
        /// <param name="writer_"></param>
        public override void WriteDescriptionTo(MessageWriter writer_)
        {
            // No op. Should never be called
        }

        /// <summary>
        /// Writes out an error message if the test has failed. See NUnit documentation for more details.
        /// 
        /// This should never be called in this implementation
        /// </summary>
        /// <param name="writer_"></param>
        public override void WriteActualValueTo(MessageWriter writer_)
        {
            // No op. Should never be called
        }

        private readonly StringBuilder _errorMessage = new StringBuilder();
    }
}
