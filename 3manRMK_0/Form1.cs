using DrvFRLib; //Библиотека ШТРИХ-М подключение
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _3manRMK_0
{
    public partial class Form1 : Form
    {
        public Form1()  //Инициация основоного окна
        {
            InitializeComponent();
            Drv = new DrvFR();
        }
        DrvFR Drv; //Создание обьекта драйвера ФР

        ////////////Блок Функций/////////////
        private bool CheckSimbols (string ChSimbol, string typeSim) //Проверка строки на посторонние символы
        {
            string DataSimbol = "";
            if (typeSim == "ФИО")
            {
                DataSimbol = " ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";
            }
            if (typeSim == "Число")
            {
                DataSimbol = "1234567890,";
            }
            if (typeSim == "ИНН")
            {
                if (ChSimbol == "")
                {
                    return true;
                }
                DataSimbol = "1234567890";
            }

            if (ChSimbol[0] == ' ')
            {
                return false;
            }
            for (int i = 0; i < ChSimbol.Length; i++)
            {
                if (DataSimbol.IndexOf(ChSimbol[i]) < 0)
                {
                    return false;
                }
            }
            return true;
        }
        private bool CheckINN(string inn) //Проверка ИНН Физ.Лица на корректность
        {
            if (inn.Length == 12)
            {
                int b2 = ((int)char.GetNumericValue(inn[0]) * 7 + (int)char.GetNumericValue(inn[1]) * 2 + (int)char.GetNumericValue(inn[2]) * 4 +
                    (int)char.GetNumericValue(inn[3]) * 10 + (int)char.GetNumericValue(inn[4]) * 3 + (int)char.GetNumericValue(inn[5]) * 5 +
                    (int)char.GetNumericValue(inn[6]) * 9 + (int)char.GetNumericValue(inn[7]) * 4 + (int)char.GetNumericValue(inn[8]) * 6 + (int)char.GetNumericValue(inn[9]) * 8) % 11;
                int b1 = ((int)char.GetNumericValue(inn[0]) * 3 + (int)char.GetNumericValue(inn[1]) * 7 + (int)char.GetNumericValue(inn[2]) * 2 +
                    (int)char.GetNumericValue(inn[3]) * 4 + (int)char.GetNumericValue(inn[4]) * 10 + (int)char.GetNumericValue(inn[5]) * 3 +
                    (int)char.GetNumericValue(inn[6]) * 5 + (int)char.GetNumericValue(inn[7]) * 9 + (int)char.GetNumericValue(inn[8]) * 4 + (int)char.GetNumericValue(inn[9]) * 6 + (int)char.GetNumericValue(inn[10]) * 8) % 11;

                if ((b2 == (int)char.GetNumericValue(inn[10])) | ((b2 == 10) & ((int)char.GetNumericValue(inn[10]) == 0)))
                {
                    if ((b1 == (int)char.GetNumericValue(inn[11])) | ((b1 == 10) & ((int)char.GetNumericValue(inn[11]) == 0)))
                    {
                        return true; }
                    else
                    {
                        return false; }
                }
                else
                {
                    return false; }
            }
            else
            { return false; }
        }
        private void UpdateResult() //Проверка состояния ККТ
        {
            toolStripStatusLabel1.Text = string.Format("Результат: {0}, {1}", Drv.ResultCode, Drv.ResultCodeDescription);
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
                if ((Str[i] == '.') | (Str[i] == ','))
                { k++; }
            }
            if (k > 1) //Проверка на повтор разделителей
            { return ""; }
            try
            { return Math.Round(Convert.ToDecimal(Str) * 1.000000m, ndp).ToString(); }
            catch
            { return ""; }
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
        ////////////Конец Блок Функций/////////////
        ///////////////////////////////////////////
        //////Начало Блока триггер виджета/////////
        private void tbPrice_1_TextChanged(object sender, EventArgs e)
        {
            string c = CheckNumber(tbPrice_1.Text, 2);
            if (c == "")
            { tbPrice_1.BackColor = Color.LightCoral; }
            else
            {
                tbPrice_1.BackColor = Color.Snow;
                tbPrice_1.Text = c;
            }
            try
            {
                tbSumm1_1.Text = Convert.ToString(
                    Math.Round(Convert.ToDecimal(tbPrice_1.Text) * Convert.ToDecimal(tbQuantity_1.Text),2));
            }
            catch
            {

            }            
        }
        private void tbQuantity_TextChanged(object sender, EventArgs e) //Измениение строки с ценой
        {
            string c = CheckNumber(tbQuantity_1.Text, 3);
            if (c == "")
            { tbQuantity_1.BackColor = Color.LightCoral; }
            else
            {
                tbQuantity_1.BackColor = Color.Snow;
                tbQuantity_1.Text = c;
            }
            try
            {
                tbSumm1_1.Text = Convert.ToString(
                    Math.Round(Convert.ToDecimal(tbPrice_1.Text) * Convert.ToDecimal(tbQuantity_1.Text), 2));
            }
            catch
            {

            }
        }
        private void tbSumm1_TextChanged(object sender, EventArgs e)
        {
            string c = CheckNumber(tbSumm1.Text, 2);
            if (c == "")
            { tbSumm1.BackColor = Color.LightCoral; }
            else
            {
                tbSumm1.BackColor = Color.Snow;
                tbSumm1.Text = c;
            }
        }
        private void tbSumm2_TextChanged(object sender, EventArgs e) //Контроль ввода безналичной оплаты
        {
            string c = CheckNumber(tbSumm2.Text, 2);
            if (c == "")
            { tbSumm2.BackColor = Color.LightCoral; }
            else
            {
                tbSumm2.BackColor = Color.Snow;
                tbSumm2.Text = c;
                if (Convert.ToDecimal(tbSumm2.Text) > Convert.ToDecimal(tbSummAll.Text))
                {
                    tbSumm2.BackColor = Color.LightCoral;
                }
            }
            
        }
        private void tbSumm1_1_TextChanged(object sender, EventArgs e)
        {
            tbSummAll.Text = tbSumm1_1.Text;
        }
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
                if (CheckINN(tbINN.Text) | (tbINN.Text == ""))
                {
                    tbINN.BackColor = Color.Snow;
                }
                else
                {
                    tbINN.BackColor = Color.LightCoral;
                }
            }
            else
            {
                tbINN.BackColor = Color.LightCoral;
            }
        }
        ///////Конец Блока триггер виджета/////////

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
                //Drv.FNBeginOpenSession(); //Начать отткрытие смены
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
                // Drv.FNBeginCloseSession(); //Начать закрытие смены
                Drv.FNCloseSession();
            }
            catch
            { UpdateResult(); }
            UpdateResult();
        }
        private void отменаЧекаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Drv.CancelCheck(); //Отмена чека
        }
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e) //Показать информацию о программе
        {
            AboutBox1 AboutBox = new AboutBox1();
            AboutBox.ShowDialog(this);
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
                string NameProduct = tbNameProduct_1.Text; //Наименование товара
                Decimal Price = Math.Round(Convert.ToDecimal(tbPrice_1.Text), 2); //Цена за еденицу товара с учетом скидки
                Double Quantity = Math.Round(Convert.ToDouble(tbQuantity_1.Text), 3); //Кол-во (Диапазон 0,001 до 9.999.999,999)
                Decimal Summ1 = Convert.ToDecimal(tbSumm1_1.Text); //Сумма позиции
                int Tax1 = EnterItems(cbTax1_1.Text); //Налоговая ставка 0..6 (0-БезНДС)
                int PaymentItemSign = EnterItems(cbPaymentItemSign_1.Text); // Признак предмета расчета 1..19 (1-Товар)

                

                try
                { Drv.Connect(); }
                catch
                { UpdateResult(); }

                

                RegPosition(CheckType, NameProduct, Price, Quantity, Summ1, Tax1, PaymentItemSign); //Регистрация позиции

                if (tbFIO.ReadOnly)
                {
                    Drv.TagNumber = 1021; //это ФИО кассира    
                    Drv.TagType = 7; // тип "строка" 
                    Drv.TagValueStr = tbFIO.Text;
                    Drv.FNSendTag();
                    if (tbINN.Text != "")
                    {
                        Drv.TagNumber = 1203; //это ИНН кассира    
                        Drv.TagType = 7; // тип "строка" 
                        Drv.TagValueStr = tbINN.Text;
                        Drv.FNSendTag();
                    }
                }
                CloseChek(); // Формируем закрытие чека
            }
        }
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            btnLogOut.Visible = false;
            btnLogin.Visible = true;
            tbFIO.ReadOnly = false;
            tbINN.ReadOnly = false;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            tbFIO_TextChanged(sender, e);
            tbINN_TextChanged(sender, e);
            if ((tbFIO.BackColor == Color.Snow)&(tbINN.BackColor == Color.Snow))
            {
                btnLogOut.Visible = true;
                btnLogin.Visible = false;
                tbFIO.ReadOnly = true;
                tbINN.ReadOnly = true;
            }
        }

        
    }
}