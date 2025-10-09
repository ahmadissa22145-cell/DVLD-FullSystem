using DVLD.User.Controls;
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

namespace DVLD.User
{
    public partial class frmListUsers : Form
    {

        private DataTable dtUsers = clsUser.GetAllUsers();

        
        public frmListUsers()
        {
            InitializeComponent();
        }

        private void _RefershUsersList()
        {
            dtUsers = clsUser.GetAllUsers();

            dtUsers = dtUsers.DefaultView.ToTable(false,"UserID","PersonID","FullName","UserName","isActive");

            dgvUsers.DataSource = dtUsers;

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();  

        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            
            cbFilterBy.SelectedIndex = 0;   
               
            dgvUsers.DataSource = dtUsers;
            int recordsCount = dgvUsers.Rows.Count;
            lblRecordsCount.Text = recordsCount.ToString();

            if(recordsCount > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].Width = 120;

                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].Width = 120;

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].Width = 350;

                dgvUsers.Columns[3].HeaderText = "User Name";
                dgvUsers.Columns[3].Width = 140;

                dgvUsers.Columns[4].HeaderText = "Is Active";
                dgvUsers.Columns[4].Width = 130;
                
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filterName = cbFilterBy.Text;

            if (filterName.Equals("Is Active"))
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
                cbIsActive.Focus();
                return;
            }


            txtFilterValue.Visible = (!filterName.Equals("None"));

            cbIsActive.Visible = false;
            txtFilterValue.Text = string.Empty;
            txtFilterValue.Focus();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // if user press on enter 
            if(e.KeyChar == (char)13)
            {
                dgvUsers.Focus();
            }

            switch (cbFilterBy.Text)
            {
                case "User ID":
                case "Person ID":

                    e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                    break;

                case "Full Name":
                    e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
                    break;
            }
        }

        private string _GetActuelColumnNameToCBFilterBy(string filterColumnName)
        {
            switch (filterColumnName)
            {
                case "User ID":
                    return "UserID";

                case "Person ID":
                    return "PersonID";

                case "Full Name":
                    return "FullName";

                case "UserName":
                    return "UserName";

                case "Is Active":
                    return "IsActive";

                default:
                    return "None";
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filterColumn = "IsActive";
            string filterValue = string.Empty;

            switch (cbIsActive.Text)
            {
                case "Yes":
                    filterValue = "1";
                    break;

                case "No":
                    filterValue = "0";
                    break;

                default:
                    break;
            }

            if(filterValue == string.Empty)
                dtUsers.DefaultView.RowFilter = string.Empty;
            else
                dtUsers.DefaultView.RowFilter = string.Format("{0} = {1}", filterColumn, filterValue);


            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumnName = _GetActuelColumnNameToCBFilterBy(cbFilterBy.Text);
            string filterValue = txtFilterValue.Text?.Trim();

            if(string.IsNullOrWhiteSpace(filterValue))
            {
                dtUsers.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = dtUsers.Rows.Count.ToString();
                return;
            }

            if(filterColumnName == "PersonID" || filterColumnName == "UserID")
            {
                dtUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterColumnName, int.Parse(filterValue));
               
            }
            else
            {
                dtUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", filterColumnName, filterValue);
            }

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddEditUser addEditUser = new frmAddEditUser();
            addEditUser.ShowDialog();
            _RefershUsersList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object cellValue = dgvUsers.CurrentRow?.Cells[0].Value;

            if(cellValue == null || !int.TryParse(cellValue.ToString(),out int userID))
            {
                MessageBox.Show("Invalid User ID. Please select a valid row",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            frmUserInfo userInfo = new frmUserInfo(userID);

            userInfo.ShowDialog();

            _RefershUsersList();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddEditUser addEditUser = new frmAddEditUser();
            addEditUser.ShowDialog();
            _RefershUsersList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object cellValue = dgvUsers.CurrentRow?.Cells[0].Value;

            if (cellValue == null || !int.TryParse(cellValue.ToString(), out int userID))
            {
                MessageBox.Show("Invalid User ID. Please select a valid row",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            frmAddEditUser addEditUser = new frmAddEditUser(userID);
            addEditUser.ShowDialog();
            _RefershUsersList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object cellValue = dgvUsers.CurrentRow?.Cells[0].Value;

            if (cellValue == null || !int.TryParse(cellValue.ToString(), out int userID))
            {
                MessageBox.Show("Invalid User ID. Please select a valid row",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"Are you sure do you want delete user with user id = {userID}",
                                                    "Confirm deletion",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                if (!clsUser.DeleteUser(userID))
                {
                    MessageBox.Show("An error occurred , user with user id = {user ID} was not deleted",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                _RefershUsersList();

                MessageBox.Show("User deleted successfully",
                                   "deleted successfully",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
            }
        }

        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            object cellValue = dgvUsers.CurrentRow?.Cells[0].Value;

            if (cellValue == null || !int.TryParse(cellValue.ToString(), out int userID))
            {
                MessageBox.Show("Invalid User ID. Please select a valid row",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }


            frmChangePassword changePassword = new frmChangePassword(userID);
            changePassword.ShowDialog();

            _RefershUsersList();
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            object cellValue = dgvUsers.CurrentRow?.Cells[0].Value;

            if (cellValue == null || !int.TryParse(cellValue.ToString(), out int userID))
            {
                MessageBox.Show("Invalid User ID. Please select a valid row",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            frmUserInfo userInfo = new frmUserInfo(userID);

            userInfo.ShowDialog();

            _RefershUsersList();
        }
    }
}
