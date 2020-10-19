using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;

namespace WpfApp1.Services
{
    interface IEmailSender
    {
        void SendMail(string mail, string name);
        void SendMails(IQueryable<EmailSender.Email> emails);
    }

    class EmailSender : IEmailSender
    {
        private string _login;
        private string _password;
        private string _smtp = "smtp.yandex.ru";
        private int _port = 25;
        private string _subject = "Заголовок сообщения";
        private string _body = "Текст сообщения";
        public EmailSender(string login, string password)
        {
            _login = login;
            _password = password;
        }
        public void SendMail(string mail, string name)
        {
            using (MailMessage mm = new MailMessage(_login, mail))
            {
                mm.Subject = _subject;
                mm.Body = _body;
                mm.IsBodyHtml = false;
                SmtpClient sc = new SmtpClient(_smtp, _port);
                sc.EnableSsl = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new NetworkCredential(_login, _password);
                try
                {
                    sc.Send(mm);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка отправки почты!\n" + ex.Message);
                }
            }
        }
        public void SendMails(IQueryable<Email> emails)
        {
            foreach (var email in emails)
            {
                SendMail(email.Value, email.Name);
            }
        }
    }
    public struct Email
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
