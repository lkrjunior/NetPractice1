using System;
using System.Runtime.Serialization;

namespace ChallengeNet.Core.Exceptions
{
    [Serializable]
    public class RepositoryException : Exception, ISerializable
    {
        private const string _messageBase = "Error on Repository: ";

        public RepositoryException(Exception exception) : base($"{_messageBase}{exception.Message}", exception)
        {

        }
    }
}