using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FinalProject
{
    [Serializable]
    public class Book
    {
        int NumOfPages;
        string nameOfBook;
        string author;
        string subject;
        int PublishedYear;
        int type;

        public Book(int NumOfPages, string nameOfBook, string author, int PublishedYear, int type,string subject) {
            this.NumOfPages = NumOfPages;
            this.nameOfBook = nameOfBook;
            this.author = author;
            this.PublishedYear = PublishedYear;
            this.type = type;
            this.subject = subject;
            setamountByType();
        }
        public Book(Book b)
        {
            this.NumOfPages = b.getNumOfPages();
            this.nameOfBook = b.getName();
            this.author = b.getAuthor();
            this.PublishedYear = b.getPublishedYear();
            this.type = b.getType();
            setamountByType();
        }
        public void setamountByType()
        {
            if(type == 1)
            {
                ReadingBook r = new ReadingBook(nameOfBook);
                r.setAfterAdd();
            }
            else
            {
                LearningBook l = new LearningBook(nameOfBook);
                l.setAfterAdd();
            }
        }
        public Book(int type)
        {
            this.type = type;
        }

        public int getNumOfPages(){
            return NumOfPages;  
        }
        public void setNumOfPages(int x)
        {
            this.NumOfPages = x;
        }
        public string getName()
        {
            return nameOfBook;
        }

        public void setName(string n)
        {
            this.nameOfBook = n;
        }
        public string getAuthor()
        {
            return author;
        }

        public void setAuthor(string n)
        {
            this.author = n;
        }
        public int getPublishedYear()
        {
            return PublishedYear;
        }
        public void setPublishedYear(int x)
        {
            this.PublishedYear = x;
        }

        public int getType()
        {
            return type;
        }
        public void setType(int x)
        {
            this.type = x;
        }

        public void setSub(string x)
        {
            this.subject = x;
        }
        public string getSub()
        {
            return subject;
        }

        public int getGrade()
        {
            Random random = new Random();
            int num;
            num = random.Next(0,13);
            return num;
        }
    }
}
