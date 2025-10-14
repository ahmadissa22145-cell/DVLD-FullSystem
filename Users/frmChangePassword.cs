using DVLD.Global_Classes;
using DVLD_Buisness;
using DVLD_Business;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.User
{
    public partial class frmChangePassword : Form
    {


        private int _userID = -1;

        private clsUser UserInfo { get { return ctrlUserCard1.UserInfo; } }

        public frmChangePassword(int userID)
        {
            InitializeComponent();

            if (userID < 0)
            {
                MessageBox.Show("Please enter a valid user id and must be positive number", "User ID Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _userID = userID;
        }

        private void _ResetDefultValues()
        {

            txtCurrentPassword.Text = txtNewPassword.Text = txtConfirmPassword.Text = string.Empty;

            txtCurrentPassword.Focus();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefultValues();

            if (!clsUser.IsUserExist(_userID))
            {
                MessageBox.Show($"No user found with user id = {_userID}", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlUserCard1.LoadUserInfo(this, _userID);

            if (!ctrlUserCard1.IsDataLoaded)
            {
                this.Close();
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text?.Trim();

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid , put the mouse over the red icon(s) to see the error",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (!clsUser.ChangePassword(_userID, newPassword, out string newHashedPassword))
            {

                MessageBox.Show("An error occured : change password process not completed",
                     "Process Error",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                return;
            }

            clsGlobal.CurrentUserInfo.Password = newHashedPassword;
            
            MessageBox.Show("Password changed successfully",
                "Password changed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            string currentPassword = txtCurrentPassword.Text?.Trim();

            if (currentPassword == string.Empty)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "this field is requried, please fill it");
                return;
            }

            string currentHashPassword = clsHasingData.HashCompute(currentPassword);

            if (!UserInfo.Password.Equals(currentHashPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current password is incorrect");
                return;
            }

            errorProvider1.SetError(txtCurrentPassword, null);
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            string newPassword = txtNewPassword.Text?.Trim();
            string confirmPassword = txtConfirmPassword.Text?.Trim();

            if (string.IsNullOrEmpty(newPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "this field is requred");
                return;
            }

            if (newPassword.Length < 8)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "Password must be longer than 8 characters");
                return;
            }

            if (!string.IsNullOrWhiteSpace(confirmPassword) && !confirmPassword.Equals(newPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password mismatch \n Please re-enter both fields");
                return;
            }

            errorProvider1.SetError(txtNewPassword, null);
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            string newPassword = txtNewPassword.Text?.Trim();
            string confirmPassword = txtConfirmPassword.Text?.Trim();

            if (string.IsNullOrEmpty(confirmPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "this field is requred");
                return;
            }

            if (!confirmPassword.Equals(newPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password mismatch \n Please re-enter both fields");
                return;
            }

            errorProvider1.SetError(txtConfirmPassword, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.AutoValidate = AutoValidate.Disable;
            this.Close();
        }
    }
}
