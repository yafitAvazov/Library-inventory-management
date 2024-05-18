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
using System.Runtime.Remoting.Contexts;

namespace FinalProject
{
    public partial class ReturningForm : Form
    {
        string path = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\shall\OneDrive - Holon Institute of Technology\מסמכים\librarydb.mdf;Integrated Security=True;Connect Timeout=30"; 
        SqlConnection con;
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter adpt;
        SqlDataReader reader;
        Book newbook;
        Math math;
        English english;
        ReadingBook read;
        public ReturningForm()
        {
            InitializeComponent();
            con = new SqlConnection(path);
            display();
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            string name;
            int type;
            if (this.textBoxID.Text == "")
            {
                MessageBox.Show("Please enter your ID!!");
            }
            else if (this.textBoxID.Text.Length > 9)
                MessageBox.Show("ID cannot be more then 9 characters!!");
            else
            {
                for (int i = 0; i < this.textBoxID.Text.Length; i++)
                {
                    if (this.textBoxID.Text[i] < 48 || this.textBoxID.Text[i] > 57)
                    {
                        MessageBox.Show("ID cannot be include with special characters such as: @ ! ");
                    }
                }
            }
            // checks if ID is Valid
            try
            {
                con.Open();
                cmd = new SqlCommand("select * from BorrowedBooks where id_user='" + textBoxID.Text +"'", con);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    try
                    {
                        dt = new DataTable();
                        adpt = new SqlDataAdapter("select name_of_book from BorrowedBooks where id_user='" + textBoxID.Text + "'", con);
                        adpt.Fill(dt);
                        name = dt.Rows[0].Field<string>("name_of_book");
                        cmd = new SqlCommand("delete from BorrowedBooks where id_user='" + textBoxID.Text + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("update CatalogBooks set is_Exists = 'Y' where name_of_book='" + name + "'", con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Your data has been Confirmed in our Database and you are free of charge");
                        con.Close();
                        display();
                        clear();
                        newbook = Form1.library.findInCatalog(name);
                        if (newbook == null)
                        {
                            MessageBox.Show("the book is not in our catalog!");
                        }
                        else
                        {
                            type = newbook.getType();
                            if(type == 1)
                            {
                                read = new ReadingBook(newbook.getNumOfPages(), newbook.getName(), newbook.getAuthor(), newbook.getPublishedYear(), newbook.getType(),"English");
                                Form1.library.RemoveFromBorrowing(read);
                            }
                            else
                            {
                                if(newbook.getSub() == "M")
                                {
                                    math = new Math(newbook.getNumOfPages(),newbook.getName(),newbook.getAuthor(),newbook.getPublishedYear(),newbook.getType(),getRandomgrade());
                                    Form1.library.RemoveFromBorrowing(math);
                                }
                                else
                                {
                                    english = new English(newbook.getNumOfPages(), newbook.getName(), newbook.getAuthor(), newbook.getPublishedYear(), newbook.getType(), getRandomgrade());
                                    Form1.library.RemoveFromBorrowing(english);
                                }
                            }
                            Form1.library.SaveSortedList();
                        }
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    reader.Close();
                    MessageBox.Show("Your id is incorrect! please check your ID!");
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void clear()
        {
            this.textBoxID.Text = "";
        }
        public void display()
        {
            try {
                dt = new DataTable();
                con.Open();
                adpt = new SqlDataAdapter("select * from BorrowedBooks", con);
                adpt.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(" the problem is :" + ex.Message);
            }
        }

        public int getRandomgrade()
        {
            return 1;
        }
    }
}
