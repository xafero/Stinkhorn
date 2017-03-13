namespace Stinkhorn.API
{
    public class HelloMessage : IMessage, IResponse
    {
        public string Local { get; set; }

        public string Remote { get; set; }

        public string Machine { get; set; }

        public string User { get; set; }
    }
}