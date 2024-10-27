using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.CalculatorClass;
using Calculator.Global;

namespace Calculator.Model
{
    public class MainModel
    {
        private Tools tools = new Tools();
        private AdvancedCalculator adCalculator = new AdvancedCalculator();

        public void SetGroup(List<string> input, ref List<double> numListOut, ref List<string> opListOut)
        {
            string inputCheck;

            for (int i = 0; i < input.Count; i++)
            {
                inputCheck = input[i];
                if (tools.NumberCheck(inputCheck))
                {
                    numListOut.Add(tools.String2Double(inputCheck));
                }
                else
                {
                    opListOut.Add(inputCheck);
                }
            }
        }

        public double Calculation(ref List<double> numList, ref List<string> opList)
        {
            string opMultiply = "*";
            string opDivide = "/";
            string opPlus = "+";
            string opMinus = "-";

            if (numList.Count == opList.Count)
            {
                opList.RemoveAt(opList.Count - 1);
            }

            CalculationRun(ref numList, ref opList, opMultiply, opDivide);
            CalculationRun(ref numList, ref opList, opPlus, opMinus);

            return numList[0];
        }

        private void CalculationRun(ref List<double> numList, ref List<string> opList, string operator1, string operator2)
        {
            int index1 = 0, index2 = 0;
            int indexNum;
            double solution;

            index1 = opList.FindIndex(x => x.Equals(operator1));
            index2 = opList.FindIndex(x => x.Equals(operator2));

            while (!(index1 == -1 && index2 == -1))
            {
                index1 = opList.FindIndex(x => x.Equals(operator1));
                index2 = opList.FindIndex(x => x.Equals(operator2));

                indexNum = (index1 != -1 && index2 != -1) ? tools.MinNum(index1, index2) : tools.MaxNum(index1, index2);
                if (indexNum == -1) { break; }

                switch (opList[indexNum])
                {
                    case "*":
                        solution = adCalculator.Multiply(numList[indexNum], numList[indexNum + 1]);
                        numList[indexNum] = solution;
                        opList.RemoveAt(indexNum);
                        numList.RemoveAt(indexNum + 1);
                        break;

                    case "/":
                        solution = adCalculator.Divide(numList[indexNum], numList[indexNum + 1]);
                        numList[indexNum] = solution;
                        opList.RemoveAt(indexNum);
                        numList.RemoveAt(indexNum + 1);
                        break;

                    case "+":
                        solution = adCalculator.Plus(numList[indexNum], numList[indexNum + 1]);
                        numList[indexNum] = solution;
                        opList.RemoveAt(indexNum);
                        numList.RemoveAt(indexNum + 1);
                        break;

                    case "-":
                        solution = adCalculator.Minus(numList[indexNum], numList[indexNum + 1]);
                        numList[indexNum] = solution;
                        opList.RemoveAt(indexNum);
                        numList.RemoveAt(indexNum + 1);
                        break;

                    default:

                        break;
                }
            }
        }

        public void MemCalculation(string oper, double num, int listNum, ref List<double> memList)
        {
            if (oper == "+")
            {
                memList[listNum] = adCalculator.Plus(memList[listNum], num);
            }
            else if (oper == "-")
            {
                memList[listNum] = adCalculator.Minus(memList[listNum], num);
            }
        }

        //private CalculatorHistory calculatorHistory = new CalculatorHistory();



        //public void GetHistory()
        //{

        //}


    }
}
