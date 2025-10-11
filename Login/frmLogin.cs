using DVLD.Global_Classes;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.LogIn
{

    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if(username == string.Empty || password == string.Empty)
            {
                MessageBox.Show("Please filled username and password ", "Invalid input data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsUser user = clsUser.FindByUsernameAndPassword(username, password);

            if (user == null) 
            {
                MessageBox.Show("please check username or password and try again", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (chkRememberMe.Checked)
            {
                clsGlobal.RememberUsernameAndPasswordInsideRegistry(username, password);
            }
            else
            {
                clsGlobal.RememberUsernameAndPasswordInsideRegistry(string.Empty, string.Empty);
            }

            if (!user.IsActive)
            {
                MessageBox.Show("this account is Inactive please contact with admin", "Inactive Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsGlobal.CurrentUserInfo = user;

            this.Hide();
            frmMain Main = new frmMain(this);
            Main.ShowDialog();
        }


        private void frmLogin_Load(object sender, EventArgs e)
        {
            string username = string.Empty , password = string.Empty;

            if (clsGlobal.GetStoredCredentialsDataFromRegistry(ref username, ref password))
            {
                txtUserName.Text = username;
                txtPassword.Text = password;
                chkRememberMe.Checked = true;
            }
            else
            {
                chkRememberMe.Checked = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
