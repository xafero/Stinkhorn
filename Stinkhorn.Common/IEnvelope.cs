using System;
using System.Collections.Generic;
using Stinkhorn.API;

namespace Stinkhorn.Common
{
    public interface IEnvelope
    {
        IParticipant Sender { get; }
        IParticipant Receiver { get; }
        IDictionary<string, int> Headers { get; }
        IMessage Body { get; }
        IFault Error { get; }
    }

    public interface IParticipant
    {
        ParticipantKind Kind { get; }
        Guid? Id { get; }
        string Topic { get; }
    }

    public enum ParticipantKind
    {
        Broadcast, Multicast, Unicast
    }

    public interface IFault
    {
        FaultCode Code { get; }
        string Description { get; }
        string Details { get; }
    }

    public enum FaultCode
    {
        VersionMismatch, ParamMismatch, ClientSide, ServerSide
    }
}