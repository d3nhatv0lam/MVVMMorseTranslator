using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMMorseTranslator.ViewModels.Base
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand _changePageCommand;
        private ICommand _minimizeApp;
        private ICommand _restoreApp;
        private ICommand _closeApp;

        private List<ViewModelBase> _pageViewModels;
        private ViewModelBase _currentViewModel;

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

        public ICommand MinimizeApp
        {
            get
            {
                if (_minimizeApp == null)
                {
                    _minimizeApp = new RelayCommand(() =>
                    {
                        Application.Current.MainWindow.WindowState = WindowState.Minimized;
                    });
                }
                return _minimizeApp;
            }
        }
        public ICommand RestoreApp
        {
            get
            {
                if (_restoreApp == null)
                {
                    _restoreApp = new RelayCommand(() =>
                    {
                        if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                        {
                            Application.Current.MainWindow.WindowState = WindowState.Normal;
                        } else
                        {
                            Application.Current.MainWindow.WindowState = WindowState.Maximized;
                        }
                    });
                }
                return _restoreApp;
            }
        }

        public ICommand CloseApp
        {
            get
            {
                if (_closeApp == null)
                {
                    _closeApp = new RelayCommand(() =>
                    {
                        ((MorseTranslatorViewModel)PageViewModels[0]).DeleteAudio.Execute(null);
                        Application.Current.Shutdown();
                    });
                }
                return _closeApp;
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
