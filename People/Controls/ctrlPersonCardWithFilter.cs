using DVLD.Global_Classes;
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

namespace DVLD.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        public event Action<int> OnPersonSelected;

        protected virtual void PersonSelcted(int personID)
        {
            Action<int> handler = OnPersonSelected;

            if(handler != null)
            {
                handler(personID); 
            }
        }

        private bool _ShowAddNewPerson = true;

        public bool ShowAddNewPerson 
        {
            get { return _ShowAddNewPerson; }

            set
            {
                _ShowAddNewPerson = value;
                btnAddNewPerson.Visible = _ShowAddNewPerson;
            }
        }


        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get { return _FilterEnabled; }

            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }
        
        public int PersonID { get { return ctrlPersonCard1.PersonID; } }

        public clsPerson SelctedPersonInfo { get { return ctrlPersonCard1.SelctedPersonInfo; } }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int PersonID)
        {

            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();

            _FindNow(PersonID.ToString());

        }

        private void _FindNow(string filterText)
        {

            switch (cbFilterBy.Text)
            {
                case "Person ID":

                    ctrlPersonCard1.LoadPersonInfo(this, int.Parse(filterText));
                    break;

                case "National No":

                    ctrlPersonCard1.LoadPersonInfo(this, filterText);
                    break;

                default:
                    break;
            }

            if (gbFilters.Enabled )
            {
                PersonSelcted(PersonID);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string filterValue = txtFilterValue.Text.Trim();

            _FindNow(filterValue);
        }

        private void DataBackEvent(object sender, int personID)
        {

            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = personID.ToString();

            _FindNow(personID.ToString());
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson AddUpdatePerson = new frmAddUpdatePerson();

            AddUpdatePerson.DataBack += DataBackEvent;


            AddUpdatePerson.ShowDialog();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                btnFind.PerformClick();
                return;
            }

            if(cbFilterBy.Text == "Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }


        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterValue.Text))
            {

                errorProvider1.SetError(txtFilterValue, "Please enter a value to search, this field is required");
                return;
            }

            errorProvider1.SetError(txtFilterValue, null);

        }
        
        public void ResetPersonCard()
        {
            ctrlPersonCard1.ResetPersonInfo();
        }

        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = string.Empty;
            txtFilterValue.Focus();
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 1;
        }


    }
}
