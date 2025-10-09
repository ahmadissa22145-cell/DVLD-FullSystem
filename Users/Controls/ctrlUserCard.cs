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

namespace DVLD.User.Controls
{
    public partial class ctrlUserCard : UserControl
    {

        private int _userID  = -1;

        public int UserID { get { return _userID; } }

        private clsUser _user = null;

        public clsUser UserInfo { get { return _user; } }

        public bool IsDataLoaded { get; private set; }

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(object sender, int userID)
        {
            _user = clsUser.FindByUserID(userID);

            if (_user == null)
            {
                MessageBox.Show($"Not found any user with user ID = {userID}, Please enter another one", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ResetUserInfo();
                IsDataLoaded = false;
                return;
            }

            _userID = userID;
            _FillUserInfo();

            IsDataLoaded = true;
        }

        private void _ResetUserInfo()
        {
            ctrlPersonCard1.ResetPersonInfo();

            lblUserID.Text = lblUsername.Text = lblIsActive.Text = "????";
        }

        private void _FillUserInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(this, _user.PersonID);

            lblUserID.Text = _user.UserID.ToString();
            lblUsername.Text = _user.UserName.ToString();
            lblIsActive.Text = _user.IsActive ? "Yes" : "No";
        }

        
    }
}
