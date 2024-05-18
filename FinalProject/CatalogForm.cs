using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class CatalogForm : Form
    {
        string path = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\shall\OneDrive - Holon Institute of Technology\מסמכים\librarydb.mdf;Integrated Security=True;Connect Timeout=30"; 
        SqlConnection con;
        DataTable dt;
        SqlDataAdapter adpt;
        public CatalogForm()
        {
            InitializeComponent();
            con = new SqlConnection(path);
            Display();
        }

        public void Display()
        {
            try
            {
                dt = new DataTable();
                con.Open();
                adpt = new SqlDataAdapter("select * from CatalogBooks", con);
                adpt.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" the problem is :" + ex.Message);
            }
        }

        private void txt_search(object sender, EventArgs e)
        {
            con.Open();
            adpt = new SqlDataAdapter("select * from CatalogBooks where Name_of_book like '%" + textBox1.Text + "%' ", con);
            dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }
    }
}
