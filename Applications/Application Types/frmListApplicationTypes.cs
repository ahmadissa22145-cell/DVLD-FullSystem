using DVLD.Applications.Application_Types;
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

namespace DVLD.Applications.Apllication_Types
{
    public partial class frmListApplicationTypes : Form
    {

        DataTable dtApplicationTypes ;

        public frmListApplicationTypes()
        {
            InitializeComponent();
        }

        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {

            dtApplicationTypes = clsApplicationType.GetAllApplicationTypes();

            if (dtApplicationTypes.Rows.Count <= 0)
            {
                MessageBox.Show("No Application Types found in the system.",
                "No Data",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                return;
            }

            dgvApplicationTypes.DataSource = dtApplicationTypes;

            dgvApplicationTypes.Columns[0].HeaderText = "ID";
            dgvApplicationTypes.Columns[0].Width = 110;

            dgvApplicationTypes.Columns[1].HeaderText = "Titel";
            dgvApplicationTypes.Columns[1].Width = 400;

            dgvApplicationTypes.Columns[2].HeaderText = "Fees";
            dgvApplicationTypes.Columns[2].Width = 120;

            lblRecordsCount.Text = dgvApplicationTypes.Rows.Count.ToString();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplictionTypeID = (int)dgvApplicationTypes.CurrentRow.Cells[0]?.Value;
            frmEditApplicationType editApplicationType = new frmEditApplicationType(ApplictionTypeID);

            editApplicationType.ShowDialog();

            frmListApplicationTypes_Load(null, null);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvApplicationTypes_DoubleClick(object sender, EventArgs e)
        {
            editToolStripMenuItem_Click(null, null);
        }
    }
}
