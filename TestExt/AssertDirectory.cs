using System.IO;
using NUnit.Framework;

namespace HmxLabs.TestExt
{
    /// <summary>
    /// Asserting helper class to assert the existence or directory structures and comare their contents
    /// </summary>
    public static class AssertDirectory
    {
        /// <summary>
        /// Asserts that a directory exists.
        /// 
        /// Directory resolution is done using the <code>Sytem.IO.Directory</code> class
        /// </summary>
        /// <param name="path_">The path of the directory to assert</param>
        public static void Exists(string path_)
        {
            Assert.That(Directory.Exists(path_), $"The path {path_} does not exist");
        }

        /// <summary>
        /// Asserts that a directory does not exist
        /// 
        /// Directory resolution is done using the <code>Sytem.IO.Directory</code> class
        /// </summary>
        /// <param name="path_">The path of the directory to assert</param>
        public static void DoesNotExist(string path_)
        {
            Assert.That(Directory.Exists(path_), Is.False, $"The path {path_} exists");
        }

        /// <summary>
        /// Asserts that the directory exists and provides the option to provide a custom assertion failure message
        /// 
        /// Directory resolution is done using the <code>Sytem.IO.Directory</code> class
        /// </summary>
        /// <param name="path_">The path to assert</param>
        /// <param name="message_">The message to provide should the assertion fail</param>
        public static void Exists(string path_, string message_)
        {
            Assert.That(Directory.Exists(path_), Is.True, string.Format(message_, path_));
        }

        /// <summary>
        /// Asserts that directory does not exist with the provision of a custom assertion failure message
        /// 
        /// Directory resolution is done using the <code>Sytem.IO.Directory</code> class
        /// </summary>
        /// <param name="path_">The path to assert</param>
        /// <param name="message_">The custom assertion failure message</param>
        public static void DoesNotExist(string path_, string message_)
        {
            Assert.That(Directory.Exists(path_), Is.False, string.Format(message_, path_));
        }

        /// <summary>
        /// Asserts that the two paths provided are identical. This will check the the two directory structures
        /// contain the same directories (recursing through each directory as specified). The files are checked to ensure that they
        /// are the same size but the timestamps are ignored.
        /// </summary>
        /// <param name="source_">The source directory to compare</param>
        /// <param name="target_">The target directory to compare</param>
        /// <param name="recursive_"><code>true</code> if the directory structure should be recursed</param>
        public static void AreEqual(string source_, string target_, bool recursive_)
        {
            Exists(source_);
            Exists(target_);
            CompareFilesInDirectory(source_, target_);
            if (!recursive_)
                return;

            CompareDirectories(source_, target_);
        }

        private static void CompareDirectories(string source_, string target_)
        {
            var sourceDirs = Directory.GetDirectories(source_);
            var targetDirs = Directory.GetDirectories(target_);

            var message =
                $"Directory count mismatch. Source {source_} contains {sourceDirs.Length} directories. Target {target_} contains {targetDirs.Length} directories.";
            Assert.That(targetDirs.Length, Is.EqualTo(sourceDirs.Length), message);

            foreach (var sourceDir in sourceDirs)
            {
                var subDirLocalName = Path.GetFileName(sourceDir);
                message = $"The directory {subDirLocalName} exists in {source_} but does not exist in {target_}";
                if (null == subDirLocalName)
                   Assert.Fail("Unable to obtain local directory name for {0}", sourceDir);
                var targetSubDir = Path.Combine(target_, subDirLocalName);
                Assert.That(Directory.Exists(targetSubDir), message);
                AreEqual(sourceDir, targetSubDir, true);
            }
        }

        private static void CompareFilesInDirectory(string source_, string target_)
        {
            var sourceFiles = Directory.GetFiles(source_);
            var targetFiles = Directory.GetFiles(target_);

            var message =
                $"File count mismatch. Source {source_} contains {sourceFiles.Length} files. Target {target_} contains {targetFiles.Length} files.";
            Assert.That(targetFiles.Length, Is.EqualTo(sourceFiles.Length), message);

            foreach (var sourceFile in sourceFiles)
            {
                CompareFile(source_, sourceFile, target_);
            }
        }

        private static void CompareFile(string sourceDir_, string sourceFile_, string targetDir_)
        {
            var localFilename = Path.GetFileName(sourceFile_);
            if (null == localFilename) Assert.Fail("Unable to determine local filename for {0}", sourceFile_);
            var targetFile = Path.Combine(targetDir_, localFilename);
            var message = $"The file {localFilename} was found in {sourceDir_} but does not exist in {targetDir_}";
            Assert.That(File.Exists(targetFile), message);

            var sourceFileInfo = new FileInfo(sourceFile_);
            var targetFileInfo = new FileInfo(targetFile);
            message =
                $"The file {localFilename} exists in both {sourceDir_} and {targetDir_} but is of different sizes {sourceFileInfo.Length} vs {targetFileInfo.Length}";
            Assert.That(targetFileInfo.Length, Is.EqualTo(sourceFileInfo.Length), message);
        }
    }
}