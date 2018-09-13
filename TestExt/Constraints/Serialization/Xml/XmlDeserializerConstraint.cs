using System;
using System.IO;
using System.Text;
using System.Xml;
using HmxLabs.Core.Serialization.Xml;
using NUnit.Framework.Constraints;

namespace HmxLabs.TestExt.Constraints.Serialization.Xml
{
    /// <summary>
    /// Delegate used to define the tests to be performed on the deserialized object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="actual_"></param>
    public delegate void DeserializationTests<in T>(T actual_);

    /// <summary>
    /// NUnit constraint used to test deserialization code 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlDeserializerConstraint<T> : Constraint where T : class 
    {
        /// <summary>
        /// Constructor taking the deserializer to use and the test constraint to apply
        /// </summary>
        /// <param name="serializer_"></param>
        /// <param name="constraint_"></param>
        public XmlDeserializerConstraint(IXmlDeserializer<T> serializer_, Constraint constraint_)
        {
            if (null == serializer_)
                throw new ArgumentNullException(nameof(serializer_));

            _serializer = serializer_;
            _constraint = constraint_;
        }

        /// <summary>
        /// Constructor taking the deserializer and a delegate to the tests to run on the deserialized object
        /// </summary>
        /// <param name="serializer_"></param>
        /// <param name="tests_"></param>
        public XmlDeserializerConstraint(IXmlDeserializer<T> serializer_, DeserializationTests<T> tests_)
        {
            if (null == serializer_)
                throw new ArgumentNullException(nameof(serializer_));

            _serializer = serializer_;
            _tests = tests_;
        }

        /// <summary>
        /// Tests if the deserialized object matches the provided
        /// contraint and runs the specified tests on the deserialized
        /// object to determine if this constraint should be passed.
        /// </summary>
        /// <param name="xmlStr_"></param>
        /// <returns></returns>
        public virtual bool Matches(string xmlStr_)
        {
            T deserializedActual;
            var xmlStringStream = new StringReader(xmlStr_);
            using (xmlStringStream)
            {
                var xmlReader = XmlReader.Create(xmlStringStream);
                using (xmlReader)
                {
                    deserializedActual = _serializer.Deserialize(xmlReader);    
                }
            }

            bool matches = true;
            if (null != _constraint)
                matches = _constraint.Matches(deserializedActual);

            matches = matches && RunTests(deserializedActual);

            return matches;
        }

        /// <summary>
        /// Implementation of the NUnit Constraint.Match method. This method
        /// wiil simply cast the provided object to the expected type and call
        /// the Match(T) method.
        /// 
        /// If the cast fails an appropriate error message will be raised.
        /// </summary>
        /// <param name="actual_"></param>
        /// <returns></returns>
        public override bool Matches(object actual_)
        {
            actual = actual_;
            var serialisedActual = actual_ as string;
            if (null != serialisedActual) 
                return Matches(serialisedActual);

            _errorMsg.Append("Deserialization failed. The provided object was of type [");
            _errorMsg.Append(actual_.GetType());
            _errorMsg.AppendLine("]. Expected a string");
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer_"></param>
        public override void WriteMessageTo(MessageWriter writer_)
        {
            writer_.WriteLine(_errorMsg);
            if (null == _constraint)
                return;

            _constraint.WriteMessageTo(writer_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer_"></param>
        public override void WriteDescriptionTo(MessageWriter writer_)
        {
            if (null == _constraint)
                return;

            _constraint.WriteDescriptionTo(writer_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer_"></param>
        public override void WriteActualValueTo(MessageWriter writer_)
        {
            if (null == _constraint)
                return;

            _constraint.WriteActualValueTo(writer_);
        }

        private bool RunTests(T actual_)
        {
            if (null == _tests)
                return true;

            try
            {
                _tests(actual_);
                return true;
            }
            catch (Exception exp)
            {
                _errorMsg.AppendLine("Post deserialisation tests failed: ");
                _errorMsg.AppendLine(exp.Message);
                return false;
            }
        }


        private readonly IXmlDeserializer<T> _serializer;
        private readonly Constraint _constraint;
        private readonly DeserializationTests<T> _tests;
        private readonly StringBuilder _errorMsg = new StringBuilder();
    }
}
