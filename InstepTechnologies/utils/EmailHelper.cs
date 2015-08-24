using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace InstepTechnologies.utils
{
    public static class EMailHelper
    {
        static EMailHelper()
        {
            DefaultAccount = new EmailAccount
            {
                DisplayName = "Instep Technologies",
                Email = "office@insteptechnologies.com",
                EnableSsl = true,
                UseDefaultCredentials = false,
                Password = "anna#123",
                Username = "office@insteptechnologies.com",
                Host = "smtp.gmail.com",
                Port = 587
            };
        }

        private static readonly EmailAccount DefaultAccount;

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="replyTo"></param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="toAddress">To address</param>
         
        public static void SendEmail(string toAddress, string subject, string body, string replyTo = null)
        {
            try
            {

                string code = Guid.NewGuid().ToString();
                string email = WebConfigurationManager.AppSettings["myemailid"];
                string password = WebConfigurationManager.AppSettings["mypassword"];

                //string message = "Your Password Is --  " + Password;

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(email);
                mail.To.Add(toAddress);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(email, password);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }

            catch (Exception exception)
            {
            }

        }
        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailAccount">Email account to use</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="from">From address</param>
        /// <param name="to">To address</param>
        /// <param name="replyTo"></param>
        /// <param name="bcc">BCC addresses list</param>
        /// <param name="cc">CC addresses ist</param>
        private static void SendEmail(EmailAccount emailAccount, string subject, string body,
                                       MailAddress from, MailAddress to, string replyTo,
                                      IEnumerable<string> bcc = null, IEnumerable<string> cc = null)
        {
            var message = new MailMessage();
            message.From = from;
            message.To.Add(to);
            if (!string.IsNullOrEmpty(replyTo))
                message.ReplyToList.Add(new MailAddress(replyTo));

            if (null != bcc)
            {
                foreach (var address in bcc.Where(bccValue => !String.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }
            if (null != cc)
            {
                foreach (var address in cc.Where(ccValue => !String.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                smtpClient.Host = emailAccount.Host;
                smtpClient.Port = emailAccount.Port;
                smtpClient.EnableSsl = emailAccount.EnableSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                if (emailAccount.UseDefaultCredentials)
                    smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
                else
                    smtpClient.Credentials = new NetworkCredential(emailAccount.Username, emailAccount.Password);

                try
                {
                    smtpClient.Send(message);
                }
                catch (Exception exception)
                {
                }
            }
        }

        private static void sendmailsimple(string subject,string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("contact@insteptechnologies.com", "instepte1"),
                EnableSsl = true
            };
            client.Send("contact@insetptechnologies.com", "contact@insetptechnologies.com", subject, body);
        }

        private const bool CheckServerOnlineOnValidation = true;

        public static bool ValidateEmail(string email)
        {
            //const string EmailRegExRFC2822 = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\\])";

            const string emailRegEx = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            if (string.IsNullOrEmpty(email) || (email.Trim().Length == 0))
            {
                return false;
            }

            try
            {
                var match = Regex.Match(email, emailRegEx, RegexOptions.Compiled);
                var matchEmailPattern = ((match.Success && (match.Index == 0)) && (match.Length == email.Length));
                if (matchEmailPattern)
                {
                    if (!CheckServerOnlineOnValidation)
                        return true;

                    var serverIndex = email.IndexOf('@') + 1;
                    var server = email.Substring(serverIndex);

                    var pingReply = new Ping().Send(server, 300, new byte[] { 0xFF });
                    var isLive = pingReply != null && pingReply.Status == IPStatus.Success;
                    var reply = new Ping().Send(server, 300, new byte[] { 0xFF });
                    if (reply != null)
                        isLive = isLive || reply.Status == IPStatus.Success;
                    var send = new Ping().Send(server, 300, new byte[] { 0xFF });
                    if (send != null)
                        isLive = isLive || send.Status == IPStatus.Success;

                    return isLive;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Represents an email account
        /// </summary>
        private class EmailAccount
        {
            /// <summary>
            /// Gets or sets an email address
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Gets or sets an email display name
            /// </summary>
            public string DisplayName { get; set; }

            /// <summary>
            /// Gets or sets an email host
            /// </summary>
            public string Host { get; set; }

            /// <summary>
            /// Gets or sets an email port
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// Gets or sets an email user name
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// Gets or sets an email password
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Gets or sets a value that controls whether the SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection
            /// </summary>
            public bool EnableSsl { get; set; }

            /// <summary>
            /// Gets or sets a value that controls whether the default system credentials of the application are sent with requests.
            /// </summary>
            public bool UseDefaultCredentials { get; set; }

            /// <summary>
            /// Gets a friendly email account name
            /// </summary>
            public string FriendlyName
            {
                get
                {
                    if (!String.IsNullOrWhiteSpace(DisplayName))
                        return Email + " (" + DisplayName + ")";
                    return Email;
                }
            }
        }
    }
}