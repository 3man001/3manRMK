﻿using DrvFRLib; //Библиотека ШТРИХ-М подключение
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;  //Подключение библиотек

namespace _3manRMK_0
{
    public partial class Form1 : Form
    {
        public Form1()  //Инициация основоного окна
        {
            InitializeComponent();
            Drv = new DrvFR();
            Size = new Size(873, 300);// 871;670
            groupBox3.Location = new Point(0, 65);
            groupBox4.Location = new Point(0, 65);
            InitialArrays();
            CBox[0] = checkBox2;
            sLabel[0] = label15;
            PaymentItemSign[0] = cbPaymentItemSign_1;
                PaymentItemSign[0].Items.CopyTo(PaymentItemSignItems, 0);
            NameProduct[0] = tbNameProduct_1;
            Price[0] = tbPrice_1;
            Quantity[0] = tbQuantity_1;
            Tax[0] = cbTax1_1;
                Tax[0].Items.CopyTo(TaxItems, 0);
            Summ[0] = tbSumm1_1;
            XY = new int[] {CBox[0].Location.X, sLabel[0].Location.X, PaymentItemSign[0].Location.X, NameProduct[0].Location.X,
                            Price[0].Location.X, Quantity[0].Location.X, Tax[0].Location.X, Summ[0].Location.X};
        }
        DrvFR Drv; //Создание обьекта драйвера ФР

        int[] XY;
        CheckBox[] CBox = new CheckBox [] {};
        Label[] sLabel = new Label[] { };
        ComboBox[] PaymentItemSign = new ComboBox[] { };
            Object[] PaymentItemSignItems = new Object[3];
        TextBox[] NameProduct = new TextBox[] { };
        TextBox[] Price = new TextBox[] { };
        TextBox[] Quantity = new TextBox[] { };
        ComboBox[] Tax = new ComboBox[] { };
            Object[] TaxItems = new Object[6];
        TextBox[] Summ = new TextBox[] { };

