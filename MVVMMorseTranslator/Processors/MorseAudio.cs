using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Documents;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using System.Windows.Controls.Primitives;

namespace Translator.Processors
{
    internal class MorseAudio
    {
        private static int _WPM = 20;
        private static int _Frequency = 600;
        public static int WPM { get { return _WPM; } set { _WPM = value; } }
        public static int Frequency { get { return _Frequency; } set { _Frequency = value; } }
        // timing for WPM : word per min
        private static int Dot = 1000 * 60 / (50 * WPM);
        private static int Dash = Dot * 3;
        private static int RushWait = Dot;
        private static int Wait = Dot * 2;
        //Audio Data Path
        private static string DotPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Dot.wav");
        private static string DashPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Dash.wav");
        private static string WaitPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Wait.wav");
        private static string RushWaitPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "RushWait.wav");
        private static string TransPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Trans.wav");


        public static void InitBaseAudio()
        {
            CreateBeep(DotPath, (UInt16)Frequency, Dot);
            CreateBeep(DashPath, (UInt16)Frequency, Dash);
            CreateBeep(WaitPath, 0, Wait);
            CreateBeep(RushWaitPath, 0, RushWait);
        }

        // đổi f -> đổi Base data
        public static void ChangeFrequency(int frequency)
        {
            Frequency = frequency;
            InitBaseAudio();
        }
        // đổi wpm -> đổi Base data
        public static void TimeCalculate(int wpm)
        {
            WPM = wpm;
            Dot = 60 / (50 * WPM);
            Dash = Dot * 3;
            RushWait = Dot;
            Wait = Dot * 7;
            InitBaseAudio();
        }

        // save audio to %Temp%
        public static void SaveMusicToDisk(MemoryStream mStrm, String Path)
        {
            using (FileStream fileStream = File.Create(Path))
            {
                mStrm.CopyTo(fileStream);
            }
        }
        //Clear audio created in %temp% when exit app
        public static void DeleteMusicFromDisk()
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
        public static void CreateBeep(String Name, UInt16 frequency, int msDuration, UInt16 volume = 65535 /*16383*/)
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


        // Play Audio Controller
        private static bool isPlayingAudio = false;
        private static MediaPlayer mediaPlayer = new MediaPlayer();
        public static void PlayMorseAudio(String MorseBox, Button speakerBtn)
        {
            if (MorseBox == String.Empty) return;
            if (!isPlayingAudio)
            {
                speakerBtn.Content = "Play";
                isPlayingAudio = true;
                List<AudioFileReader> AudioPart = new List<AudioFileReader>();
                foreach (char data in MorseBox)
                {
                    String path = "";
                    // Add another WAV file to merge
                    switch (data)
                    {
                        case '.':
                            path = DotPath;
                            break;
                        case '-':
                            path = DashPath;
                            break;
                        case '/':
                            path = WaitPath;
                            break;
                        default:
                            path = RushWaitPath;
                            break;
                    }
                    // AudioPart.Add(tempAudio);
                    AudioPart.Add(new AudioFileReader(path));
                    if (data != '/') AudioPart.Add(new AudioFileReader(RushWaitPath));
                    else if (data is ' ') AudioPart.Add(new AudioFileReader(RushWaitPath));
                }

                var Trans = new ConcatenatingSampleProvider(AudioPart);
                WaveFileWriter.CreateWaveFile16(TransPath, Trans);
                // release all file open
                foreach (var part in AudioPart)
                {
                    part.Close();
                }
                // play morse audio
                mediaPlayer.Open(new Uri(TransPath));
                mediaPlayer.MediaEnded += new EventHandler((p, _) =>
                {
                    speakerBtn.Content = "VolumeUp";
                    isPlayingAudio = false;
                    mediaPlayer.Close();
                });
                mediaPlayer.Volume = 1;
                mediaPlayer.Play();
            }
            else
            {
                speakerBtn.Content = "VolumeUp";
                isPlayingAudio = false;
                mediaPlayer.Stop();
                mediaPlayer.Close();
            }

        }
        public static void AudioStop()
        {
            isPlayingAudio = false;
            mediaPlayer.Stop();
            mediaPlayer.Close();
        }



    }
}
