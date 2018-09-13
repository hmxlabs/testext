using System;
using System.IO;
using HmxLabs.Core.IO;
using NUnit.Framework;

namespace HmxLabs.TestExt.Tests
{
    [TestFixture]
    public class AssertDirectoryTests
    {
        public static readonly string TestDataDirectory = Path.Combine(".", "TestData");
        public static readonly string ReferenceDir = Path.Combine(TestDataDirectory, "Reference");
        public static readonly string MissingFileDir = Path.Combine(TestDataDirectory, "MissingFile");
        public static readonly string MissingDirectoryDir = Path.Combine(TestDataDirectory, "MissingDirectory");
        public static readonly string MismatchedFileDir = Path.Combine(TestDataDirectory, "MismatchedFile");
        public static readonly string EqualDir = Path.Combine(TestDataDirectory, "EqualDir");
        public static readonly string AdditionalFileDir = Path.Combine(TestDataDirectory, "AdditionalFile");

        [Test]
        public void TestAssertEqualsWhenFileMissing()
        {
            if (Directory.Exists(MissingFileDir))
                Directory.Delete(MissingFileDir, true);

            Assert.IsFalse(Directory.Exists(MissingFileDir));
            DirectoryExt.CopyContents(ReferenceDir, MissingFileDir);
            var fileToDelete = Path.Combine(MissingFileDir, "SubDir1", "nunit.core.interfaces.dll");
            File.Delete(fileToDelete);
            Assert.Throws<AssertionException>(() => AssertDirectory.AreEqual(ReferenceDir, MissingFileDir, true));
        }

        [Test]
        public void TestAssertEqualWhenDirectoryMissing()
        {
            if (Directory.Exists(MissingDirectoryDir))
                Directory.Delete(MissingDirectoryDir, true);

            Assert.IsFalse(Directory.Exists(MissingDirectoryDir));
            DirectoryExt.CopyContents(ReferenceDir, MissingDirectoryDir);
            var dirToDelete = Path.Combine(MissingDirectoryDir, "SubDir2");
            Directory.Delete(dirToDelete, true);
            Assert.Throws<AssertionException>(() => AssertDirectory.AreEqual(ReferenceDir, MissingDirectoryDir, true));
        }

        [Test]
        public void TestAssertEqualWhenMismatchedFile()
        {
            if (Directory.Exists(MismatchedFileDir))
                Directory.Delete(MismatchedFileDir, true);

            Assert.IsFalse(Directory.Exists(MismatchedFileDir));
            DirectoryExt.CopyContents(ReferenceDir, MismatchedFileDir);
            var filenameToModify = Path.Combine(MismatchedFileDir, "AssemblyInfo.txt");
            var contents = File.ReadAllText(filenameToModify);
            contents = contents + Environment.NewLine + "HELLO" + Environment.NewLine + contents;
            File.Delete(filenameToModify);
            File.WriteAllText(filenameToModify, contents);

            Assert.Throws<AssertionException>(() => AssertDirectory.AreEqual(ReferenceDir, MismatchedFileDir, true));
        }

        [Test]
        public void TestAssertEqualWhenAdditionalFile()
        {
            if (Directory.Exists(AdditionalFileDir))
                Directory.Delete(AdditionalFileDir, true);

            Assert.IsFalse(Directory.Exists(AdditionalFileDir));
            DirectoryExt.CopyContents(ReferenceDir, AdditionalFileDir);
            var fileToCopy = Path.Combine(AdditionalFileDir, "AssemblyInfo.txt");
            var newFilename = Path.Combine(AdditionalFileDir, "NewFile.txt");
            File.Copy(fileToCopy, newFilename);

            Assert.Throws<AssertionException>(() => AssertDirectory.AreEqual(ReferenceDir, AdditionalFileDir, true));
        }

        [Test]
        public void TestAssertEqualWhenEqual()
        {
            if (Directory.Exists(EqualDir))
                Directory.Delete(EqualDir, true);

            Assert.IsFalse(Directory.Exists(EqualDir));
            DirectoryExt.CopyContents(ReferenceDir, EqualDir);
            AssertDirectory.AreEqual(ReferenceDir, EqualDir, true);
        }
    }
}
