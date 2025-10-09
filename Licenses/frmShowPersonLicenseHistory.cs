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

namespace DVLD.Licenses
{
    public partial class frmShowPersonLicenseHistory : Form
    {
        private int _personID;

        public frmShowPersonLicenseHistory()
        {
            InitializeComponent();
        }

        public frmShowPersonLicenseHistory(int personID)
        {
            InitializeComponent();

            _personID = personID;
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            //Check if has person ID
            if (_personID != -1)
            {
                ctrlPersonCardWithFilter1.LoadPersonInfo(_personID);
                ctrlPersonCardWithFilter1.FilterEnabled = false;

            }
            else
            {
                ctrlPersonCardWithFilter1.FilterEnabled = true;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _personID = obj;

            if(_personID == -1)
            {
                ctrlDriverLicenses1.Clear();
                ctrlPersonCardWithFilter1.FilterEnabled = true;
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            ctrlPersonCardWithFilter1.FilterEnabled = false;
            ctrlDriverLicenses1.LoadDataByPersonID(_personID);
        }
    }
}
