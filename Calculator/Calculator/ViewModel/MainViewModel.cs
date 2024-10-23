using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Calculator.Model;
using Calculator.CalculatorClass;

namespace Calculator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainModel model = null;
        public AdvancedCalculator adCalculator = new AdvancedCalculator();

        string inputString = "";
        string displayText = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            this.model = new MainModel();
            this.Append = new Append(this);
            this.Operator = new Operator(this);
            this.Calculate = new Calculate(this);
        }

        public string InputString
        {
            internal set
            {
                if (inputString != value)
                {
                    inputString = value;
                    OnPropertyChanged("InputString");
                    if (value != "")
                    {
                        DisplayText = value;
                    }
                }
            }
            get { return inputString; }
        }

        public string DisplayText
        {
            internal set
            {
                if (displayText != value)
                {
                    displayText = value;
                    OnPropertyChanged("DisplayText");
                }
            }
            get { return displayText; }
        }

        public string Op { get; set; } //연산자 저장
        public double? Num1 { get; set; } //첫번째 입력 숫자 저장

        public ICommand Append { protected set; get; }
        public ICommand Operator { protected set; get; }
        public ICommand Calculate { protected set; get; }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    class Append : ICommand
    {
        private MainViewModel mainViewModel;
        public event EventHandler CanExecuteChanged;

        public Append(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mainViewModel.InputString += parameter;
        }
    }

    class Operator : ICommand
    {
        private MainViewModel mainViewModel;
        public Operator(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return 0 < mainViewModel.InputString.Length || parameter.ToString() == "-";
        }

        public void Execute(object parameter)
        {
            string op = parameter.ToString();
            double num1;

            if (double.TryParse(mainViewModel.InputString, out num1))
            {
                mainViewModel.Num1 = num1;
                mainViewModel.Op = op;
                mainViewModel.InputString = "";
            }
            else if (mainViewModel.InputString == "" && op == "-")
            {
                mainViewModel.InputString = "-";
            }
        }
    }

    class Calculate : ICommand
    {
        private MainViewModel mainViewModel;
        public Calculate(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            double num2;
            return mainViewModel.Num1 != null && double.TryParse(mainViewModel.InputString, out num2) && (mainViewModel.Op != "/" || num2 != 0);
        }

        public void Execute(object parameter)
        {
            double num2 = double.Parse(mainViewModel.InputString);
            mainViewModel.InputString = Calculation(mainViewModel.Op, (double)mainViewModel.Num1, num2).ToString();
            mainViewModel.Num1 = null;
        }

        public double Calculation(string op, double num1, double num2)
        {
            switch (op)
            {
                case "+": return mainViewModel.adCalculator.Plus(num1, num2);
                case "-": return mainViewModel.adCalculator.Minus(num1, num2);
                case "*": return mainViewModel.adCalculator.Multiply(num1, num2);
                case "/": return mainViewModel.adCalculator.Divide(num1, num2);
                    //case "/": return advancedCalculator.Sqrt(num1);
                    //case "/": return advancedCalculator.Pow(num1);
                    //case "/": return advancedCalculator.Frac(num1);
            }
            return 0;
        }
    }
}
