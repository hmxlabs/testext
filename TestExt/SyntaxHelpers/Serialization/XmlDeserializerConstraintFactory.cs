using HmxLabs.Core.Serialization.Xml;
using HmxLabs.TestExt.Constraints.Serialization.Xml;
using NUnit.Framework.Constraints;

namespace HmxLabs.TestExt.SyntaxHelpers.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlDeserializerConstraintFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public XmlDeserializerConstraintFactory()
        {
            FileLoaderMode = false;
        }

        /// <summary>
        /// If set to <code>true</code> then the string will be treated as a filename and
        /// and the factory will provide a deserializer that loads the XML from a file.
        /// 
        /// Otherwise a deserializer that treats the string value as the XML will be created
        /// </summary>
        public bool FileLoaderMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer_"></param>
        /// <param name="constraint_"></param>
        /// <returns></returns>
        public XmlDeserializerConstraint<T> Create<T>(IXmlDeserializer<T> serializer_, Constraint constraint_) where T : class
        {
            return FileLoaderMode ? new XmlFileDeserializerConstraint<T>(serializer_, constraint_) : new XmlDeserializerConstraint<T>(serializer_, constraint_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer_"></param>
        /// <param name="tests_"></param>
        /// <returns></returns>
        public XmlDeserializerConstraint<T> Create<T>(IXmlDeserializer<T> serializer_, DeserializationTests<T> tests_) where T : class
        {
            return FileLoaderMode ? new XmlFileDeserializerConstraint<T>(serializer_, tests_) : new XmlDeserializerConstraint<T>(serializer_, tests_);
        }
    }
}
