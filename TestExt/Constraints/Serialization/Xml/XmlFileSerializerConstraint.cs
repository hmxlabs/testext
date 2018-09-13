using HmxLabs.Core.Serialization.Xml;

namespace HmxLabs.TestExt.Constraints.Serialization.Xml
{
    /// <summary>
    /// As per XmlSerializerConstraint except that the expected
    /// XML is loaded from a file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlFileSerializerConstraint<T> : XmlSerializerConstraint<T> where T : class
    {
        /// <summary>
        /// Loads the expected XML from a file instead of it being provided as a string but in all other regards
        /// this peforms identically to the <code>XmlSerializer</code>
        /// </summary>
        /// <param name="serializer_">T</param>
        /// <param name="refXmlFilename_">The filename of the file to read the expected XML output from</param>
        public XmlFileSerializerConstraint(IXmlSerializer<T> serializer_, string refXmlFilename_) : base(serializer_, XmlFileLoader.LoadXmlFromFile(refXmlFilename_))
        {
        }
    }
}
