using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Calculator.CalculatorClass;

namespace Calculator.ViewModel
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<string> displayList = new ObservableCollection<string>();

        public HistoryViewModel()
        {
            this.AddItem = new AddItem(this);
        }
        
        public ICommand AddItem { protected set; get; }

        public ObservableCollection<string> DisplayList
        {
            get { return displayList; }
            internal set
            {
                if (displayList != value)
                {
                    displayList = value;
                    OnPropertyChanged(nameof(DisplayList));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    class AddItem : ICommand
    {
        private HistoryViewModel hisViewModel;
        public event EventHandler CanExecuteChanged;
        private CalculatorHistory calHistory = CalculatorHistory.GetInstance();

        public AddItem(HistoryViewModel hisViewModel)
        {
            this.hisViewModel = hisViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string param = parameter.ToString();
            hisViewModel.DisplayList.Clear();
            List<string> outList = new List<string>();
            int listCount = 0;
            if(param == "M")
            {
                listCount = calHistory.inputMemList.Count;
                outList = calHistory.inputMemList.Select(d => d.ToString()).ToList();
            }
            else if(param == "H")
            {
                listCount = calHistory.inputLogList.Count;
                outList = calHistory.inputLogList;
            }

            if(listCount != 0)
            {
                foreach (var item in outList)
                {
                    hisViewModel.DisplayList.Add(item);
                }
            }
        }
    }
}
