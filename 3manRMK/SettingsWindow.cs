using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _3manRMK
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
            CB_PaymentItemSign0[0] = checkBox1;
            CB_PaymentItemSign0[1] = chB_0_0;
            CB_PaymentItemSign0[2] = chB_0_1;
            CB_PaymentItemSign0[3] = chB_0_2;
            CB_PaymentItemSign0[4] = chB_0_3;
            CB_PaymentItemSign0[5] = chB_0_4;
            CB_PaymentItemSign0[6] = chB_0_5;
            CB_PaymentItemSign0[7] = chB_0_6;
            CB_PaymentItemSign0[8] = chB_0_7;
            CB_PaymentItemSign0[9] = chB_0_8;
            CB_PaymentItemSign0[10] = chB_0_9;
            CB_PaymentItemSign0[11] = chB_0_10;
            CB_PaymentItemSign0[12] = chB_0_11;
            CB_PaymentItemSign0[13] = chB_0_12;
            CB_PaymentItemSign0[14] = chB_0_13;
            CB_PaymentItemSign0[15] = chB_0_14;
            CB_PaymentItemSign0[16] = chB_0_15;
            CB_PaymentItemSign0[17] = chB_0_16;
            CB_PaymentItemSign0[18] = chB_0_17;

            CB_PaymentTypeSign[0] = checkBox2;
            CB_PaymentTypeSign[1] = chB_1_0;
            CB_PaymentTypeSign[2] = chB_1_1;
            CB_PaymentTypeSign[3] = chB_1_2;
            CB_PaymentTypeSign[4] = chB_1_3;
            CB_PaymentTypeSign[5] = chB_1_4;
            CB_PaymentTypeSign[6] = chB_1_5;
            CB_PaymentTypeSign[7] = chB_1_6;

            CB_Tax[0] = checkBox3;
            CB_Tax[1] = chB_2_0;
            CB_Tax[2] = chB_2_1;
            CB_Tax[3] = chB_2_2;
            CB_Tax[4] = chB_2_3;
            CB_Tax[5] = chB_2_4;
            CB_Tax[6] = chB_2_5;
            //for (int i=2; i<CB_Tax.Length; i++)
            //{
            //   CB_Tax[i].CheckedChanged += new EventHandler ();
            //}

        }

        CheckBox[] CB_PaymentItemSign0 = new CheckBox[19];
        CheckBox[] CB_PaymentTypeSign = new CheckBox[8];
        CheckBox[] CB_Tax = new CheckBox[7];
        private CheckBox[] NewMass(int i)
        {
            if (i == 1)
            {
                CheckBox[] mas1 = new CheckBox[18] {chB_0_0, chB_0_1, chB_0_2, chB_0_3, chB_0_4,
                                                chB_0_5, chB_0_6, chB_0_7, chB_0_8, chB_0_9,
                                                chB_0_10, chB_0_11, chB_0_12, chB_0_13, chB_0_14,
                                                chB_0_15, chB_0_16, chB_0_17 };
                return mas1;
            }
            if (i == 2)
            {
                CheckBox[] mas2 = new CheckBox[7] { chB_1_0, chB_1_1, chB_1_2, chB_1_3,
                                                chB_1_4, chB_1_5, chB_1_6 };
                return mas2;
            }
            if (i == 3)
            {
                CheckBox[] mas3 = new CheckBox[6] { chB_2_0, chB_2_1, chB_2_2,
                                                chB_2_3, chB_2_4, chB_2_5 };
                return mas3;
            }
            CheckBox[] mas0 = new CheckBox[] { };
            return mas0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CheckBox[] mas1 = NewMass(1);
            comboBox1.Items.Clear();
            for (int i=0; i< 18; i++)
            {
                if (mas1[i].Checked)
                {
                    comboBox1.Items.Add(mas1[i].Text);
                }
            }
            CheckBox[] mas2 = NewMass(2);
            comboBox2.Items.Clear();
            for (int i = 0; i < 7; i++)
            {
                if (mas2[i].Checked)
                {
                    comboBox2.Items.Add(mas2[i].Text);
                }
            }
            CheckBox[] mas3 = NewMass(3);
            comboBox3.Items.Clear();
            for (int i = 0; i < 6; i++)
            {
                if (mas3[i].Checked)
                {
                    comboBox3.Items.Add(mas3[i].Text);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox[] mas1 = NewMass(1);
            comboBox1.Items.Clear();
            for (int i = 0; i < 18; i++)
            {
                mas1[i].Checked = checkBox1.Checked;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox[] mas2 = NewMass(2);
            comboBox2.Items.Clear();
            for (int i = 0; i < 7; i++)
            {
                mas2[i].Checked = checkBox2.Checked;
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox[] mas3 = NewMass(3);
            comboBox3.Items.Clear();
            for (int i = 0; i < 6; i++)
            {
                mas3[i].Checked = checkBox3.Checked;
            }
        }
    }
}
