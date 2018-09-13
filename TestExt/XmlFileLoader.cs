using System;
using System.IO;
using System.Reflection;

namespace HmxLabs.TestExt
{
    /// <summary>
    /// Utility class that helps load XML files from disk for serialization comparisions.
    /// </summary>
    public class XmlFileLoader
    {
        /// <summary>
        /// Load the specified file from disk and return its contents as a string
        /// </summary>
        /// <param name="filename_">The filename of the file to load</param>
        /// <returns></returns>
        public static string LoadXmlFromFile(string filename_)
        {
            var reader = new StreamReader(GetFullPath(filename_));
            string xml;
            using (reader)
            {
                xml = reader.ReadToEnd();
                reader.Close();
            }

            xml = xml.Replace("\r", ""); // Generated XML has only \n for new lines
            return xml;
        }

        private static string GetFullPath(string filename_)
        {
            var fl = new XmlFileLoader();
            var asm = Assembly.GetAssembly(fl.GetType());
            var asmPath = asm.CodeBase.Replace("file:///", "");
            var path = asmPath.Remove(asmPath.LastIndexOf("/", StringComparison.Ordinal));
            return path + filename_;
        }
    }
}
