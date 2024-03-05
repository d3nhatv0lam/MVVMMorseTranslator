using CommunityToolkit.Mvvm.Input;
using MVVMMorseTranslator.Models;
using MVVMMorseTranslator.Themes;
using MVVMMorseTranslator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace MVVMMorseTranslator.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        private ThemeController _themeController;

        private SettingModel _settingModel;

        private ICommand _changeTheme;
        private ICommand _settingLoaded;
        private ICommand _settingUnLoaded;
        public bool IsDarkTheme
        {
            get => _settingModel.isDarkTheme;
            set
            {
                _settingModel.isDarkTheme = value;
                OnPropertyChanged(nameof(IsDarkTheme));
            }
        }

        public bool IsSettingLoaded
        {
            get => _settingModel.isSettingLoaded;
            set
            {
                _settingModel.isSettingLoaded = value;
                OnPropertyChanged(nameof(IsSettingLoaded));
            }
        }


        public ICommand SettingLoaded
        {
            get
            {
                if (_settingLoaded == null)
                {
                    _settingLoaded = new RelayCommand(() => 
                    {
                        IsSettingLoaded = !IsSettingLoaded;
                    });
                }
                return _settingLoaded;
            }
        }

        public ICommand SettingUnLoaded
        {
            get
            {
                if (_settingUnLoaded == null)
                {
                    _settingUnLoaded = new RelayCommand(() =>
                    {
                        IsSettingLoaded = false;
                    });
                }
                return _settingUnLoaded;
            }
        }

        public ICommand ChangeTheme
        {
            get
            {
                if (_changeTheme == null)
                {
                   
                    _changeTheme = new RelayCommand<bool>((newTheme) =>
                    {
                        IsDarkTheme = !IsDarkTheme;
                        int NewTheme = IsDarkTheme ? 1 : 0;
                        _themeController.ChangeTheme(NewTheme);
                    });
                    
                }
                return _changeTheme;
            }
        }

      
        public SettingViewModel()
        {
            _themeController = new ThemeController();
            _settingModel = new SettingModel();

            Task.Run(() =>
            {
                while (true)
                {
                    Debug.WriteLine("setting " + IsSettingLoaded);
                    Debug.WriteLine("Dark " + IsDarkTheme);
                }
                
            });
        }
    }
}
