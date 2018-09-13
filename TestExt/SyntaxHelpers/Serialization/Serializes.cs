using System;
using HmxLabs.Core.Serialization.Xml;
using HmxLabs.TestExt.Constraints.Serialization.Xml;
using NUnit.Framework.Constraints;

namespace HmxLabs.TestExt.SyntaxHelpers.Serialization
{
    /// <summary>
    /// Syntax helper class to allow fluent test expresions when
    /// writing XML serialization test code.
    /// </summary>
    public static class Serializes
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer_"></param>
        /// <returns></returns>
        public static SerializeConstraintCreator<T> With<T>(IXmlSerializer<T> serializer_) where T : class
        {
            if (null == serializer_)
                throw new ArgumentNullException("serializer_");

            return new SerializeConstraintCreator<T>(serializer_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class SerializeConstraintCreator<T> where T : class
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="serializer_"></param>
            public SerializeConstraintCreator(IXmlSerializer<T> serializer_)
            {
                if (null == serializer_)
                    throw new ArgumentNullException("serializer_");

                Serializer = serializer_;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="refXmlFile_"></param>
            /// <returns></returns>
            public Constraint ToFileContents(string refXmlFile_)
            {
                return new XmlFileSerializerConstraint<T>(Serializer, refXmlFile_);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="referenceXml_"></param>
            /// <returns></returns>
            public Constraint To(string referenceXml_)
            {
                return new XmlSerializerConstraint<T>(Serializer, referenceXml_);
            }

            /// <summary>
            /// 
            /// </summary>
            public IXmlSerializer<T> Serializer { get; private set; }
        }
    }
}
