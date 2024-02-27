using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVMMorseTranslator.Views
{
    /// <summary>
    /// Interaction logic for MorseTranslatorView.xaml
    /// </summary>
    public partial class MorseTranslatorView : UserControl
    {
        public MorseTranslatorView()
        {
            InitializeComponent();
        }


        //private void click(object sender, RoutedEventArgs e)
        //{
        //    MorseAudioTextBox.Document.Blocks.Clear();
        //    String inputText = "asdasd";

        //    TextRange textRange = new TextRange(MorseAudioTextBox.Document.ContentEnd, MorseAudioTextBox.Document.ContentEnd);

        //    // Đặt nội dung của TextRange bằng chuỗi đầu vào
        //    //textRange.Text += inputText;
        //    Paragraph paragraph = new Paragraph();

        //    foreach (char character in inputText)
        //    {
        //        Run run = new Run(character.ToString());

        //        paragraph.Inlines.Add(run);

        //        // Đặt thuộc tính cho từng phần tử Run (ví dụ: màu sắc)
        //        // run.Foreground = Brushes.Red; // Đặt màu sắc theo ý muốn

        //        // Thêm phần tử Run vào RichTextBox
                

        //        //MorseTextBox.SelectAll();
        //        //string Str = MorseTextBox.Selection.Text;
        //        //Debug.Write(Str);
        //    }
        //    MorseAudioTextBox.Document.Blocks.Add(paragraph);

        //    foreach (var chr in MorseAudioTextBox.Document.Blocks)
        //    {

        //    }

        //    MorseAudioTextBox.Selection.Select(MorseAudioTextBox.Document.ContentStart.GetPositionAtOffset(1), MorseAudioTextBox.Document.ContentStart.GetPositionAtOffset(4).GetPositionAtOffset(1));
        //    Debug.Write(MorseAudioTextBox.Selection.Text);
        //    }

    }
}
