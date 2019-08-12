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
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SampleProject
{
    public partial class CRUDPerson : Form
    {
        public CRUDPerson()
        {
            InitializeComponent();
            dsPerson = PersonClass.FillNames();
            label5.Visible = false;
            btnAddress.Visible = false;
        }
        public CRUDPerson(int pID)
        {
            InitializeComponent();
            dsPerson = PersonClass.FillNames();
            lbID.Text = pID.ToString();
            DataRow row = dsPerson.Tables[0].Select("ID = " + pID + "").FirstOrDefault();
            txtFirstName.Text = row[0].ToString();
            txtLastName.Text = row[1].ToString();

            dpDOB.Value = DateTime.ParseExact(row[2].ToString(),"dd/MM/yyyy",CultureInfo.InvariantCulture);
            txtNickName.Text = row[3].ToString();

        }

        private void CRUDPerson_Load(object sender, EventArgs e)
        {
            

        }

        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtFirstName.Text.Trim() == "")
            {
                lbFirstName.Visible = true;
                return;
            }
             if (txtLastName.Text.Trim() == "")
            {
                lbLastName.Visible = true;
                return;
            }
             if (dpDOB.Value > DateTime.Today)
            {
                lbDOB.Visible = true;
                return;

            }
             if (dsPerson.Tables[0].Select("[First Name] = '"+txtFirstName.Text.Trim()+"' and [Last Name] = '"+txtLastName.Text.Trim()+"'").FirstOrDefault()!= null && lbID.Text.Trim()=="")
            {
                lbFirstName.Visible = true;
                lbLastName.Visible = true;
                MessageBox.Show("Person Already Exists","Error");
                return;

            }

            SavePerson();
            ClearAll();
            this.Close();
        }

       
        private void txtNickName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public void SavePerson()
        {
            int result = 0;
            try
            {
                var connstring = ConfigurationManager.ConnectionStrings["PeopleDB"].ConnectionString;
                SqlConnection con = new SqlConnection(connstring);
                SqlCommand comm = new SqlCommand("AddEditPerson", con);
                if (lbID.Text.Trim()!="")
                //if (firstname == "")
                    comm.Parameters.AddWithValue("@Pid", Convert.ToInt32(lbID.Text.Trim()));
                comm.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                comm.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                comm.Parameters.AddWithValue("@DOB", dpDOB.Value);
                comm.Parameters.AddWithValue("@NickName", txtNickName.Text.Trim());
                //else
                //    comm.Parameters.AddWithValue("@Fname", firstname);
                //if (lastname == "")
                //    comm.Parameters.AddWithValue("@Lname", DBNull.Value);
                //else
                //    comm.Parameters.AddWithValue("@Lname", lastname);
                comm.CommandType = CommandType.StoredProcedure;
                con.Open();
                result = comm.ExecuteNonQuery();
                con.Close();
                //SqlDataAdapter sda = new SqlDataAdapter(comm);
                ////DataSet ds = new DataSet();
                //sda.Fill(ds);
                //grdNames.DataSource = ds.Tables[0];

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); return; };

            if (result > 0)
            {
                MessageBox.Show("Person Added Succesfully", "Success");
                Clear();
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ClearAll()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            dpDOB.Value = DateTime.Today;
            txtNickName.Text = "";
            lbID.Text = "";
        }

        public void Clear()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            dpDOB.Value = DateTime.Today;
            txtNickName.Text = "";
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (lbID.Text == "")
                ClearAll();
            else
                Clear();
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            if (lbID.Text.Trim()!= "")
            {
                CRUDAddress crdper = new CRUDAddress(Convert.ToInt32(lbID.Text.Trim()));
                crdper.ShowDialog(this);
            }
            else
            {
                CRUDAddress crdper = new CRUDAddress();
                crdper.ShowDialog(this);
            }
        }
    }
}
