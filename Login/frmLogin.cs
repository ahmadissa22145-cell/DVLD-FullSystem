using DVLD.Global_Classes;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Shared_clsGlobal = DVLD_Shared.clsGlobal;
using Presentation_clsGlobal = DVLD.Global_Classes.clsGlobal;
using DVLD_Shared;

namespace DVLD.LogIn
{

    public partial class frmLogin : Form
    {
        static int eventID = 0;

        private int _failedLoginAttempts = 0;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
          
            if (_failedLoginAttempts >= 3)
            {
                clsLogger.LogIntoEventViewer(Shared_clsGlobal.source, "Login attempt after 3 failed attempts", type : EventLogEntryType.Warning);
                
                MessageBox.Show("Your Account is Locked After 3 trials to log in", "Your Account Is Locked!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

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
                ++_failedLoginAttempts;
                MessageBox.Show("please check username or password and try again", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (chkRememberMe.Checked)
            {
                Presentation_clsGlobal.RememberUsernameAndPasswordInsideRegistry(username, password);
            }
            else
            {
                Presentation_clsGlobal.RememberUsernameAndPasswordInsideRegistry(string.Empty, string.Empty);
            }

            if (!user.IsActive)
            {
                MessageBox.Show("this account is Inactive please contact with admin", "Inactive Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Presentation_clsGlobal.CurrentUserInfo = user;

            this.Hide();
            frmMain Main = new frmMain(this);
            Main.ShowDialog();

            if (!chkRememberMe.Checked)
            {
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
            }
        }


        private void frmLogin_Load(object sender, EventArgs e)
        {
            string username = string.Empty , password = string.Empty;

            if (Presentation_clsGlobal.GetStoredCredentialsDataFromRegistry(ref username, ref password))
            {
                txtUserName.Text = username;
                txtPassword.Text = password;
                chkRememberMe.Checked = true;
            }
            else
            {

                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                chkRememberMe.Checked = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
