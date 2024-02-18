using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Translator.Processors;

namespace MVVMMorseTranslator.Models
{
    public partial class MorseAudioModel
    {
        // data to create Auido
        private int _WPM = 20;
        private int _Frequency = 600;
        private int Dot;
        private int Dash;
        private int RushWait;
        private int Wait;


        public int WPM { 
            get => _WPM;
            set {
                if (_WPM != value)
                {
                    _WPM = value;
                    MorseAudioDataInit();
                }
                
            }
        }

        public  int Frequency
        {
            get => _Frequency;
            set 
            {
                if (_Frequency != value)
                {
                    _Frequency = value;
                    MorseAudioBeepInit();
                }
            }
        }


        public MorseAudioModel()
        {
            MorseAudioDataInit();
            MorseAudioBeepInit();
        }
    }

    // Beep Audio 
    public partial class MorseAudioModel
    {
        private void MorseAudioDataInit()
        {
            // Dash = 3 Dot
            // Rush Wait = Dot
            // Wait = 7 Dot
            Dot = 1000 * 60 / (50 * WPM);
            Dash = Dot * 3;
            RushWait = Dot;
            Wait = Dot * 2;
        }

        private void MorseAudioBeepInit()
        {
            CreateBeep(DotPath, (UInt16)Frequency, Dot);
            CreateBeep(DashPath, (UInt16)Frequency, Dash);
            CreateBeep(WaitPath, 0, Wait);
            CreateBeep(RushWaitPath, 0, RushWait);
        }
        // Path data to create Audio file
        private String DotPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Dot.wav");
        private String DashPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Dash.wav");
        private String WaitPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Wait.wav");
        private String RushWaitPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "RushWait.wav");
        private String TransPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Trans.wav");


        // save audio to %Temp%
        public void SaveMusicToDisk(MemoryStream mStrm, String Path)
        {
            using (FileStream fileStream = File.Create(Path))
            {
                mStrm.CopyTo(fileStream);
            }
        }
        //Clear audio created in %temp% when exit app
        public void DeleteMusicFromDisk()
        {
            foreach (var file in (new List<String>() { TransPath, DotPath, DashPath, WaitPath, RushWaitPath }))
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        // Create Base Audio
        public void CreateBeep(String Name, UInt16 frequency, int msDuration, UInt16 volume = 65535 /*16383*/)
        {
            var mStrm = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mStrm);

            const double TAU = 2 * Math.PI;
            int formatChunkSize = 16;
            int headerSize = 8;
            short formatType = 1;
            short tracks = 1;
            int samplesPerSecond = 44100;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = samplesPerSecond * frameSize;
            int waveSize = 4;
            int samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
            int dataChunkSize = samples * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
            // var encoding = new System.Text.UTF8Encoding();
            writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
            writer.Write(fileSize);
            writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
            writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
            writer.Write(formatChunkSize);
            writer.Write(formatType);
            writer.Write(tracks);
            writer.Write(samplesPerSecond);
            writer.Write(bytesPerSecond);
            writer.Write(frameSize);
            writer.Write(bitsPerSample);
            writer.Write(0x61746164); // = encoding.GetBytes("data")
            writer.Write(dataChunkSize);
            {
                double theta = frequency * TAU / (double)samplesPerSecond;
                // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
                // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
                double amp = volume >> 2; // so we simply set amp = volume / 2
                for (int step = 0; step < samples; step++)
                {
                    short s = (short)(amp * Math.Sin(theta * (double)step));
                    writer.Write(s);
                }
            }
            mStrm.Seek(0, SeekOrigin.Begin);
            SaveMusicToDisk(mStrm, Name);
            mStrm.Close();
            writer.Close();
        }
    }
}
