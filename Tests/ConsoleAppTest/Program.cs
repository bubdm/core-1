using System;
using System.Net;
using System.Net.Mail;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var from = new MailAddress("kanadeiar@gmail.com", "Отправитель");
            var to = new MailAddress("kanadei@mail.ru", "Получатель");

            using var message = new MailMessage(from, to)
            {
                Subject = "Заголовок",
                Body = $"Текст письма {DateTime.Now}",
            };

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                string login = "kanadeiar@gmail.com";
                Console.Write("Please input e-mail password 'kanadeiar@gmail.com' :>");
                string password = Console.ReadLine();

                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(login, password);
                client.Send(message);
            }
            Console.WriteLine("Почта отправлена!");

            Console.ReadKey();
        }
    }
}
