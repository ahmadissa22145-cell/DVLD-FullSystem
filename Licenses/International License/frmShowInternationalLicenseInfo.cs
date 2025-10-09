using System;
using System.Windows.Forms;

namespace DVLD.Licenses.International_License
{
    public partial class frmShowInternationalLicenseInfo : Form
    {
        int _internationalLicenseID = -1;
        public frmShowInternationalLicenseInfo(int internationalLicenseID)
        {
            InitializeComponent();

            _internationalLicenseID = internationalLicenseID;
        }

        private void frmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlInternationalLicenseInfo1.LoadInfo(_internationalLicenseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
