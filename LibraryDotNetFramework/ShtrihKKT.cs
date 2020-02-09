using System;
using DrvFRLib;

namespace LibraryDotNetFramework
{
    public class ShtrihKKT
    {
        /// <summary>
        /// Выводит диалоговое окно подключения к ККТ
        /// </summary>
        public static void ConnectToKKT(DrvFR Drv)
        {
            Drv.ShowProperties();
        }

        /// <summary>
        /// Возвращает ИНН пользователя из отчета о регистрации ККТ в ФНС
        /// </summary>
        public static string GetINN(DrvFR Drv)
        {
            Drv.FNGetFiscalizationResult();
            return Drv.INN;
        }

        /// <summary>
        /// Возвращает сумму Наличности находящейся в денежном ящике
        /// </summary>
        public static decimal GetCashReg(DrvFR Drv)
        {
            Drv.RegisterNumber = 241; //Накопление наличности в кассе.Вохможно другое значение у другой модели
            Drv.GetCashReg();
            return Drv.ContentsOfCashRegister;
        }

        /// <summary>
        /// Регистрирует ВНЕСЕНИЕ или ВЫПЛАТУ в денежный ящик
        /// </summary>
        public static bool CashInOutCome(DrvFR Drv, string operation, decimal summCash)
        {
            if (operation == "Внесение")
            {
                Drv.Summ1 = summCash;
                Drv.CashIncome();
                return true;
            }
            else if (operation == "Выплата")
            {
                Drv.Summ1 = summCash;
                Drv.CashOutcome();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Регистрирует позицию в чеке ККТ
        /// </summary>
        public static bool RegPosition(DrvFR Drv, int CheckType, int PaymentItemSign, string NameProduct, Decimal Price, 
            Double Quantity, int Tax1, Decimal Summ1) //Регистрация позиции в чеке
        {
            Drv.CheckType = CheckType;
            Drv.Price = Price;
            Drv.Quantity = Quantity;
            Drv.Summ1Enabled = true; //Использовать сумму операции (сами рассчитываем цену)
            Drv.Summ1 = Summ1;
            Drv.TaxValueEnabled = false; //Налог мы не рассчитываем
            Drv.Tax1 = Tax1;
            Drv.Department = 1; //Отдел (0-16 режим свободной продажи)
            Drv.PaymentTypeSign = 4; // Признак способа расчета 1..7 (4-Полный расчет)
            Drv.PaymentItemSign = PaymentItemSign;
            Drv.StringForPrinting = NameProduct;
            try
            {
                Drv.FNOperation(); // Пробиваем позицию
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Вводим оплату и Закрывает чек в ККТ
        /// </summary>
        public static bool CloseChek(DrvFR Drv, Decimal cashPayment, Decimal electronicPayment, int TaxType = 2) // Формируем закрытие чека
        {
            Drv.Summ1 = cashPayment; // Наличные
            Drv.Summ2 = electronicPayment; // Остальные типы оплаты нулевые, но их необходимо заполнить
            Drv.Summ3 = 0;
            Drv.Summ4 = 0;
            Drv.Summ5 = 0;
            Drv.Summ6 = 0;
            Drv.Summ7 = 0;
            Drv.Summ8 = 0;
            Drv.Summ9 = 0;
            Drv.Summ10 = 0;
            Drv.Summ11 = 0;
            Drv.Summ12 = 0;
            Drv.Summ13 = 0;
            Drv.Summ14 = 0;
            Drv.Summ15 = 0;
            Drv.Summ16 = 0;
            Drv.RoundingSumm = 0; // Сумма округления
            Drv.TaxValue1 = 0; // Налоги мы не считаем
            Drv.TaxValue2 = 0;
            Drv.TaxValue3 = 0;
            Drv.TaxValue4 = 0;
            Drv.TaxValue5 = 0;
            Drv.TaxValue6 = 0;
            Drv.TaxType = TaxType; //система налогообложения
            Drv.StringForPrinting = "";
            try
            {
                Drv.FNCloseCheckEx(); //Закрытие чека
            }
            catch
            {
                return false;
            }
            if (Drv.ResultCode == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Снимает дневной отчет с ККТ
        /// </summary>
        public static bool TakeDalyReport(DrvFR Drv) //Снять Х-Отчет
        {
            try
            {
                Drv.PrintReportWithoutCleaning(); //Отчет без гашения
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Возвращает строку, содуржвщую список Типов СНО указанных при активации ФН в ККТ
        /// </summary>
        public static string GetTaxType (DrvFR Drv)
        {
            Drv.FNGetFiscalizationResult(); //Полусить итоги фискализации
            string taxType = Convert.ToString(Drv.TaxType, 2); //Получить Ситемы налогообложения
            taxType = new string('0', 6 - taxType.Length) + taxType;
            //TaxType = "111111";

            string str = "";
            if (taxType[5] == '1')
                str += ";Основная;";
            if (taxType[4] == '1')
                str += ";УСН доход;";
            if (taxType[3] == '1')
                str += ";УСН доход-расход;";
            if (taxType[2] == '1')
                str += ";ЕНВД;";
            if (taxType[1] == '1')
                str += ";ЕСХН;";
            if (taxType[0] == '1')
            str += ";Патент;";
            return str;
        }

        /// <summary>
        /// Указывает Зарегестрированного кассира в документах
        /// </summary>
        public static void SendFIO(DrvFR Drv, string FIO, string INN)
        {
            if (FIO != "")
            {
                Drv.TagNumber = 1021; //Отправка Должности и Фамилии кассира
                Drv.TagType = 7;
                Drv.TagValueStr = FIO;
                Drv.FNSendTag();
                if (INN != "")
                {
                    Drv.TagNumber = 1203; //Отправка ИНН кассира
                    Drv.TagType = 7;
                    Drv.TagValueStr = INN;
                    Drv.FNSendTag();
                }
            }
        }

        /// <summary>
        /// Указывает данные покупателя в документах
        /// </summary>
        public static void SendCustomer (DrvFR Drv, string customerName, string customerINN)
        {
            Drv.TagNumber = 1227;
            Drv.TagType = 7;
            Drv.TagValueStr = customerName;
            Drv.FNSendTag();
            Drv.TagNumber = 1228;
            Drv.TagType = 7;
            Drv.TagValueStr = customerINN;
            if (Drv.TagValueStr.Length == 10)
                Drv.TagValueStr += "00";
            Drv.FNSendTag();
        }

        /// <summary>
        /// Открывает смену в ККТ с ФИО и ИНН оператора
        /// </summary>
        public static bool OpenShift (DrvFR Drv, string FIO, string INN)
        {
            try
            {
                Drv.FNBeginOpenSession();
                SendFIO(Drv, FIO, INN);
                Drv.FNOpenSession();
                System.Threading.Thread.Sleep(2000);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Закрывает смену в ККТ с ФИО и ИНН оператора
        /// </summary>
        public static bool CloseShift (DrvFR Drv, string FIO, string INN)
        {
            try
            {
                Drv.FNBeginCloseSession();
                SendFIO(Drv, FIO, INN);
                Drv.FNCloseSession();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Отмена не закрытого чека в ККТ
        /// </summary>
        public static bool CancelCashReciept (DrvFR Drv)
        {
            try
            {
                Drv.CancelCheck();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static void SendCustomerPhoneOrEmail(DrvFR Drv, string phoneOrEmail)
        {
            Drv.CustomerEmail = phoneOrEmail;
            Drv.FNSendCustomerEmail();
        }
    }
}
