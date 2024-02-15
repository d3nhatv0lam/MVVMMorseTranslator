using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMMorseTranslator.ViewModels.Base
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand _changePageCommand;
        private ViewModelBase _currentViewModel;
        private List<ViewModelBase> _pageViewModels;

        public enum PageName
        {
            MorseTranslatorViewModel,
            SettingViewModel
        }

        public MainViewModel()
        {
            // Add available pages
            PageViewModels.Add(new MorseTranslatorViewModel());
            PageViewModels.Add(new SettingViewModel() );

            CurrentViewModel = PageViewModels.First();

        }

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand<ViewModelBase>(
                        p => changeViewModel((ViewModelBase)p),
                         p => true);
                }
                return _changePageCommand;
            }
        }

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
                if ( _currentViewModel != value )
                {
                    _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }


        private void changeViewModel(ViewModelBase viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);
            CurrentViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);   

        }

    }

}
