using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.CalculatorClass
{
    public class AdvancedCalculator : MainCalculator
    {
        public double Pow(double num)
        {
            return Multiply(num, num);
        }

        public double Frac(double num)
        {
            return Divide(1, num);
        }

        //public double Percent(double num)
        //{
        //    return 0;
        //}
    }
}
