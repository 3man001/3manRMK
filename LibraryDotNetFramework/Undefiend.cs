using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace LibraryDotNetFramework
{
    class Undefiend
    {
        public static bool SendMail(string subject, string message)
        {
            string fromAddress = "feedback@3man001.ru";
            string fromName = "3manRMK";
            string toAddress = "igor-viv001@yandex.ru";

            MailAddress from = new MailAddress(fromAddress, fromName); //Отправитель - Адресс и отображаемое Имя
            MailAddress to = new MailAddress(toAddress); //Адрес получателя
            MailMessage m = new MailMessage(from, to); // создаем объект сообщения

            m.Subject = subject; // тема письма
            m.Body = "<h2> " + message.Replace("\n", "<br>") + " </h2>"; // текст письма
            m.IsBodyHtml = true; // письмо представляет код html
            SmtpClient smtp = new SmtpClient("mail.3man001.ru", 25); // адрес smtp-сервера и порт отправки письма
            smtp.Credentials = new NetworkCredential("feedback@3man001.ru", "W1h7A4a4"); // логин и пароль
            smtp.EnableSsl = false;
            try
            {
                smtp.Send(m);
            }
            catch
            {
                return false; //Не удалось отправить письмо
            }
            return true; //Письмо отправилось
        }
        public static string[] AddDictToArray(Dictionary<string, int> dict, string[] array)
        {
            int lenght = array.Length;
            foreach (string element in dict.Keys)
            {
                Array.Resize(ref array, lenght + 1);
                array[lenght] = element;
                lenght++;
            }
            return array;
        }
        public static string ConvertItemsToString(ComboBox.ObjectCollection items)
        {
            string str = "";
            for (int i = 0; i < items.Count; i++)
            {
                str = str + ";" + items[i] + ";";
            }
            return str;
        }
        public static void ConvertStringToItems(string str, ComboBox.ObjectCollection items)
        {
            items.Clear();
            int index = str.IndexOf(';');
            while (index >= 0)
            {
                str = str.Remove(index, 1);
                int nextIndex = str.IndexOf(';');
                if (nextIndex >= 0)
                {
                    items.Add(str.Substring(index, nextIndex - index));
                    str = str.Remove(index, nextIndex - index + 1);
                }
                index = str.IndexOf(';');
            }
        }
    }
}
