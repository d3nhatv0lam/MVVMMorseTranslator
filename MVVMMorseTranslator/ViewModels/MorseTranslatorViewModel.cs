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
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Windows.Documents;
using System.Windows.Media;
using System.Runtime.ConstrainedExecution;
using NAudio.Wave;

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
        private ICommand _loadAlphabetAudioText;
        private ICommand _loadMorseAudioText;
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
                _morseAudio._isCreateTransAudio = false;
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
                        Alphabet = Alphabet;
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

        public ICommand LoadAlphabetAudioText
        {
            get
            {
                if (_loadAlphabetAudioText == null)
                {
                    _loadAlphabetAudioText = new RelayCommand<TextBlock>((textBlock) => FillAlphabetText(textBlock));
                }
                return _loadAlphabetAudioText;
            }
        }

        public ICommand LoadMorseAudioText
        {
            get
            {
                if (_loadMorseAudioText == null)
                {
                    _loadMorseAudioText = new RelayCommand<TextBlock>((textBlock) => FillMorseCodeText(textBlock));
                }
                return _loadMorseAudioText;
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

        private CancellationTokenSource cts_morse;
        private CancellationTokenSource cts_Alphabet;

        private async void FillAlphabetText(TextBlock textBlock)
        {
            textBlock.Text = String.Empty;
            if (cts_Alphabet == null)
            {
                cts_Alphabet = new CancellationTokenSource();
                try
                {
                    await ColorAlphabetText(textBlock, cts_Alphabet.Token);
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    cts_Alphabet = null;
                }
            }
            else
            {
                cts_Alphabet.Cancel();
                cts_Alphabet = null;
                textBlock.Text = String.Empty;
            }
        }

        private async void FillMorseCodeText(TextBlock textBlock)
        {
            textBlock.Text = String.Empty;
            if (cts_morse == null)
            {
                cts_morse = new CancellationTokenSource();
                try
                {
                    await ColorMorseText(textBlock, cts_morse.Token);
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    cts_morse = null;
                }
            }
            else
            {
                cts_morse.Cancel();
                cts_morse = null;
                textBlock.Text = String.Empty;
            }
            
        }

        private async Task ColorAlphabetText(TextBlock textBlock , CancellationToken token)
        {
            String AlphabetText = Alphabet.ToUpper();

            for (int i = 0; i < AlphabetText.Length; i++)
            {
                token.ThrowIfCancellationRequested();

                textBlock.Dispatcher.Invoke(() =>
                {
                    textBlock.Text += AlphabetText[i];
                });

                String MorseOfChar = _morse.GetMorseOfChar(AlphabetText[i]);

                for (int j = 0; j < MorseOfChar.Length; j++)
                {
                    int timeDelay = 0;
                    switch (MorseOfChar[j])
                    {
                        case '.':
                            timeDelay = _morseAudio.Dot;
                            break;
                        case '-':
                            timeDelay = _morseAudio.Dash;
                            break;
                        case '/':
                            timeDelay = _morseAudio.Wait;
                            break;
                        default:
                            break;
                    }

                    if (MorseOfChar[j] is '.' || MorseOfChar[j] is '-')
                    {
                        if (j + 1 < MorseOfChar.Length)
                            timeDelay += _morseAudio.RushWait;
                        else
                            if (i + 1 < AlphabetText.Length && AlphabetText[i + 1] != ' ')
                            timeDelay += 3 * _morseAudio.RushWait;

                    }

                    //textBlock.Dispatcher.Invoke(() =>
                    //{
                    //    textBlock.Text += MorseOfChar[j];
                    //});

                    await Task.Delay(timeDelay);
                }




            }
        }

        private async Task ColorMorseText(TextBlock textBlock , CancellationToken token)
        {
            for (int i = 0; i < MorseCode.Length; i++)
            {
                token.ThrowIfCancellationRequested();
                int timeDelay = 0;
                switch (MorseCode[i])
                {
                    case '.':
                        timeDelay = _morseAudio.Dot;
                        break;
                    case '-':
                        timeDelay = _morseAudio.Dash;
                        break;
                    case '/':
                        timeDelay = _morseAudio.Wait;
                        break;
                    case ' ':
                        timeDelay = ((i + 1 < MorseCode.Length) && (MorseCode[i + 1] == '/' || MorseCode[i - 1] == '/')) ? 0 : _morseAudio.RushWait;
                        break;
                    default:
                        break;
                }

                if (timeDelay != 0)
                {
                    // space = CharacterWait = 3 Dot
                    if (MorseCode[i] is ' ')
                    {
                        timeDelay += 2 * _morseAudio.RushWait;
                    }
                    else 

                    if (MorseCode[i] is '.' || MorseCode[i] is '-')
                    {
                        if ((i + 1 < MorseCode.Length) && (MorseCode[i + 1] != ' '))
                            timeDelay += _morseAudio.RushWait;
                    }
                }

                textBlock.Dispatcher.Invoke(() =>
                    {
                        textBlock.Text += MorseCode[i];
                    });


                 await Task.Delay(timeDelay);
            }

        }

    }

}
