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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FinalProject
{
    public partial class BorrowingForm : Form
    {
        string path = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\shall\OneDrive - Holon Institute of Technology\מסמכים\librarydb.mdf;Integrated Security=True;Connect Timeout=30"; 
        SqlConnection con;
        SqlCommand cmd;
        DataTable dt;
        SqlDataReader reader;
        SqlDataAdapter adpt;
        int id,type = 0;
        ReadingBook readbook = null;
        Math mathbook = null;
        English engbook = null;
        string Sub = "";
        bool isThereBook = true;

        public BorrowingForm()
        {
            InitializeComponent();
            con = new SqlConnection(path);
            Display();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Y, M, D;
            int year = 0; 
            if(this.textBox5.Text == "" || this.textBox2.Text == "" || this.textBox3.Text == "" || this.textBox4.Text == "")
            {
                MessageBox.Show("one of the Data is Missing! please find it and return it to us!");
            }
            //until here checking if all textbox are empty
            else if (!(this.radioButton1.Checked) && !(this.radioButton2.Checked))
            {
                MessageBox.Show("You need to pick either learning book or reading book!");
            }
            if ((this.radioButton1.Checked))
            {
                type = 1;
            }
            else
            {
                type = 2;
            }
            //until here Reading or Learning 
            if (this.textBox4.Text.Length > 4)
                MessageBox.Show("Year cannot be higher then todays Year!!!");
            else {
                for (int i = 0; i < this.textBox4.Text.Length; i++)
                {
                    if (i == 0 && this.textBox4.Text[i] == 48)
                    {
                        MessageBox.Show("Year is invalid!");
                    }
                    else if (this.textBox4.Text[i] < 48 || this.textBox4.Text[i] > 57)
                    {
                        MessageBox.Show("Year cannot be with special characters such as: @ ! ");
                    }
                }
                year = int.Parse(this.textBox4.Text);
            }
            if (year > 2023)
                MessageBox.Show("Year cannot be higher then todays Year!!!");

            //until here Published Year
            if (this.textBox5.Text.Length > 9)
                MessageBox.Show("ID cannot be more then 9 characters!!");
            else
            {
                for (int i = 0; i < this.textBox5.Text.Length; i++)
                {
                    if (this.textBox5.Text[i] < 48 || this.textBox5.Text[i] > 57)
                    {
                        MessageBox.Show("ID cannot be include with special characters such as: @ ! ");
                    }
                }
            }
            //until here User ID
            if (dateBorrow.Value.Year > dateReturn.Value.Year)
            {
                MessageBox.Show("you cannot return before you borrow!");
            }
            else if (dateBorrow.Value.Year == dateReturn.Value.Year && dateBorrow.Value.Month > dateReturn.Value.Month)
            {
                MessageBox.Show("you cannot return before you borrow!");
            }

            else if (dateBorrow.Value.Year == dateReturn.Value.Year && dateBorrow.Value.Month == dateReturn.Value.Month && dateBorrow.Value.Day >= dateReturn.Value.Day)
            {
                MessageBox.Show("you cannot return on the same day you borrow or before!");
            }
            else
            {
                con.Open();
                cmd = new SqlCommand("select * from CatalogBooks where Name_of_Book ='" + textBox2.Text + "' and is_Exists='Y' ;", con);
                reader = cmd.ExecuteReader();
                if (reader.Read())//need to check for multypule times
                {
                    reader.Close();
                    try
                    { // need to add a check about if the book is available int our Library!!
                        cmd = new SqlCommand("select * from BorrowedBooks where id_user='" + textBox5.Text + "'", con);
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            MessageBox.Show("You must return the book you borrowed before borrowing another book!!");
                            reader.Close();
                            con.Close();
                        }
                        else
                        {
                            reader.Close();
                            if (type == 1)
                            {
                                readbook = new ReadingBook(getRandom(), textBox2.Text, textBox3.Text, int.Parse(textBox4.Text), type, "English");
                                Sub = null;
                                if (Form1.library.findBookInCatalog(textBox2.Text, type, Sub))
                                    Form1.library.addBook(readbook, true);
                                else
                                    isThereBook = false;
                                readbook = null;
                            }
                            else
                            {
                                if (MessageBox.Show("Which category did you pick. for Math click Yes and for english click No", "confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    mathbook = new Math(getRandom(), textBox2.Text, textBox3.Text, int.Parse(textBox4.Text), type, getRandomGrade());
                                    Sub = "M";
                                    if (Form1.library.findBookInCatalog(textBox2.Text, type, Sub))
                                        Form1.library.addBook(mathbook, true);
                                    else
                                        isThereBook = false;
                                    mathbook = null;
                                }
                                else
                                {
                                    engbook = new English(getRandom(), textBox2.Text, textBox3.Text, int.Parse(textBox4.Text), type, getRandomGrade());
                                    Sub = "E";
                                    if (Form1.library.findBookInCatalog(textBox2.Text, type, Sub))
                                        Form1.library.addBook(engbook, true);
                                    else
                                        isThereBook = false;
                                    engbook = null;
                                }
                            }
                            if (!isThereBook)
                            {
                                con.Close();
                                MessageBox.Show("we dont have your Book!!!!!");
                            }
                            else {
                                Form1.library.SaveSortedList();
                                cmd = new SqlCommand("insert into BorrowedBooks(Name_of_book,Year_of_book,Name_of_author,id_user) values ('" + textBox2.Text + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox5.Text + "');", con);
                                cmd.ExecuteNonQuery();
                                cmd = new SqlCommand("update CatalogBooks set is_EXists = 'N' where Name_of_Book='" + textBox2.Text + "'", con);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Your data has been saved in our Database");
                                con.Close();
                                Display();
                                Clear();
                                Y = dateReturn.Value.Year - dateBorrow.Value.Year;
                                M = dateReturn.Value.Month - dateBorrow.Value.Month;
                                D = dateReturn.Value.Day - dateBorrow.Value.Day;
                                if (D < 0)
                                {
                                    M--;
                                    D += 30;
                                }
                                if (M < 0)
                                {
                                    Y--;
                                    M += 12;
                                }
                                MessageBox.Show("You have " + Y + " years, " + M + " months and " + D + " days to return it!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        con.Close();
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    con.Close();
                    MessageBox.Show("we dont have your Book!!!!");
                }
                Display();
            }
        }
        public void Clear()
        {
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.textBox5.Text = "";
            Display();      
        }
        public void Display()
        { 
            try
            {
                dt = new DataTable();
                con.Open();
                adpt = new SqlDataAdapter("select * from CatalogBooks where is_Exists='Y'",con);
                adpt.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" the problem is :" + ex.Message);
            }
        }

        private void txt_check(object sender, EventArgs e)
        {
            con.Open();
            adpt = new SqlDataAdapter("select * from CatalogBooks where Name_of_book like '%" + textBox2.Text + "%' ", con);
            dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox4.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.Rows[0].Cells[3].Value.ToString();            
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        public int getRandom()
        {
            int num;
            Random rnd = new Random();
            num = rnd.Next(50, 1000);
            return num;
        }

      

        public int getRandomGrade()
        {
            int num;
            Random rnd = new Random();
            num = rnd.Next(1, 12);
            return num;
        }
    }
}
