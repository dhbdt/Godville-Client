using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GodvilleClient
{
    public partial class LoginForm : Form
    {
        Model.LoginData loginData;
        public LoginForm(Model.LoginData loginData)
        {
            InitializeComponent();
            this.loginData = loginData;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            loginData.Login = txtLogin.Text;
            loginData.Password = txtPassword.Text;
        }

        private void awayLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
        }

        private void registerLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
