using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FinalProject
{
    [Serializable]
    public class Library
    {
        SortedList Catalog;
        SortedList Currentbooks;
        SortedList BorrowedBooks;
        int amountTotalLibrary, amountBorrowedBooks,amountCatalog;
        public Library()
        {
            this.amountCatalog = 0;
            this.amountBorrowedBooks = 0;
            this.amountTotalLibrary = 0;
            Catalog = new SortedList();
            Currentbooks = new SortedList();
            BorrowedBooks = new SortedList();
        }
        public void addBook(ReadingBook b, bool flag)
        {
            if (flag)
            {
                RemoveBook(b);
            }
            else
            {
                amountTotalLibrary++;
                amountCatalog++;
                Catalog.Add(amountTotalLibrary, b);
                Currentbooks.Add(amountCatalog,b);              
            }
        }
         public void addBook(Math b, bool flag)
        {
            if (flag)
            {
                RemoveBook(b);
            }
            else
            {
                amountTotalLibrary++;
                amountCatalog++;
                Catalog.Add(amountTotalLibrary, b);
                Currentbooks.Add(amountCatalog,b);
            }
        }
         public void addBook(English b, bool flag)
        {
            if (flag)
            {
                RemoveBook(b);
            }
            else
            {
                amountTotalLibrary++;
                amountCatalog++;
                Catalog.Add(amountTotalLibrary, b);
                Currentbooks.Add(amountCatalog,b);           
            }
        }
        public void RemoveBook(ReadingBook b)
        {
            int newFreeIndex;
            if (amountCatalog == 0)
            {
                Currentbooks[0] = null;
            }
            else
            {
                newFreeIndex = findIndex(b);
                amountBorrowedBooks++;
                Currentbooks.Remove(newFreeIndex);
                ReorderSortedList(newFreeIndex);
                amountCatalog--;
                BorrowedBooks.Add(amountBorrowedBooks,b);
                
            }
        }
        public void RemoveBook(Math b)
        {
            int newFreeIndex;
            if (amountCatalog == 0)
            {
                Currentbooks[0] = null;
            }
            else
            {
                newFreeIndex = findIndex(b);
                amountBorrowedBooks++;
                Currentbooks.Remove(newFreeIndex);
                ReorderSortedList(newFreeIndex);
                amountCatalog--;
                BorrowedBooks.Add(amountBorrowedBooks,b);
                
            }
        }
        public void RemoveBook(English b)
        {
            int newFreeIndex;
            if (amountCatalog == 0)
            {
                Currentbooks[0] = null;
            }
            else
            {
                newFreeIndex = findIndex(b);
                amountBorrowedBooks++;
                Currentbooks.Remove(newFreeIndex);
                ReorderSortedList(newFreeIndex);
                amountCatalog--;
                BorrowedBooks.Add(amountBorrowedBooks,b);    
            }
        }

        public SortedList getCatalog()
        {
            return this.Catalog;
        }
        public SortedList getCurrent()
        {
            return this.Currentbooks;
        }
        public SortedList getBorrowed()
        {
            return this.BorrowedBooks;
        }
        public void RemoveFromBorrowing(ReadingBook r)
        {
            BorrowedBooks.Remove(findIndex(r));
            amountBorrowedBooks--;
            amountCatalog++;
            Currentbooks.Add(amountCatalog, r);
        }
        public void RemoveFromBorrowing(Math m)
        {
            BorrowedBooks.Remove(findIndex(m));
            amountBorrowedBooks--;
            amountCatalog++;
            Currentbooks.Add(amountCatalog, m);
        }
        public void RemoveFromBorrowing(English e)
        {
            BorrowedBooks.Remove(findIndex(e));
            amountBorrowedBooks--;
            amountCatalog++;
            Currentbooks.Add(amountCatalog, e);
        }
        public Book findInCatalog(string name)
        {
            Book answer = null;
            Book[] temp = new Book[amountBorrowedBooks];
            for (int i = 1; i <= amountBorrowedBooks; i++)
            {
                temp[i - 1] = (Book)BorrowedBooks[i];
            }
            for (int i = 0; i < amountBorrowedBooks; i++)
            {
                if (temp[i].getName() == name)
                {
                    answer = temp[i];
                }
            }
            return answer;
        }

        public bool findBookInCatalog(string name,int type,string subject)
        {
            Book[] temp = new Book[amountCatalog];
            for (int i = 1; i <= amountCatalog; i++)
            {
                temp[i - 1] = (Book)Catalog[i];
            }
            for (int i = 0; i < amountCatalog; i++)
            {
                if (temp[i].getName() == name)
                {
                    if (temp[i].getType() == type)
                    {
                        if (subject == temp[i].getSub())
                            return true;
                    }
                }
            }
            return false;
        }

        public int findIndex(ReadingBook r)
        {
            int result = 0;
            Book temp = null;
            for (int i = 1; i <= amountTotalLibrary; i++)
            {
                temp = (Book)Catalog[i];
                if(r.getName() == temp.getName() && r.getAuthor() == temp.getAuthor() && r.getPublishedYear() == temp.getPublishedYear() && r.getType() == temp.getType())
                {
                    result = i;
                    break;
                }

            }
            return result;
        }
        public int findIndex(Math r)
        {
            int result = 0;
            Book temp = null;
            for (int i = 1; i <= amountTotalLibrary; i++)
            {
                temp = (Book)Catalog[i];
                if (r.getName() == temp.getName() && r.getAuthor() == temp.getAuthor() && r.getPublishedYear() == temp.getPublishedYear() && r.getType() == temp.getType())
                {
                    result = i;
                    break;
                }

            }
            return result;
        }
        public int findIndex(English r)
        {
            int result = 0;
            Book temp = null;
            for (int i = 1; i <= amountCatalog; i++)
            {
                temp = (Book)Catalog[i];
                if (r == temp)
                {
                    result = i;
                    break;
                }

            }
            return result;
        }
        public string getCatalogName(int index)
        {
            Book temp = (Book)Catalog[index];
            return temp.getName();
        }
        public int getCatalogYear(int index)
        {
            Book temp = (Book)Catalog[index];
            return temp.getPublishedYear();
        }
        public int getCatalogType(int index)
        {
            Book temp = (Book)Catalog[index];
            return temp.getType();
        }
        public string getCatalogAuthor(int index)
        {
            Book temp = (Book)Catalog[index];
            return temp.getAuthor();
        }
        public string getCatalogSub(int index)
        {
            Book temp = (Book)Catalog[index];
            return temp.getSub();
        }
        public int getCatalogNumOfPages(int index)
        {
            Book temp = (Book)Catalog[index];
            return temp.getNumOfPages();
        }
        public void ReorderSortedList(int index)
        {
            int counter = 0;
            Book[] temp = new Book[amountCatalog - 1];
            for (int i = 1; i <= amountCatalog; i++)
            {
                if (i != index)
                {
                    temp[counter] = (Book)Currentbooks[i];
                    counter++;
                }
            }
            Currentbooks.Clear();
            for (int i = 0; i < counter; i++)
            {
                Currentbooks.Add(i + 1, temp[i]);
            }
        }

        public void SaveSortedList()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, Catalog);
                }
            }
        }
        public void LoadSortedList()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open);
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    //!!!!
                    Catalog = (SortedList)binaryFormatter.Deserialize(stream);
                    amountTotalLibrary = findAmount(Catalog);
                }
            }
            catch (FileNotFoundException)
            {
                amountTotalLibrary = 0;
            }
        }

        public void addBorrowedBook(Book b)
        {
            amountBorrowedBooks++;
            if (b.getType() == 1)
            {
                ReadingBook r = new ReadingBook(b.getNumOfPages(),b.getName(),b.getAuthor(),b.getPublishedYear(),b.getType(),b.getSub());
                BorrowedBooks.Add(r, amountBorrowedBooks);
            }
            else
            {
                if(b.getSub() == "M")
                {
                    Math r = new Math(b.getNumOfPages(), b.getName(), b.getAuthor(), b.getPublishedYear(), b.getType(),b.getGrade());
                    BorrowedBooks.Add(r, amountBorrowedBooks);
                }
                else
                {
                    English r = new English(b.getNumOfPages(), b.getName(), b.getAuthor(), b.getPublishedYear(), b.getType(),b.getGrade());
                    BorrowedBooks.Add(r, amountBorrowedBooks);
                }
            }
        }
        public void addCurrentBook(Book b)
        {
            amountCatalog++;
            if (b.getType() == 1)
            {
                ReadingBook r = new ReadingBook(b.getNumOfPages(), b.getName(), b.getAuthor(), b.getPublishedYear(), b.getType(), b.getSub());
                Currentbooks.Add(amountCatalog,r);
            }
            else
            {
                if (b.getSub() == "M")
                {
                    Math r = new Math(b.getNumOfPages(), b.getName(), b.getAuthor(), b.getPublishedYear(), b.getType(), b.getGrade());
                    Currentbooks.Add(amountCatalog,r);
                }
                else
                {
                    English r = new English(b.getNumOfPages(), b.getName(), b.getAuthor(), b.getPublishedYear(), b.getType(), b.getGrade());
                    Currentbooks.Add(amountCatalog,r);
                }
            }
        }

        public int findAmount(SortedList t)
        {
            int amount = 0,i = 1;
            bool flag = true;
            while (flag)
            {
                if (t.ContainsKey(i) == false)
                {
                    amount = i - 1;
                    flag = false;
                }
                else
                {
                    i++;
                }
            }
            return amount;
        }
    }
}
