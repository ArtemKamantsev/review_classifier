using System;
using System.Windows.Forms;

namespace review_classifier
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminForm form = new AdminForm();
            form.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResultForm form = new ResultForm();
            form.Show();
            Hide();
        }

        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
