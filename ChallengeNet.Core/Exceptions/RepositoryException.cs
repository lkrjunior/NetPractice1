using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace ChallengeNet.Core.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class RepositoryException : Exception
    {
        private const string _messageBase = "Error on Repository: ";

        public RepositoryException(Exception exception) :
            base($"{_messageBase}{exception.Message}", exception)
        {

        }

        protected RepositoryException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }
    }
}