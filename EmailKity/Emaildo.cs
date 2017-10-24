using System;
using System.Collections.Generic;
using System.Text;
using MailKit.Net;
using MimeKit;
using System.IO;

namespace EmailKity
{
    class Emaildo
    {
        public static  void SentEmail()
        {
            var path = @"C:\Users\mingdao\Pictures\Camera Roll\s.pdf";
            var message = new MimeMessage();
            //获取From标头中的地址列表，添加指定的地址
            message.From.Add(new MailboxAddress("ww", ""));
            //获取To头中的地址列表，添加指定的地址
            message.To.Add(new MailboxAddress("11", "117657860@qq.com"));
            //获取或设置消息的主题
            message.Subject = "How you doin?";
            // 创建我们的消息文本，就像以前一样（除了不设置为message.Body）
            var body = new TextPart("plain")
            {
                Text = @"Hey Alice-- Joey"
            };

            FileStream fs = new FileStream(path, FileMode.Open);
            
             //获取文件大小
             long size = fs.Length;
            
            byte[] array = new byte[size];
            
             //将文件读到byte数组中
            fs.Read(array, 0, array.Length);

            fs.Close();

            //string path1 =  @"C:\Users\mingdao\Pictures\Camera Roll\cc.txt";
            ////创建一个文件流
            //FileStream fs1 = new FileStream(path1, FileMode.Create);

            // //将byte数组写入文件中
            // fs1.Write(array, 0, array.Length);
            //            //所有流类型都要关闭流，否则会出现内存泄露问题
            //fs1.Close();
            MemoryStream ms = new MemoryStream(array);
            // 为位于路径的文件创建图像附件
            var attachment = new MimePart()
            {
                ContentObject = new ContentObject(ms, ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName("s.pdf")
            };
            // 现在创建multipart / mixed容器来保存消息文本和图像附件
            var multipart = new Multipart()
            {
                body, attachment
            };
            // 现在将multipart / mixed设置为消息正文 
            message.Body = multipart;


            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtpdm.aliyun.com", 25, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("", "");

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public void TestSendMailDemo()
        {
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MimeKit.MailboxAddress("hotmail", "china-psu@hotmail.com"));
            message.To.Add(new MimeKit.MailboxAddress("qq", "283775652@qq.com"));
            message.Subject = "This is a Test Mail";
            var plain = new MimeKit.TextPart("plain")
            {
                Text = @"不好意思，我在测试程序，Sorry！"
            };
            var html = new MimeKit.TextPart("html")
            {
                Text = @"<p>Hey geffzhang<br>
<p>不好意思，我在测试程序，Sorry！<br>
<p>-- Geffzhang<br>"
            };
            // create an image attachment for the file located at path
            var path = "D:\\雄安.jpg";
            var fs = File.OpenRead(path);
            var attachment = new MimeKit.MimePart("image", "jpeg")
            {

                ContentObject = new MimeKit.ContentObject(fs, MimeKit.ContentEncoding.Default),
                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                ContentTransferEncoding = MimeKit.ContentEncoding.Base64,
                FileName = Path.GetFileName(path)
            };
            var alternative = new MimeKit.Multipart("alternative");
            alternative.Add(plain);
            alternative.Add(html);
            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new MimeKit.Multipart("mixed");
            multipart.Add(alternative);
            multipart.Add(attachment);
            message.Body = multipart;
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.live.com", 587, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                var mailFromAccount = "china-psu@hotmail.com";
                var mailPassword = "xxxxxxxxxxxxxxxxxxx";
                client.Authenticate(mailFromAccount, mailPassword);

                client.Send(message);
                client.Disconnect(true);
            }
            fs.Dispose();
        }
    }
}
