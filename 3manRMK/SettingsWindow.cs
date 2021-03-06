﻿using LibraryDotNetFramework;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace _3manRMK
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
            
            Test();            
            LoadDefaultSettings();
            CreateWindjetsOnWindow();
        }
        string[] arrayPaimentItemSign =
                new string[19] { "",
                    "Товар", "Подакцизный товар",
                    "Работа", "Услуга",
                    "Ставка азартной игры", "Выигрыш азартной игры",
                    "Лотерейный билет", "Выигрыш лотереи",
                    "Предоставление РИД", "Платеж",
                    "Агентское вознаграждение", "Составной предмет расчета",
                    "Иной предмет расчета", "Имущественное право",
                    "Внереализационный доход", "Страховые взносы",
                    "Торговый сбор", "Курортный сбор"};
        
        private void Test()
        {
            Array.Resize(ref arrayPaimentItemSign, 1);
            arrayPaimentItemSign = Undefiend.AddDictToArray(WorkWithDKKT.DicItems.getDicPaimentItemSign(), arrayPaimentItemSign);
        }
        string[] arrayPaymentTypeSign =
            new string[8] { "",
                    "Предоплата 100%", "Частичная предоплата",
                    "Аванс", "Полный расчет",
                    "Частичный расчет и кредит", "Передача в кредит",
                    "Оплата кредита" };
        string[] arrayTax =
            new string[7] { "",
                    "НДС 20%", "НДС 10%",
                    "НДС 20/120", "НДС 10/110",
                    "НДС 0%", "Без НДС" };
        string[] arrayTaxSystem =
            new string[7] {"", 
                    "Основная", "УСН доход",
                    "УСН доход-расход", "ЕНВД",
                    "ЕСХН", "Патент" };
        CheckBox[] CB_PaymentItemSign = new CheckBox[0];
        CheckBox[] CB_PaymentTypeSign = new CheckBox[0];
        CheckBox[] CB_Tax = new CheckBox[0];
        CheckBox[] CB_TaxSystem = new CheckBox[0];
        private void LoadDefaultSettings()
        {

            Undefiend.ConvertStringToItems(Properties.Settings.Default.paymentItemsSign, comBoxPaymentItemSign.Items);
            Undefiend.ConvertStringToItems(Properties.Settings.Default.paymentTypeSign, comboBox2.Items);
            Undefiend.ConvertStringToItems(Properties.Settings.Default.taxItems, comBoxTaxItem.Items);
            Undefiend.ConvertStringToItems(Properties.Settings.Default.taxSystem, comBoxTaxSystem.Items);
            comBoxPaymentItemSign.SelectedIndex = Properties.Settings.Default.paymentItemSignDefault;
            comboBox2.SelectedIndex = Properties.Settings.Default.paymentTypeSignDefault;
            comBoxTaxItem.SelectedIndex = Properties.Settings.Default.taxItemsDeault;
            comBoxTaxSystem.SelectedIndex = Properties.Settings.Default.taxSystemDeault;
        }
        private void CreateWindjetsOnWindow()
        {
            CB_PaymentItemSign = arrayStringToArrayCheckBox(arrayPaimentItemSign, CB_PaymentItemSign, comBoxPaymentItemSign, 15, 67);
            CB_PaymentItemSign[0].CheckedChanged += new EventHandler(cbPayItemSignZeroCheckedChanged);
            for (int i = 1; i < CB_PaymentItemSign.Length; i++)
            {
                CB_PaymentItemSign[i].CheckedChanged += new EventHandler(cbPayItemSignCheckedChanged);
            }
            arrayCheckBoxAddCotrols(CB_PaymentItemSign);

            CB_PaymentTypeSign = arrayStringToArrayCheckBox(arrayPaymentTypeSign, CB_PaymentTypeSign, comboBox2, 220, 67);
            CB_PaymentTypeSign[0].CheckedChanged += new EventHandler(CB_PaymentTypeSignZeroCheckedChanged);
            for (int i = 1; i < CB_PaymentTypeSign.Length; i++)
            {
                CB_PaymentTypeSign[i].CheckedChanged += new EventHandler(CB_PaymentTypeSignCheckedChanged);
            }
            arrayCheckBoxAddCotrols(CB_PaymentTypeSign);

            CB_Tax = arrayStringToArrayCheckBox(arrayTax, CB_Tax, comBoxTaxItem, 410, 67);
            CB_Tax[0].CheckedChanged += new EventHandler(CB_TaxZeroCheckedChanged);
            for (int i = 1; i < CB_Tax.Length; i++)
            {
                CB_Tax[i].CheckedChanged += new EventHandler(CB_TaxCheckedChanged);
            }
            arrayCheckBoxAddCotrols(CB_Tax);

            CB_TaxSystem = arrayStringToArrayCheckBox(arrayTaxSystem, CB_TaxSystem, comBoxTaxSystem, 550, 67);
            CB_TaxSystem[0].CheckedChanged += new EventHandler(CB_TaxSystemZeroCheckedChanged);
            for (int i = 1; i < CB_TaxSystem.Length; i++)
            {
                CB_TaxSystem[i].CheckedChanged += new EventHandler(CB_TaxSystemCheckedChanged);
            }
            arrayCheckBoxAddCotrols(CB_TaxSystem);
        }
        private CheckBox[] arrayStringToArrayCheckBox(string [] arrayString, CheckBox [] arrayCheckBox,ComboBox comboBox, int x, int y)
        {
            for (int i = 0; i < arrayString.Length; i++)
            {
                Array.Resize(ref arrayCheckBox, i + 1);
                arrayCheckBox[i] = new CheckBox
                {
                    AutoSize = true,
                    Text = arrayString[i],
                    Checked = comboBox.Items.Contains(arrayString[i]),
                    Location = new Point(x, y + 20 * i)
                };
            }
            return arrayCheckBox;
        }
        private void arrayCheckBoxAddCotrols(CheckBox [] comboBox)
        {
            for (int i = 0; i < comboBox.Length; i++)
            {
                SuspendLayout();
                Controls.Add(comboBox[i]);
                ResumeLayout(false);
                PerformLayout();
            }
        }
        private void arrayZeroChekedChanged(CheckBox [] arrayCheckBox)
        {
            bool Check = arrayCheckBox[0].Checked;
            if (arrayCheckBox[0].CheckState != CheckState.Indeterminate)
            {
                for (int i = 1; i < arrayCheckBox.Length; i++)
                { arrayCheckBox[i].Checked = Check; }
            }
        }
        private void arrayCheckedChanged(CheckBox[] arrayCheckBox)
        {
            int State = 0;
            for (int i = 1; i < arrayCheckBox.Length; i++)
            {
                if (arrayCheckBox[i].Checked)
                    State++;
            }
            if (State == 0)
                arrayCheckBox[0].CheckState = CheckState.Unchecked;
            else if (State == arrayCheckBox.Length - 1)
                arrayCheckBox[0].CheckState = CheckState.Checked;
            else
                arrayCheckBox[0].CheckState = CheckState.Indeterminate;
        }
        private void arrayCheckBoxToComboItems(CheckBox [] arrayCheckBox, ComboBox.ObjectCollection items)
        {
            items.Clear();
            for (int i=1; i< arrayCheckBox.Length; i++)
            {
                if (arrayCheckBox[i].Checked)
                    items.Add(arrayCheckBox[i].Text);
            }
        }
        private void cbPayItemSignZeroCheckedChanged(object sender, EventArgs e)
        {
            arrayZeroChekedChanged(CB_PaymentItemSign);
        }
        private void cbPayItemSignCheckedChanged(object sender, EventArgs e)
        {
            arrayCheckedChanged(CB_PaymentItemSign);
            arrayCheckBoxToComboItems(CB_PaymentItemSign, comBoxPaymentItemSign.Items);
        }
        private void CB_PaymentTypeSignZeroCheckedChanged(object sender, EventArgs e)
        {
            arrayZeroChekedChanged(CB_PaymentTypeSign);
        }
        private void CB_PaymentTypeSignCheckedChanged(object sender, EventArgs e)
        {
            arrayCheckedChanged(CB_PaymentTypeSign);
            arrayCheckBoxToComboItems(CB_PaymentTypeSign, comboBox2.Items);
        }
        private void CB_TaxZeroCheckedChanged(object sender, EventArgs e)
        {
            arrayZeroChekedChanged(CB_Tax);
        }
        private void CB_TaxCheckedChanged(object sender, EventArgs e)
        {
            arrayCheckedChanged(CB_Tax);
            arrayCheckBoxToComboItems(CB_Tax, comBoxTaxItem.Items);
        }
        private void CB_TaxSystemZeroCheckedChanged(object sender, EventArgs e)
        {
            arrayZeroChekedChanged(CB_TaxSystem);
        }
        private void CB_TaxSystemCheckedChanged(object sender, EventArgs e)
        {
            arrayCheckedChanged(CB_TaxSystem);
            arrayCheckBoxToComboItems(CB_TaxSystem, comBoxTaxSystem.Items);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.paymentItemSignDefault = comBoxPaymentItemSign.SelectedIndex;
            Properties.Settings.Default.paymentTypeSignDefault = comboBox2.SelectedIndex;
            Properties.Settings.Default.taxItemsDeault = comBoxTaxItem.SelectedIndex;
            Properties.Settings.Default.taxSystemDeault = comBoxTaxSystem.SelectedIndex;
            Properties.Settings.Default.paymentItemsSign = Undefiend.ConvertItemsToString(comBoxPaymentItemSign.Items);
            Properties.Settings.Default.paymentTypeSign = Undefiend.ConvertItemsToString(comboBox2.Items);
            Properties.Settings.Default.taxItems = Undefiend.ConvertItemsToString(comBoxTaxItem.Items);
            Properties.Settings.Default.taxSystem = Undefiend.ConvertItemsToString(comBoxTaxSystem.Items);
            Properties.Settings.Default.Save();
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
