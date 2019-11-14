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
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckBox[] mas1 = new CheckBox[18] {chB_0_0, chB_0_1, chB_0_2, chB_0_3, chB_0_4,
                                                chB_0_5, chB_0_6, chB_0_7, chB_0_8, chB_0_9,
                                                chB_0_10, chB_0_11, chB_0_12, chB_0_13, chB_0_14,
                                                chB_0_15, chB_0_16, chB_0_17 };
            comboBox1.Items.Clear();
            for (int i=0; i< 18; i++)
            {
                if (mas1[i].Checked)
                {
                    comboBox1.Items.Add(mas1[i].Text);
                }
            }
            CheckBox[] mas2 = new CheckBox[7] { chB_1_0, chB_1_1, chB_1_2, chB_1_3,
                                                chB_1_4, chB_1_5, chB_1_6 };
            comboBox2.Items.Clear();
            for (int i = 0; i < 7; i++)
            {
                if (mas2[i].Checked)
                {
                    comboBox2.Items.Add(mas2[i].Text);
                }
            }
            CheckBox[] mas3 = new CheckBox[6] { chB_2_0, chB_2_1, chB_2_2,
                                                chB_2_3, chB_2_4, chB_2_5 };
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
            CheckBox[] mas1 = new CheckBox[18] {chB_0_0, chB_0_1, chB_0_2, chB_0_3, chB_0_4,
                                                chB_0_5, chB_0_6, chB_0_7, chB_0_8, chB_0_9,
                                                chB_0_10, chB_0_11, chB_0_12, chB_0_13, chB_0_14,
                                                chB_0_15, chB_0_16, chB_0_17 };
            comboBox1.Items.Clear();
            for (int i = 0; i < 18; i++)
            {
                mas1[i].Checked = checkBox1.Checked;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox[] mas2 = new CheckBox[7] { chB_1_0, chB_1_1, chB_1_2, chB_1_3,
                                                chB_1_4, chB_1_5, chB_1_6 };
            comboBox2.Items.Clear();
            for (int i = 0; i < 7; i++)
            {
                mas2[i].Checked = checkBox2.Checked;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox[] mas3 = new CheckBox[6] { chB_2_0, chB_2_1, chB_2_2,
                                                chB_2_3, chB_2_4, chB_2_5 };
            comboBox3.Items.Clear();
            for (int i = 0; i < 6; i++)
            {
                mas3[i].Checked = checkBox3.Checked;
            }
        }
    }
}
