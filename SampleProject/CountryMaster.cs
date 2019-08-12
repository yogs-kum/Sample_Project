using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleProject
{
    public partial class CountryMaster : Form
    {
        public CountryMaster()
        {
            InitializeComponent();
        }

        private void CountryMaster_Load(object sender, EventArgs e)
        {
            DataSet dsCountry = PersonClass.FillCountryNames();
            lstCountryMaster.DataSource = dsCountry.Tables[0];
            lstCountryMaster.DisplayMember = "CountryName";
            lstCountryMaster.ValueMember = "CountryID";
        }

        private void lstCountryMaster_DoubleClick(object sender, EventArgs e)
        {
            txtCountryName.Text = lstCountryMaster.Text;
            lblCountryID.Text = lstCountryMaster.SelectedValue.ToString();
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }
    }
}
