using CommunityToolkit.Mvvm.Input;
using MVVMMorseTranslator.Themes;
using MVVMMorseTranslator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMMorseTranslator.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        private ThemeController _themeController;

        private ICommand _changeTheme;
        public ICommand ChangeTheme
        {
            get
            {
                if (_changeTheme == null)
                {
                    _changeTheme = new RelayCommand<bool>((newTheme) =>
                    {
                        int NewTheme = newTheme ? 1 : 0;
                        _themeController.ChangeTheme(NewTheme);
                    });
                }
                return _changeTheme;
            }
        }
        public SettingViewModel()
        {
            _themeController = new ThemeController();
        }
    }
}
