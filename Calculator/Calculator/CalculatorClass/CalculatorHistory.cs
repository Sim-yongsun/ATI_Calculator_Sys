using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.CalculatorClass
{
    public class CalculatorHistory
    {
        private static CalculatorHistory calHIstory = null;
        public static CalculatorHistory GetInstance()
        {
            if (calHIstory != null)
            {
                return calHIstory;
            }
            else
            {
                calHIstory = new CalculatorHistory();
                return calHIstory;
            }
        }

        public List<string> inputList = new List<string>();
        public List<double> inputNumber = new List<double>();
        public List<string> inputOperator = new List<string>();

        public List<double> inputMemList = new List<double>();
        public List<string> inputLogList = new List<string>();        
    }
}
