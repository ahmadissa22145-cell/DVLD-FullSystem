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
    public partial class frmUserInfo : Form
    {
        private int _userID = -1;

        public int UserID {get {return _userID;}}

        public clsUser UserInfo { get { return ctrlUserCard1.UserInfo; } }

        public frmUserInfo(int userID)
        {
            InitializeComponent();

            if (userID <= 0)
            {
                MessageBox.Show("Enter User ID Bigger Than 0", "User ID Not Valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            _userID = userID;
            
        }

        private void frmUserInfo_Load(object sender, EventArgs e)
        {
            ctrlUserCard1.LoadUserInfo(this, _userID);

            if (!ctrlUserCard1.IsDataLoaded)
            {
                this.Close();
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
