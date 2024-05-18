using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    [Serializable]
    public class English :LearningBook
    {
        int grade;
        int amount = 0;
        public English(int grade):base("English",grade)
        {
            amount++;
        }

        public English(int num, string name, string auth, int Y, int T, int grade) : base(num, name, auth, Y, T,"E")
        {
            this.grade = grade;
            amount++;
        }
        public int getAmountOfEnglishBooks()
        {
            return amount;
        }


    }
}
