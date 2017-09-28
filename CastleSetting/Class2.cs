using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CastleSetting
{
    class Class2
    {
        private const string fromAddress1 = "sender@register2.mingdao.com";
        private const string fromAddress2 = "sender@register2.mingdao.net";
        private const string smtpPwd = "newFr9P4CmqdI9XT";
        private static Random Rand;
        private static string[] fromAddressArray = new[] { fromAddress1, fromAddress2 };
        static Class2()
        {
            Rand = new Random();
        }
        public static void send(string toEmail, string subject, string body, Dictionary<string, byte[]> attachmentInfos)
        {
            var randId = Rand.Next(fromAddressArray.Length);
            var fromAddress = fromAddressArray[randId];
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("明道", fromAddress));
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            message.Subject = subject;

            var html = new TextPart(TextFormat.Html)
            {
                Text = body

            };

            Multipart multipart = new Multipart();
            if (attachmentInfos != null && attachmentInfos.Count > 0)
            {
                multipart = new Multipart("mixed");
                foreach (var file in attachmentInfos)
                {
                    MemoryStream stream = new MemoryStream(file.Value);
                    var attachment = new MimePart()
                    {
                        ContentObject = new ContentObject(stream, ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Default,
                        FileName = file.Key
                    };

                    multipart.Add(attachment);


                }
            }



                multipart.Add(html);

                message.Body = multipart;
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("smtpdm.aliyun.com", 25, false);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(fromAddress, smtpPwd);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
        }
    
}
