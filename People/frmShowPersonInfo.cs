using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People
{
    public partial class frmShowPersonInfo : Form
    {
        public frmShowPersonInfo(int personID)
        {
            InitializeComponent();
            ctrlPersonCard1.LoadPersonInfo(this, personID);
        }

        public frmShowPersonInfo(string nationalNo)
        {
            InitializeComponent();
            ctrlPersonCard1.LoadPersonInfo(this, nationalNo);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
