using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace _3manRMK_0
{
    public partial class ErrorsForm : Form
    {
        int ResultCode;
        int Mode;
        int ECRMode8Status;
        int ECRModeStatus;
        int ECRAdvancedMode;
        private void SendMail(string message)
        {
            MailAddress from = new MailAddress("feedback@3man001.ru", "3manRMK"); //Отправитель - Адресс и отображаемое Имя
            MailAddress to = new MailAddress("igor-viv001@yandex.ru"); //Адрес получателя
            MailMessage m = new MailMessage(from, to); // создаем объект сообщения
            m.Subject = "Ошибка работы"; // тема письма
            m.Body = "<h2> "+message+" </h2>"; // текст письма
            m.IsBodyHtml = true; // письмо представляет код html
            SmtpClient smtp = new SmtpClient("mail.3man001.ru", 25); // адрес smtp-сервера и порт отправки письма
            smtp.Credentials = new NetworkCredential("feedback@3man001.ru", "W1h7A4a4"); // логин и пароль
            smtp.EnableSsl = false;
            smtp.Send(m);
        }
        public ErrorsForm(int ResultCodeIn, string ResultCodeDesc, int ModeIn, string ModeIndesc, int ECRMode8StatusIn, int ECRModeStatusIn, int ECRAdvancedModeIn, string ECRAdvancedModeDescription)
        {
            InitializeComponent();
            ResultCode = ResultCodeIn; //Код ошибки ниже его расшифровка
            LTextError.Text = string.Format("Ошибка = {0}, {1}", ResultCode, ResultCodeDesc);
            Mode = ModeIn; //Код режима ниде его расшифровка
            LMode.Text = string.Format("Режим = {0}, {1}", Mode, ModeIndesc);
            ECRMode8Status = ECRMode8StatusIn; //Код статуса режима 8
            ECRModeStatus = ECRModeStatusIn; //Код статуса режима 13 и 14
            ECRAdvancedMode = ECRAdvancedModeIn; //Код Статуса Расширенного Подрежима ККМ
            LAdvancedmode.Text = string.Format("Расширенный Режим = {0}, {1}", ECRAdvancedMode, ECRAdvancedModeDescription); //Описание подрежима ККМ
            LAllCode.Text = string.Format("Список всех кодов: Ошибка={0}; Режим={1};Режим8={2};Режим13_14={3};Подрежим={4}", ResultCode, Mode, ECRMode8Status, ECRModeStatus, ECRAdvancedMode);
            LSD.Text = ProcessError();
            
        }
        private string ProcessError()
        {
            string MessageBack = "Решения проблемы нет в базе, отправьте запрос в СТП приложите скрин. e-mail: igor@3man001.ru";

            if (ResultCode == 0)
            {
                return "Ошибок нет, все хорошо :)";
            }
            if (ResultCode == 115)
            {
                if (Mode == 4)
                {
                    return "В кассовом аппарате закрыта смена, для перехода в рабочий режим её нужно открыть.";
                }
                else
                { return MessageBack; }   
            }
            else
            { return MessageBack; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMail(LAllCode.Text);
            this.Close();
        }
    }
}
