using System;

namespace KMBL.StepupAuthentication.CoreComponents
{
    public class InsufficientHeaderDetails : Exception
    {
        public string StatusCode { get; }

        public InsufficientHeaderDetails() : base()
        { }

        public InsufficientHeaderDetails(string ErrorCode, string message) : base(message)
        {
            this.StatusCode = ErrorCode;
        }
        public InsufficientHeaderDetails(string ErrorCode, string message, Exception InEx) : base(message, InEx)
        {
            this.StatusCode = ErrorCode;
        }
    }
}