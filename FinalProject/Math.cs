using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    [Serializable]
    public class Math : LearningBook
    {
        int grade;
        int amount = 0;
        public Math(int grade) : base("Math", grade)
        {
            amount++;
        }

        public Math(int num, string name, string auth, int Y, int T,int grade) : base(num, name, auth, Y, T,"M")
        {
            this.grade = grade;
            amount++;
        }

        public int getAmountOfMathBooks()
        {
            return amount;
        }
    }
}
