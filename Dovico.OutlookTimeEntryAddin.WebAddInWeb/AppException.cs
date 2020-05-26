using System;
using System.Runtime.Serialization;

namespace Dovico.OutlookTimeEntryAddin.WebAddInWeb
{
    /// <summary>
    /// Custom exception class for Application layer exceptions
    /// </summary>
    [Serializable]
    internal class AppException : Exception
    {
        public AppException()
        {
        }

        public AppException(string message) : base(message)
        {
        }

        public AppException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}