﻿using System;
using System.Configuration;
using System.Net.Mail;
using IBT.Messaging;

namespace IBT.Consumer.PartnerA
{
    public class EmailService : IEmailService
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(EmailService));

        public void SendEmail(PartnerAMessage message)
        {
            _log.Info("Sending email with the following body:");
            var emailBody = $"{message.ProductNameFull}, {message.IbtTypeCode}, {message.EventType}, {message.Isin}";

            _log.Info(emailBody); // Log message

            SendEmail(emailBody);
        }

        private void SendEmail(string body)
        {
            var mail = new MailMessage(ConfigurationManager.AppSettings["FromEmailAddress"],
                ConfigurationManager.AppSettings["ToEmailAddress"]);

            var client = new SmtpClient
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = ConfigurationManager.AppSettings["SMTPServer"]
            };
            mail.Subject = "PartnerA IBT message";
            mail.Body = body;

            try
            {
                client.Send(mail);
            }
            catch (SmtpException smtpException)
            {
                _log.Info(smtpException);
            }
        }
    }
}