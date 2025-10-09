using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Local_Licenses
{
    public partial class frmShowLicenseInfo : Form
    {
        private readonly int _LicenseID;

        public frmShowLicenseInfo(int licenseID)
        {
            InitializeComponent();
            _LicenseID = licenseID;
        }

        private void frmShowLicenseInfo_Load(object sender, EventArgs e)
        {
            if (!ctrlDriverLicenseInfo1.LoadControlInfo(_LicenseID)) this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
