using System;
using IBT.Messaging;

namespace IBT.Consumer.PartnerA
{
    public class EmailService : IEmailService
    {
        public void SendEmail(PartnerAMessage message)
        {
            Console.WriteLine("Sending email with the following body:");
            var emailBody = $"{message.ProductNameFull}, {message.IbtTypeCode}, {message.EventType}, {message.Isin}";
        }
    }

    public interface IEmailService
    {
        void SendEmail(PartnerAMessage message);
    }
}