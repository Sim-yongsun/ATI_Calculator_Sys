using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Global
{
    public class Tools
    {
        public bool NumberCheck(string data)
        {
            double dummy;
            return Double.TryParse(data, out dummy);
        }

        public double String2Double(string data)
        {
            return Double.Parse(data);
        }

        public int MinNum(int num1, int num2) // 작은 수 return
        {
            int minNum = (num1 < num2) ? num1 : num2;
            return minNum;
        }

        public int MaxNum(int num1, int num2) // 큰 수 return
        {
            int maxNum = (num1 < num2) ? num2 : num1;
            return maxNum;
        }
    }
}
