using DrvFRLib; //Библиотека ШТРИХ-М подключение
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;  //Подключение библиотек

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
        DrvFR Drv;
        int[] XY;
        CheckBox[] arrayCheckBox = new CheckBox [] { };
        ComboBox[] arrayPaymentItemSign = new ComboBox[] { };
        TextBox[] arrayNameProduct = new TextBox[] { };
        TextBox[] arrayPrice = new TextBox[] { };
        TextBox[] arrayQuantity = new TextBox[] { };
        ComboBox[] arrayTax = new ComboBox[] { };
        TextBox[] arraySumm = new TextBox[] { };

        ////////////Блок Функций/////////////
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
            MainMethods.Setting.ConvertStringToItems(Properties.Settings.Default.paymentItemsSign, arrayPaymentItemSign[0].Items);
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
            Drv.FNGetFiscalizationResult();
            if (Drv.INN == Id)
                return true;
            else
                return false;
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

                Drv.FNGetFiscalizationResult(); //Полусить итоги фискализации
                string FN_TaxType = Convert.ToString(Drv.TaxType, 2); //Получить Ситемы налогообложения
                FN_TaxType = new string('0', 6 - FN_TaxType.Length) + FN_TaxType;
                //FN_TaxType = "111111";
                cB_FN_TaxType.Items.Clear();
                if (FN_TaxType[5] == '1')
                    cB_FN_TaxType.Items.Add("Основная");
                if (FN_TaxType[4] == '1')
                    cB_FN_TaxType.Items.Add("УСН доход");
                if (FN_TaxType[3] == '1')
                    cB_FN_TaxType.Items.Add("УСН доход-расход");
                if (FN_TaxType[2] == '1')
                    cB_FN_TaxType.Items.Add("ЕНВД");
                if (FN_TaxType[1] == '1')
                    cB_FN_TaxType.Items.Add("ЕСХН");
                if (FN_TaxType[0] == '1')
                    cB_FN_TaxType.Items.Add("Патент");
            }
        }
        public decimal ToDecimal (string s)
        { return Convert.ToDecimal(s); }
        private bool UpdateResult() //Проверка состояния ККТ
        {
            int ResultCode = Drv.ResultCode;
            string ResultCodeDesc = Drv.ResultCodeDescription;

            toolStripStatusLabel1.Text = string.Format("Результат: {0}, {1}", ResultCode, ResultCodeDesc);
            if (ResultCode != 0)
            {
                Drv.GetShortECRStatus(); //Запрос состояния ФР
                ErrorsForm ErorrsformP = new ErrorsForm(ResultCode, ResultCodeDesc, Drv.ECRMode, Drv.ECRModeDescription, Drv.ECRMode8Status, Drv.ECRModeStatus, Drv.ECRAdvancedMode, Drv.ECRAdvancedModeDescription);
                ErorrsformP.ShowDialog(this);
                ErorrsformP.Dispose();
                return false;
            }
            else
            {
                return true;
            }
        }
        private void GetCashReg() //Запрашивает сумму наличности из ККТ
        {
            Drv.RegisterNumber = 241; //Накопление наличности в кассе.Вохможно другое значение у другой модели
            Drv.GetCashReg();
            label18.Text = "Сумма в денежном ящике = " + Drv.ContentsOfCashRegister + " ₽";
            toolStripStatusLabel3.Text = "ДЯ = " + Drv.ContentsOfCashRegister + " ₽";
        }
        private int EnterItems(string Item) //Проверка выбираемых значений
        {
            Dictionary<string, int> Items = new Dictionary<string, int>();
            Items.Add("Товар", 1);
            Items.Add("Работа", 3);
            Items.Add("Услуга", 4);

            //Items.Add("Без НДС", 0);
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
        private bool RegPosition(int CheckType, string NameProduct, Decimal Price, Double Quantity,
            Decimal Summ1, int Tax1, int PaymentItemSign) //Регистрация позиции в чеке
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
            { Drv.FNOperation(); } // Пробиваем позицию
            catch
            { return false; }
            return true;
        }
        private bool CloseChek(int TaxType = 2) // Формируем закрытие чека
        {
            Drv.Summ1 = ToDecimal(tbSumm1.Text); // Наличные
            Drv.Summ2 = ToDecimal(tbSumm2.Text); // Остальные типы оплаты нулевые, но их необходимо заполнить
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
            Drv.TaxType = TaxType; //1 - Основная система налогообложения
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
        private void SendFIO() //Указывает Зарегестрированного кассира в документах
        {
            if (tbFIO.ReadOnly)
            {
                Drv.TagNumber = 1021; //Отправка Должности и Фамилии кассира
                Drv.TagType = 7;
                Drv.TagValueStr = tbFIO.Text;
                Drv.FNSendTag();
                if (tbINN.BackColor == Color.LightGreen)
                {
                    Drv.TagNumber = 1203; //Отправка ИНН кассира
                    Drv.TagType = 7;
                    Drv.TagValueStr = tbINN.Text;
                    Drv.FNSendTag();
                }
            }
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
        private void tbFIO_TextChanged(object sender, EventArgs e)
        {
            if (MainMethods.CheckSimbols.FullName(tbFIO.Text))
                tbFIO.BackColor = Color.Snow;
            else
                tbFIO.BackColor = Color.LightCoral;
        }
        private void tbINN_TextChanged(object sender, EventArgs e)
        {
            tbINN.BackColor = MainMethods.CheckSimbols.GetColorAfterCheckINN(tbINN.Text);
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
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool Check = checkBox1.Checked;
            if (checkBox1.CheckState != CheckState.Indeterminate)
            {
                for (int i = 0; i < arrayCheckBox.Length; i++)
                { arrayCheckBox[i].Checked = Check; }
            }
        }
        private void tbPrice_TextChanged(object sender, EventArgs e) //Изменение строки с ценой
        {
            int error = 0;
            label3.BackColor = SystemColors.InactiveCaption;
            for (int i=0; i<arrayCheckBox.Length; i++)
            {
                if (arrayCheckBox[i].Checked)
                {
                    if (MainMethods.CheckSimbols.Numbers(arrayPrice[i].Text))
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
        private void tbQuantity_TextChanged(object sender, EventArgs e) //Измениение строки с кол-вом
        {
            int error = 0;
            label5.BackColor = SystemColors.InactiveCaption;
            for (int i = 0; i < arrayCheckBox.Length; i++)
            {
                if (arrayCheckBox[i].Checked)
                {
                    if (MainMethods.CheckSimbols.Numbers(arrayQuantity[i].Text))
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
        private void tbSumm_TextChanged(object sender, EventArgs e) //Сумма товара
        {
            decimal S = 0.00m;
            if ((label3.BackColor != Color.LightCoral)&&(label5.BackColor != Color.LightCoral))
            {
                for (int i = 0; i < arraySumm.Length; i++)
                {
                    if (arrayCheckBox[i].Checked)
                        S = S + ToDecimal(arraySumm[i].Text);
                }
                tbSummAll.Text = Convert.ToString(S);
            }
            else
                tbSummAll.Text = "Error";
        }
        private void tbSumm1_TextChanged(object sender, EventArgs e)
        {
            if (MainMethods.CheckSimbols.Numbers(tbSumm1.Text))
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
        private void tbSumm2_TextChanged(object sender, EventArgs e) //Контроль ввода безналичной оплаты
        {
            if (MainMethods.CheckSimbols.Numbers(tbSumm2.Text))
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
        private void tbChange_TextChanged(object sender, EventArgs e) //Расчет суммы сдачи
        {
            if (ToDecimal(tbChange.Text) >= 0)
                button4.Visible = true;
            else
                button4.Visible = false;
            if (!tbChange.Visible)
                button4.Visible = false;
        }
        private void maskTBPhone_MaskInputRejected(object sender, EventArgs e) //Проверка тел. на корректность
        {
            if (MainMethods.CheckSimbols.Phone(maskTBPhone.Text))
                maskTBPhone.BackColor = Color.LightGreen;
            else if (maskTBPhone.Text.Replace("+7(", "").Replace(")", "").Replace("-", "").Replace(" ", "") == "")
                maskTBPhone.BackColor = Color.Snow;
            else
                maskTBPhone.BackColor = Color.LightCoral;
        }
        private void tbEmail_TextChanged(object sender, EventArgs e) //Проверка Email на корректность
        {
            if (tbEmail.Text == "")
                tbEmail.BackColor = Color.Snow;
            else if (MainMethods.CheckSimbols.Email(tbEmail.Text))
                tbEmail.BackColor = Color.LightGreen;
            else
                tbEmail.BackColor = Color.LightCoral;
        }
        private void tbCustomerINN_TextChanged(object sender, EventArgs e)
        {
            tbCustomerINN.BackColor = MainMethods.CheckSimbols.GetColorAfterCheckINN(tbCustomerINN.Text);
        }
        private void tbCustomer_TextChanged(object sender, EventArgs e)
        {
            if (tbCustomer.Text == "")
                tbCustomer.BackColor = Color.Snow;
            else if (MainMethods.CheckSimbols.Buyer(tbCustomer.Text))
                tbCustomer.BackColor = Color.LightGreen;
            else
                tbCustomer.BackColor = Color.LightCoral;
        }
        ///////////Начало Блок МЕНЮ////////////////
        private void подключитьФРToolStripMenuItem_Click(object sender, EventArgs e) //Показать свойство оборуования
        {
            Drv.ShowProperties();
            UpdateResult();
        }
        private void открытьСменуToolStripMenuItem_Click(object sender, EventArgs e) //Открыть смену в ККТ
        {
            try
            {
                Drv.FNBeginOpenSession();
                SendFIO();
                Drv.FNOpenSession();
                System.Threading.Thread.Sleep(2000);
            }
            catch
            { UpdateResult(); }
            UpdateResult();
        }
        private void закрытьСменуToolStripMenuItem_Click(object sender, EventArgs e) //Снять Z-отчет(Закрыть смену на ФР)
        {
            try
            {
                Drv.FNBeginCloseSession();
                SendFIO();
                Drv.FNCloseSession();
                UpdateResult();
            }
            catch
            { UpdateResult(); }
        }
        private void приходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelCheckType.Text = "Приход";
        }
        private void возвратПриходаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelCheckType.Text = "Возврат прихода";
        }
        private void расходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelCheckType.Text = "Расход";
        }
        private void возвратРасходаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelCheckType.Text = "Возврат расхода";
        }
        private void отменаЧекаToolStripMenuItem_Click(object sender, EventArgs e) //Отмена чека в ККТ
        {
            try
            {
                Drv.CancelCheck(); //Отмена чека
                UpdateResult();
            }
            catch
            {
                UpdateResult();
            }
        }
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e) //Показать информацию о программе
        {
            AboutBox1 AboutBox = new AboutBox1();
            AboutBox.ShowDialog(this);
            AboutBox.Dispose();
        }
        ////////////Конец Блок МЕНЮ////////////////
        private void button4_Click(object sender, EventArgs e) //Продажа товара
        {
            if (ToDecimal(tbSumm2.Text) > ToDecimal(tbSummAll.Text)) //Если сумма безнала больше суммы чека то ошибка
            {
                tbSumm2.BackColor = Color.LightCoral;
            }
            else
            {
                int CheckType = EnterItems(labelCheckType.Text); //Операция приход((1 - Приход, 2 - Возврат прихода 3 - расход, 4 - возврат расхода)
                string NameProduct0; //Наименование товара
                Decimal Price0; //Цена за еденицу товара с учетом скидки
                Double Quantity0; //Кол-во (Диапазон 0,001 до 9.999.999,999)
                Decimal Summ10; //Сумма позиции
                int Tax10; //Налоговая ставка 0..6 (0-БезНДС)
                int PaymentItemSign0; // Признак предмета расчета 1..19 (1-Товар)

                try
                {
                    Drv.Connect();
                    Drv.GetShortECRStatus();
                    if (Drv.ECRMode == 4)
                        открытьСменуToolStripMenuItem_Click(sender, e);
                }
                catch
                { UpdateResult(); } //Если смена закрыта то Открыть как положено

                groupBox3.Visible = false;

                for (int i=0; i<arrayCheckBox.Length; i++) //Регистрация позиций в чеке
                {
                    if (arrayCheckBox[i].Checked)
                    {
                        NameProduct0 = arrayNameProduct[i].Text; //Наименование товара
                        Price0 = Math.Round(ToDecimal(arrayPrice[i].Text), 2); //Цена за еденицу товара с учетом скидки
                        Quantity0 = Math.Round(Convert.ToDouble(arrayQuantity[i].Text), 3); //Кол-во (Диапазон 0,001 до 9.999.999,999)
                        Summ10 = ToDecimal(arraySumm[i].Text); //Сумма позиции
                        Tax10 = EnterItems(arrayTax[i].Text); //Налоговая ставка 0..6 (0-БезНДС)
                        PaymentItemSign0 = EnterItems(arrayPaymentItemSign[i].Text); // Признак предмета расчета 1..19 (1-Товар)

                        if (!RegPosition(CheckType, NameProduct0, Price0, Quantity0, Summ10, Tax10, PaymentItemSign0)) //Регистрация позиции
                            break;
                    }
                }
                if (Drv.ResultCode == 0) //Если позиции пробитилсь то идем дальше
                {
                    SendFIO(); //Регистрация кассира в чеке

                    if (maskTBPhone.BackColor == Color.LightGreen) //Отправка чека СМС если есть номер
                    {
                        Drv.CustomerEmail = maskTBPhone.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                        Drv.FNSendCustomerEmail();
                    }
                    else if (tbEmail.BackColor == Color.LightGreen) //Отправка чека на Email если введен адрес
                    {
                        Drv.CustomerEmail = tbEmail.Text;
                        Drv.FNSendCustomerEmail();
                    }
                    
                    if (tbCustomer.BackColor == Color.LightGreen && tbCustomerINN.BackColor == Color.LightGreen) //Регистрация покупателя
                    {
                        Drv.TagNumber = 1227; //Отправка Должности и Фамилии кассира
                        Drv.TagType = 7;
                        Drv.TagValueStr = tbCustomer.Text;
                        Drv.FNSendTag();
                        Drv.TagNumber = 1228; //Отправка Должности и Фамилии кассира
                        Drv.TagType = 7;
                        Drv.TagValueStr = tbCustomerINN.Text;
                        if (Drv.TagValueStr.Length == 10)
                            Drv.TagValueStr = Drv.TagValueStr + "00";
                        Drv.FNSendTag();
                    }
            
                    if (CloseChek(EnterItems(cB_FN_TaxType.Text))) // Формирует закрытие чека нужной СНО
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
                {
                    UpdateResult();
                }
                panel2.Visible = true;
            }
        }
        private void btnLogin_Click(object sender, EventArgs e) //Регистрация кассира
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
                tbFIO_TextChanged(sender, e);
                tbINN_TextChanged(sender, e);
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
        }
        private void xотчетToolStripMenuItem_Click(object sender, EventArgs e) //Снять Х-Отчет
        {
            try
            {
                Drv.PrintReportWithoutCleaning(); //Отчет без гашения
                UpdateResult();
            }
            catch
            {
                UpdateResult();
            }
        }
        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsWindow Setting1 = new SettingsWindow();
            Setting1.ShowDialog(this);
            Setting1.Dispose();
        }
        private void обратнаяСвязьToolStripMenuItem_Click(object sender, EventArgs e) //Открыть формк обратной связи
        {
            FeedbackWindows Feedback1 = new FeedbackWindows();
            Feedback1.ShowDialog(this);
            Feedback1.Dispose();
        }
        private void button1_Click(object sender, EventArgs e) //Оплата Наличными
        {
            tbSumm1.Text = "0,00";
            tbSumm1.Visible = true;
            label6.Visible = true;
            tbSumm2.Text = "0,00";
            tbSumm2.Visible = false;
            label7.Visible = false;

        }
        private void button2_Click(object sender, EventArgs e) //Оплат безналичными
        {
            tbSumm1.Text = "0,00";
            tbSumm1.Visible = false;
            label6.Visible = false;
            tbSumm2.Text = "0,00";
            tbSumm2.Visible = true;
            label7.Visible = true;
        }
        private void button3_Click(object sender, EventArgs e) //Смешанная оплата
        {
            tbSumm1.Text = "0,00";
            tbSumm1.Visible = true;
            label6.Visible = true;
            tbSumm2.Text = "0,00";
            tbSumm2.Visible = true;
            label7.Visible = true;
        }
        private void button11_Click(object sender, EventArgs e) //Перейти к оплате
        {
            tbPrice_TextChanged(sender, e);
            tbQuantity_TextChanged(sender, e);
            tbSumm_TextChanged(sender, e);
            cB_FN_TaxType_TextChanged(sender, e);
            if (tbSummAll.Text != "Error" && cB_FN_TaxType.Text != "")
            {
                groupBox3.Visible = true;
                panel2.Visible = false;
            }
        }
        private void button12_Click(object sender, EventArgs e) //Перейти к позициям
        {
            groupBox3.Visible = false;
            panel2.Visible = true;
        }
        private void CopyToFromObjectCollection (ComboBox.ObjectCollection fromItems, ComboBox.ObjectCollection toItems)
        {
            toItems.Clear();
            for (int i = 0; i < fromItems.Count; i++)
                toItems.Add(fromItems[i]);
        }
        private void bAdd_Click(object sender, EventArgs e)
        {
            int Poz = arrayCheckBox.Length;
            if (Poz >= 50)
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
                CopyToFromObjectCollection(arrayPaymentItemSign[0].Items, arrayPaymentItemSign[Poz].Items);
                arrayPaymentItemSign[Poz].SelectedIndex = arrayPaymentItemSign[0].SelectedIndex;
            arrayNameProduct[Poz] = new TextBox {Size = arrayNameProduct[0].Size,
                                            Location = new Point(XY[2], Y),
                                            Text = ""};
            arrayPrice[Poz] = new TextBox {Size = arrayPrice[0].Size,
                                      Location = new Point(XY[3], Y),
                                      Text = ""};
            arrayPrice[Poz].TextChanged += new EventHandler(tbPrice_TextChanged);
            arrayQuantity[Poz] = new TextBox {Size = arrayQuantity[0].Size,
                                        Location = new Point(XY[4], Y),
                                        Text = ""};
            arrayQuantity[Poz].TextChanged += new EventHandler(tbQuantity_TextChanged);
            arrayTax[Poz] = new ComboBox {Size = arrayTax[0].Size,
                                    Location = new Point(XY[5], Y),
                                    DropDownStyle = ComboBoxStyle.DropDownList};
                CopyToFromObjectCollection(arrayTax[0].Items, arrayTax[Poz].Items);
                arrayTax[Poz].SelectedIndex = arrayTax[0].SelectedIndex;
            arraySumm[Poz] = new TextBox {Size = arraySumm[0].Size,
                                    Location = new Point(XY[6], Y),
                                    ReadOnly = true,
                                    Text = "0,00"};
            arraySumm[Poz].TextChanged += new EventHandler(tbSumm_TextChanged);

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
        private void cB_FN_TaxType_TextChanged(object sender, EventArgs e)
        {
            if (cB_FN_TaxType.Text != "")
                label1.BackColor = Color.LightGreen;
            else
                label1.BackColor = Color.LightCoral;
        }
        private void tbCash_In_Outcome_TextChanged(object sender, EventArgs e)
        {
            if (MainMethods.CheckSimbols.Numbers(tbCash_In_Outcome.Text))
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
        private void buttonCash_In_Outcome_Click(object sender, EventArgs e)
        {
            if (cB_In_OutCash.Text == "Внесение")
            {
                cB_In_OutCash.BackColor = Color.Snow;
                Drv.Summ1 = Math.Round(ToDecimal(tbCash_In_Outcome.Text) * 1.000m, 2);
                Drv.CashIncome();
                tbCash_In_Outcome.Text = cB_In_OutCash.Text = "";
                GetCashReg();
            }
            else if (cB_In_OutCash.Text == "Выплата")
            {
                cB_In_OutCash.BackColor = Color.Snow;
                Drv.Summ1 = Math.Round(ToDecimal(tbCash_In_Outcome.Text) * 1.000m, 2);
                Drv.CashOutcome();
                tbCash_In_Outcome.Text = cB_In_OutCash.Text = "";
                GetCashReg();
            }
            else
                cB_In_OutCash.BackColor = Color.LightCoral;
        }
        private void внесениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.Lime;
            btnLogin.Text = "Login";
            tbFIO.ReadOnly = tbINN.ReadOnly = label13.Visible = tbSummAll.Visible =
                groupBox3.Visible = panel2.Visible = false;

            groupBox4.Visible = true;
            GetCashReg();
        }
        private void cB_In_OutCash_TextChanged(object sender, EventArgs e)
        {
            buttonCash_In_Outcome.Text = cB_In_OutCash.Text;
        }
    }
}