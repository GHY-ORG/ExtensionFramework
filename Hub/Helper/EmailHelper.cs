using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Helper
{
    public class EmailHelper
    {
        /// <summary>
        /// 用ghyers@ghy.cn发送邮件，最好不要发太凶。。。
        /// </summary>
        /// <param name="toList">发送邮箱列表</param>
        /// <param name="ccList">抄送邮箱列表</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="isHtml">邮件内容是否为Html</param>
        /// <param name="myEmail">我的邮箱地址</param>
        /// <param name="myName">我的显示名</param>
        public static void SendEmailSSL(string[] toList, string[] ccList, string title, string content, bool isHtml, string myEmail, string myName)
        {
            SmtpClient client = new SmtpClient("smtp.qq.com", 587);
            client.Credentials = new System.Net.NetworkCredential("ghyers@ghy.cn", "ghyinswufe12");
            SendEmailSSL(toList, ccList, title, content, isHtml, myEmail, myName, client);
        }

        public static void SendEmailSSL(string[] toList, string[] ccList, string title, string content, bool isHtml, string myEmail, string myName, SmtpClient client)
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            foreach (var i in toList)
            {
                msg.To.Add(i);
            }
            foreach (var i in ccList)
            {
                msg.CC.Add(i);
            }
            msg.From = new MailAddress(myEmail, myName, System.Text.Encoding.UTF8);
            msg.Subject = title;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = content;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = isHtml;
            client.EnableSsl = true;
            try
            {
                client.Send(msg);
                Console.WriteLine("success");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Console.WriteLine("err" + ex);
            }
        }
    }
}
