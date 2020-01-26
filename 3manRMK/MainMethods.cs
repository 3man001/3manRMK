using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Drawing;

namespace _3manRMK
{
    /// <summary>
    /// Класс с самописными метода используемыми в решении
    /// </summary>
    public static class MainMethods
    {
        /// <summary>
        /// Проверка введенной строки на посторонние символы
        /// </summary>
        public static class CheckSimbols
        {
            /// <summary>
            /// Проверка введенного числа на отсутствие посторонних символов
            /// </summary>
            public static bool Numbers (string CheckString)
            {
                int index = CheckString.IndexOf(',');
                if (index == 0 || CheckString == "")
                    return false;
                else
                {
                    int lenght = CheckString.Length;
                    if (index > 0)
                    {
                        CheckString = CheckString.Remove(index, 1);
                        lenght--;
                    }
                    string DataSimbol = "1234567890";
                    for (int i = 0; i < lenght; i++)
                    {
                        if (DataSimbol.IndexOf(CheckString[i]) < 0)
                            return false;
                    }
                    return true;
                }
            }
            /// <summary>
            /// Проверка введенного Email на отсутсвие посторонних символов
            /// </summary>
            public static bool Email(string CheckString)
            {
                int index = CheckString.IndexOf('@');
                int lenght = CheckString.Length;
                if ((index < 3) || (index + 1 == lenght))
                    return false;
                else
                {
                    string DataSimbol = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM.-";
                    CheckString = CheckString.Remove(index, 1);
                    for (int i = 0; i < lenght - 1; i++)
                    {
                        if (DataSimbol.IndexOf(CheckString[i]) < 0)
                            return false;
                    }
                    return true;
                }
            }
            /// <summary>
            /// Проверка введенного номера телефона на корректность
            /// </summary>
            public static bool Phone(string CheckString)
            {
                if (CheckString.Length != 17)
                    return false;
                CheckString = CheckString.Replace("(", "").Replace(")","").Replace("-","").Replace(" ","");
                if (CheckString.Length == 12)
                    return true;
                else
                    return false;
            }
            /// <summary>
            /// Проверка введенного ФИО на отсутствие посторонних символов
            /// </summary>
            public static bool FullName (string CheckString)
            {
                CheckString = CheckString.Trim(' ');
                if (CheckString.Length < 3)
                    return false;
                string DataSimbol = " ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";
                for (int i = 0; i < CheckString.Length; i++)
                {
                    if (DataSimbol.IndexOf(CheckString[i]) < 0)
                        return false;
                }
                return true;
            }
            /// <summary>
            /// Проверка строки с наименованием покупателя на постороннние символы
            /// </summary>
            public static bool Buyer(string CheckString)
            {
                CheckString = CheckString.Trim(' ');
                if (CheckString.Length < 3)
                    return false;
                string DataSimbol = " ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ1234567890" + '"';
                for (int i = 0; i < CheckString.Length; i++)
                {
                    if (DataSimbol.IndexOf(CheckString[i]) < 0)
                        return false;
                }
                return true;
            }
            /// <summary>
            /// Проверка введенного ИНН по контрольным числам
            /// </summary>
            public static bool TaxpayerIdentificationNumber(string CheckString)
            {
                string inn = CheckString;
                if (inn.Length == 12)
                {
                    int N0 = (int)char.GetNumericValue(inn[0]); int N1 = (int)char.GetNumericValue(inn[1]);
                    int N2 = (int)char.GetNumericValue(inn[2]); int N3 = (int)char.GetNumericValue(inn[3]);
                    int N4 = (int)char.GetNumericValue(inn[4]); int N5 = (int)char.GetNumericValue(inn[5]);
                    int N6 = (int)char.GetNumericValue(inn[6]); int N7 = (int)char.GetNumericValue(inn[7]);
                    int N8 = (int)char.GetNumericValue(inn[8]); int N9 = (int)char.GetNumericValue(inn[9]);
                    int N10 = (int)char.GetNumericValue(inn[10]); int N11 = (int)char.GetNumericValue(inn[11]);

                    int b2 = (N0 * 7 + N1 * 2 + N2 * 4 + N3 * 10 + N4 * 3 + N5 * 5 + N6 * 9 + N7 * 4 + N8 * 6 + N9 * 8) % 11;
                    int b1 = (N0 * 3 + N1 * 7 + N2 * 2 + N3 * 4 + N4 * 10 + N5 * 3 + N6 * 5 + N7 * 9 + N8 * 4 + N9 * 6 + N10 * 8) % 11;

                    if ((b2 == N10) || ((b2 == 10) && (N10 == 0)))
                    {
                        if ((b1 == N11) || ((b1 == 10) && (N11 == 0)))
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else if (inn.Length == 10)
                {
                    int N0 = (int)char.GetNumericValue(inn[0]); int N1 = (int)char.GetNumericValue(inn[1]);
                    int N2 = (int)char.GetNumericValue(inn[2]); int N3 = (int)char.GetNumericValue(inn[3]);
                    int N4 = (int)char.GetNumericValue(inn[4]); int N5 = (int)char.GetNumericValue(inn[5]);
                    int N6 = (int)char.GetNumericValue(inn[6]); int N7 = (int)char.GetNumericValue(inn[7]);
                    int N8 = (int)char.GetNumericValue(inn[8]); int N9 = (int)char.GetNumericValue(inn[9]);

                    int b1 = (N0 * 2 + N1 * 4 + N2 * 10 + N3 * 3 + N4 * 5 + N5 * 9 + N6 * 4 + N7 * 6 + N8 * 8) % 11;

                    if ((b1 == N9) || ((b1 == 10) && (N9 == 0)))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            public static Color GetColorAfterCheckINN (string INN)
            {
                if (INN == "")
                    return Color.Snow;
                else if (TaxpayerIdentificationNumber(INN))
                   return Color.LightGreen;
                else
                    return Color.LightCoral;
            }
        }
        public static class Email
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
        }
        public static class Setting
        {
            public static string ConvertItemsToString(System.Windows.Forms.ComboBox.ObjectCollection items)
            {
                string str = "";
                for (int i = 0; i < items.Count; i++)
                {
                    str = str + ";" + items[i] + ";";
                }
                return str;
            }
            public static void ConvertStringToItems(string str, System.Windows.Forms.ComboBox.ObjectCollection items)
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
}
