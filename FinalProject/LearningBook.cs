using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    [Serializable]
    public class LearningBook : Book
    {
        private int amount = 0;
        private string Subject;
        private int grade;
        private string nameOfBook;

        public LearningBook(string subject,int grade) : base(2)
        {
            this.grade = grade;
            this.Subject = subject;
            amount++;
        }

        public LearningBook(string nameOfBook) : base(2)
        {
            this.nameOfBook = nameOfBook;
        }

        public LearningBook(int num, string name, string auth, int Y, int T,string subject) : base(num, name, auth, Y, T,subject)
        {
            this.Subject = subject;
            amount++;
        }
        public int getGrade()
        {
            return grade;
        }
        public void setGrade(int x)
        {
            this.grade = x;
        }

        public void setAfterAdd()
        {
            amount++;
        }
        public void setAfterRemove()
        {
            amount--;
        }
        public string getSubject()
        {
            return Subject;
            
        }

        

        public void setSubject(string n)
        {
            this.Subject = n;
        }
    }
}
