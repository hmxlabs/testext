using System.IO;
using NUnit.Framework;

namespace HmxLabs.TestExt
{
    /// <summary>
    /// Utility class that asserts the existence of files
    /// </summary>
    public static class AssertFile
    {
        /// <summary>
        /// Asserts that the specified file exists
        /// File resolution is performed by the <code>System.IO.File</code> class
        /// </summary>
        /// <param name="path_">The path of the file to assert</param>
        public static void Exists(string path_)
        {
            Assert.That(File.Exists(path_), $"The path {path_} does not exist");
        }

        /// <summary>
        /// Asserts that specified file does not exist
        /// 
        /// File resolution is performed by the <code>System.IO.File</code> class
        /// </summary>
        /// <param name="path_">The path of the file to assert</param>
        public static void DoesNotExist(string path_)
        {
            Assert.That(File.Exists(path_), Is.False, $"The path {path_} exists but should not");
        }

        /// <summary>
        /// Asserts that a specified file exists with a custom assertion failure message
        /// 
        /// File resolution is performed by the <code>System.IO.File</code> class
        /// </summary>
        /// <param name="path_">The path of the file to assert</param>
        /// <param name="message_">the custom error message on assertion failure</param>
        public static void Exists(string path_, string message_)
        {
            Assert.That(File.Exists(path_), string.Format(message_, path_));
        }

        /// <summary>
        /// Asserts that a specified file does not exist with a custom assertion failure message
        /// 
        /// File resolution is performed by the <code>System.IO.File</code> class
        /// </summary>
        /// <param name="path_">The path of the file to assert</param>
        /// <param name="message_">the custom error message on assertion failure</param>
        public static void DoesNotExist(string path_, string message_)
        {
            Assert.That(File.Exists(path_), Is.False, string.Format(message_, path_));
        }
    }
}