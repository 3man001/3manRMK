using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _3manRMK_0
{
    public partial class ErrorsForm : Form
    {
        int ResultCode;
        int Mode;
        int ECRMode8Status;
        int ECRModeStatus;
        int ECRAdvancedMode;
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
            ProcessError();
        }
        private void ProcessError()
        {
            string MessageBack = "Решения проблемы нет в базе, отправьте запрос в СТП приложите скрин. e-mail: igor@3man001.ru";

            if (ResultCode == 0)
            {
                LSD.Text = "Ошибок нет, все хорошо :)";
            }
            if (ResultCode == 115)
            {
                if (Mode == 4)
                {
                    LSD.Text = "В кассовом аппарате закрыта смена, для перехода в рабочий режим её нужно открыть.";
                }
                else
                { LSD.Text = MessageBack; }   
            }
            else
            { LSD.Text = MessageBack; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
