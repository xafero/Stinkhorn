using Stinkhorn.API;
using System;
using System.Collections.Generic;

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
    }

    interface IBrokerParticipant : IParticipant
    {
        string Exchange { get; }
        string Route { get; }
        string Queue { get; set; }
        string Tag { get; set; }
    }

    class Participant : IBrokerParticipant
    {
        public string Exchange { get; }
        public string Route { get; }
        public string Queue { get; set; }
        public string Tag { get; set; }

        Guid id;
        public Guid? Id => id == default(Guid) ? default(Guid?) : id;
        public string Topic { get; }
        public ParticipantKind Kind { get; }

        internal Participant(string target, Type type, Lazy<Guid> myId)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                Exchange = RabbitExtensions.ToGeneral(type);
                Route = string.Empty;
                Kind = ParticipantKind.Broadcast;
            }
            else if (Guid.TryParse(target, out id))
            {
                if (Id == default(Guid))
                    Exchange = myId.Value.ToIdString();
                else
                    Exchange = Id.Value.ToIdString();
                Route = RabbitExtensions.ToGeneral(type);
                Kind = ParticipantKind.Unicast;
            }
            else
            {
                Exchange = target;
                Route = type.FullName;
                Kind = ParticipantKind.Multicast;
                Topic = target;
            }
        }
    }

    class Envelope<T> : IEnvelope<T>
    {
        public IParticipant Receiver { get; }
        public T Body { get; }
        public IFault Error { get; }

        internal Envelope(Participant source, T body)
        {
            Receiver = source;
            Body = body;
            Error = null;
        }

        internal Envelope(Participant source, Exception error)
        {
            Receiver = source;
            Body = default(T);
            Error = new Fault(FaultCode.GeneralError, error.Message, error.StackTrace);
        }

        public IDictionary<string, int> Headers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IParticipant Sender
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}