using System;

namespace Stinkhorn.Common
{
    public class FaultException : Exception
    {
        public IFault Fault { get; set; }

        public override string Message
            => $"[{Fault.Code}] {Fault.Description}";

        public override string StackTrace
        => Fault.Details;
    }
}