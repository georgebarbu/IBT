using IBT.Messaging;

namespace IBT.Consumer.PartnerA
{
    public interface IEmailService
    {
        void SendEmail(PartnerAMessage message);
    }
}