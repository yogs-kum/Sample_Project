using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;


namespace SampleProject
{
    public partial class Person : Form
    {
        public Person()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            
            grdNames.DataSource = "";
            grdAddresses.DataSource = false;
            grdAddresses.Visible = false;
            
        }

        private void Person_Load(object sender, EventArgs e)
        {
            PersonClass.FillNames();
        }



       

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataSet dsNames=PersonClass.FillNames(txtFirstName.Text.Trim(), txtLastName.Text.Trim());
            grdNames.DataSource = dsNames.Tables[0];
            grdAddresses.DataSource = null;
        }

        private void grdNames_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            grdNames.Columns[0].Width = (grdNames.Width / 4);
            grdNames.Columns[1].Width = (grdNames.Width / 4);
            grdNames.Columns[2].Width = (grdNames.Width / 4)-2;
            grdNames.Columns[3].Width = (grdNames.Width / 4);
            grdNames.Columns[4].Visible = false;

        }

        private void grdNames_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataSet dsAddress = PersonClass.FillAllAddresses(Convert.ToInt32(grdNames.Rows[e.RowIndex].Cells[4].Value));
                //FillAllAddresses(Convert.ToInt32(grdNames.Rows[e.RowIndex].Cells[4].Value));
                if (dsAddress.Tables[0].Rows.Count >0)
                {
                    grdAddresses.DataSource = dsAddress.Tables[0];
                    grdAddresses.Visible = true;
                }
                else
                {
                    MessageBox.Show("No Address Found.Please Edit Person to Add Address", "Message");
                    grdAddresses.DataSource = null;
                }
            }
        }

      

        private void grdAddresses_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (grdAddresses.DataSource != null)
            {
                grdAddresses.Columns[0].Width = (grdAddresses.Width / 4);
                grdAddresses.Columns[1].Width = (grdAddresses.Width / 4);
                grdAddresses.Columns[2].Width = (grdAddresses.Width / 4) - 2;
                grdAddresses.Columns[3].Width = (grdAddresses.Width / 4);
                grdAddresses.Columns[4].Visible = false;
                grdAddresses.Columns[5].Visible = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CRUDPerson crdPer = new CRUDPerson();
           
            crdPer.ShowDialog(this);
            btnSearch_Click(sender, e);

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdNames.Rows.Count > 0)
            {
                if (grdNames.CurrentRow.Index > -1)
                {
                    CRUDPerson crdper = new CRUDPerson(Convert.ToInt32(grdNames.CurrentRow.Cells[4].Value));
                    crdper.ShowDialog(this);
                    btnSearch_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Please select Person", "Error");
                btnSearch_Click(sender, e);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdNames.CurrentRow.Index > -1)
            {
               if (DeletePerson(grdNames.Rows[grdNames.CurrentRow.Index].Cells[4].Value)>0)
                {
                    MessageBox.Show("Person Deleted Succesfully", "Message");
                    grdAddresses.DataSource = null;
                    grdAddresses.Visible = false;
                    btnSearch_Click(sender, e);
                }
            }
        }

        public int DeletePerson(object pID)
        {
            int result = 0;
            try
            {
                var connstring = ConfigurationManager.ConnectionStrings["PeopleDB"].ConnectionString;
                SqlConnection con = new SqlConnection(connstring);
                SqlCommand comm = new SqlCommand("DeletePerson", con);
                
                    comm.Parameters.AddWithValue("@Pid", pID);
               
                comm.CommandType = CommandType.StoredProcedure;
                con.Open();
                result = comm.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex) { };
            return result;
        }

        private void grdAddresses_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (grdAddresses.CurrentRow.Index > -1)
            {
                CRUDAddress crdper = new CRUDAddress(Convert.ToInt32(grdAddresses.CurrentRow.Cells[5].Value), Convert.ToInt32(grdAddresses.CurrentRow.Cells[4].Value));
                crdper.ShowDialog(this);
                btnSearch_Click(this, e);
            }
        }

        private void btnExportPer_Click(object sender, EventArgs e)
        {
            if (grdNames.DataSource != null)
            {
                DataTable dt = (DataTable)grdNames.DataSource;
                SaveFileDialog sav = new SaveFileDialog();
                sav.Filter = "XML|*.xml";
                if (sav.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dt.WriteXml(sav.FileName);
                        MessageBox.Show("File Saved Successfully", "Success");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }
                }
            }
        }

        private void btnExportAdr_Click(object sender, EventArgs e)
        {
            if (grdAddresses.DataSource != null)
            {
                DataTable dt = (DataTable)grdAddresses.DataSource;
                SaveFileDialog sav = new SaveFileDialog();
                sav.Filter = "XML|*.xml";
                if (sav.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dt.WriteXml(sav.FileName);
                        MessageBox.Show("File Saved Successfully", "Success");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }
                }
            }
        }
    }
}
