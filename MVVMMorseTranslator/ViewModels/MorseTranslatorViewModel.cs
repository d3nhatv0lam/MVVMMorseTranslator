using MVVMMorseTranslator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MVVMMorseTranslator.ViewModels
{
    public partial class MorseTranslatorViewModel : ViewModelBase
    {
        public String Alphabet
        {
            get { return _alphabet; }
            set
            {
                _alphabet = value;
                Changed(Alphabet);
                OnPropertyChanged(nameof(Alphabet));
            }
        }
        public String MorseCode
        {
            get { return _morseCode; }
            set
            {
                _morseCode = value;
                Changed(MorseCode);
                OnPropertyChanged(nameof(MorseCode));
            }
        }

        public MorseTranslatorViewModel()
        {

        }


    }

    // MorseCode translator medhood
    public partial class MorseTranslatorViewModel
    {

        private readonly Dictionary<char, string> _morseCodeDictionary = new Dictionary<char, string>()
        {
            {'A', ".-"},
            {'B', "-..."},
            {'C', "-.-."},
            {'D', "-.."},
            {'E', "."},
            {'F', "..-."},
            {'G', "--."},
            {'H', "...."},
            {'I', ".."},
            {'J', ".---"},
            {'K', "-.-"},
            {'L', ".-.."},
            {'M', "--"},
            {'N', "-."},
            {'O', "---"},
            {'P', ".--."},
            {'Q', "--.-"},
            {'R', ".-."},
            {'S', "..."},
            {'T', "-"},
            {'U', "..-"},
            {'V', "...-"},
            {'W', ".--"},
            {'X', "-..-"},
            {'Y', "-.--"},
            {'Z', "--.."},
            {'0', "-----"},
            {'1', ".----"},
            {'2', "..---"},
            {'3', "...--"},
            {'4', "....-"},
            {'5', "....."},
            {'6', "-...."},
            {'7', "--..."},
            {'8', "---.."},
            {'9', "----."},
            {'.', ".-.-.-"},
            {',', "--..--"},
            {'?', "..--.."},
            {'\'', ".----."},
            {'!', "-.-.--"},
            {'/', "-..-."},
            {'(', "-.--."},
            {')', "-.--.-"},
            {'&', ".-..."},
            {':', "---..."},
            {';', "-.-.-."},
            {'=', "-...-"},
            {'+', ".-.-."},
            {'-', "-....-"},
            {'_', "..--.-"},
            {'\"', ".-..-."},
            {'$', "...-..-"},
            {'@', ".--.-."},
            {' ', "/"},
        };

        private String _alphabet;
        private String _morseCode;
        private bool _alphachanged = false;
        private bool _morsechanged = false;

        private void Changed(String code)
        {
            if (Alphabet == String.Empty && MorseCode == String.Empty) return;
            // 2 cái form bị lỗi như nhau
            if (Alphabet == MorseCode) return;

            if (code == Alphabet && !_morsechanged)
            {
                _alphachanged = true;
                MorseCode = Encode(Alphabet);
                _alphachanged = false;

            }

            if (code == MorseCode && !_alphachanged)
            {
                _morsechanged = true;
                Alphabet = Decode(MorseCode);
                _morsechanged = false;
            }
        }
        private string Encode(string Message)
        {
            if (Message == String.Empty) return String.Empty;
            //string Mess = Message.Trim(Environment.NewLine.ToCharArray()).ToUpper();
            string Mess = string.Join(" ", Message.Split('\n').Select(s => s.Trim())).ToUpper();
            string encodeMessaged = "";
            foreach (char character in Mess)
            {
                if (_morseCodeDictionary.ContainsKey(character))
                {
                    encodeMessaged += _morseCodeDictionary[character] + " ";
                }
                else encodeMessaged += character + " ";
            }

            return encodeMessaged;
        }


        public string Decode(string Message)
        {
            if (Message == String.Empty) return String.Empty;

            // cắt ra thành các từ
            string[] Words = Message.Split('/');

            string DecodeMessaged = "";

            foreach (string word in Words)
            {
                // cắt ra thành các chữ cái từ 1 từ
                string[] letters = word.Trim().Split(' ');
                // kiểm tra chữ cái đã cho có nằm trong danh sách mã morse không
                foreach (string letter in letters)
                {
                    if (letter == "") continue;

                    bool Found = false;
                    foreach (KeyValuePair<char, string> kvp in _morseCodeDictionary)
                    {
                        if (kvp.Value == letter)
                        {
                            DecodeMessaged += kvp.Key;
                            Found = true;
                            break;
                        }
                    }
                    if (!Found) DecodeMessaged += "#";
                }
                DecodeMessaged += " ";
            }
            return DecodeMessaged;
        }
    }
}
