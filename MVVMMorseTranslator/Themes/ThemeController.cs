using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMMorseTranslator.Themes
{


    public class ThemeController: ObservableObject
    {

        private enum ThemeType
        {
            LightTheme,
            DarkTheme,
        };

        public ResourceDictionary CurrentTheme
        {
            get => Application.Current.Resources.MergedDictionaries[0];
            set {
                Application.Current.Resources.MergedDictionaries[0] = value; 
                OnPropertyChanged(nameof(CurrentTheme));
            }
        }

        private String FindThemeName(int Theme)
        {
            String ThemeName = String.Empty;

            switch (Theme)
            {
                case (int)ThemeType.LightTheme: ThemeName = "LightTheme";
                    break;
                case (int)ThemeType.DarkTheme: ThemeName = "DarkTheme";
                    break;
            }
            return ThemeName;
        }


        public void ChangeTheme(int Theme)
        {
            String ThemeName = FindThemeName(Theme);

            CurrentTheme = new ResourceDictionary {Source = new Uri($"Themes\\{ThemeName}.xaml", UriKind.Relative) };

            Debug.WriteLine(CurrentTheme.ToString());
        }


    }
}
