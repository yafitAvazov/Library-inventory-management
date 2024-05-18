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
    public partial class AddNewBookForm : Form
    {
        string path = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\shall\OneDrive - Holon Institute of Technology\מסמכים\librarydb.mdf;Integrated Security=True;Connect Timeout=30"; 
        SqlConnection con;
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter adpt;
        ReadingBook readingBook = null;
        Math mathBook = null;
        English englishBook = null;
        int type;
        public AddNewBookForm()
        {
            InitializeComponent();
            con = new SqlConnection(path);
            Display();
        }

        public void Clear()
        {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.radioButton1.Checked = false;
            this.radioButton2.Checked = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            int year = 0,index = 0;
            if (this.textBox1.Text == "" || this.textBox2.Text == "" || this.textBox3.Text == "")
            {
                MessageBox.Show("one of the Data is Missing! please find it and return it to us!");
            }
            //until here checking if all textbox are empty
            else {
                if (!(this.radioButton1.Checked) && !(this.radioButton2.Checked))
                {
                    MessageBox.Show("You need to pick either learning book or reading book!");
                }
                else
                {
                    if ((this.radioButton1.Checked))
                    {
                        type = 2;
                    }else
                    {
                        type = 1;
                    }
                    //until here Reading or Learning 
                    if (this.textBox3.Text.Length > 4)
                        MessageBox.Show("Year cannot be higher then todays Year!!!");
                    else
                    {
                        for (int i = 0; i < this.textBox3.Text.Length; i++)
                        {
                            if (i == 0 && this.textBox3.Text[i] == 48)
                            {
                                MessageBox.Show("Year is invalid!");
                            }
                            else if (this.textBox3.Text[i] < 48 || this.textBox3.Text[i] > 57)
                            {
                                MessageBox.Show("Year cannot be with special characters such as: @ ! ");
                            }
                        }
                        year = int.Parse(this.textBox3.Text);
                    }
                    if (year > 2023)
                        MessageBox.Show("Year cannot be higher then todays Year!!!");
                    else
                    {
                        try
                        {
                            if (type == 1)
                            {
                                readingBook = new ReadingBook(getRandom(), textBox1.Text, textBox2.Text, int.Parse(textBox3.Text), type,"English");
                            }
                            else
                            {
                                if (MessageBox.Show("Which category did you pick. for Math click Yes and for english click No", "confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    mathBook = new Math(getRandom(), textBox1.Text, textBox2.Text, int.Parse(textBox3.Text), type, getRandomGrade());
                                }
                                else
                                {
                                    englishBook = new English(getRandom(), textBox1.Text, textBox2.Text, int.Parse(textBox3.Text), type, getRandomGrade());
                                }
                            }
                            if (readingBook != null)
                            {
                                Form1.library.addBook(readingBook, false);
                                index = Form1.library.findIndex(readingBook);
                            }
                            else if (mathBook != null)
                            {
                                Form1.library.addBook(mathBook, false);
                                index = Form1.library.findIndex(mathBook);
                            }
                            else
                            {
                                Form1.library.addBook(englishBook, false);
                                index = Form1.library.findIndex(englishBook);
                            }
                            Form1.library.SaveSortedList();
                            con.Open();
                            cmd = new SqlCommand("insert into CatalogBooks(Name_of_book,Year_of_book,Name_of_author,is_Exists,type_of_book) values ('" + Form1.library.getCatalogName(index) + "','" + Form1.library.getCatalogYear(index) + "','" + Form1.library.getCatalogAuthor(index) + "','Y' , '" + Form1.library.getCatalogType(index) + "');", con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Your data has been saved in our Database");
                            Display();
                            Clear();  
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
        public int getRandomGrade()
        {
            int num;
            Random rnd = new Random();
            num = rnd.Next(1, 12);
            return num;
        }
        public int getRandom()
        {
            int num;
            Random rnd = new Random();
            num = rnd.Next(50, 1000);
            return num;
        }
    }
}
