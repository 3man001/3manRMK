using DrvFRLib; //Библиотека ШТРИХ-М подключение
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3manRMK
{
    class WorkWithDKKT
    {
        public static Color CheckFNStatusInColor (string FNWarningFlag)
        {
            if (FNWarningFlag == "0000") //Все хорошо
                return Color.LightGreen;
            else if (FNWarningFlag[3] == '1') //Срочная замена ФН, осталось 3 дня
                return Color.Red;
            else if(FNWarningFlag[1] == '1') //ФН заполнен на 90%
                return Color.Red;
            else if (FNWarningFlag[2] == '1') //До замены ФН 30 дней
                return Color.Yellow;
            else //Превышено время ожидания ответа от ОФД
                return Color.Yellow;
        }
        public static Color CheckTheTimeDiffereceInColor(DateTime timePC, DateTime timeKKT)
        {
            TimeSpan Interval = timePC - timeKKT;
            if (Interval.Days == 0)
            {
                if (Interval.Hours == 0)
                {
                    if (-3 <= Interval.Minutes && Interval.Minutes <= 3)
                        return Color.LightGreen; //Не значительное расхождение
                    else if (-5 <= Interval.Minutes && Interval.Minutes <= 5)
                        return Color.Yellow; //Обратить внимание
                    else
                        return Color.Red; //Расхождение более 5 мин
                }
                else
                    return Color.Red; //Расхождение более 5 мин
            }
            else
                return Color.Red; //Расхождение более 5 мин
        }
        public static Color CheckOFDStatusInColor(string ExchangeStatus, DateTime dateTimePC, DateTime dateFirstFD)
        {
            if (ExchangeStatus[1] == '1')
            {
                int Interval_Days = (dateTimePC - dateFirstFD).Days;
                if (0 <= Interval_Days && Interval_Days < 5)
                    return Color.LightGreen;
                else if (Interval_Days < 0)
                    return Color.Red;
                else if (5 <= Interval_Days && Interval_Days < 15)
                    return Color.Yellow;
                else
                    return Color.Red;
            }
            else
                return Color.LightGreen;
        }
        public class DicItems
        {
            public static Dictionary<string, int> getDicTaxItem()
            {
                Dictionary<string, int> TaxItem = new Dictionary<string, int>();
                TaxItem.Add("НДС 20%", 1);
                TaxItem.Add("НДС 10%", 2);
                TaxItem.Add("НДС 0%", 3);
                TaxItem.Add("Без НДС", 4);
                TaxItem.Add("НДС 20/120", 5);
                TaxItem.Add("НДС 10/110", 6);
                return TaxItem;
            }
            public static Dictionary<string, int> getDicTaxSystem()
            {
                Dictionary<string, int> TaxSystem = new Dictionary<string, int>();
                TaxSystem.Add("Основная", 1);
                TaxSystem.Add("УСН доход", 2);
                TaxSystem.Add("УСН доход-расход", 4);
                TaxSystem.Add("ЕНВД", 8);
                TaxSystem.Add("ЕСХН", 16);
                TaxSystem.Add("Патент", 32);
                return TaxSystem;
            }
            public static Dictionary<string, int> getDicPaimentItemSign()
            {
                Dictionary<string, int> PaimentItemSign = new Dictionary<string, int>();
                PaimentItemSign.Add("Товар", 1);
                PaimentItemSign.Add("Подакцизный товар", 2);
                PaimentItemSign.Add("Работа", 3);
                PaimentItemSign.Add("Услуга", 4);
                PaimentItemSign.Add("Ставка азартной игры", 5);
                PaimentItemSign.Add("Выигрыш азартной игры", 6);
                PaimentItemSign.Add("Лотерейный билет", 7);
                PaimentItemSign.Add("Выигрыш лотереи", 8);
                PaimentItemSign.Add("Предоставление РИД", 9);
                PaimentItemSign.Add("Платеж", 10);
                PaimentItemSign.Add("Агентское вознаграждение", 11);
                PaimentItemSign.Add("Составной предмет расчета", 12);
                PaimentItemSign.Add("Иной предмет расчета", 13);
                PaimentItemSign.Add("Имущественное право", 14);
                PaimentItemSign.Add("Внереализационный доход", 15);
                PaimentItemSign.Add("Страховые взносы", 16);
                PaimentItemSign.Add("Торговый сбор", 17);
                PaimentItemSign.Add("Курортный сбор", 18);
                return PaimentItemSign;
            }
            public static Dictionary<string, int> getDicCheckType()
            {
                Dictionary<string, int> CheckType = new Dictionary<string, int>();
                CheckType.Add("Приход", 1);
                CheckType.Add("Возврат прихода", 2);
                CheckType.Add("Расход", 3);
                CheckType.Add("Возврат расхода", 4);
                return CheckType;
            }
            private int EnterItems(string Item) //Проверка выбираемых значений
            {
                Dictionary<string, int> CheckType = new Dictionary<string, int>();
                CheckType.Add("Приход", 1);
                CheckType.Add("Возврат прихода", 2);
                CheckType.Add("Расход", 3);
                CheckType.Add("Возврат расхода", 4);
                return CheckType[Item];
            }
        }
        
    }
}
