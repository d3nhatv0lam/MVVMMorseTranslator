using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MVVMMorseTranslator.Core;
using System.Windows.Interop;
using System.Windows.Controls.Primitives;

namespace MVVMMorseTranslator.ViewModels.Base
{
    public class MainViewModel : ViewModelBase
    {
        private Navigation _navigation;
        private WindowAnimations windowAnimation;


        private ICommand _changePageCommand;
        private ICommand _minimizeApp;
        private ICommand _restoreApp;
        private ICommand _closeApp;
        private ICommand _windowLoad;
        private ICommand _windowStateChanged;
        private ICommand _windowPositionChanged;
        private ICommand _dragMoveWindow;


        public MainViewModel()
        {
            _navigation = new Navigation();
            // Add available pages
            PageViewModels.Add(new MorseTranslatorViewModel());
            PageViewModels.Add(new SettingViewModel());
            PageViewModels.Add(new AboutAppViewModel());
            PageViewModels.Add(new ReferencesViewModel());

            _navigation.CurrentViewModel = PageViewModels.First();
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand<ViewModelBase>(
                        p => {

                            _navigation.changeViewModel((ViewModelBase)p);

                            // Unload UserControl Property, Fixed bug when change Home so fast > 500ms
                            (PageViewModels[1] as SettingViewModel).SettingUnLoaded.Execute(null);

                            OnPropertyChanged(nameof(CurrentViewModel));
                        },
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
                        //Application.Current.MainWindow.WindowState = WindowState.Minimized;
                        SystemCommands.MinimizeWindow(Application.Current.MainWindow);
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
                        ((MorseTranslatorViewModel)PageViewModels[0]).CleanMorseAudioModel.Execute(null);
                        Application.Current.Shutdown();
                    });
                }
                return _closeApp;
            }
        }

        public ICommand WindowLoad
        {
            get
            {
                if (_windowLoad == null)
                {
                    _windowLoad = new RelayCommand<Window>((window) =>
                    {
                        windowAnimation = new WindowAnimations(window);

                    });
                }
                return _windowLoad;
            }
        }

        public ICommand WindowStateChanged
        {
            get
            {
                if (_windowStateChanged == null)
                {
                    _windowStateChanged = new RelayCommand(() =>
                    {
                        if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
                            Thread.Sleep(100);
                    });
                }
                return _windowStateChanged;
            }
        }

        public ICommand WindowPositionChanged
        {
            get
            {
                if (_windowPositionChanged == null)
                {
                    _windowPositionChanged = new RelayCommand<Window>((window) =>
                    {
                        
                    });
                }
                return _windowPositionChanged;
            }
        }

        public ICommand DragMoveWindow
        {
            get
            {
                if (_dragMoveWindow == null)
                {
                    _dragMoveWindow = new RelayCommand(() =>
                    {
                        Application.Current.MainWindow.DragMove();
                        Debug.WriteLine(Application.Current.MainWindow.Height.ToString() + " " + Application.Current.MainWindow.Width.ToString());
                    });
                }
                return _dragMoveWindow;
            }
        }

        public List<ViewModelBase> PageViewModels
        {
            get
               {
                return _navigation.PageViewModels;
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _navigation.CurrentViewModel;
            }
        }

    }

}
