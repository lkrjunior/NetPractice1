using System;

namespace ChallengeNet.Core.Exceptions
{
    [Serializable]
    public class RepositoryException : Exception
    {
        private const string _messageBase = "Error on Repository: ";

        public RepositoryException(Exception exception) : base($"{_messageBase}{exception.Message}", exception)
        {

        }
    }
}