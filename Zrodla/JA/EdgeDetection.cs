using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JA {

    unsafe class EdgeDetection
    {
        /// <summary>
        /// Input image.
        /// </summary>
        private Image Image;
        /// <summary>
        /// Array to fill by algoritm.
        /// </summary>
        private byte[] LUT;
        /// <summary>
        /// Array with pixels of the image.
        /// </summary>
        private byte[] pixelValues;
        /// <summary>
        /// Stride of image.
        /// </summary>
        private int Stride;
        /// <summary>
        /// Contains bitmap info. 
        /// </summary>
        private BitmapData bmpData;

        byte[] toAdd;


        public EdgeDetection(Image image)
        {
            this.Image = image;
            LoadBitmapToPixelsArray();
        }


        [DllImport(@"C:\Users\wojte\Desktop\Studia\JA\ProjektJA\x64\Debug\AsmDll.dll")]
        private static unsafe extern void operateOnPixelsAsm(byte* pixels, byte* copy1);


        private const string DllFilePath = @"C:\Users\wojte\Desktop\Studia\JA\ProjektJA\x64\Debug\DLL_C.dll";

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]

        public static unsafe extern byte* BitmapConvert(byte* pixels, byte* copy1);


        private void LoadBitmapToPixelsArray()
        {
            Bitmap bitmap = (Bitmap)Image;
            bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            Stride = Math.Abs(bmpData.Stride);
            pixelValues = new byte[Stride * Image.Height];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixelValues, 0, pixelValues.Length);
            toAdd = new byte[Stride * Image.Height];
        }
        public void LoadLut(int value)
        {
            LUT = new byte[256];
            double a;
            if (value <= 0)
            {
                a = 1.0 + (value / 256.0);
            }
            else
            {
                a = 256.0 / Math.Pow(2, Math.Log(257 - value, 2));
            }
            for (int i = 0; i < 256; i++)
            {
                if ((a * (i - 127) + 127) > 255)
                {
                    LUT[i] = 255;
                }
                else if ((a * (i - 127) + 127) < 0)
                {
                    LUT[i] = 0;
                }
                else
                {
                    LUT[i] = (byte)(a * (i - 127) + 127);
                }
            }
        }

        public int getNoOfVectors()
        {
            return 2 * Stride * Image.Height / 16;
        }

        

        private void AddPairs()
        {
            for (int i = 0; i < pixelValues.Length; i++)
            {
                pixelValues[i] = LUT[pixelValues[i]];
            }
        }
        
        public void RunAsmDll(int begin, int end)///run the ASM file
        {
            int j = begin;
            while (j < end/2)
            {
                fixed (byte* pix = &toAdd[j]) fixed (byte* cpy1 = &toAdd[toAdd[j]])
                {
                    operateOnPixelsAsm(pix, cpy1);
                }
                j += 16;
            }
        }
        

        public void RunCppDll(int begin, int end)/// Run the cpp file 
        {
            int j = begin;
            while (j < end/2)
            {

                unsafe
                {
                    fixed (byte* pix = &toAdd[j]) fixed (byte* cpy1 = &toAdd[toAdd[j]])
                    {
                        BitmapConvert(pix, cpy1);
                    }
                }
                j +=16;
            }
        }
        public Bitmap LoadToOutput()
        {
            AddPairs();
            Bitmap bitmap = (Bitmap)Image;
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, bmpData.Scan0, pixelValues.Length);
            bitmap.UnlockBits(bmpData);
            return bitmap;
        }
    }
}
