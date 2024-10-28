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
using System.Windows;

namespace Calculator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainModel model = null;

        string inputString = "";
        string displayText = "";
        string displayNumText = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            this.model = new MainModel();
            this.Append = new Append(this);
            this.Calculate = new Calculate(this);
            this.Memory = new Memory(this);
            this.InputClear = new InputClear(this);
            this.OpenWindow = new OpenWindow(OpenHistoryWindow);
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

        public string DisplayNumText //계산된 결과 Text
        {
            internal set
            {
                if (displayNumText != value)
                {
                    displayNumText = value;
                    OnPropertyChanged("DisplayNumText");
                }
            }
            get { return displayNumText; }
        }

        public string DisplayText //버튼 키보드 입력 Text
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

        public void OpenHistoryWindow()
        {
            HistoryWindow hisWindow = new HistoryWindow();

            var mainWindow = Application.Current.MainWindow;
            hisWindow.Left = mainWindow.Left + mainWindow.Width;
            hisWindow.Top = mainWindow.Top;

            hisWindow.Owner = mainWindow;
            hisWindow.Show();
        }

        public ICommand Append { protected set; get; }
        public ICommand Calculate { protected set; get; }
        public ICommand Memory { protected set; get; }
        public ICommand InputClear { protected set; get; }
        public ICommand OpenWindow { get; }

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
        private CalculatorHistory calHistory = CalculatorHistory.GetInstance();

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
            int listCount = calHistory.inputList.Count;
            int listindex = 0;
            if (listCount != 0) { listindex = listCount - 1; }
            if (mainViewModel.DisplayNumText != string.Empty) { mainViewModel.DisplayNumText = string.Empty; }

            if (param == "+" || param == "-" || param == "*" || param == "/")
            {
                if (listCount != 0 && tools.NumberCheck(calHistory.inputList[listindex]))
                {
                    AddList(param);
                }
            }
            else if (listCount == 0 || !tools.NumberCheck(calHistory.inputList[listindex]))
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
            calHistory.inputList.Add(param);
            mainViewModel.InputString += param;
        }
        private void PlusList(string param, int listNum)
        {
            calHistory.inputList[listNum] += param;
            mainViewModel.InputString += param;
        }
    }

    class Calculate : ICommand
    {
        private MainViewModel mainViewModel;
        private CalculatorHistory calHistory = CalculatorHistory.GetInstance();
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
            mainViewModel.model.SetGroup(calHistory.inputList, ref calHistory.inputNumber, ref calHistory.inputOperator);
            solution = mainViewModel.model.Calculation(ref calHistory.inputNumber, ref calHistory.inputOperator);
            mainViewModel.DisplayNumText = solution.ToString();
            SaveHistory();
            ListAllClear();
        }

        public void ListAllClear()
        {
            calHistory.inputList.Clear();
            calHistory.inputNumber.Clear();
            calHistory.inputOperator.Clear();
        }

        public void SaveHistory()
        {
            calHistory.inputLogList.Add(mainViewModel.DisplayText);
            calHistory.inputLogList.Add(mainViewModel.DisplayNumText);
        }
    }

    class Memory : ICommand
    {
        private MainViewModel mainViewModel;
        private Tools tools = new Tools();
        public event EventHandler CanExecuteChanged;
        private CalculatorHistory calHistory = CalculatorHistory.GetInstance();

        public Memory(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            int inputListCount = calHistory.inputList.Count;
            int memListCount = calHistory.inputMemList.Count;
            string inputNum = mainViewModel.InputString;
            string param = parameter.ToString();

            if (inputListCount == 1)
            {
                if (memListCount == 0 || param == "S")
                {
                    if (tools.NumberCheck(inputNum))
                    {
                        AddList(tools.String2Double(inputNum));
                    }
                }
                else if (param == "R")
                {
                    ReadList(memListCount - 1);
                }
                else if (param == "C")
                {
                    ClearList();
                }
                else
                {
                    mainViewModel.model.MemCalculation(param, tools.String2Double(inputNum), memListCount - 1, ref calHistory.inputMemList);
                }
            }
        }

        private void AddList(double inputNum)
        {
            calHistory.inputMemList.Add(inputNum);
        }

        private void ReadList(int listNum)
        {
            calHistory.inputList[0] = calHistory.inputMemList[listNum].ToString();
            mainViewModel.DisplayNumText = calHistory.inputList[0];
        }

        private void ClearList()
        {
            calHistory.inputMemList.Clear();
        }
    }

    class InputClear : ICommand
    {
        private MainViewModel mainViewModel;
        private Tools tools = new Tools();
        private CalculatorHistory calHistory = CalculatorHistory.GetInstance();
        public InputClear(MainViewModel mainViewModel)
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
            string inputParam = parameter.ToString();
            if (inputParam == "C")
            {
                AllClear();
            }
            else if (inputParam == "CE")
            {
                NowInputClear();
            }
        }

        private void AllClear()
        {
            mainViewModel.InputString = string.Empty;
            mainViewModel.DisplayNumText = string.Empty;
            mainViewModel.DisplayText = string.Empty;

            calHistory.inputList.Clear();
        }

        private void NowInputClear()
        {
            string oper = string.Empty;
            string num = string.Empty;
            string delString = string.Empty;
            int listCount = calHistory.inputList.Count();
            if (listCount != 0)
            {
                if (tools.NumberCheck(calHistory.inputList[listCount - 1]))
                {
                    num = calHistory.inputList[listCount - 1];
                    calHistory.inputList.RemoveAt(listCount - 1);
                    delString = num;
                }
                else
                {
                    oper = calHistory.inputList[listCount - 1];
                    num = calHistory.inputList[listCount - 2];
                    calHistory.inputList.RemoveRange(listCount - 2, 2);
                    delString = string.Format("{0}{1}", num, oper);
                }
                mainViewModel.InputString = mainViewModel.InputString.Remove(mainViewModel.InputString.Length - delString.Length);
                mainViewModel.DisplayNumText = string.Empty;
            }
        }
    }

    public class OpenWindow : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public OpenWindow(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }

        
    }
}