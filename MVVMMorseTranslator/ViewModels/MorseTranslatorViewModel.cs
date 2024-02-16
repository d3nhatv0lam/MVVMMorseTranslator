using MVVMMorseTranslator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;
using MVVMMorseTranslator.Models;
using System.Threading;

namespace MVVMMorseTranslator.ViewModels
{
    public class MorseTranslatorViewModel : ViewModelBase
    {
        private readonly MorseTranslatorModel _morse = new MorseTranslatorModel();

        public String Alphabet
        {
            get => _morse.Alphabet;
               
            set
            {
                _morse.Alphabet = value;
                OnPropertyChanged(nameof(MorseCode));
            }
        }

        public String MorseCode
        {
            get => _morse.MorseCode;
            set
            {
                _morse.MorseCode = value;
                OnPropertyChanged(nameof(Alphabet));
            }
        }


        public MorseTranslatorViewModel()
        {
           
        }


    }

}
