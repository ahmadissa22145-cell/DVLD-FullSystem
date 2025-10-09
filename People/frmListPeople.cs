using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People
{
    public partial class frmListPeople : Form
    {

        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();

        private static DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GenderCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

        private void _RefreshPeoplList()
        {
            _dtAllPeople = clsPerson.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GenderCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

            dgvPeople.DataSource = _dtPeople;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
        }

        public frmListPeople()
        {
            InitializeComponent();
        }

      
        private void frmListPeople_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            dgvPeople.DataSource = _dtPeople;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();

            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 110;

                dgvPeople.Columns[1].HeaderText = "National No.";
                dgvPeople.Columns[1].Width = 120;


                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 140;


                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 120;

                dgvPeople.Columns[6].HeaderText = "Gender";
                dgvPeople.Columns[6].Width = 120;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 120;


                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;


                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 170;
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = !(cbFilterBy.Text.Equals("None"));

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = string.Empty;
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 13 == Enter in Assci Code
            if(e.KeyChar == (char)13)
            {
                dgvPeople.Focus();
            }

            switch (cbFilterBy.Text)
            {
                case "Person ID":
                case "Phone":
                    e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                    break;

                case "First Name":
                case "Second Name":
                case "Third Name":
                case "Last Name":
                    e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
                    break;
            }
        }


        private string GetActualeFilterCoulmnName()
        {
            if (_dtPeople.Rows.Count > 0)
            {
                switch (cbFilterBy.Text)
                {
                    case "Person ID":
                        return "PersonID";

                    case "National No.":
                        return "NationalNo";

                    case "First Name":
                        return "FirstName";

                    case "Second Name":
                        return "SecondName";

                    case "Third Name":
                        return "ThirdName";

                    case "Last Name":
                        return "LastName";

                    case "Nationality":
                        return "Nationality";

                    case "Gender":
                        return "GenderCaption";

                    case "Email":
                        return "Email";

                    case "Phone":
                        return "Phone";

                    default:
                        return "None";

                }
            }
            return "None";
        }

        private bool TryDeleteImage(string imagePath, out string imgErr)
        {
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                try
                {
                    File.Delete(imagePath);

                }
                catch (Exception ex)
                {
                    imgErr = ex.Message;
                    return false;
                }
            }
            imgErr = string.Empty;
            return true;
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

            string filterColumn = GetActualeFilterCoulmnName();

            string filterValue = txtFilterValue.Text.Trim();

            if (filterColumn.Equals("None") || string.IsNullOrEmpty(filterValue))
            {
                _dtPeople.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
                return;
            }

            if (filterColumn == "PersonID")
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterColumn, int.Parse(filterValue));

            else
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", filterColumn, filterValue);

            lblRecordsCount.Text = dgvPeople.RowCount.ToString();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson addUpdatePerson = new frmAddUpdatePerson();

            addUpdatePerson.ShowDialog();

            _RefreshPeoplList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = (int)dgvPeople.CurrentRow.Cells[0].Value;

            frmShowPersonInfo showPersonInfo = new frmShowPersonInfo(personID);

            showPersonInfo.ShowDialog();
        }

        private void AddtoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson addUpdatePerson = new frmAddUpdatePerson();

            addUpdatePerson.ShowDialog();

            _RefreshPeoplList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = (int)dgvPeople.CurrentRow.Cells[0].Value;

            frmAddUpdatePerson addUpdatePerson = new frmAddUpdatePerson(personID);

            addUpdatePerson.ShowDialog();

            _RefreshPeoplList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = (int)dgvPeople.CurrentRow.Cells[0].Value;

            DataRow[] row = _dtAllPeople.Select($"PersonID = {personID}");
            string imagePath = string.Empty;


            if(row != null && row.Length > 0)
            {

                imagePath = row[0][14].ToString();

            }

            if (MessageBox.Show("Are you sure you want to delete Person [" + personID + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
             

                if (clsPerson.DeletePerson(personID))
                {
                    MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeoplList();


                    if (!TryDeleteImage(imagePath, out string imgErr))
                    {
                        MessageBox.Show(
                            "Person deleted, but the image file could not be deleted." +
                            "\nImage Path: " + imagePath +
                            "\nError: " + imgErr,
                            "Image Delete Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }

                else
                    MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvPeople_DoubleClick(object sender, EventArgs e)
        {
            int personID = (int)dgvPeople.CurrentRow.Cells[0].Value;

            frmShowPersonInfo showPersonInfo = new frmShowPersonInfo(personID);

            showPersonInfo.ShowDialog();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
