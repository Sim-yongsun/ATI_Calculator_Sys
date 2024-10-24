using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Calculator.Model;
using Calculator.CalculatorClass;
using Calculator.Global;

namespace Calculator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainModel model = null;

        string inputString = "";
        string displayText = "";
        

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            this.model = new MainModel();
            this.Append = new Append(this);
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

        public ICommand Append { protected set; get; }
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
        private Tools tools = new Tools();
        public event EventHandler CanExecuteChanged;
        private GlobalVar global = GlobalVar.GetInstance();

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
            string param = parameter.ToString();
            int listCount = global.inputList.Count;
            int listindex = 0;
            if (listCount != 0) { listindex = listCount - 1; }

            if (param == "+" || param == "-" || param == "*" || param == "/")
            {
                if (listCount != 0 && tools.NumberCheck(global.inputList[listindex]))
                {
                    AddList(param);
                }
            }
            else if (listCount == 0 || !tools.NumberCheck(global.inputList[listindex]))
            {
                AddList(param);
            }
            else
            {
                PlusList(param, listindex);
            }
        }

        private void AddList(string param)
        {
            global.inputList.Add(param);
            mainViewModel.InputString += param;
        }
        private void PlusList(string param, int listNum)
        {
            global.inputList[listNum] += param;
            mainViewModel.InputString += param;
        }
    }

    class Calculate : ICommand
    {
        private MainViewModel mainViewModel;
        private Tools tools = new Tools();
        private GlobalVar global = GlobalVar.GetInstance();
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
            return true;
        }

        public void Execute(object parameter)
        {
            double solution;
            mainViewModel.InputString = string.Empty;
            mainViewModel.model.SetGroup(global.inputList, ref global.inputNumber, ref global.inputOperator);
            solution = mainViewModel.model.Calculation(ref global.inputNumber, ref global.inputOperator);
            mainViewModel.DisplayText = solution.ToString();
            ListAllClear();
        }

        public void ListAllClear()
        {
            global.inputList.Clear();
            global.inputNumber.Clear();
            global.inputOperator.Clear();
        }
    }
}