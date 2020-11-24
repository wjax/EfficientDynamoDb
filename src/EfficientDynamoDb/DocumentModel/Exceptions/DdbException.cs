using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.DocumentModel.Exceptions
{
    /// <summary>
    /// A base class for all DynamoDb exceptions.
    /// </summary>
    public class DdbException : Exception
    {
        public DdbException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DdbException(string message) : base(message)
        {
        }

        public DdbException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}