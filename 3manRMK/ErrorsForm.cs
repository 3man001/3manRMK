using LibraryDotNetFramework;
using System;
using System.Windows.Forms;

namespace _3manRMK
{
    public partial class ErrorsForm : Form
    {
        int ResultCode;
        string GlobResultCodeDesc;
        int Mode;
        int ECRMode8Status;
        int ECRModeStatus;
        int ECRAdvancedMode;
        public ErrorsForm(int ResultCodeIn, string ResultCodeDesc, int ModeIn, string ModeIndesc, int ECRMode8StatusIn, int ECRModeStatusIn, int ECRAdvancedModeIn, string ECRAdvancedModeDescription)
        {
            InitializeComponent();
            ResultCode = ResultCodeIn; //Код ошибки ниже его расшифровка
            GlobResultCodeDesc = ResultCodeDesc;
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
            else if (ResultCode == -8)
            {
                return "Нет связи с ККТ проверьте его питание и подключение к ПК,\n" +
                    "либо нажмите Сервис => подключить ФР";
            }
            else if (ResultCode == 115)
            {
                if (Mode == 3)
                {
                    return "В кассовом аппарате смена первысили 24 часа, для перехода в рабочий режим её нужно закрыть";
                }
                else if (Mode == 4)
                {
                    return "В кассовом аппарате закрыта смена, для перехода в рабочий режим её нужно открыть.";
                }
                else
                    return MessageBack;
            }
            else if (ResultCode == 6666)
            {
                return "ID программы не совпадает с ИНН организации в ФН ККТ.\n" +
                        "Для продолжения работы Запустите программу с ID = " + GlobResultCodeDesc + ".\n" +
                        "Либо обратитесь в СТП по e-mail: igor@3man001.ru тел.+7(999)999-99-99";
            }
            else
                return MessageBack;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Undefiend.SendMail("Ошибка работы", LAllCode.Text);
            Close();
        }
    }
}