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
    public partial class RegisterForm : Form
    {
        Model.RegisterData regData;
        public RegisterForm(Model.RegisterData regData)
        {
            InitializeComponent();
            this.regData = regData;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            regData.Login = txtLogin.Text;
            regData.Password = txtPassword.Text;
            regData.Nickname = txtNickname.Text;
        }
    }
}
