using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMMorseTranslator.ViewModels.Base
{
    public class Navigation : ViewModelBase
    {

        // i'm desiced write changepagecommand in MVVM MainViewModel

        //private ICommand _changePageCommand;

        //public ICommand ChangePageCommand
        //{
        //    get
        //    {
        //        if (_changePageCommand == null)
        //        {
        //            _changePageCommand = new RelayCommand<ViewModelBase>(
        //                p =>  changeViewModel((ViewModelBase)p),
        //                p => true);
        //        }
        //        return _changePageCommand;
        //    }
        //}

        private List<ViewModelBase> _pageViewModels;
        private ViewModelBase _currentViewModel;


       

        public List<ViewModelBase> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<ViewModelBase>();

                return _pageViewModels;
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }



        public void changeViewModel(ViewModelBase viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);
            CurrentViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }
    }
}
