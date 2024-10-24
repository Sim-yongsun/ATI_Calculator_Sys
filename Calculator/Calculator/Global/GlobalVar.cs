using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Global
{
    public class GlobalVar
    {
        private static GlobalVar global = null;
        public static GlobalVar GetInstance()
        {
            if(global != null)
            {
                return global;
            }
            else
            {
                global = new GlobalVar();
                return global;
            }
        }

        public List<string> inputList = new List<string>();
        public List<double> inputNumber = new List<double>();
        public List<string> inputOperator = new List<string>();
    }
}
