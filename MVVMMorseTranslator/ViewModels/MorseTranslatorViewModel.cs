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
using System.Security.Policy;
using System.Data.Common;
using System.Timers;
using System.Windows;

namespace MVVMMorseTranslator.ViewModels
{
    public class MorseTranslatorViewModel : ViewModelBase
    {
        // Model Connection
        private MorseTranslatorModel _morse = new MorseTranslatorModel();
        private MorseAudioModel _morseAudio = new MorseAudioModel();

        private readonly Dictionary<String, String> _connectionLink = new Dictionary<String, String>()
        {
            {"Youtube" , "https://www.youtube.com/@ucduong9984"},
            {"Facebook" , "https://www.facebook.com/profile.php?id=100088452777261" },
            {"Github" , "https://github.com/d3nhatv0lam" }
        };

        private ICommand _connection;
        private ICommand _playMorseAudio;
        private ICommand _trontronVN;




        public Dictionary<String, String> ConnectionLink
        {
            get => _connectionLink;
        }



        public String Alphabet
        {
            get => _morse.Alphabet;
               
            set
            {
                _morse.Alphabet = value;
                _morseAudio._isCreateTransAudio = false;
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


        public bool IsPlayingMorseAudio
        {
            get => _morseAudio._isPlayingMorseAudio;

            set
            {
                _morseAudio._isPlayingMorseAudio = value;
                OnPropertyChanged(nameof(IsPlayingMorseAudio));
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

        public ICommand Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new RelayCommand<String>(
                        (Link) => System.Diagnostics.Process.Start(Link));
                }
                return _connection;
            }
        }

        public ICommand PlayMorseAudio
        {
            get
            {
                if (_playMorseAudio == null)
                {
                    _playMorseAudio = new RelayCommand(() =>
                    {
                        if (!_morseAudio._isCreateTransAudio)
                        {
                            _morseAudio.CreateTransAudio(MorseCode);
                        }

                        _morseAudio.PlayTransAudio(value => IsPlayingMorseAudio = value);

                    });
                }
                return _playMorseAudio;
            }
        }

        public ICommand TronTronVN
        {
            get
            {
                if (_trontronVN == null)
                {
                    _trontronVN = new RelayCommand(() =>
                    {
                        MessageBox.Show("trời ơi nghĩ sao mà dám mở link X lên vạy =))))\nMuốn link thì hãy DM ta hehhe!", "Đừng Xin Link X :( ");
                    });
                }
                return _trontronVN;
            }
        }

        public MorseTranslatorViewModel()
        {
           
        }

    }

}
