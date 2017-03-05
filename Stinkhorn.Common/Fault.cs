using System;

namespace Stinkhorn.Common
{
    class Fault : IFault
    {
        public FaultCode Code { get; }
        public string Description { get; }
        public string Details { get; }

        internal Fault(FaultCode code, string description, string details)
        {
            Code = code;
            Description = description;
            Details = details;
        }

        public static implicit operator Fault(Exception error) => new Fault(
            FaultCode.GeneralError, error.Message, error.StackTrace);

        public static implicit operator Exception(Fault fault) =>
            new FaultException { Fault = fault };
    }
}