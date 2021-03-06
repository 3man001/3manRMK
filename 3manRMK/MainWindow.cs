﻿using DrvFRLib;
using LibraryDotNetFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _3manRMK
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            InitialArrays();
            InitialRMK();
            Drv = new DrvFR();
        }

        const string Id = "662400385900";
        const int NumberOfPosition = 1000;
        readonly DrvFR Drv;
        string fioCasher = "";
        string innCasher = "";
        int[] XY;
        CheckBox[] arrayCheckBox = new CheckBox [] { };
        ComboBox[] arrayPaymentItemSign = new ComboBox[] { };
        TextBox[] arrayNameProduct = new TextBox[] { };
        TextBox[] arrayPrice = new TextBox[] { };
        TextBox[] arrayQuantity = new TextBox[] { };
        ComboBox[] arrayTax = new ComboBox[] { };
        TextBox[] arraySumm = new TextBox[] { };

        ////////////Блок Функций/////////////
        public decimal ToDecimal(string s)
        { return Convert.ToDecimal(s); }
        private void InitialRMK()
        {
            Size = new Size(878, 300);
            Text = AboutBox1.AssemblyProduct + String.Format(" v {0}", AboutBox1.AssemblyVersion) + " For id = " + Id;
            groupBox3.Location = groupBox4.Location = new Point(0, 40);
            tbFIO.Text = Properties.Settings.Default.userFIO;
            tbINN.Text = Properties.Settings.Default.userINN;

            arrayCheckBox[0] = checkBox2;
            arrayPaymentItemSign[0] = cbPaymentItemSign_1;
            arrayPaymentItemSign[0].Items.Clear();
            Undefiend.ConvertStringToItems(Properties.Settings.Default.paymentItemsSign, arrayPaymentItemSign[0].Items);
            arrayPaymentItemSign[0].SelectedIndex = Properties.Settings.Default.paymentItemSignDefault;
            arrayNameProduct[0] = tbNameProduct_1;
            arrayPrice[0] = tbPrice_1;
            arrayQuantity[0] = tbQuantity_1;
            arrayTax[0] = cbTax1_1;
            arrayTax[0].SelectedIndex = 0;
            arraySumm[0] = tbSumm1_1;
            XY = new int[] {arrayCheckBox[0].Location.X, arrayPaymentItemSign[0].Location.X, arrayNameProduct[0].Location.X,
                            arrayPrice[0].Location.X, arrayQuantity[0].Location.X, arrayTax[0].Location.X, arraySumm[0].Location.X};
        }
        private void InitialArrays()
        {
            for (int i = 1; i < arrayCheckBox.Length; i++)
            {
                arrayCheckBox[i].Dispose();
                arrayPaymentItemSign[i].Dispose();
                arrayNameProduct[i].Dispose();
                arrayPrice[i].Dispose();
                arrayQuantity[i].Dispose();
                arrayTax[i].Dispose();
                arraySumm[i].Dispose();
            }
            Array.Resize(ref arrayCheckBox, 1);
            Array.Resize(ref arrayPaymentItemSign, 1);
            Array.Resize(ref arrayNameProduct, 1);
            Array.Resize(ref arrayPrice, 1);
            Array.Resize(ref arrayQuantity, 1);
            Array.Resize(ref arrayTax, 1);
            Array.Resize(ref arraySumm, 1);
            try
            {
                arrayCheckBox[0].Checked = true;
                arrayPaymentItemSign[0].SelectedIndex = 0;
                arrayNameProduct[0].Text = "";
                arrayPrice[0].Text = "";
                arrayQuantity[0].Text = "";
                arrayTax[0].SelectedIndex = 0;
                arraySumm[0].Text = "0,00";
            }
            catch
            { }

            bAdd.Location = new Point(6, 116);
            bAdd.Visible = true;
            panel2.Size = new Size(860, 180);
        }
        private bool CheckId()
        {
            if (ShtrihKKT.GetINN(Drv) == Id)
                return true;
            else
                return false;
        }
        private bool UpdateResult() //Проверка состояния ККТ
        {
            int ResultCode = Drv.ResultCode;
            string ResultCodeDesc = Drv.ResultCodeDescription;

            toolStripStatusLabel1.Text = string.Format("Результат: {0}, {1}", ResultCode, ResultCodeDesc);
            if (ResultCode != 0)
            {
                Drv.GetShortECRStatus();
                ErrorsForm ErorrsformP =
                    new ErrorsForm(ResultCode, ResultCodeDesc, Drv.ECRMode, Drv.ECRModeDescription, Drv.ECRMode8Status, Drv.ECRModeStatus, Drv.ECRAdvancedMode, Drv.ECRAdvancedModeDescription);
                ErorrsformP.ShowDialog(this);
                ErorrsformP.Dispose();
                return false;
            }
            else
                return true;
        }
        private void KKT_StatusCheck() //проверяет статус ОФД и ФН приотткрытии и закрытии смены
        {
            if (UpdateResult())
            {
                GetCashReg();
                DateTime dateTimePC = DateTime.Today;
                Drv.FNGetStatus(); //Запрос Статуса ФН
                string FNWarningFlags = Convert.ToString(Drv.FNWarningFlags, 2); //ФНФлагиПредупреждения
                FNWarningFlags = new string('0', 4 - FNWarningFlags.Length) + FNWarningFlags;
                toolStripStatus_FN.BackColor = WorkWithDKKT.CheckFNStatusInColor(FNWarningFlags);

                Drv.FNGetInfoExchangeStatus(); //Статус обмена с ОФД
                string ExchangeStatus = Convert.ToString(Drv.InfoExchangeStatus, 2); //СтатусИнфОбмена
                ExchangeStatus = new string('0', 5 - ExchangeStatus.Length) + ExchangeStatus;
                toolStripStatus_OFD.BackColor = WorkWithDKKT.CheckOFDStatusInColor(ExchangeStatus, dateTimePC, Drv.Date);

                Drv.GetECRStatus(); //ПолучитьСостояниеККМ
                DateTime DateTime_KKT = DateTime.Parse(Drv.Date.Day + "." + Drv.Date.Month + "." + Drv.Date.Year + " "
                    + Drv.Time.Hour + ":" + Drv.Time.Minute + ":" + Drv.Time.Second); //Внутренняя дата время ККМ
                toolStripStatus_TimeKKT.BackColor = WorkWithDKKT.CheckTheTimeDiffereceInColor(DateTime.Now, DateTime_KKT);

                cB_FN_TaxType.Items.Clear();
                Undefiend.ConvertStringToItems(ShtrihKKT.GetTaxType(Drv), cB_FN_TaxType.Items);
            }
        }
        private void GetCashReg() //Запрашивает сумму наличности из ККТ
        {
            decimal cash = ShtrihKKT.GetCashReg(Drv);
            label18.Text = "Сумма в денежном ящике = " + cash + " ₽";
            toolStripStatusLabel3.Text = "ДЯ = " + cash + " ₽";
        }
        private int EnterItems(string Item) //Проверка выбираемых значений
        {
            Dictionary<string, int> Items = new Dictionary<string, int>();
            Items.Add("Товар", 1);
            Items.Add("Работа", 3);
            Items.Add("Услуга", 4);

            Items.Add("НДС 20%", 1);
            Items.Add("НДС 10%", 2);
            Items.Add("НДС 0%", 3);
            Items.Add("Без НДС", 4);
            Items.Add("НДС 20/120", 5);
            Items.Add("НДС 10/110", 6);

            Items.Add("Приход", 1);
            Items.Add("Возврат прихода", 2);
            Items.Add("Расход", 3);
            Items.Add("Возврат расхода", 4);

            Items.Add("Основная", 1);
            Items.Add("УСН доход", 2);
            Items.Add("УСН доход-расход", 4);
            Items.Add("ЕНВД", 8);
            Items.Add("ЕСХН", 16);
            Items.Add("Патент", 32);
            return Items[Item];
        }
        private void ChangePoz(int index, bool stateCheckBox)
        {
            Font newFont;
            if (stateCheckBox)
                newFont = new Font(arrayPaymentItemSign[index].Font, FontStyle.Regular);
            else
                newFont = new Font(arrayPaymentItemSign[index].Font, FontStyle.Strikeout);
            arrayPaymentItemSign[index].Font = 
                arrayNameProduct[index].Font = 
                arrayPrice[index].Font = 
                arrayQuantity[index].Font = 
                arrayTax[index].Font = 
                arraySumm[index].Font = newFont;
        }
        private void ActCashTextBoxs(bool cash, bool electron)
        {
            tbSumm1.Text = "0,00";
            tbSumm1.Visible = label6.Visible = cash;
            tbSumm2.Text = "0,00";
            tbSumm2.Visible = label7.Visible = electron;
        }
        //////Начало Блока триггер виджета/////////
        private void MainWindowSizeChanged(object sender, EventArgs e)
        {
            //Width = 878 без полсы прокрутки
            int width = Size.Width;
            int height = Size.Height;
            if (width >= 800)
            {
                int newX = 0;
                int newWidth = 880;
                if (width > 878)
                    newX =  (width - 878) / 2;
                else
                    newWidth = width - 18;
                panel0.Location = new Point(newX, 25);
                panel0.Size = new Size(newWidth, height - 90);
                if (bAdd.Location.Y+64 > height - 130)
                    panel2.Size = new Size(860, bAdd.Location.Y + 64);
                else
                    panel2.Size = new Size(860, height - 130);
            }
        }
        private void TextBoxFIO_TextChanged(object sender, EventArgs e)
        {
            if (CheckString.FullName(tbFIO.Text))
                tbFIO.BackColor = Color.Snow;
            else
                tbFIO.BackColor = Color.LightCoral;
        }
        private void TextBoxINN_TextChanged(object sender, EventArgs e)
        {
            tbINN.BackColor = 
                CheckString.GetColorAfterCheckString(tbINN.Text, CheckString.TaxpayerIdentificationNumber(tbINN.Text));
        }
        private void CBox_ChekedChanged(object sender, EventArgs e) //Проверка Чекбоксов
        {
            int State = 0;
            for (int i = 0; i < arrayCheckBox.Length; i++)
            {
                if (arrayCheckBox[i].Checked)
                {
                    State++;
                    ChangePoz(i, true);
                }
                else
                {
                    ChangePoz(i, false);
                }
            }
            if (State == 0)
                checkBox1.CheckState = CheckState.Unchecked;
            else if(State == arrayCheckBox.Length)
                checkBox1.CheckState = CheckState.Checked;
            else
                checkBox1.CheckState = CheckState.Indeterminate;
        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool Check = checkBox1.Checked;
            if (checkBox1.CheckState != CheckState.Indeterminate)
            {
                for (int i = 0; i < arrayCheckBox.Length; i++)
                { arrayCheckBox[i].Checked = Check; }
            }
        }
        private void TextBoxPrice_TextChanged(object sender, EventArgs e) //Изменение строки с ценой
        {
            int error = 0;
            label3.BackColor = SystemColors.InactiveCaption;
            for (int i=0; i<arrayCheckBox.Length; i++)
            {
                if (arrayCheckBox[i].Checked)
                {
                    if (CheckString.Numbers(arrayPrice[i].Text))
                    {
                        arrayPrice[i].BackColor = Color.Snow;
                        arrayPrice[i].Text = Convert.ToString(Math.Round(ToDecimal(arrayPrice[i].Text) * 1.000m, 2));
                        arraySumm[i].Text = "0,00";
                        if (arrayQuantity[i].BackColor != Color.LightCoral)
                        {
                            try
                            {
                                arraySumm[i].Text = Convert.ToString(Math.Round(ToDecimal(arrayPrice[i].Text) * ToDecimal(arrayQuantity[i].Text), 2));
                            }
                            catch
                            {
                                arrayQuantity[i].BackColor = Color.LightCoral;
                            }
                        }
                    }
                    else
                    {
                        error++;
                        arrayPrice[i].BackColor = Color.LightCoral;
                    }
                }
            }
            if (error > 0)
                label3.BackColor = Color.LightCoral;
        }
        private void TextBoxQuantity_TextChanged(object sender, EventArgs e) //Измениение строки с кол-вом
        {
            int error = 0;
            label5.BackColor = SystemColors.InactiveCaption;
            for (int i = 0; i < arrayCheckBox.Length; i++)
            {
                if (arrayCheckBox[i].Checked)
                {
                    if (CheckString.Numbers(arrayQuantity[i].Text))
                    {
                        arrayQuantity[i].BackColor = Color.Snow;
                        arrayQuantity[i].Text = Convert.ToString(Math.Round(ToDecimal(arrayQuantity[i].Text) * 1.0000m, 3));
                        arraySumm[i].Text = "0,00";
                        if (arrayPrice[i].BackColor != Color.LightCoral)
                        {
                            try
                            {
                                arraySumm[i].Text = Convert.ToString(Math.Round(ToDecimal(arrayPrice[i].Text) * ToDecimal(arrayQuantity[i].Text), 2));
                            }
                            catch
                            {
                                arrayPrice[i].BackColor = Color.LightCoral;
                            }
                        }
                    }
                    else
                    {
                        error++;
                        arrayQuantity[i].BackColor = Color.LightCoral;
                    }
                }
            }
            if (error > 0)
                label5.BackColor = Color.LightCoral;
        }
        private void TextBoxSumm_TextChanged(object sender, EventArgs e) //Сумма товара
        {
            decimal S = 0.00m;
            if ((label3.BackColor != Color.LightCoral)&&(label5.BackColor != Color.LightCoral))
            {
                for (int i = 0; i < arraySumm.Length; i++)
                {
                    if (arrayCheckBox[i].Checked)
                        S += ToDecimal(arraySumm[i].Text);
                }
                tbSummAll.Text = Convert.ToString(S);
            }
            else
                tbSummAll.Text = "Error";
        }
        private void TextBoxSumm1_TextChanged(object sender, EventArgs e)
        {
            
            if (CheckString.Numbers(tbSumm1.Text))
            {
                tbSumm1.BackColor = Color.Snow;
                tbSumm1.Text = Convert.ToString(Math.Round(ToDecimal(tbSumm1.Text)*1.000m,2));
                if (tbSumm2.BackColor != Color.LightCoral)
                {
                    tbChange.Text = Convert.ToString((ToDecimal(tbSummAll.Text) - ToDecimal(tbSumm2.Text) - ToDecimal(tbSumm1.Text)) * -1);
                    tbChange.Visible = true;
                }
            }
            else
            {
                tbSumm1.BackColor = Color.LightCoral;
                tbChange.Visible = button4.Visible = false;
            }
        }
        private void TextBoxSumm2_TextChanged(object sender, EventArgs e) //Контроль ввода безналичной оплаты
        {
            if (CheckString.Numbers(tbSumm2.Text))
            {
                if (ToDecimal(tbSumm2.Text) > ToDecimal(tbSummAll.Text))
                {
                    tbSumm2.BackColor = Color.LightCoral;
                    tbChange.Visible = button4.Visible = false;
                }
                else
                {
                    tbSumm2.BackColor = Color.Snow;
                    tbSumm2.Text = Math.Round(ToDecimal(tbSumm2.Text) * 1.000m, 2).ToString();
                    if (tbSumm1.BackColor != Color.LightCoral)
                    {
                        tbChange.Text = Convert.ToString((ToDecimal(tbSummAll.Text) - ToDecimal(tbSumm2.Text) - ToDecimal(tbSumm1.Text)) * -1);
                        tbChange.Visible = true;
                    }
                }
            }
            else
            {
                tbSumm2.BackColor = Color.LightCoral;
                tbChange.Visible = button4.Visible = false;
            }
        }
        private void TextBoxChange_TextChanged(object sender, EventArgs e) //Расчет суммы сдачи
        {
            if (ToDecimal(tbChange.Text) >= 0)
                button4.Visible = true;
            else
                button4.Visible = false;
            if (!tbChange.Visible)
                button4.Visible = false;
        }
        private void MaskTBPhone_MaskInputRejected(object sender, EventArgs e) //Проверка тел. на корректность
        {
            if (CheckString.Phone(maskTBPhone.Text))
                maskTBPhone.BackColor = Color.LightGreen;
            else if (maskTBPhone.Text.Replace("+7(", "").Replace(")", "").Replace("-", "").Replace(" ", "") == "")
                maskTBPhone.BackColor = Color.Snow;
            else
                maskTBPhone.BackColor = Color.LightCoral;
        }
        private void TextBoxEmail_TextChanged(object sender, EventArgs e)
        {
            tbEmail.BackColor = CheckString.GetColorAfterCheckString(tbEmail.Text, CheckString.Email(tbEmail.Text));
        }
        private void TextBoxCustomerINN_TextChanged(object sender, EventArgs e)
        {
            tbCustomerINN.BackColor = CheckString.GetColorAfterCheckString(tbCustomerINN.Text, CheckString.TaxpayerIdentificationNumber(tbCustomerINN.Text));
        }
        private void TextBoxCustomer_TextChanged(object sender, EventArgs e)
        {
            tbCustomer.BackColor =  CheckString.GetColorAfterCheckString(tbCustomer.Text, CheckString.Buyer(tbCustomer.Text));
        }
        private void ComBox_FN_TaxType_TextChanged(object sender, EventArgs e)
        {
            if (cB_FN_TaxType.Text != "")
                label1.BackColor = Color.LightGreen;
            else
                label1.BackColor = Color.LightCoral;
        }
        private void TextBoxCash_In_Outcome_TextChanged(object sender, EventArgs e)
        {
            if (CheckString.Numbers(tbCash_In_Outcome.Text))
            {
                tbCash_In_Outcome.BackColor = Color.Snow;
                tbCash_In_Outcome.Text = Convert.ToString(Math.Round(ToDecimal(tbCash_In_Outcome.Text) * 1.000m, 2));
                buttonCash_In_Outcome.Visible = true;
            }
            else
            {
                tbCash_In_Outcome.BackColor = Color.LightCoral;
                buttonCash_In_Outcome.Visible = false;
            }
        }
        private void ComBox_In_OutCash_TextChanged(object sender, EventArgs e)
        {
            buttonCash_In_Outcome.Text = cB_In_OutCash.Text;
        }
        ///////////Начало Блок МЕНЮ////////////////
        private void ПодключитьФРToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShtrihKKT.ConnectToKKT(Drv);
            UpdateResult();
        }
        private void ОткрытьСменуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShtrihKKT.OpenShift(Drv, fioCasher, innCasher);
            UpdateResult();
        }
        private void ЗакрытьСменуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShtrihKKT.CloseShift(Drv, fioCasher, innCasher);
            UpdateResult();
        }

        private void ПриходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelCheckType.Text = "Приход";
        }
        private void ВозвратПриходаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelCheckType.Text = "Возврат прихода";
        }
        private void РасходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelCheckType.Text = "Расход";
        }
        private void ВозвратРасходаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelCheckType.Text = "Возврат расхода";
        }

        private void ОтменаЧекаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShtrihKKT.CancelCashReciept(Drv);
            UpdateResult();
        }
        private void XотчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShtrihKKT.TakeDalyReport(Drv);
            UpdateResult();
        }
        private void ВнесениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.Lime;
            btnLogin.Text = "Login";
            tbFIO.ReadOnly = tbINN.ReadOnly = label13.Visible = tbSummAll.Visible =
                groupBox3.Visible = panel2.Visible = false;

            groupBox4.Visible = true;
            GetCashReg();
        }
        private void НастройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsWindow Setting1 = new SettingsWindow();
            Setting1.ShowDialog(this);
            Setting1.Dispose();
        }
        private void ОбратнаяСвязьToolStripMenuItem_Click(object sender, EventArgs e) //Открыть формк обратной связи
        {
            FeedbackWindows Feedback1 = new FeedbackWindows();
            Feedback1.ShowDialog(this);
            Feedback1.Dispose();
        }
        private void ОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 AboutBox = new AboutBox1();
            AboutBox.ShowDialog(this);
            AboutBox.Dispose();
        }
        ////////////Конец Блок МЕНЮ////////////////
        private void Button4_Click(object sender, EventArgs e) //Продажа товара
        {
            if (ToDecimal(tbSumm2.Text) > ToDecimal(tbSummAll.Text))
                tbSumm2.BackColor = Color.LightCoral;
            else
            {
                try
                {
                    Drv.Connect();
                    Drv.GetShortECRStatus();
                    if (Drv.ECRMode == 4)
                        ОткрытьСменуToolStripMenuItem_Click(sender, e);
                }
                catch
                { UpdateResult(); } //Если смена закрыта то Открыть как положено

                groupBox3.Visible = false;

                int CheckType = EnterItems(labelCheckType.Text);
                int PaymentItemSign_i;
                string NameProduct_i;
                decimal Price_i;
                double Quantity_i;
                int Tax1_i;
                decimal Summ1_i;
                for (int i=0; i<arrayCheckBox.Length; i++) //Регистрация позиций в чеке
                {
                    if (arrayCheckBox[i].Checked)
                    {
                        PaymentItemSign_i = EnterItems(arrayPaymentItemSign[i].Text);
                        NameProduct_i = arrayNameProduct[i].Text;
                        Price_i = ToDecimal(arrayPrice[i].Text);
                        Quantity_i = Convert.ToDouble(arrayQuantity[i].Text);
                        Summ1_i = ToDecimal(arraySumm[i].Text);
                        Tax1_i = EnterItems(arrayTax[i].Text);
                        if (!ShtrihKKT.RegPosition(Drv, CheckType, PaymentItemSign_i, NameProduct_i, Price_i, Quantity_i, Tax1_i, Summ1_i))
                            break;
                    }
                }
                if (Drv.ResultCode == 0) //Если позиции пробитилсь то идем дальше
                {
                    ShtrihKKT.SendFIO(Drv, fioCasher, innCasher);

                    if (maskTBPhone.BackColor == Color.LightGreen)
                        ShtrihKKT.SendCustomerPhoneOrEmail(Drv, maskTBPhone.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""));
                    else if (tbEmail.BackColor == Color.LightGreen)
                        ShtrihKKT.SendCustomerPhoneOrEmail(Drv, tbEmail.Text);
                    
                    if (tbCustomer.BackColor == Color.LightGreen && tbCustomerINN.BackColor == Color.LightGreen)
                        ShtrihKKT.SendCustomer(Drv, tbCustomer.Text, tbCustomerINN.Text);

                    decimal cashPayment = ToDecimal(tbSumm1.Text);
                    decimal electronicPayment = ToDecimal(tbSumm2.Text);
                    int TaxType = EnterItems(cB_FN_TaxType.Text);
                    if (ShtrihKKT.CloseChek(Drv, cashPayment, electronicPayment, TaxType))
                    {
                        tbSumm1.Text = "0,00";
                        tbSumm2.Text = "0,00";
                        maskTBPhone.Text = "";
                        tbEmail.Text = "";
                        tbCustomer.Text = "";
                        tbCustomerINN.Text = "";
                        InitialArrays();
                        labelCheckType.Text = "Приход";
                        tbSumm1.Visible = false;
                        tbSumm2.Visible = false;
                        GetCashReg();
                    }
                    else
                        UpdateResult();
                }
                else
                    UpdateResult();
                panel2.Visible = true;
            }
        }
        private void BtnLogin_Click(object sender, EventArgs e) //Регистрация кассира
        {
            if (!CheckId())
            {
                Drv.GetShortECRStatus(); //Запрос состояния ФР
                ErrorsForm ErorrsformP = new ErrorsForm(6666, Drv.INN, Drv.ECRMode, Drv.ECRModeDescription, Drv.ECRMode8Status, Drv.ECRModeStatus, Drv.ECRAdvancedMode, Drv.ECRAdvancedModeDescription);
                ErorrsformP.ShowDialog(this);
                ErorrsformP.Dispose();
                return;
            }
            if (btnLogin.BackColor == Color.Lime )
            {
                TextBoxFIO_TextChanged(sender, e);
                TextBoxINN_TextChanged(sender, e);
                if ((tbFIO.BackColor == Color.Snow) && (tbINN.BackColor != Color.LightCoral))
                {
                    Properties.Settings.Default.userFIO = tbFIO.Text;
                    Properties.Settings.Default.userINN = tbINN.Text;
                    Properties.Settings.Default.Save();
                    KKT_StatusCheck();
                    btnLogin.BackColor = Color.DodgerBlue;
                    btnLogin.Text = "LogOut";
                    tbFIO.ReadOnly = tbINN.ReadOnly = label13.Visible = tbSummAll.Visible = panel2.Visible = true;
                    groupBox4.Visible = false;
                }
            }
            else
            {
                btnLogin.BackColor = Color.Lime;
                btnLogin.Text = "Login";
                tbFIO.ReadOnly = tbINN.ReadOnly = label13.Visible = tbSummAll.Visible = 
                    groupBox3.Visible = groupBox4.Visible = panel2.Visible = false;
            }
            if (tbFIO.ReadOnly)
            {
                fioCasher = tbFIO.Text;
                innCasher = tbINN.Text;
            }
            else
                fioCasher = innCasher = "";
        }
        private void Button1_Click(object sender, EventArgs e) //Оплата Наличными
        {
            ActCashTextBoxs(true, false);
        }
        private void Button2_Click(object sender, EventArgs e) //Оплат безналичными
        {
            ActCashTextBoxs(false, true);
        }
        private void Button3_Click(object sender, EventArgs e) //Смешанная оплата
        {
            ActCashTextBoxs(true, true);
        }
        private void Button11_Click(object sender, EventArgs e) //Перейти к оплате
        {
            TextBoxPrice_TextChanged(sender, e);
            TextBoxQuantity_TextChanged(sender, e);
            TextBoxSumm_TextChanged(sender, e);
            ComBox_FN_TaxType_TextChanged(sender, e);
            if (tbSummAll.Text != "Error" && cB_FN_TaxType.Text != "")
            {
                groupBox3.Visible = true;
                panel2.Visible = false;
            }
        }
        private void Button12_Click(object sender, EventArgs e) //Перейти к позициям
        {
            groupBox3.Visible = false;
            panel2.Visible = true;
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            int Poz = arrayCheckBox.Length;
            if (Poz >= NumberOfPosition)
                bAdd.Visible = false;
            int Y = bAdd.Location.Y;
            Array.Resize(ref arrayCheckBox, Poz + 1);
            Array.Resize(ref arrayPaymentItemSign, Poz + 1);
            Array.Resize(ref arrayNameProduct, Poz + 1);
            Array.Resize(ref arrayPrice, Poz + 1);
            Array.Resize(ref arrayQuantity, Poz + 1);
            Array.Resize(ref arrayTax, Poz + 1);
            Array.Resize(ref arraySumm, Poz + 1);
            arrayCheckBox[Poz] = new CheckBox {Size = arrayCheckBox[0].Size, Text = Convert.ToString(Poz + 1) + '.', 
                                      Checked = true, 
                                      Location = new Point(XY[0], Y) };
            arrayCheckBox[Poz].CheckedChanged += new EventHandler(CBox_ChekedChanged);
            arrayPaymentItemSign[Poz] = new ComboBox { Size = arrayPaymentItemSign[0].Size,
                Location = new Point(XY[1], Y),
                DropDownStyle = ComboBoxStyle.DropDownList};
                Undefiend.CopyToFromObjectCollection(arrayPaymentItemSign[0].Items, arrayPaymentItemSign[Poz].Items);
                arrayPaymentItemSign[Poz].SelectedIndex = arrayPaymentItemSign[0].SelectedIndex;
            arrayNameProduct[Poz] = new TextBox {Size = arrayNameProduct[0].Size,
                                            Location = new Point(XY[2], Y),
                                            Text = ""};
            arrayPrice[Poz] = new TextBox {Size = arrayPrice[0].Size,
                                      Location = new Point(XY[3], Y),
                                      Text = ""};
            arrayPrice[Poz].TextChanged += new EventHandler(TextBoxPrice_TextChanged);
            arrayQuantity[Poz] = new TextBox {Size = arrayQuantity[0].Size,
                                        Location = new Point(XY[4], Y),
                                        Text = ""};
            arrayQuantity[Poz].TextChanged += new EventHandler(TextBoxQuantity_TextChanged);
            arrayTax[Poz] = new ComboBox {Size = arrayTax[0].Size,
                                    Location = new Point(XY[5], Y),
                                    DropDownStyle = ComboBoxStyle.DropDownList};
                Undefiend.CopyToFromObjectCollection(arrayTax[0].Items, arrayTax[Poz].Items);
                arrayTax[Poz].SelectedIndex = arrayTax[0].SelectedIndex;
            arraySumm[Poz] = new TextBox {Size = arraySumm[0].Size,
                                    Location = new Point(XY[6], Y),
                                    ReadOnly = true,
                                    Text = "0,00"};
            arraySumm[Poz].TextChanged += new EventHandler(TextBoxSumm_TextChanged);

            SuspendLayout();
            panel2.Controls.Add(arrayCheckBox[Poz]);
            panel2.Controls.Add(arrayPaymentItemSign[Poz]);
            panel2.Controls.Add(arrayNameProduct[Poz]);
            panel2.Controls.Add(arrayPrice[Poz]);
            panel2.Controls.Add(arrayQuantity[Poz]);
            panel2.Controls.Add(arrayTax[Poz]);
            panel2.Controls.Add(arraySumm[Poz]);
            ResumeLayout(false);
            PerformLayout();
            bAdd.Location = new Point(bAdd.Location.X, bAdd.Location.Y+30);
            if (bAdd.Location.Y + 64 > panel2.Size.Height)
                panel2.Size = new Size(panel2.Size.Width, panel2.Size.Height + 30);
        }
        private void ButtonCash_In_Outcome_Click(object sender, EventArgs e)
        {
            decimal cash = ToDecimal(tbCash_In_Outcome.Text);
            string operation = cB_In_OutCash.Text;
            if (ShtrihKKT.CashInOutCome(Drv, operation, cash))
            {
                cB_In_OutCash.BackColor = Color.Snow;
                tbCash_In_Outcome.Text = cB_In_OutCash.Text = "";
                GetCashReg();
            }
            else
                cB_In_OutCash.BackColor = Color.LightCoral;
        }
    }
}