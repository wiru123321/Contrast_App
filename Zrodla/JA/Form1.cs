using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JA
{
    public partial class Form1 : Form
    {
        
        private Image zrodlo;
        public byte[] bitmaparray;
        private String file;
        public byte[] LUT;
        private int height, width, offset;
        public int Stride;
        private List<Thread> threads = new List<Thread>();
        public Form1 form1;
        public Form1()
        {
            InitializeComponent();
            TextBlock1.Text =Environment.ProcessorCount.ToString();
            trackBar1.Value = Environment.ProcessorCount;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Otwórz obraz";
            dlg.Filter = "pliki jpg (*.jpg)|*.jpg|pliki png (*.png)|*.png|wszystkie pliki (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    file = dlg.FileName;
                    zrodlo = new Bitmap(file);
                    if (zrodlo.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                    {
                        Bitmap bmp = new Bitmap(file);
                        pictureBox1.Image = bmp;
                    }
                    else
                    {
                        MessageBox.Show("Illegal format.");
                    }
                }
                catch (Exception todo)//to do
                {
                    MessageBox.Show(todo.Message);
                }
            }
            else
            {
                MessageBox.Show("Unable to load file");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void CalcBytes()
        {
            offset = bitmaparray[13] << 24 | bitmaparray[12] << 16 | bitmaparray[11] << 8 | bitmaparray[10];
            width = bitmaparray[21] << 24 | bitmaparray[20] << 16 | bitmaparray[19] << 8 | bitmaparray[18];
            height = bitmaparray[25] << 24 | bitmaparray[24] << 16 | bitmaparray[23] << 8 | bitmaparray[22];
        }

/*
        private void ShowBitmap()
        {
            
            BitmapImage bmp = new BitmapImage();
            using (var ms = new MemoryStream(bitmaparray))
            {
                bmp.BeginInit();
                bmp.StreamSource = ms;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
            }
            pictureBox2.Image = bmp;
            

            BitmapImage bmImg = new BitmapImage();
            using (MemoryStream memStream2 = new MemoryStream())
            {
                zrodlo.Save(memStream2, System.Drawing.Imaging.ImageFormat.Png);

                bmImg.BeginInit();
                bmImg.CacheOption = BitmapCacheOption.OnLoad;
                bmImg.UriSource = null;
                bmImg.StreamSource = memStream2;
                bmImg.EndInit();
            }
            pictureBox2.Image = LoadImage(bmImg);
        }
*/
        private Image LoadImage(BitmapImage bmImg)
        {
            throw new NotImplementedException();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            zrodlo = new Bitmap(file);
            EdgeDetection algorithm = new EdgeDetection(zrodlo);
            algorithm.LoadLut(suwak.Value);
            //CalcBytes();
            if (checkBox1.CheckState == CheckState.Checked)
            {
                bool cpp = true;
                ThreadManager threadsSet = new ThreadManager(trackBar1.Value, cpp, ref algorithm);
                threadsSet.CreateThreadsSet();
                TextBlock.Text = threadsSet.RunThreads();
                pictureBox2.Image = algorithm.LoadToOutput();
                pictureBox2.Refresh();
            }
            else
            {
                bool cpp = false;
                ThreadManager threadsSet = new ThreadManager(trackBar1.Value, cpp, ref algorithm);
                threadsSet.CreateThreadsSet();
                TextBlock.Text = threadsSet.RunThreads();
                pictureBox2.Image = algorithm.LoadToOutput();
                pictureBox2.Refresh();
            }
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            TextBlock1.Text = trackBar1.Value.ToString();

        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                checkBox2.CheckState = CheckState.Unchecked;
            }
        }

        private void TextBlock_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.CheckState == CheckState.Checked)
            {
                checkBox1.CheckState = CheckState.Unchecked;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
