using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User
{
    public partial class frmAddEditUser : Form
    {
        public enum enMode {AddNew , Update};

        private enMode Mode;
        public int UserID { get; private set; } = -1;
        public clsUser User {  get; private set; }

        public frmAddEditUser()
        {
            InitializeComponent();

            Mode = enMode.AddNew;
        }
        public frmAddEditUser(int userID)
        {
            InitializeComponent();

            UserID = userID;
            Mode = enMode.Update;
        }


        private void _ResetDefulatValues()
        {
            if (this.Mode == enMode.AddNew)
            {
                User = new clsUser();
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";

                tbLoginInfo.Enabled = false;
                btnSave.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                tbLoginInfo.Enabled = true;
                btnSave.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tbLoginInfo"];
            }

            txtUserName.Text = txtPassword.Text = txtConfirmPassword.Text = string.Empty;
            chkIsActive.Checked = true;
        }

        private void _LoadData()
        {
            User = clsUser.FindByUserID(this.UserID);

            if (User == null)
            {
                MessageBox.Show($"Not found any user with user ID = {this.UserID}", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(User.PersonID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            lblUserID.Text = User.UserID.ToString();
            txtUserName.Text = User.UserName;
            txtPassword.Text = User.Password;
            txtConfirmPassword.Text = User.Password;
            chkIsActive.Checked = User.IsActive;

        }

        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            _ResetDefulatValues();

            if (Mode == enMode.Update)
                _LoadData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int personID = ctrlPersonCardWithFilter1.PersonID;

            if (Mode == enMode.AddNew)
            {
                if (personID != -1)
                {
                    if (clsUser.DoesPersonHaveUser(personID))
                    {
                        MessageBox.Show("You cannot create a new account because you already have one.", "Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ctrlPersonCardWithFilter1.FilterFocus();
                        return;
                    }

                    btnSave.Enabled = true;
                    tbLoginInfo.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Please select a person", "Select a Person ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ctrlPersonCardWithFilter1.FilterFocus();
                    return;
                }
            }

            tcUserInfo.SelectedTab = tcUserInfo.TabPages["tbLoginInfo"];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid , put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            User.UserName = txtUserName.Text.Trim();   
            User.Password = txtPassword.Text.Trim();
            User.IsActive = chkIsActive.Checked;

            if (!User.Save())
            {
                MessageBox.Show("An error oocured , The save process was not completed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Data Saved Successfully", "Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (this.Mode == enMode.AddNew)
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                lblUserID.Text = User.UserID.ToString();
                ctrlPersonCardWithFilter1.FilterEnabled = false;

                this.Mode = enMode.Update;
            }

            
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            string username = txtUserName.Text?.Trim();

            if (string.IsNullOrEmpty(username))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "this field is requred");
                return;
            }

            if (Mode == enMode.AddNew)
            {
                if (clsUser.IsUserExist(username))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "username is alredy taken , choice another one");
                    return;
                }
            }
            else
            {
                if (User.UserName != username && clsUser.IsUserExist(username))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "username is alredy taken , choice another one");
                    return;
                }
            }

                errorProvider1.SetError(txtUserName, null);
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            string password = txtPassword.Text?.Trim();

            if (string.IsNullOrEmpty(password))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "this field is requred");
                return;
            }

            if(password.Length < 8)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password must be longer than 8 characters");
                return;
            }

            errorProvider1.SetError(txtPassword, null);
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(confirmPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "this field is required");
                return;
            }

            if (!confirmPassword.Equals(password))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password mismatch\n Please re-enter both fields");
                return;
            }

            errorProvider1.SetError(txtConfirmPassword, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
