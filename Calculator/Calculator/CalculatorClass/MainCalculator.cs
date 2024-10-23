using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Operator;

namespace Calculator.CalculatorClass
{
    public class MainCalculator
    {
        //OperatorManager opManager = OperatorManager.Getinstance();
        private MyOperator myOperator = new MyOperator();

        public double Plus(double num1, double num2)
        {
            return myOperator.Plus(num1, num2);
        }

        public double Minus(double num1, double num2)
        {
            return myOperator.Minus(num1, num2);
        }

        public double Multiply(double num1, double num2)
        {
            return myOperator.Multiply(num1, num2);
        }

        public double Divide(double num1, double num2)
        {
            return myOperator.Divide(num1, num2);
        }

        public double Sqrt(double num1)
        {
            return myOperator.Sqrt(num1);
        }
    }
}
