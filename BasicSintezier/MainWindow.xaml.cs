using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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

namespace BasicSintezier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int SAMPLE_RATE = 44100;
        private const short BITS_PER_SAMPLE = 16;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            short[] wave= new short[SAMPLE_RATE];
            byte[] binaryWave = new byte[SAMPLE_RATE * sizeof(short)];
            float frequency = 640.0f;
            for (int i  = 0; i < SAMPLE_RATE; i++)
            {
                wave[i] = Convert.ToInt16(short.MaxValue * Math.Sin(((2*Math.PI * frequency)/SAMPLE_RATE)*i));
            }
            Buffer.BlockCopy(wave,0,binaryWave,0,wave.Length * sizeof(short));
            using(MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(ms))
                {
                    short blockAlign = BITS_PER_SAMPLE / 8;
                    int subChunckTwoSize = SAMPLE_RATE * blockAlign;
                    binaryWriter.Write(new[] { 'R', 'I', 'F', 'F' });
                    binaryWriter.Write(36 + subChunckTwoSize);
                    binaryWriter.Write(new[] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });
                    binaryWriter.Write(16);
                    binaryWriter.Write((short)1);
                    binaryWriter.Write((short)1);
                    binaryWriter.Write(SAMPLE_RATE);
                    binaryWriter.Write(SAMPLE_RATE * blockAlign);
                    binaryWriter.Write(blockAlign);
                    binaryWriter.Write(BITS_PER_SAMPLE);
                    binaryWriter.Write(new[] { 'd', 'a', 't', 'a' });
                    binaryWriter.Write(subChunckTwoSize);
                    binaryWriter.Write(binaryWave);
                    ms.Position = 0;
                    new SoundPlayer(ms).Play();
                }
            }

        }
    }
}
