using System;
using System.Windows.Forms;

namespace _3manRMK
{
    public partial class Feedback : Form
    {
        public Feedback()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MainMethods.Email.SendMail(tbSubject.Text, tbMessage.Text);
            Close();
        }
    }
}
