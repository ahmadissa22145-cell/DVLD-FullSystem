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

namespace DVLD.Tests.Test_Types
{

    public partial class frmListTestTypes : Form
    {

        private DataTable _dtTestTypes;
        public frmListTestTypes()
        {
            InitializeComponent();
        }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _dtTestTypes = clsTestType.GetAllTestTypes();

            if (_dtTestTypes.Rows.Count <= 0)
            {
                MessageBox.Show("No Application Types found in the system.",
                "No Data",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                return;
            }

            dgvTestTypes.DataSource = _dtTestTypes;

            lblRecordsCount.Text = dgvTestTypes.Rows.Count.ToString();

            dgvTestTypes.Columns[0].HeaderText = "ID";
            dgvTestTypes.Columns[0].Width = 120;

            dgvTestTypes.Columns[1].HeaderText = "Titel";
            dgvTestTypes.Columns[1].Width = 180;

            dgvTestTypes.Columns[2].HeaderText = "Discription";
            dgvTestTypes.Columns[2].Width = 450;

            dgvTestTypes.Columns[3].HeaderText = "Fees";
            dgvTestTypes.Columns[3].Width = 120;

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsTestType.enTestTypes testTypeID = (clsTestType.enTestTypes)dgvTestTypes.CurrentRow.Cells[0].Value;

            frmEditTestType editTestType = new frmEditTestType(testTypeID);

            editTestType.ShowDialog();

            frmListTestTypes_Load(null, null);
        }

        private void dgvTestTypes_DoubleClick(object sender, EventArgs e)
        {
            editToolStripMenuItem_Click(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();  
        }
    }
}
