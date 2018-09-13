using System;
using HmxLabs.Core.Serialization.Xml;
using NUnit.Framework.Constraints;

namespace HmxLabs.TestExt.Constraints.Serialization.Xml
{
    /// <summary>
    /// As per the XmlDeserializerConstraint except that the XML to deserialize is read from a file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlFileDeserializerConstraint<T> : XmlDeserializerConstraint<T> where T : class 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer_"></param>
        /// <param name="constraint_"></param>
        public XmlFileDeserializerConstraint(IXmlDeserializer<T> serializer_, Constraint constraint_) : base(serializer_, constraint_)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer_"></param>
        /// <param name="tests_"></param>
        public XmlFileDeserializerConstraint(IXmlDeserializer<T> serializer_, DeserializationTests<T> tests_) : base(serializer_, tests_)
        {            
        }

        /// <summary>
        /// Overrides the match method to load the XML from file instead.
        /// </summary>
        /// <param name="xmlFilename_"></param>
        /// <returns></returns>
        public override bool Matches(string xmlFilename_)
        {
            if (null == xmlFilename_)
                throw new ArgumentNullException(nameof(xmlFilename_));

            var xmlString = XmlFileLoader.LoadXmlFromFile(xmlFilename_);
            return base.Matches(xmlString);
        }
    }
}
