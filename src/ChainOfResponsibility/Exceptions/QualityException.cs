using System;

namespace ChainOfResponsibility.Exceptions
{
    public class QualityException : Exception
    {
        public QualityException(string errorMessage) : base(errorMessage)
        {
        }
    }
}