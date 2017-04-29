using System;
using System.Windows.Forms;

namespace VStudio.Extensions.Path2Improve.Views
{
    public partial class Credentials : Form
    {
        public Credentials()
        {
            InitializeComponent();
        }

        public string User { get { return txtUser.Text; } }
        public string Password { get { return txtPassword.Text; } }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password)) return;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
