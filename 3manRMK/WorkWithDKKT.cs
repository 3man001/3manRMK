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
    }
}
