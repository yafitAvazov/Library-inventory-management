using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Reflection;
using static System.Windows.Forms.AxHost;

namespace FinalProject
{
    public partial class Form1 : Form
    {
        bool isDrag = false;
        private Point clickPosition;
        public static Library library;
        BorrowingForm bf = new BorrowingForm();
        ReturningForm rf = new ReturningForm();
        AddNewBookForm nb = new AddNewBookForm();
        CatalogForm cf = new CatalogForm();
        string path = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\shall\OneDrive - Holon Institute of Technology\מסמכים\librarydb.mdf;Integrated Security=True;Connect Timeout=30"; 
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(path);
            library = new Library();
            UpdateCatalog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void UpdateCatalog()
        {
            library.LoadSortedList();
            int numofRows = library.findAmount(library.getCatalog()), i;
            ReadingBook rbbook;
            Math m;
            English e;
            DataTable dt;
            SqlDataAdapter adpt;
            try
            {
                dt = new DataTable();
                con.Open();
                adpt = new SqlDataAdapter("select * from CatalogBooks where is_Exists='Y'", con);
                adpt.Fill(dt);
                for (i = 1; i <= numofRows; i++)
                {
                    if (isExists(library.getCatalogName(i)))
                    {
                        if (library.getCatalogType(i) == 1)
                        {
                            rbbook = new ReadingBook(library.getCatalogNumOfPages(i), library.getCatalogName(i), library.getCatalogAuthor(i), library.getCatalogYear(i), library.getCatalogType(i),"English");
                            library.addCurrentBook(rbbook);
                        }
                        else {
                            if (library.getCatalogSub(i) == "M")
                            {
                                m = new Math(library.getCatalogNumOfPages(i), library.getCatalogName(i), library.getCatalogAuthor(i), library.getCatalogYear(i), library.getCatalogType(i), getRandomGrade());
                                library.addCurrentBook(m);
                            }
                            else
                            {
                                e = new English(library.getCatalogNumOfPages(i), library.getCatalogName(i), library.getCatalogAuthor(i), library.getCatalogYear(i), library.getCatalogType(i), getRandomGrade());
                                library.addCurrentBook(e);
                            }
                            
                        }
                    }
                    else
                    {
                        if (library.getCatalogType(i) == 1)
                        {
                            rbbook = new ReadingBook(library.getCatalogNumOfPages(i), library.getCatalogName(i), library.getCatalogAuthor(i), library.getCatalogYear(i), library.getCatalogType(i), "English");
                            library.addBorrowedBook(rbbook);
                        }
                        else
                        {
                            if (library.getCatalogSub(i) == "M")
                            {
                                m = new Math(library.getCatalogNumOfPages(i), library.getCatalogName(i), library.getCatalogAuthor(i), library.getCatalogYear(i), library.getCatalogType(i), getRandomGrade());
                                library.addBorrowedBook(m);
                            }
                            else
                            {
                                e = new English(library.getCatalogNumOfPages(i), library.getCatalogName(i), library.getCatalogAuthor(i), library.getCatalogYear(i), library.getCatalogType(i),getRandomGrade());
                                library.addBorrowedBook(e);
                            }

                        }                        
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public int getRandom()
        {
            int num;
            Random rnd = new Random();
            num = rnd.Next(20, 1000);
            return num;
        }

        public bool isExists(string name)
        {
            cmd = new SqlCommand("SELECT * FROM CatalogBooks where is_Exists='Y' and Name_of_book='" + name + "'", con);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close(); 
                return true;
            }
            reader.Close();
            return false;

        }

        public int getRandomGrade()
        {
            int num;
            Random rnd = new Random();
            num = rnd.Next(1, 12);
            return num;
        }

        private void book_clicked_down(object sender, MouseEventArgs e)
        {
            isDrag = true;
            clickPosition = e.Location;
        }

        private void book_clicked_move(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {
                Point newPosition = pictureBox1.Location;
                newPosition.X += e.X - clickPosition.X;
                newPosition.Y += e.Y - clickPosition.Y;
                pictureBox1.Location = newPosition;
            }
        }

        private void book_clicked_up(object sender, MouseEventArgs e)
        {
            isDrag = false;
            Point p = new Point(350,106);
            if (pictureBox1.Left >= pictureBox5.Left && pictureBox1.Right <= pictureBox5.Right && pictureBox1.Top >= pictureBox5.Top && pictureBox1.Bottom <= pictureBox5.Bottom)
            {
                nb = new AddNewBookForm();
                try
                {
                    nb.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                pictureBox1.Location = p;
            }
            else if (pictureBox1.Left >= pictureBox2.Left && pictureBox1.Right <= pictureBox2.Right && pictureBox1.Top >= pictureBox2.Top && pictureBox1.Bottom <= pictureBox2.Bottom)
            {
                con.Open();
                cmd = new SqlCommand("select * from CatalogBooks;", con);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                    con.Close();
                    bf = new BorrowingForm();
                    try
                    {
                        bf.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    } 
                }
                else
                {
                    reader.Close();
                    MessageBox.Show("the library is Empty!!");
                    con.Close();
                }
                pictureBox1.Location = p;
            }
            else if (pictureBox1.Left >= pictureBox3.Left && pictureBox1.Right <= pictureBox3.Right && pictureBox1.Top >= pictureBox3.Top && pictureBox1.Bottom <= pictureBox3.Bottom)
            {
                con.Open();
                cmd = new SqlCommand("select * from BorrowedBooks;", con);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                    con.Close();
                    rf = new ReturningForm();
                    try
                    {
                        rf.Show();
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    } 
                }
                else
                {
                    reader.Close();
                    MessageBox.Show("We dont have any books for you!!");
                    con.Close();
                }
                pictureBox1.Location = p;
            }
            else if (pictureBox1.Left >= pictureBox4.Left && pictureBox1.Right <= pictureBox4.Right && pictureBox1.Top >= pictureBox4.Top && pictureBox1.Bottom <= pictureBox4.Bottom)
            {
                cf = new CatalogForm();
                try
                {
                    cf.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                pictureBox1.Location = p;
            }
        }


    }
}
