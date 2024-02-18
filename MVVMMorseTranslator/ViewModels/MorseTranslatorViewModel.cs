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
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace MVVMMorseTranslator.ViewModels
{
    public class MorseTranslatorViewModel : ViewModelBase
    {
        private ICommand _deleteAudio;
        private readonly MorseTranslatorModel _morse = new MorseTranslatorModel();
        private readonly MorseAudioModel _morseAudio = new MorseAudioModel();

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

        public int WPM
        {
            get => _morseAudio.WPM;
            set 
            {
                _morseAudio.WPM = value;
            }
        }

        public int Frequency
        {
            get => _morseAudio.Frequency;
            set
            {
                _morseAudio.Frequency = value;
            }
        }

        public ICommand DeleteAudio
        {
            get
            {
                if (_deleteAudio == null)
                {
                    _deleteAudio = new RelayCommand(() =>
                    {
                        _morseAudio.DeleteMusicFromDisk();
                    });
                }
                return _deleteAudio;
            }
        }


        public MorseTranslatorViewModel()
        {
           
        }


    }

}
