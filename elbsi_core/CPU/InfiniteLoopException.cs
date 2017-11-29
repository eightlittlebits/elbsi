using System;
using System.Runtime.Serialization;

namespace elbsi_core.CPU
{
    [Serializable]
    public class InfiniteLoopException : Exception
    {
        public InfiniteLoopException()
        {
        }

        public InfiniteLoopException(string message) : base(message)
        {
        }

        public InfiniteLoopException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InfiniteLoopException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}