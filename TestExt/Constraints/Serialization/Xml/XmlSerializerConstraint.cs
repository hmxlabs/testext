using System;
using System.Text;
using System.Xml;
using HmxLabs.Core.Serialization.Xml;
using NUnit.Framework.Constraints;

namespace HmxLabs.TestExt.Constraints.Serialization.Xml
{
    /// <summary>
    /// Used to enable testing to XML Serialization code with NUnit.
    /// 
    /// Written as an NUnit Constraint, this enable code like 
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlSerializerConstraint<T> : Constraint where T : class
    {
        /// <summary>
        /// Constructor. Takes the <code>IXmlSerializer</code> to operate on and the ouput
        /// XML that should be generated as a string.
        /// </summary>
        /// <param name="serializer_">The serializer to use</param>
        /// <param name="referenceXml_">The expected ouput XML as a string</param>
        public XmlSerializerConstraint(IXmlSerializer<T> serializer_, string referenceXml_)
        {
            if (null == serializer_)
                throw new ArgumentNullException(nameof(serializer_));

            if (null == referenceXml_)
                throw new ArgumentNullException(nameof(referenceXml_));

            _serializer = serializer_;
            _errorMessage = string.Empty;
            _referenceXml = referenceXml_;
        }

        /// <summary>
        /// The actual implementation of the Matches method as required by the 
        /// <code>NUnit.Framework.Contraints.Constrant</code> class.
        /// 
        /// The actual Matches(Object) method simply calls through to this type
        /// specific method.
        /// </summary>
        /// <param name="actual_">The actual output of the XMlSerializer</param>
        /// <returns></returns>
        public bool Matches(T actual_)
        {
            var outputXml = new StringBuilder();
            var xmlWriter = XmlWriter.Create(outputXml, XmlSerializerSettings.GetDefaultWriterSettings());
            using (xmlWriter)
            {
                _serializer.Serialize(actual_, xmlWriter);
                xmlWriter.Close();
            }

            _actualXml = outputXml.ToString();
            if (_referenceXml.Equals(_actualXml))
                return true;

            var error = new StringBuilder();
            error.AppendLine("XML Serialization was incorrect.");
            error.AppendLine("Expected: ");
            error.AppendLine(_referenceXml);
            error.AppendLine("Actual: ");
            error.AppendLine(_actualXml);
            _errorMessage = error.ToString();
            return false;
        }

        /// <summary>
        /// Implementation of the <code>NUnit.Framework.Contraints.Constrant</code> class method.
        /// 
        /// This simply tries to cast the provided object to the expected type, if this cast fails
        /// the error message is set accordingly and the match is failed.
        /// 
        /// If the cast succeeds then the Match(T) method is called to check if the objects actually
        /// match.
        /// </summary>
        /// <param name="actual_"></param>
        /// <returns></returns>
        public override bool Matches(object actual_)
        {
            actual = actual_;
            var toTest = actual_ as T;
            if (null == toTest)
            {
                _errorMessage = string.Format("Unexpected type provided for serializer. Provided {0}, expected {1}", typeof(T), actual_.GetType());
                return false;
            }

            return Matches(toTest);
        }

        /// <summary>
        /// Override of the NUnit method to write a descriptive error message in the case of a match failure
        /// </summary>
        /// <param name="writer_"></param>
        public override void WriteMessageTo(MessageWriter writer_)
        {
            writer_.WriteMessageLine(_errorMessage);
        }

        /// <summary>
        /// Oerride of the NUnit method to write a descriptive error message in the case of a match failure
        /// </summary>
        /// <param name="writer_"></param>
        public override void WriteDescriptionTo(MessageWriter writer_)
        {
            writer_.WriteMessageLine(_errorMessage);
        }

        private string _errorMessage;
        private string _actualXml;
        private readonly string _referenceXml;
        private readonly IXmlSerializer<T> _serializer;
    }
}
        