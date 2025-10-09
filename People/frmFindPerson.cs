using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People
{
    public partial class frmFindPerson : Form
    {
        public delegate void DataBackEventHandler(Object sender, int personID);

        public event DataBackEventHandler DataBack;

        public int PersonID { get { return ctrlPersonCardWithFilter1.PersonID; } }
        public frmFindPerson()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DataBack?.Invoke(this, PersonID);
            this.Close();
        }
    }
}
