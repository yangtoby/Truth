using MailKit.Net.Smtp;
using MimeKit;
using NETCore.MailKit;
using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace CastleSetting
{
    public class email
    {
        private  IEmailService _EmailService;
        private  string _MailToOne;
        private  string _MailToMulti;
        public void send()
        {
            var provider = new MailKitProvider(new MailKitOptions()
            {
                Server = "smtp server address",
                Port = 25,
                SenderName = "mail from user name",
                SenderEmail = "mail from ",
                Account = "your email",
                Password = "your password"
            });

            _EmailService = new EmailService(provider);

            _MailToOne = "email address";
            _MailToMulti = "address1,address2";

            _EmailService.Send(_MailToOne, "Test MailKit Extensions Html Body", "<html><h1>Hello MailKit<h1><br/><p style='font-size:18px;color:red'>Font size is 18px ,color is red</p></html>", true);
        }

        public void ss()
        {

            var message = new MimeKit.MimeMessage();
            message.From.Add(new MailboxAddress("Joey Tribbiani", "joey@friends.com"));
            message.To.Add(new MailboxAddress("Mrs. Chanandler Bong", "chandler@friends.com"));
            message.Subject = "How you doin'?";

            message.Body = new TextPart("plain")
            {
                Text = @"Hey Chandler,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?

-- Joey"
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.friends.com", 587, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("joey", "password");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
