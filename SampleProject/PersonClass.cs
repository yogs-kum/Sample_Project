using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SampleProject
{
   public static class PersonClass
    {

        public static DataSet FillAllAddresses(int ID=-1)
        {
            DataSet ds = new DataSet();
            try
            {
                var connstring = ConfigurationManager.ConnectionStrings["PeopleDB"].ConnectionString;
                SqlConnection con = new SqlConnection(connstring);
                SqlCommand comm = new SqlCommand("FillAllAddresses", con);
                if (ID!=-1)
                comm.Parameters.AddWithValue("@pID", ID);
                comm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(comm);
                
                sda.Fill(ds);

                
                

            }
            catch (Exception ex) { };
            return ds;
        }

        public static DataSet FillNames(string firstname = "", string lastname = "")
        {
            DataSet ds = new DataSet();
            try
            {
                var connstring = ConfigurationManager.ConnectionStrings["PeopleDB"].ConnectionString;
                SqlConnection con = new SqlConnection(connstring);
                SqlCommand comm = new SqlCommand("FillAllNames", con);
                if (firstname != "")
                    //comm.Parameters.AddWithValue("@Fname", DBNull.Value);
                //else
                    comm.Parameters.AddWithValue("@Fname", firstname);
                if (lastname != "")
                    //comm.Parameters.AddWithValue("@Lname", DBNull.Value);
                //else
                    comm.Parameters.AddWithValue("@Lname", lastname);
                comm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(comm);
                //DataSet ds = new DataSet();
                sda.Fill(ds);
                //grdNames.DataSource = ds.Tables[0];

            }
            catch (Exception ex) { };
            return ds;
        }

        public static DataSet FillCountryNames()
        {
            DataSet ds = new DataSet();
            try
            {
                var connstring = ConfigurationManager.ConnectionStrings["PeopleDB"].ConnectionString;
                SqlConnection con = new SqlConnection(connstring);
                SqlCommand comm = new SqlCommand("FillAllCountries", con);
              
                comm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(comm);

                sda.Fill(ds);




            }
            catch (Exception ex) { };
            return ds;
        }
    }
}
