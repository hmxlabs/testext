using System;
using HmxLabs.Core.Serialization.Xml;
using HmxLabs.TestExt.Constraints.Serialization.Xml;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace HmxLabs.TestExt.SyntaxHelpers.Serialization
{
    /// <summary>
    /// Syntax helper class to allow fluent test expresions when
    /// writing XML serialization test code.
    /// </summary>
    public static class Deserializes
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer_"></param>
        /// <returns></returns>
        public static DeserializeConstraintCreator<T> With<T>(IXmlDeserializer<T> serializer_) where T : class
        {
            return new DeserializeConstraintCreator<T>(serializer_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class DeserializeConstraintCreator<T> where T  : class
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="serializer_"></param>
            public DeserializeConstraintCreator(IXmlDeserializer<T> serializer_)
            {
                if (null == serializer_)
                    throw new ArgumentNullException("serializer_");

                Serializer = serializer_;
                Factory = new XmlDeserializerConstraintFactory();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public DeserializeConstraintCreator<T> FromFile()
            {
                Factory.FileLoaderMode = true;
                return this;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="expected_"></param>
            /// <returns></returns>
            public XmlDeserializerConstraint<T> To(T expected_)
            {
                var deserializedConstraint = new EqualConstraint(expected_);
                var deserializerConstraint = Factory.Create(Serializer, deserializedConstraint);
                return deserializerConstraint;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public XmlDeserializerConstraint<T> AndIsNotNull()
            {
                var notNullConstraint = Is.Not.Null;
                var deserializerConstraint = Factory.Create(Serializer, notNullConstraint);
                return deserializerConstraint;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="tests_"></param>
            /// <returns></returns>
            public XmlDeserializerConstraint<T> AndPasses(DeserializationTests<T> tests_)
            {
                return Factory.Create(Serializer, tests_);
            }

            /// <summary>
            /// 
            /// </summary>
            public IXmlDeserializer<T> Serializer { get; }

            /// <summary>
            /// 
            /// </summary>
            public XmlDeserializerConstraintFactory Factory { get; }
        }
    }
}
