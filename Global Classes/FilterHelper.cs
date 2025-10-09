using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    public static class FilterHelper
    {

        public static int ApplyFilter(string filterQuery, DataTable dt)
        {

            dt.DefaultView.RowFilter = filterQuery;

            return dt.DefaultView.Count;
        }
    }
}
