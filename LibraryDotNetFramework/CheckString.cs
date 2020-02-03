using System;
using System.Drawing;

namespace LibraryDotNetFramework
{
    /// <summary>
    /// Проверка введенной строки на посторонние символы
    /// </summary>
    public class CheckString
    {
        /// <summary>
        /// Возвращает цвет в завимости резултатов проверки строки
        /// </summary>
        public static Color GetColorAfterCheckString(string str, bool checkResult)
        {
            if (str == "")
                return Color.Snow;
            else if (checkResult)
                return Color.LightGreen;
            else
                return Color.LightCoral;
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

        /// <summary>
        /// Проверка введенного номера телефона на корректность
        /// </summary>
        public static bool Phone(string CheckString)
        {
            if (CheckString.Length != 17)
                return false;
            CheckString = CheckString.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            if (CheckString.Length == 12)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Проверка введенного числа на отсутствие посторонних символов
        /// </summary>
        public static bool Numbers(string CheckString)
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
        /// Проверка введенного ФИО на отсутствие посторонних символов
        /// </summary>
        public static bool FullName(string CheckString)
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

    }
}
