using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    [Serializable]
    public class ReadingBook : Book
    {
        int amount = 0;
        string language;
        public ReadingBook(string lan):base(1)
        {
            this.language = lan;
            amount++;
        }

        public ReadingBook(int num,string name,string auth,int Y,int T,string lan) : base(num,name,auth,Y,T,null)
        {
            this.language = lan;
            amount++;
        }

        public string getLanguage()
        {
            return language;
        }

        public int getAmountOfReadingBooks()
        {
            return amount;
        }

        public void setAfterAdd()
        {
            amount++;
        }
        public void setAfterRemove()
        {
            amount--;
        }
        public void seLanguage(string n)
        {
            this.language = n;
        }
    }
}
