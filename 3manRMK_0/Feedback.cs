using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace _3manRMK
{
    public partial class Feedback : Form
    {
        public Feedback()
        {
            InitializeComponent();
        }
        private void SendMail(string theme, string message)
        {
            MailAddress from = new MailAddress("feedback@3man001.ru", "3manRMK"); //Отправитель - Адресс и отображаемое Имя
            MailAddress to = new MailAddress("igor-viv001@yandex.ru"); //Адрес получателя
            MailMessage m = new MailMessage(from, to); // создаем объект сообщения
            m.Subject = theme; // тема письма
            m.Body = message; // текст письма
            m.IsBodyHtml = true; // письмо представляет код html
            SmtpClient smtp = new SmtpClient("mail.3man001.ru", 25); // адрес smtp-сервера и порт отправки письма
            smtp.Credentials = new NetworkCredential("feedback@3man001.ru", "W1h7A4a4"); // логин и пароль
            smtp.EnableSsl = false;
            try { smtp.Send(m); }
            catch { }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SendMail(textBox1.Text, textBox2.Text);
            this.Close();
        }
    }
}