        ////////////Блок Функций/////////////
        private void InitialRMK()
        {
            FileOperation("ComNumber=" + Drv.ComNumber +
                            "\nBaudRate=" + Drv.BaudRate +
                            "\nTimeout=" + Drv.Timeout +
                            "\nComputerName=" + Drv.ComputerName +
                            "\nProtocolType=" + Drv.ProtocolType +
                            "\nConnectionType=" + Drv.ConnectionType +
                            "\nTCPPort=" + Drv.TCPPort +
                            "\nIPAddress=" + Drv.IPAddress +
                            "\nUseIPAddress=" + Drv.UseIPAddress, "connect.ini");
            Drv.FNGetFiscalizationResult();
            FileOperation("ИНН=" + Drv.INN +
                            "\nСНО=" + Convert.ToString(Drv.TaxType, 2), "aboutkkt.ini");
        }
        static void FileOperation(string InText, string NameFile) //Работа с файлами
        {
            File.WriteAllText(NameFile, InText);
        }
        private string CheckNumber(string Str, int ndp) //Проверка строкии с числом
        {
            if (Str == "")
            { return ""; }
            string simbols = "1234567890,";
            int k = 0;
            for (int i = 0; i < Str.Length; i++) // проверка на посторонние символы
            {
                if (simbols.IndexOf(Str[i]) < 0)
                { return ""; }
                if (Str[i] == ',')
                    { k++; }
                if (k > 1)
                { return ""; }
            }
            try
                { return Math.Round(Convert.ToDecimal(Str) * 1.000000m, ndp).ToString(); }
            catch
                { return ""; }
        }
        private bool CheckSimbols(string ChSimbol, string typeSim)  //Проверка строки
        {
            if (ChSimbol == "")
                { return false; }
            string DataSimbol = "";
            if (typeSim == "Число")
            {
                DataSimbol = "1234567890";
                int I = ChSimbol.IndexOf(',');
                int L = ChSimbol.Length;
                if (I == 0)
                    { return false; }
                else
                {
                    if (I > 0)
                    {
                        ChSimbol = ChSimbol.Substring(0, I) + ChSimbol.Substring(I + 1, L - I - 1);
                    }
                    for (int i = 0; i < L-1; i++)
                    {
                        if (DataSimbol.IndexOf(ChSimbol[i]) < 0)
                        { return false; }
                    }
                    return true;
                }
            }
            if (typeSim == "ФИО")
            {
                if (ChSimbol[0] == ' ')
                    { return false; }
                DataSimbol = " ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";
            }
            if (typeSim == "ИНН")   //Проверка ИНН Физ.Лица на корректность
            {
                string inn = ChSimbol;
                if (inn.Length == 12)
                {
                    int N0 = (int)char.GetNumericValue(inn[0]); int N1 = (int)char.GetNumericValue(inn[1]);
                    int N2 = (int)char.GetNumericValue(inn[2]); int N3 = (int)char.GetNumericValue(inn[3]);
                    int N4 = (int)char.GetNumericValue(inn[4]); int N5 = (int)char.GetNumericValue(inn[5]);
                    int N6 = (int)char.GetNumericValue(inn[6]); int N7 = (int)char.GetNumericValue(inn[7]);
                    int N8 = (int)char.GetNumericValue(inn[8]); int N9 = (int)char.GetNumericValue(inn[9]);
                    int N10 = (int)char.GetNumericValue(inn[10]);   int N11 = (int)char.GetNumericValue(inn[11]);

                    int b2 = (N0*7 + N1*2 + N2*4 + N3*10 + N4*3 + N5*5 + N6*9 + N7*4 + N8*6 + N9*8) % 11;
                    int b1 = (N0*3 + N1*7 + N2*2 + N3*4 + N4*10 + N5*3 + N6*5 + N7*9 + N8*4 + N9*6 + N10*8) % 11;

                    if ((b2 == N10) | ((b2 == 10) & (N10 == 0)))
                    {
                        if ((b1 == N11) | ((b1 == 10) & (N11 == 0)))
                            { return true; }
                        else
                            { return false; }
                    }
                    else
                        { return false; }
                }
                else
                    { return false; }
            }
            if (typeSim == "Email")
            {
                DataSimbol = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM.-";
                int I = ChSimbol.IndexOf('@');
                int L = ChSimbol.Length;
                if ((I <= 0) | (L < 3) | (I+1 == L))
                    { return false; }
                else
                {
                    ChSimbol = ChSimbol.Substring(0,I) + ChSimbol.Substring(I+1, L-I-1);
                    for (int i = 0; i < L-1; i++)
                    {
                        if (DataSimbol.IndexOf(ChSimbol[i]) < 0)
                        { return false; }
                    }
                    return true;
                }
            }
            if (typeSim == "Phone")
            {
                string s = ChSimbol;
                int L = ChSimbol.Length;
                if (L == 17)
                {
                    int sI = (s.Substring(0, 2) + s.Substring(3, 3) + s.Substring(8, 3) + s.Substring(12, 2) + s.Substring(15, 2)).IndexOf(' ');
                    if (sI > 0)
                        { return false; }
                    else
                        { return true; }
                }
                else
                    { return false; }
            }
            for (int i = 0; i < ChSimbol.Length; i++)
            {
                if (DataSimbol.IndexOf(ChSimbol[i]) < 0)
                    { return false; }
            }
            return true;
        }
        private void UpdateResult() //Проверка состояния ККТ
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
            }
            Drv.FNGetInfoExchangeStatus();
            toolStripStatusLabel4.Text = "Не отправлено документов = " + Convert.ToString(Drv.MessageCount);
            toolStripStatusLabel2.Text = "||" + Convert.ToString(Drv.Date);
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
            return Items[Item];
        }
        private void RegPosition(int CheckType, string NameProduct, Decimal Price, Double Quantity,
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
            { UpdateResult(); }
        }
        private void CloseChek() // Формируем закрытие чека
        {
            Drv.Summ1 = Convert.ToDecimal(tbSumm1.Text); // Наличные
            Drv.Summ2 = Convert.ToDecimal(tbSumm2.Text); // Остальные типы оплаты нулевые, но их необходимо заполнить
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
            Drv.TaxType = 1; // Основная система налогообложения
            Drv.StringForPrinting = "";
            try
            { Drv.FNCloseCheckEx(); } //Закрытие чека
            catch
            { UpdateResult(); }
            UpdateResult();
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
        private void InitialArrays()
        {
            for (int i=1; i<CBox.Length; i++)
            {
                CBox[i].Dispose();
                sLabel[i].Dispose();
                PaymentItemSign[i].Dispose();
                NameProduct[i].Dispose();
                Price[i].Dispose();
                Quantity[i].Dispose();
                Tax[i].Dispose();
                Summ[i].Dispose();
            }
            //Array.Clear(CBox, 1, CBox.Length - 1);
            Array.Resize(ref CBox, 1);
            Array.Resize(ref sLabel, 1);
            Array.Resize(ref PaymentItemSign, 1);
            Array.Resize(ref NameProduct, 1);
            Array.Resize(ref Price, 1);
            Array.Resize(ref Quantity, 1);
            Array.Resize(ref Tax, 1);
            Array.Resize(ref Summ, 1);
            bAdd.Location = new Point(6, 116);
            bAdd.Visible = true;
            groupBox2.Size = new Size(860, 180);
        }
        //////Начало Блока триггер виджета/////////
        private void tbFIO_TextChanged(object sender, EventArgs e)
        {
            if (CheckSimbols(tbFIO.Text, "ФИО"))
            {
                tbFIO.BackColor = Color.Snow;
            }
            else
            {
                tbFIO.BackColor = Color.LightCoral;
            }
        }
        private void tbINN_TextChanged(object sender, EventArgs e)
        {
            if (CheckSimbols(tbINN.Text, "ИНН"))
            {
                tbINN.BackColor = Color.LightGreen;
            }
            else
            {
                tbINN.BackColor = Color.LightCoral;
                if (tbINN.Text == "")
                {
                    tbINN.BackColor = Color.Snow;
                }
            }
        }
        private void tbPrice_TextChanged(object sender, EventArgs e) //Изменение строки с ценой
        {
            for (int i=0; i<CBox.Length; i++)
            {
                if (CBox[i].Checked)
                {
                    if (CheckSimbols(Price[i].Text, "Число"))
                    {
                        Price[i].BackColor = Color.Snow;
                        Price[i].Text = Convert.ToString(Math.Round(Convert.ToDecimal(Price[i].Text) * 1.000m, 2));
                        Summ[i].Text = Convert.ToString(Math.Round(Convert.ToDecimal(Price[i].Text) * Convert.ToDecimal(Quantity[i].Text), 2));
                    }
                    else
                    {
                        Price[i].BackColor = Color.LightCoral;
                    }
                }
            }
        }
        private void tbQuantity_TextChanged(object sender, EventArgs e) //Измениение строки с кол-вом
        {
            for (int i = 0; i < CBox.Length; i++)
            {
                if (CBox[i].Checked)
                {
                    if (CheckSimbols(Quantity[i].Text, "Число"))
                    {
                        Quantity[i].BackColor = Color.Snow;
                        Quantity[i].Text = Convert.ToString(Math.Round(Convert.ToDecimal(Quantity[i].Text) * 1.0000m, 3));
                        Summ[i].Text = Convert.ToString(Math.Round(Convert.ToDecimal(Price[i].Text) * Convert.ToDecimal(Quantity[i].Text), 2));
                    }
                    else
                    {
                        Quantity[i].BackColor = Color.LightCoral;
                    }
                }
            }       
        }
        private void tbSumm_TextChanged(object sender, EventArgs e)
        {
            decimal S = 0.00m;
            for (int i = 0; i < Summ.Length; i++)
            {
                S = S + Convert.ToDecimal(Summ[i].Text);
            }
            tbSummAll.Text = Convert.ToString(S); //tbSumm1_1.Text;
        }
        private void tbSumm1_TextChanged(object sender, EventArgs e)
        {
            string c = CheckNumber(tbSumm1.Text, 2);
            if (c == "")
            { 
                tbSumm1.BackColor = Color.LightCoral;
                tbChange.Visible = false;
                button4.Visible = false;
            }
            else
            {
                tbSumm1.BackColor = Color.Snow;
                tbSumm1.Text = c;
                decimal Change = Convert.ToDecimal(tbSummAll.Text) - Convert.ToDecimal(tbSumm2.Text) - Convert.ToDecimal(tbSumm1.Text);
                tbChange.Text = Convert.ToString(Change*-1);
                tbChange.Visible = true;
            }
        }
        private void tbSumm2_TextChanged(object sender, EventArgs e) //Контроль ввода безналичной оплаты
        {
            string c = CheckNumber(tbSumm2.Text, 2);
            if (c == "")
            {
                tbSumm2.BackColor = Color.LightCoral;
                tbChange.Visible = false;
                button4.Visible = false;
            }
            else
            {
                tbSumm2.BackColor = Color.Snow;
                decimal Change = Convert.ToDecimal(tbSummAll.Text) - Convert.ToDecimal(tbSumm2.Text) - Convert.ToDecimal(tbSumm1.Text);
                tbChange.Text = Convert.ToString(Change * -1);
                tbChange.Visible = true;
                tbSumm2.Text = c;
                if (Convert.ToDecimal(tbSumm2.Text) > Convert.ToDecimal(tbSummAll.Text))
                {
                    tbSumm2.BackColor = Color.LightCoral;
                    tbChange.Visible = false;
                    button4.Visible = false;
                }
            }

        }
        private void tbChange_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(tbChange.Text) >= 0)
            {
                button4.Visible = true;
            }
            else
            {

                button4.Visible = false;
            }
        }
        private void maskTBPhone_MaskInputRejected(object sender, EventArgs e) //Проверка тел. на корректность
        {
            string s = maskTBPhone.Text;
            if (CheckSimbols(maskTBPhone.Text, "Phone"))
            { maskTBPhone.BackColor = Color.LightGreen; }
            else
            {
                maskTBPhone.BackColor = Color.LightCoral;
                if ((maskTBPhone.Text.Length == 15) & ((s.Substring(3, 3) + s.Substring(8, 3) + s.Substring(12, 2)).Trim() == ""))
                { maskTBPhone.BackColor = Color.Snow; }
            }
        }
        private void tbEmail_TextChanged(object sender, EventArgs e) //Проверка Email на корректность
        {
            if (CheckSimbols(tbEmail.Text, "Email"))
            { tbEmail.BackColor = Color.LightGreen; }
            else
            { tbEmail.BackColor = Color.LightCoral; }
            if (tbEmail.Text == "")
            { tbEmail.BackColor = Color.Snow; }
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
        private void button4_Click(object sender, EventArgs e) //Продажа тестового товара
        {
            if (Convert.ToDecimal(tbSumm2.Text) > Convert.ToDecimal(tbSummAll.Text))
            {
                tbSumm2.BackColor = Color.LightCoral;
            }
            else
            {
                int CheckType = 1; //Операция приход(1-продажа, 3-возвр продажи)
                string NameProduct0 = tbNameProduct_1.Text; //Наименование товара
                Decimal Price0 = Math.Round(Convert.ToDecimal(tbPrice_1.Text), 2); //Цена за еденицу товара с учетом скидки
                Double Quantity0 = Math.Round(Convert.ToDouble(tbQuantity_1.Text), 3); //Кол-во (Диапазон 0,001 до 9.999.999,999)
                Decimal Summ10 = Convert.ToDecimal(tbSumm1_1.Text); //Сумма позиции
                int Tax10 = EnterItems(cbTax1_1.Text); //Налоговая ставка 0..6 (0-БезНДС)
                int PaymentItemSign0 = EnterItems(cbPaymentItemSign_1.Text); // Признак предмета расчета 1..19 (1-Товар)

                try
                {
                    Drv.Connect();
                    Drv.GetShortECRStatus();
                    if (Drv.ECRMode == 4)
                    {
                        открытьСменуToolStripMenuItem_Click(sender, e);
                    }
                }
                catch
                { UpdateResult(); }

                groupBox3.Visible = false;
                groupBox4.Visible = true;
                for (int i=0; i<CBox.Length; i++)
                {
                    if (CBox[i].Checked)
                    {
                        NameProduct0 = NameProduct[i].Text; //Наименование товара
                        Price0 = Math.Round(Convert.ToDecimal(Price[i].Text), 2); //Цена за еденицу товара с учетом скидки
                        Quantity0 = Math.Round(Convert.ToDouble(Quantity[i].Text), 3); //Кол-во (Диапазон 0,001 до 9.999.999,999)
                        Summ10 = Convert.ToDecimal(Summ[i].Text); //Сумма позиции
                        Tax10 = EnterItems(Tax[i].Text); //Налоговая ставка 0..6 (0-БезНДС)
                        PaymentItemSign0 = EnterItems(PaymentItemSign[i].Text); // Признак предмета расчета 1..19 (1-Товар)

                        RegPosition(CheckType, NameProduct0, Price0, Quantity0, Summ10, Tax10, PaymentItemSign0); //Регистрация позиции
                    }
                }
                SendFIO();
                
                if (maskTBPhone.BackColor == Color.LightGreen) //Отправка чека СМС если есть номер
                {
                    string s = maskTBPhone.Text;
                    s = s.Substring(0, 2) + s.Substring(3, 3) + s.Substring(8, 3) + s.Substring(12, 2) + s.Substring(15, 2);
                    Drv.CustomerEmail = s;
                    Drv.FNSendCustomerEmail();
                }
                if (tbEmail.BackColor == Color.LightGreen) //Отправка чека на Email если введен адрес
                {
                    Drv.CustomerEmail = tbEmail.Text;
                    Drv.FNSendCustomerEmail();
                }
                CloseChek(); // Формирует закрытие чека
                tbSumm1.Text = "0,00";
                tbSumm2.Text = "0,00";
                InitialArrays();
                tbSumm1.Visible = false;
                tbSumm2.Visible = false;
                groupBox2.Visible = true;
                groupBox4.Visible = false;
            }
        }
        private void btnLogin_Click(object sender, EventArgs e) //Регистрация кассира
        {
            if (btnLogin.BackColor == Color.Lime)
            {
                tbFIO_TextChanged(sender, e);
                tbINN_TextChanged(sender, e);
                if ((tbFIO.BackColor == Color.Snow) & (tbINN.BackColor == Color.Snow))
                {
                    btnLogin.BackColor = Color.DodgerBlue;
                    btnLogin.Text = "LogOut";
                    tbFIO.ReadOnly = tbINN.ReadOnly = label13.Visible = tbSummAll.Visible = groupBox2.Visible = true;
                }
            }
            else
            {
                btnLogin.BackColor = Color.Lime;
                btnLogin.Text = "Login";
                tbFIO.ReadOnly = tbINN.ReadOnly = label13.Visible = tbSummAll.Visible = groupBox2.Visible = false;
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
            Setting Setting1 = new Setting();
            Setting1.ShowDialog(this);
            Setting1.Dispose();
        }
        private void обратнаяСвязьToolStripMenuItem_Click(object sender, EventArgs e) //Открыть формк обратной связи
        {
            Feedback Feedback1 = new Feedback();
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
            groupBox3.Visible = true;
            groupBox2.Visible = false;
        }
        private void button12_Click(object sender, EventArgs e) //Перейти к позициям
        {
            groupBox3.Visible = false;
            groupBox2.Visible = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i< CBox.Length; i++)
            {
                CBox[i].Checked = checkBox1.Checked;
            }
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            
            int Poz = CBox.Length;
            if (Poz >= 50)
            {
                bAdd.Visible = false;
            }
            int Y = bAdd.Location.Y;
            Array.Resize(ref CBox, Poz + 1);
            Array.Resize(ref sLabel, Poz + 1);
            Array.Resize(ref PaymentItemSign, Poz + 1);
            Array.Resize(ref NameProduct, Poz + 1);
            Array.Resize(ref Price, Poz + 1);
            Array.Resize(ref Quantity, Poz + 1);
            Array.Resize(ref Tax, Poz + 1);
            Array.Resize(ref Summ, Poz + 1);
            CBox[Poz] = new CheckBox {Size = CBox[0].Size, 
                                      Checked = true, 
                                      Location = new Point(XY[0], Y) };
            sLabel[Poz] = new Label {Size = sLabel[0].Size,
                                    Text = Convert.ToString(Poz+1)+'.',
                                    Location = new Point(XY[1], Y) };
            PaymentItemSign[Poz] = new ComboBox { Size = PaymentItemSign[0].Size,
                                                  Location = new Point(XY[2], Y),
                                                  Text = PaymentItemSign[0].Text };
                PaymentItemSign[Poz].Items.AddRange(PaymentItemSignItems);
            NameProduct[Poz] = new TextBox {Size = NameProduct[0].Size,
                                            Location = new Point(XY[3], Y),
                                            Text = Convert.ToString(Poz)+". "+NameProduct[0].Text};
            Price[Poz] = new TextBox {Size = Price[0].Size,
                                      Location = new Point(XY[4], Y),
                                      Text = Price[0].Text};
            Price[Poz].TextChanged += new EventHandler(tbPrice_TextChanged);
            Quantity[Poz] = new TextBox {Size = Quantity[0].Size,
                                        Location = new Point(XY[5], Y),
                                        Text = Quantity[0].Text};
            Quantity[Poz].TextChanged += new EventHandler(tbQuantity_TextChanged);
            Tax[Poz] = new ComboBox {Size = Tax[0].Size,
                                    Location = new Point(XY[6], Y),
                                    Text = Tax[0].Text};
                Tax[Poz].Items.AddRange(TaxItems);
            Summ[Poz] = new TextBox {Size = Summ[0].Size,
                                    Location = new Point(XY[7], Y),
                                    ReadOnly = true,
                                    Text = Summ[0].Text};
            Summ[Poz].TextChanged += new EventHandler(tbSumm_TextChanged);

            SuspendLayout();
            groupBox2.Controls.Add(CBox[Poz]);
            groupBox2.Controls.Add(sLabel[Poz]);
            groupBox2.Controls.Add(PaymentItemSign[Poz]);
            groupBox2.Controls.Add(NameProduct[Poz]);
            groupBox2.Controls.Add(Price[Poz]);
            groupBox2.Controls.Add(Quantity[Poz]);
            groupBox2.Controls.Add(Tax[Poz]);
            groupBox2.Controls.Add(Summ[Poz]);
            ResumeLayout(false);
            PerformLayout();
            bAdd.Location = new Point(bAdd.Location.X, bAdd.Location.Y+30);
        }
    }
}