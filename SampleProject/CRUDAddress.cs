using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace SampleProject
{
    public partial class CRUDAddress : Form
    {
        public CRUDAddress()
        {
            InitializeComponent();
            BindCountry();
        }

        public CRUDAddress(int pID)
        {
            InitializeComponent();
            BindCountry();
            lbPersonId.Text = pID.ToString();
            dsAddress = PersonClass.FillAllAddresses(pID);
        }

        public CRUDAddress(int pID,int ID)
        {
            InitializeComponent();
            lbPersonId.Text = pID.ToString();
            lbID.Text = ID.ToString();
            dsAddress = PersonClass.FillAllAddresses(pID);
            BindCountry();
            DataRow row = dsAddress.Tables[0].Select("ID = " + ID + "").FirstOrDefault();
            txtAddressLine1.Text = row[0].ToString();
            txtAddressLine2.Text = row[1].ToString();

            cmbCountry.Text= row[2].ToString().Trim();
            txtPostCode.Text = row[3].ToString();
        }
        private void CRUDAddress_Load(object sender, EventArgs e)
        {
            
        }

        private void txtAddressLine2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public void BindCountry()
        {
            DataSet dsCountry = PersonClass.FillCountryNames();
            cmbCountry.DataSource = dsCountry.Tables[0];
            cmbCountry.DisplayMember = "CountryName";
            cmbCountry.ValueMember = "CountryID";
        }

        private void txtPostCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtAddressLine1.Text.Trim() == "")
            {
                lbAddressLine1.Visible = true;
                return;
            }
            if (dsAddress.Tables.Count>0 )
            {
                if (dsAddress.Tables[0].Select("[Line 1] = '" + txtAddressLine1.Text.Trim() + "' and [Line 2] = '" + txtAddressLine2.Text.Trim() + "' and [Country] = '" + cmbCountry.Text.Trim() + "' and [PIN] = '" + txtPostCode.Text.Trim() + "'").FirstOrDefault() != null)
                {

                    MessageBox.Show("Address Already Exists", "Error");
                    return;

                }
            }
            SaveAddress();
            Clear();
            this.Close();
        }

        public void SaveAddress()
        {
            int result = 0;
            try
            {
                var connstring = ConfigurationManager.ConnectionStrings["PeopleDB"].ConnectionString;
                SqlConnection con = new SqlConnection(connstring);
                SqlCommand comm = new SqlCommand("AddEditAddress", con);
                if (lbID.Text.Trim() != "")
                comm.Parameters.AddWithValue("@Id", Convert.ToInt32(lbID.Text.Trim()));
                comm.Parameters.AddWithValue("@Pid", Convert.ToInt32(lbPersonId.Text.Trim()));
                comm.Parameters.AddWithValue("@Line1", txtAddressLine1.Text.Trim());
                comm.Parameters.AddWithValue("@Line2", txtAddressLine2.Text.Trim());
                comm.Parameters.AddWithValue("@Country", cmbCountry.Text);
                comm.Parameters.AddWithValue("@Postcode", txtPostCode.Text.Trim());
                
                comm.CommandType = CommandType.StoredProcedure;
                con.Open();
                result = comm.ExecuteNonQuery();
                con.Close();
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); return; };

            if (result > 0)
            {
                MessageBox.Show("Address Added Succesfully", "Success");
                Clear();
                this.Close();
            }
        }

        public void Clear()
        {
            txtAddressLine1.Text = "";
            txtAddressLine2.Text = "";
            cmbCountry.SelectedValue = -1;
            txtPostCode.Text = "";

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (lbID.Text.Trim() != "")
            {
                if (DeleteAddress() > 0)
                {
                    MessageBox.Show("Address Deleted Succesfully", "Message");
                    
                }
                Clear();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please double click Address to delete", "Message");

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public int DeleteAddress()
        {
            int result = 0;
            try
            {
                var connstring = ConfigurationManager.ConnectionStrings["PeopleDB"].ConnectionString;
                SqlConnection con = new SqlConnection(connstring);
                SqlCommand comm = new SqlCommand("DeleteAddress", con);

                comm.Parameters.AddWithValue("@ID", Convert.ToInt32(lbID.Text.Trim()));
                

                comm.CommandType = CommandType.StoredProcedure;
                con.Open();
                result = comm.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex) { };
            return result;
        }
    }
}
