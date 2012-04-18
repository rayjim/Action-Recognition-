/*****************************************************************************
 *****************************************************************************
 **********      Algorithms for action recongnition                    *******
 **********          Ray Bao 2009.05.08                                *******
 **********    The algorithm is used for the new hieratical approach   *******
 **********    for action recognition.  common computer 0515 1802      *******
 ***************************************************************************** 
 ******************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using myImgp;
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace myImgp
{
    public unsafe class FastBitmap
    {
        public struct PixelData
        { public byte blue; public byte green; public byte red; }
        Bitmap Subject; int SubjectWidth;
        BitmapData bitmapData = null;
        Byte* pBase = null;
        public FastBitmap(Bitmap SubjectBitmap)
        {
            this.Subject = SubjectBitmap;
            //try { 
            LockBitmap(SubjectBitmap);
            //} 
            //  catch (Exception ex)
            //{ throw ex; }
        }

        public void Release()
        { try { UnlockBitmap(); } catch (Exception ex) { throw ex; } }

        public Bitmap Bitmap { get { return Subject; } }
        public void SetPixel(int X, int Y, Color Colour)
        {
            try
            {
                PixelData* p = PixelAt(X, Y);
                p->red = Colour.R;
                p->green = Colour.G;
                p->blue = Colour.B;
            }
            catch (AccessViolationException ave) { throw (ave); }
            catch (Exception ex) { throw ex; }
        }
        public Color GetPixel(int X, int Y)
        {
            try
            {
                PixelData* p = PixelAt(X, Y);
                return Color.FromArgb((int)p->red, (int)p->green, (int)p->blue);
            }
            catch (AccessViolationException ave)
            { throw (ave); }
            catch (Exception ex) { throw ex; }
        }
        private void LockBitmap(Bitmap image)
        {
            GraphicsUnit unit = GraphicsUnit.Pixel;
            RectangleF boundsF = Subject.GetBounds(ref unit);
            Rectangle bounds = new Rectangle((int)boundsF.X, (int)boundsF.Y, (int)boundsF.Width, (int)boundsF.Height);
            SubjectWidth = (int)boundsF.Width * sizeof(PixelData);
            if (SubjectWidth % 4 != 0) { SubjectWidth = 4 * (SubjectWidth / 4 + 1); }
            bitmapData = Subject.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            pBase = (Byte*)bitmapData.Scan0.ToPointer();
        }

        private PixelData* PixelAt(int x, int y) { return (PixelData*)(pBase + y * SubjectWidth + x * sizeof(PixelData)); }

        private void UnlockBitmap()

        { Subject.UnlockBits(bitmapData); bitmapData = null; pBase = null; }
    }






    public class exam

    {

        public struct PixelData
        {
            public byte Blue;
            public byte Green;
            public byte Red;
        }
        static Bitmap image;//solve the problem about the bitmap's constructor
        static Form2 probsolve = new Form2();//sovle the problem about closing the process
        static Random random = new Random();
        static int[] result = new int[1000];
        static public bool stopMethod = false;
        static public int maxFrame = 1500; // indicate maxFrame


        static public void CreateFile(string filePath, int[][] array)			//Generate PPED File
        {
            filePath = @"d:\works\" + filePath;

            using (StreamWriter sw = new StreamWriter(System.IO.File.Open(filePath, System.IO.FileMode.Append)))
            {
                for (int i = 0; i < array.LongLength; i++)
                {
                    for (int j = 0; j < array[i].Length; j++)
                    {

                        sw.WriteLine(array[i][j]);
                    }
                    sw.WriteLine(",");
                }


            }
            //System.Console.WriteLine("file created with data using the streamWriter class.");


        }
        static public void CreateFile(string filePath, int[] array)                  //Generate PPED File
        {
            filePath = @"D:\works\" + filePath;
            using (StreamWriter sw = new StreamWriter(System.IO.File.Open(filePath, System.IO.FileMode.Append)))
            {

                {
                    for (int j = 0; j < array.Length; j++)
                    {

                        sw.WriteLine(array[j]);
                    }
                    sw.WriteLine(",");
                    

                }


            }
            //System.Console.WriteLine("file created with data using the streamWriter class.");


        }
        /// <summary>
        /// maximum num of database is 100
        /// </summary>
        /// <param name="filePath">Input the file name of the Database</param>
        /// <returns></returns>
        /// 
        static public int[][] OutputFile(string filePath)							//Load data...
        {
            filePath = @"d:\works\" + filePath;
            using (StreamReader sr = new StreamReader(filePath))
            {
                int[][] array = new int[100][];
                for (int q = 0; q < 100; q++)
                    array[q] = new int[64];
                int i = 0, j = 0;

                while (sr.Peek() > 0)
                {

                    string lineofText = sr.ReadLine();
                    if (lineofText == ",")
                    { i++; j = 0; }
                    else
                    {
                        array[i][j++] = Convert.ToInt32(lineofText);

                    }
                }
                return array;

            }

        }
        /// <summary>
        /// Load picture, and return a 2D byte arrayse
        /// </summary>
        /// <param name="FileName">The input of Filename</param>
        /// <returns>Byte arrays of 2D</returns>
        public static byte[,] LoadImg(string FileName)
        {
            FileName = (FileName + ".bmp");
            Bitmap image = new Bitmap(FileName);
            FastBitmap image1 = new FastBitmap(image);
            Color cc = new Color();
            int xres, yres, i, j;
            xres = image.Width;
            yres = image.Height;
            byte[,] imMatrix = new byte[xres, yres];
            for (i = 0; i <= xres - 1; i++)
                for (j = 0; j <= yres - 1; j++)
                    imMatrix[i, j] = 0;
            for (i = 0; i <= xres - 1; i++)
                for (j = 0; j <= yres - 1; j++)
                {
                    cc = image1.GetPixel(i, j);
                    imMatrix[i, j] = (byte)((cc.R + cc.G + cc.B) / 3);
                    

                }
            return imMatrix;








        }

        /// <summary>
        /// Convert Byte value to edge value from a given 2D array
        /// </summary>
        /// <param name="f">Input a 2D array with byte value</param>
        /// <returns>Return a int value array</returns>

        public static int[,] pic2edge(byte[,] f)
        {  
            int w,h,i,j;
            w = f.GetLength(0);
            h = f.GetLength(1);
            int[,] g = new int[w, h];
            int[] trans = new int[256];
            trans[0] = 1;
            trans[255] = 0;
            for (i = 0; i < w; i++)
                for (j = 0; j < h; j++)
                { 
                    g[i,j] = trans[f[i,j]];
                  
                }


            return g;
        }
        public static byte[,] edge2pic(int[,] f)
        { 
            int w,h,i,j;
            w = f.GetLength(0);
            h = f.GetLength(1);
            byte[,] g = new byte[w, h];
            byte[] trans = new byte[2];
            trans[0] = 255;
            trans[1] = 0;
            for (i = 0; i < w; i++)
                for (j = 0; j < h; j++)
                { 
                    g[i,j] = trans[f[i,j]];
                  
                }
            return g;
        
        
        }
        /// <summary>
        /// select picture and load it.
        /// </summary>
        /// <returns></returns>
        public static byte[,] LoadImg()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            //openFile.Filter="BMP File(*.bmp)|*.bmp|JPEG File(*.jpg)|*.jpg|All File(*.*)|*.*";
            openFile.Filter = "图像文件 (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|所有文件 (*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                //Form2 child=new Form2();
                string Text = openFile.FileName;
                image = new Bitmap(openFile.FileName);

                FastBitmap image1 = new FastBitmap(image);
                //child.PictureBox1.Image=image;
                //child.Show();
                Color cc = new Color();
                int xres, yres, i, j;
                xres = image.Width;
                yres = image.Height;
                byte[,] imMatrix = new byte[xres, yres];
                for (i = 0; i <= xres - 1; i++)
                    for (j = 0; j <= yres - 1; j++)
                        imMatrix[i, j] = 0;
                for (i = 0; i <= xres - 1; i++)
                    for (j = 0; j <= yres - 1; j++)
                    {
                        cc = image1.GetPixel(i, j);
                        imMatrix[i, j] = (byte)((cc.R + cc.G + cc.B) / 3);


                    }
                image1.Release();
                return imMatrix;
            }
            return null;
        }
        /// <summary>
        /// class ARGB, which had three sub-value, including red,green and blue
        /// </summary>
        public class ARGB
        {

            public int R;
            public int G;
            public int B;
            public ARGB(int r, int g, int b)
            {
                R = r;
                G = g;
                B = b;

            }
            public ARGB()
            {
                R = 0;
                G = 0;
                B = 0;

            }


        }

        public class motionfield
        {
        public int pos;
        public int neg;
        public motionfield (int p, int n)
        {
            pos = p;
            neg = n;
        
        }
        public motionfield()
        {
            pos = 0;
            neg = 0;
        
            }
    
        
        }

        struct erro
        {
            public int classnum;
            public int righclass;
            public int wrongclass;
        
        
        }

       
        /// <summary>
        /// Load color picture, you can select file by mouse and will return a ARGB arrays
        /// </summary>
        /// <returns></returns>
        public static ARGB[,] LoadColorImg()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Openfile (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All Files (*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(openFile.FileName);
                FastBitmap image1 = new FastBitmap(image);
                Color cc = new Color();
                int xres, yres, i, j;
                xres = image.Width;
                yres = image.Height;
                ARGB[,] argb = new ARGB[xres, yres];

                for (i = 0; i <= xres - 1; i++)
                    for (j = 0; j <= yres - 1; j++)
                    {
                        argb[i, j] = new ARGB(0, 0, 0);

                    }
                for (i = 0; i <= xres - 1; i++)
                    for (j = 0; j <= yres - 1; j++)
                    {
                        cc = image1.GetPixel(i, j);
                        argb[i, j].R = cc.R;
                        argb[i, j].G = cc.G;
                        argb[i, j].B = cc.B;



                    }
                return argb;

            }
            return null;
        }
        /// <summary>
        /// show picture which is saved in matrix
        /// </summary>
        /// <param name="str"></param>
        /// <param name="imMatrix"></param>
        public static void ShowImg(string str, byte[,] imMatrix)
        {
            int i, j, w, h;
            byte pp;
            Form2 child = new Form2();
            probsolve = child;
            child.Show();
            child.Text = str;
            w = imMatrix.GetLength(0);
            h = imMatrix.GetLength(1);
            Graphics g1 = child.PictureBox1.CreateGraphics();
            Bitmap box1 = new Bitmap(w, h);
            FastBitmap box11 = new FastBitmap(box1);
            
            Color cc = new Color();
            child.CLientSize = new Size(w, h);
            for (i = 0; i < w; i++)
                for (j = 0; j < h; j++)
                {
                    pp = imMatrix[i, j];
                    cc = Color.FromArgb(pp, pp, pp);
                    box11.SetPixel(i, j, cc);


                }
            child.PictureBox1.Refresh();
            box1 = box11.Bitmap;
            child.PictureBox1.Image = box1;
            box11.Release();
            // Application.Run(child);

        }

        /// <summary>
        /// Save image to the file
        /// </summary>
        /// <param name="imMatrix">Image matrix needed to be saved</param>
        /// <param name="fileName">Specify the filename that need to save</param>
         public static void saveImage(byte[,] imMatrix,string fileName)
        {
            int i, j;
            byte pp;
            ;//PGM Picuture;
            fileName = (fileName+".bmp");
            
            int w = imMatrix.GetLength(0);
            int h = imMatrix.GetLength(1);
            Bitmap box1 = new Bitmap(w,h);
            FastBitmap box11 = new FastBitmap(box1);
            Color cc = new Color();
            for (i = 0; i < w; i++)
                for (j = 0; j < h; j++)
                {
                    pp = imMatrix[i, j];
                    cc = Color.FromArgb(pp, pp, pp);
                    box11.SetPixel(i, j, cc);


                }
            for (i = 0; i < w; i++)
                for (j = 0; j < h; j++)
                {
                 
                   
                    cc = box11.GetPixel(i, j);
                    imMatrix[i, j] = cc.R;

                }
           
            box1 = box11.Bitmap;
            ShowImg("box1", imMatrix);
            System.IO.File.Delete(fileName);
             
            box1.Save(fileName);
            box11.Release();
            
        
        }

        /// <summary>
        /// Save color image to files
        /// </summary>
        /// <param name="imMatrix">Image matrix needed to be saved</param>
        /// <param name="fileName">Specify the filename to be save in</param>
        public static void saveColorImage(ARGB[,] imMatrix, string fileName)
        {
            int i, j;
            ARGB pp;
            //PGM Picuture;
            fileName = (fileName + ".bmp");

            int w = imMatrix.GetLength(0);
            int h = imMatrix.GetLength(1);
            Bitmap box1 = new Bitmap(w,h);
          //  FastBitmap box11 = new FastBitmap(box1);
            Color cc = new Color();
            for (i = 0; i < w; i++)
                for (j = 0; j < h; j++)
                {
                    pp = imMatrix[i, j];
                   // cc = Color.FromArgb(255, 255, 255);
                    cc = Color.FromArgb(pp.R, pp.G, pp.B);
                    box1.SetPixel(i, j, cc);


                }
           /*
            for (i = 0; i < w; i++)
                for (j = 0; j < h; j++)
                {


                    cc = box11.GetPixel(i, j);
                    imMatrix[i, j] = cc.R;

                }
            */
        //    box1 = box11.Bitmap;
          //  ShowImg("box1", imMatrix);
         //   System.IO.File.Delete(fileName);

            box1.Save(fileName);
            //box11.Release();


        }

        /// <summary>
        /// show the recongnition result
        /// </summary>

        /// <summary>
        /// show the result;
        /// </summary>
        /// <param name="num"></param>

        /// <summary>
        /// Show color image
        /// </summary>
        /// <param name="str">The name for the form</param>
        /// <param name="imMatrix">Matrix needed to be show</param>
        public static void ShowImg(string str, ARGB[,] imMatrix)
        {
            int i, j, w, h;
            int rr, gg, bb;
            Form2 child = new Form2();
            probsolve = child;
            child.Text = str;
            w = imMatrix.GetLength(0);
            h = imMatrix.GetLength(1);
            Bitmap box1 = new Bitmap(w, h);
            FastBitmap box11 = new FastBitmap(box1);
            child.Show();


            Color cc = new Color();


            for (i = 0; i < w; i++)
                for (j = 0; j < h; j++)
                {
                    rr = imMatrix[i, j].R;
                    gg = imMatrix[i, j].G;
                    bb = imMatrix[i, j].B;
                    cc = Color.FromArgb(rr, gg, bb);
                    box11.SetPixel(i, j, cc);


                }
            child.PictureBox1.Refresh();
            box1 = box11.Bitmap;
            child.PictureBox1.Image = box1;
            box11.Release();
        

           // Application.Run(child);

        }

        /// <summary>
        /// Generate a random number which is double type
        /// </summary>
        /// <returns></returns>
        public static double Rand()
        {

            return random.NextDouble();
            
        }

        /// <summary>
        /// Generate random numbers between lowest bond and up-most bond
        /// </summary>
        /// <param name="lowest">Specify the lower bond</param>
        /// <param name="highest">Specify the upper bond</param>
        /// <returns></returns>
        public static int Rand(int lowest, int highest)
        {
            return random.Next(lowest, highest);

        }
        /// <summary>
        /// Generate a 1D array of random number
        /// </summary>
        /// <param name="lowest">Lower bond of array</param>
        /// <param name="highest">Upper bond of the array</param>
        /// <param name="num">Specify the size of the random array</param>
        /// <returns></returns>
        public static int[]  Rand(int lowest, int highest, int num)
        {
            int[] result = new int[num];
            for(int i = 0; i<num; i++)
            {
                result[i] = random.Next(lowest,highest);
            }
            return result;
        
        }

        /// <summary>
        /// For accumulating method, if one patch is too popular, eliminate it
        /// </summary>
        /// <param name="original">The original feature vector</param>
        /// <param name="threshold">Threshold if one patch is popular</param>
        /// <returns></returns>
        public static int[][] unpopular (int[][] original, int threshold)
        {
            int length = original.GetLength(0);
            int size = original[1].GetLength(0);
            int[][] result = new int[length][];
            
            for (int i = 0; i <length - 1; i++)
            {

                result[i] = new int[size];
                for (int j = 0; j < size - 1; j++)
                {
                    try
                    {
                        if (original[i][j] > threshold)
                            result[i][j] = 0;
                        else
                            result[i][j] = original[i][j];
                    }
                    catch (NullReferenceException ex) {
                        return result;
                    
                    }
                    
                
                }
                }
            return result;
    
    }

/// <summary>
/// Working as filter, return byte array
/// </summary>
/// <param name="f">Image matrix</param>
/// <param name="mask">Mask</param>
/// <param name="name">The name for the window</param>
/// <returns></returns>


        public static byte[,] filter2D(byte[,] f, float[] mask, string name)
        {
            int w = f.GetLength(0);
            int h = f.GetLength(1);

            byte[,] g = new byte[w, h];
            for (int i = 0; i < w - 1; i++)
                for (int j = 0; j < h - 1; j++)
                {
                    g[i, j] = 0;
                }

            for (int j = 0; j < h; j++)
                for (int i = 2; i < w - 3; i++)
                {
                    float s = 0;
                    for (int m = -2; m < 3; m++)
                    {
                        s += (f[i + m, j] * mask[m + 2]);

                    }
                    g[i, j] = (byte)Math.Abs(s);



                }
            //ShowImg("test",g);
            //binary 
            /*byte [] histo=new byte[256];
            for (int i=0;i<256;i++)
            {
                if (i<128) histo[i]=0;
                else histo[i]=1;
            }
            for(int i=0;i<w-1;i++)
                for(int j=0;j<h-1;j++)
                {
                    g[i,j]=histo[g[i,j]];
                }
        */
            return g;

        }
        /// <summary>

        // method for filtering
        /// </summary>
        /// <param name="f"></param>
        /// <param name="mask"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static byte[,] filter2D(byte[,] f, int[,] mask, string name)
        {
            int w = f.GetLength(0);
            int h = f.GetLength(1);
            int w_mask = mask.GetLength(0);
            int h_mask = mask.GetLength(0);

            byte[,] g = new byte[w, h];
           // byte[,] f = image;  // when convert to function, must be commented
            //int[,] mask = Kernel_Ver;// when convert to function, must be commented
            int sum_mask = 0;
            for (int ii = 0; ii < w_mask; ii++)
                for (int jj = 0; jj < h_mask; jj++)
                    sum_mask = sum_mask + mask[ii, jj];

                for (int i = 0; i < w; i++)
                    for (int j = 0; j < h; j++)
                    {
                        g[i, j] = 0;
                    }
            for (int i = w_mask / 2; i < w - (w_mask / 2+1); i++)
                for (int j = h_mask / 2; j < h - (h_mask/2+1); j++)
                {
                    int s = 0;
                    for (int m = -w_mask/2; m < (w_mask/2+1); m++)  //{(2,-3)}
                        for (int n = -h_mask/2; n <( h_mask/2+1); n++)
                        {
                            s += f[i + m, j + n] * mask[m + w_mask/2, n + h_mask/2];

                        }
                    if (sum_mask != 0)
                        g[i, j] = (byte)Math.Abs(s / sum_mask);
                    else
                        g[i, j] = (byte)Math.Abs(s);
                }

          // ShowImg(name, g);
                    return g;
        }
        public static byte[,] filter2D(byte[,] f, float[,] mask, string name)
        {
            int w = f.GetLength(0);
            int h = f.GetLength(1);

            byte[,] g = new byte[w, h];
            for (int i = 0; i < w - 1; i++)
                for (int j = 0; j < h - 1; j++)
                {
                    g[i, j] = 0;
                }
            for (int i = 2; i < w - 3; i++)
                for (int j = 2; j < h - 3; j++)
                {
                    float s = 0;
                    for (int m = -2; m < 3; m++)
                        for (int n = -2; n < 3; n++)
                        {
                            s += f[i + m, j + n] * mask[m + 2, n + 2];

                        }
                    g[i, j] = (byte)Math.Abs(s);


                }
            //ShowImg(name,g);

            /*
                byte [] histo=new byte[256];
                for (int i=0;i<256;i++)
                {
                    if (i<128) histo[i]=0;
                    else histo[i]=1;
                }
                for(int i=0;i<w-1;i++)
                    for(int j=0;j<h-1;j++)
                    {
                        g[i,j]=histo[g[i,j]];
                    }
            */
            return g;
        }
        /// <summary>
        /// Generate PPED vector
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>



        /// <summary>
        /// show Histo of certain vector
        /// </summary>
        /// <param name="histo"></param>
        /// <param name="name"></param>
        
        public static void ShowHisto(int[] histo, string name)
        {
            int w = histo.GetLength(0);
            byte[,] hImg = new byte[w * 9, 300];
            for (int x = 0; x < w * 9; x++)
                for (int y = 0; y < 300; y++)
                    hImg[x, y] = 255;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < 300; y++)
                {
                    if (y < histo[x]) hImg[x * 9, 299-y/2] = 0;
                    else hImg[x * 9, 299 - y/2] = 255;
                }
            ShowImg(name, hImg);


        }

        //thresholding 
        //Per means how many percentage left, name means the name of pic
        /// <summary>
        /// To perform thresholding and save given number of edges
        /// from original picture
        /// </summary>
        /// <param name="f"></param>
        /// <param name="Per"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static byte[,] Thresholding(byte[,] f, int Per, string name, int threWin, int threVal)
        {
            int w = f.GetLength(0);
            int h = f.GetLength(1);
            byte[,] g = f;
            int Image_size = w * h;// set Threshold as 20% of edge size;
            int Edge_Num = Convert.ToInt32(Image_size * Per/100);// w*h should be changed to image_size
            int[] histogram = new int[256]; //caculate histogram
            for (int i = 0; i < 256; i++)
            {
                histogram[i] = 0;
            }

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    histogram[(g[i, j])]++;
                }



            //ShowHisto(histogram, "test");

            // caculate the threshold
            int Sum = 0;
            int Threshold = 0;
            for (int i = 255; i > 0; i--)
            {
                Sum += histogram[i];
                if (Sum > Edge_Num)
                { Threshold = i+1; break; }
            }
            
            byte[,] Show_Img = new byte[w, h];
            for (int i = 0; i < w - 1; i++)
                for (int j = 0; j < h - 1; j++)
                {
                    if (g[i, j] < Threshold)
                    {
                        g[i, j] = 255;    // should be 0 for caculation
                      
                    }
                    else
                    {
                        g[i, j] = 0;     //should be 1 for caculation
                     
                    }
                }
            int sumWin;
            for (int i = 0; i < w - threWin; i++)
                for (int j = 0; j < h - threWin; j++)
                {
                    sumWin = 0;
                    for (int kk = 0; kk<threWin;kk++)
                        for (int qq = 0; qq < threWin; qq++)
                        {
                            if (g[i + kk, j + qq] == 0)
                                sumWin++;
                        
                        }
                    if (sumWin < threVal)
                        g[i, j] = 255;

                
                }

            //ShowImg("result of "+ name, Show_Img);
            return g;
          
            
        
        }

        public static int Caculate_Per(int[,] f)
        {
            int Per = 0;
            int w = f.GetLength(0);
            int h = f.GetLength(1);
            float sum = 0;
            float Image_size = w * h;
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    sum = f[i, j] + sum;
                }
            Per = Convert.ToInt32(sum / Image_size * 100);
            return Per;
        }

        /// <summary>
        /// Generate Temper
        /// </summary>
        /// <param name="Filename"></param>
        /// <param name="PicNum"></param>


        /// <summary>
        ///METHOD FOR COMPARING
        /// </summary>
        /// <param name="PPED"></param>
        /// <returns></returns>


        /// <summary>
        /// converlution method.
        /// 
        /// </summary>
        public static int[] conjj(int[] f, int[] g)
        {
            int i, j, jjsize;
            int lenf = f.GetLength(0);
            int leng = g.GetLength(0);
            int[] jj = new int[lenf + leng - 1];
            for (i = 0; i < lenf + leng - 1; i++)
                jj[i] = 0;
            for (i = 0; i < leng; i++)
                for (jjsize = i, j = 0; j < lenf; j++)
                    jj[jjsize++] += f[j] * g[i];
            return jj;

        }

        public static float[] conjj(float[] f, float[] g)
        {
            int i, j, jjsize;
            int lenf = f.GetLength(0);
            int leng = g.GetLength(0);
            float[] jj = new float[lenf + leng - 1];
            for (i = 0; i < lenf + leng - 1; i++)
                jj[i] = 0;
            for (i = 0; i < leng; i++)
                for (jjsize = i, j = 0; j < lenf; j++)
                    jj[jjsize++] += f[j] * g[i];
            return jj;

        }
        /// <summary>
        ///Generate edges on both direction and take threshold.
        /// </summary>
        /// <param name="fileName_Sor"></param>
        /// <param name="fileName_Des"></param>
        /// <param name="numStart"></param>
        /// <param name="num"></param>
        /// <param name="cut"></param>
        /// <param name="max"></param>
        /// <param name="Exp_show"></param>
        public static void Generate_EdgeB(string fileName_Sor, string fileName_Des, int numStart, int num, int cut)
        { 
            for (int i = 0; i < num; i++)
            {
                PGM imageInp;
                string fileName_Hori, fileName_Ver;
                if ((i+numStart) < 100)
                {
                    
                      imageInp = new PGM(fileName_Sor + "00" + Convert.ToString(i + numStart)+".pgm");
                      if (stopMethod)
                      {
                          stopMethod = false;
                          return;
                      }
                }
                else { 
                      imageInp = new PGM(fileName_Sor + "0" + Convert.ToString(i + numStart)+".pgm");
                      if (stopMethod)
                      {
                          stopMethod = false;
                          return;
                      }
                }
                PGM imag_Hori = imageInp;
                PGM imag_Ver = imageInp;
                PGM imag_fnl = imageInp;
                 int w = imageInp.Length;
                int h = imageInp.Width;

                int[,] Kernel_Ver = new int[5, 5] { { 0, 0, 0, 0, 0 }, { 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 0 }, { -1, -1, -1, -1, -1 }, { 0, 0, 0, 0, 0 } };    //Define the kernel
                int[,] Kernel_Hori = new int[5, 5] { { 0, 1, 0, -1, 0 }, { 0, 1, 0, -1, 0 }, { 0, 1, 0, -1, 0 }, { 0, 1, 0, -1, 0 }, { 0, 1, 0, -1, 0 } };
                byte[,] Imag_Hori = (filter2D(imageInp.mImg, Kernel_Hori, "Horizantal_edge"));
                byte[,] Imag_Ver = (filter2D(imageInp.mImg, Kernel_Ver, "Vertical_edge"));
                byte[,] g = new byte[w, h];
                byte[,] f = new byte[w, h];
                byte[,] congf = new byte[w, h];
                int congfInt;
                g = Imag_Hori;
                f = Imag_Ver;
                for (int ii = 0; ii < w; ii++)
                {
                    for (int jj = 0; jj < h; jj++)
                    {

                        congfInt = Convert.ToInt16(g[ii, jj]) + Convert.ToInt16( f[ii, jj]);
                        if (congfInt > 255)
                            congfInt = 255;
                        congf[ii, jj] = Convert.ToByte(congfInt);
                    
                    }
                
                }
                
                    congf = Thresholding(congf, cut, Convert.ToString(i), 7,20);
               // f = Thresholding(f, cut, Convert.ToString(i));
                    string fileName;
                    if ((i + numStart) < 100)
                    {
                        fileName = fileName_Des + "_00" + Convert.ToString(i + numStart) + "" + ".pgm";

                    }
                    else
                    {
                        fileName = fileName_Des + "_0" + Convert.ToString(i + numStart) + "" + ".pgm";
                    }
                
                imag_fnl.mImg = congf;
                imag_fnl.Save(fileName);
                


            }
        }

        public static void Generate_Edge(string fileName_Sor,string fileName_Des, int numStart, int num,int cut, bool max,bool Exp_show)
        {

            int threWin = 7;
            int threVal = 0;
            for (int i = 0; i < num; i++)
            {
                PGM imageInp;
                string fileName_Hori, fileName_Ver;
                if ((i+numStart) < 100)
                {
                    
                      imageInp = new PGM(fileName_Sor + "00" + Convert.ToString(i + numStart)+".pgm");
                      if (stopMethod)
                      {
                          stopMethod = false;
                          return;
                      }
                }
                else { 
                      imageInp = new PGM(fileName_Sor + "0" + Convert.ToString(i + numStart)+".pgm");
                      if (stopMethod)
                      {
                          stopMethod = false;
                          return;
                      }
                }
                PGM imag_Hori = imageInp;
                PGM imag_Ver = imageInp;

                int w = imageInp.Length;
                int h = imageInp.Width;

                int[,] Kernel_Hori = new int[5, 5] { { 0, 0, 0, 0, 0 }, { 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 0 }, { -1, -1, -1, -1, -1 }, { 0, 0, 0, 0, 0 } };    //Define the kernel
                int[,] Kernel_Ver = new int[5, 5] { { 0, 1, 0, -1, 0 }, { 0, 1, 0, -1, 0 }, { 0, 1, 0, -1, 0 }, { 0, 1, 0, -1, 0 }, { 0, 1, 0, -1, 0 } };
              //int[,] Kernel_Smooth = new int[3, 3] { {  1, 1, 1 }, {  1, 1, 1 }, {  1, 1, 1 }};
              int[,] Kernel_Smooth = new int[7, 7] { { 1, 1, 1, 1, 1,1,1 }, { 1, 1, 1, 1, 1,1,1 }, { 1, 1, 1, 1, 1,1,1 }, { 1, 1, 1, 1, 1,1,1 }, { 1, 1, 1, 1, 1,1,1 },{1,1,1,1,1,1,1},{1,1,1,1,1,1,1} }; 
               // int[,] Kernel_Smooth = new int[9, 9] { { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 } };
                byte[,] Imag_smooth = filter2D(imageInp.mImg, Kernel_Smooth, "smooth");
                byte[,] Imag_Hori = (filter2D(Imag_smooth, Kernel_Hori, "Horizantal_edge"));
                byte[,] Imag_Ver = (filter2D(Imag_smooth, Kernel_Ver, "Vertical_edge"));
                // ShowImg(Convert.ToString(i), Imag_Ver);
                //********************************************************************************************
                byte[,] g = new byte[w, h];
                byte[,] f = new byte[w, h];
                 
                //ShowImg("1", g);
       
              
                {
                    g = Imag_Hori;
                    f = Imag_Ver;
                }

                if (Exp_show)
                {
                    ShowImg("f", f);
                    ShowImg("g", g);
                }
                
                    g = Thresholding(g, cut, Convert.ToString(i), threWin,threVal);
                    f = Thresholding(f, cut, Convert.ToString(i),threWin,threVal);

                    if (Exp_show)
                    {
                        ShowImg("f", f);
                        ShowImg("g", g);
                    }
                

                //*****************************************************************************************


                if ((i + numStart) < 100)
                {
                    fileName_Hori = fileName_Des + "_00" + Convert.ToString(i + numStart) + "_Hori"+".pgm";
                    fileName_Ver = fileName_Des + "_00" + Convert.ToString(i + numStart) + "_Ver" + ".pgm";
                }
                else
                {
                    fileName_Hori = fileName_Des + "_0" + Convert.ToString(i + numStart) + "_Hori" + ".pgm";
                    fileName_Ver = fileName_Des + "_0" + Convert.ToString(i + numStart) + "_Ver" + ".pgm";
                }
              //  ShowImg("see", Imag_Ver);
               // ShowImg("f", imag_Hori.mImg);
               // ShowImg("g", imag_Ver.mImg);
               // ShowImg("f", f);
               // ShowImg("g", g);
                imag_Hori.mImg = g;
                imag_Hori.Save(fileName_Hori);
                imag_Ver.mImg = f;
               
               
                
               
                imag_Ver.Save(fileName_Ver);
            
            
            
            }
        
        }
        public static void Generate_DEGB(string fileName_Sor, string fileName_Des, int numStart, int num, int thre, bool Exp_show)
        {

            byte[,] imageInp;

            int[,] Imag_Or = new int[256, 256];


            int Per_test = new int();
            int[,] Imag_first = new int[256, 256];
            string fileName, fileName_Des1;
            int i = 0;
            for (int q = 0; q < num; q++)
            {
                PGM image_inp;


                byte[,] forShow = new byte[256, 256];
                i = q;  // startpoint
                for (int ii = 0; ii < 256; ii++)
                    for (int jj = 0; jj < 256; jj++)
                        Imag_Or[ii, jj] = 0;


                if ((q + numStart) < 100)
                {

                    fileName = (fileName_Sor + "_00" + Convert.ToString(q + numStart) + "" + ".pgm");

                }
                else
                {
                    fileName = (fileName_Sor + "_0" + Convert.ToString(q + numStart) + ""  + ".pgm");

                }
                image_inp = new PGM(fileName);
                if (stopMethod)
                {
                    stopMethod = false;
                    return;
                }
                Imag_first = pic2edge(image_inp.mImg);

                int w = Imag_first.GetLength(0);
                int h = Imag_first.GetLength(1);
                while (Per_test < thre)
                {

                    if ((i + numStart) < 100)
                    {
                        image_inp = new PGM(fileName_Sor + "_00" + Convert.ToString(i + numStart) + ""  + ".pgm");
                        if (stopMethod)
                        {
                            stopMethod = false;
                            return;
                        }
                    }
                    else
                    {
                        image_inp = new PGM(fileName_Sor + "_0" + Convert.ToString(i + numStart) + "" + ".pgm");
                        if (stopMethod)
                        {
                            stopMethod = false;
                            return;
                        }
                    }
                    imageInp = image_inp.mImg;


                    int[,] img_edg = pic2edge(imageInp);
                    for (int ii = 0; ii < w; ii++)
                        for (int jj = 0; jj < h; jj++)
                        {
                            Imag_Or[ii, jj] = Convert.ToInt16(Convert.ToBoolean(Imag_Or[ii, jj]) | Convert.ToBoolean(img_edg[ii, jj]));
                            forShow[ii, jj] = Convert.ToByte(Imag_Or[ii, jj]);
                        }
                    Per_test = Caculate_Per(Imag_Or);
                    i++;
                    // ShowImg(Convert.ToString(i),edge2pic(Imag_Or));
                }

                Per_test = 0;

                //  ShowImg("Or,", edge2pic(Imag_Or));
                //  ShowImg("first,", edge2pic(Imag_first));
                int[,] Imag_Result = new int[w, h];
                for (int ii = 0; ii < w; ii++)
                    for (int jj = 0; jj < h; jj++)
                    {
                        if ((Imag_Or[ii, jj] + Imag_first[ii, jj]) == 1)
                        {
                            Imag_Result[ii, jj] = 1;
                        }
                        else
                            Imag_Result[ii, jj] = 0;

                    }

                if (Exp_show)
                {
                    ShowImg(Convert.ToString(thre), edge2pic(Imag_Result));
                }

                int result = Caculate_Per(Imag_Result);   // test
                byte[,] save_Image = edge2pic(Imag_Result);
                if ((q + numStart) < 100)
                {
                    fileName_Des1 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "" + ".pgm");
                }
                else
                {
                    fileName_Des1 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "" + ".pgm");
                }

                PGM image4Save = image_inp;
                image4Save.mImg = save_Image;
                image4Save.Save(fileName_Des1);
            }

        }



        //opt: Horizantal or vertical
        public static void Generate_DEG(string fileName_Sor, string fileName_Des, int numStart, int num, string opt, int increaThre, bool Exp_show)
        {
            
            byte[,] imageInp;
            
            
           int windowsize = 16;   //using for threshold window
            int windowthre = 45;  //threshold for DEG, values above is preserved
          //  int localSize = 2;

           
            
            
            string fileName, fileName_Des1;
            int i = 0;
           
            for (int q = 0; q < num; q++)
            {
                PGM image_inp;
                
               
            
                i = q;  // startpoint
              

                
                if ((q + numStart) < 100)
                {
                    
                    fileName = (fileName_Sor + "_00" + Convert.ToString(q + numStart) + "_" + opt+".pgm");
                    
                }
                else
                {
                    fileName = (fileName_Sor + "_0" + Convert.ToString(q + numStart) + "_" + opt+".pgm");
                    
                }
                image_inp = new PGM(fileName);
                if (stopMethod)
                {
                    stopMethod = false;
                    return;
                }
              
                int[,] Imag_first = pic2edge(image_inp.mImg);

                int w = Imag_first.GetLength(0);
                int h = Imag_first.GetLength(1);

                int[,] Imag_Or = new int[w,h];
                byte[,] forShow = new byte[w,h];
                for (int ii = 0; ii < w; ii++)
                    for (int jj = 0; jj < h; jj++)
                        Imag_Or[ii, jj] = 0;

                int Per_test;
               
                Per_test = Caculate_Per(Imag_first);
               int  thre = Per_test + increaThre;

                while (Per_test < thre)
                {
                    
                    if ((i + numStart) < 100)   //i is the number of picture
                    {
                        image_inp = new PGM(fileName_Sor + "_00" + Convert.ToString(i + numStart) + "_" + opt+".pgm");
                        if (stopMethod)
                        {
                            stopMethod = false;
                            return;
                        }
                    }
                    else{
                        image_inp = new PGM(fileName_Sor + "_0" + Convert.ToString(i + numStart) + "_" + opt+".pgm");
                        if (stopMethod)
                        {
                            stopMethod = false;
                            return;
                        }
                    }
                    imageInp = image_inp.mImg;
                    int[,] img_edg = pic2edge(imageInp);
                    
                   
                   
                    for (int ii = 0; ii < w; ii++)
                        for (int jj = 0; jj < h; jj++)
                        {
                            Imag_Or[ii, jj] = Convert.ToInt16(Convert.ToBoolean(Imag_Or[ii, jj]) | Convert.ToBoolean(img_edg[ii, jj]));
                            forShow[ii,jj] =Convert.ToByte(Imag_Or[ii,jj]);
                        }
                    Per_test = Caculate_Per(Imag_Or);
                    i++;
                //  ShowImg(Convert.ToString(i),edge2pic(Imag_Or));
                }
               
                Per_test = 0;
                
              //  ShowImg("Or,", edge2pic(Imag_Or));
               // ShowImg("first,", edge2pic(Imag_first));
                int[,] Imag_Result = new int[w, h];
                for (int ii = 0; ii < w; ii++)
                    for (int jj = 0; jj < h; jj++)
                    {
                        if ((Imag_Or[ii, jj] + Imag_first[ii, jj]) == 1)
                        {
                            Imag_Result[ii, jj] = 1;
                        }
                        else
                            Imag_Result[ii, jj] = 0;
               
                    }

                int[,] Imag_Result1 = new int[w, h];
                for (int ii = 0; ii < w; ii++)
                    for (int jj = 0; jj < h; jj++)
                        Imag_Result1[ii, jj] = 0;
                
                for (int ii = 0; ii < w - windowsize; ii++)
                    for (int jj = 0; jj < h-windowsize; jj++)
                    {
                        int sumcheck = 0;
                     for (int qq = 0; qq < windowsize; qq++)
                         for (int zz = 0; zz < windowsize; zz++)
                         {
                             sumcheck = sumcheck + Imag_Result[ii + qq, jj + zz];
                         
                         }

                     if (sumcheck >= windowthre)
                     {
                        for (int qq = 0; qq < windowsize; qq++)
                             for (int zz = 0; zz < windowsize; zz++)
                             {
                         /*   for (int qq = 0; qq < windowsize-localSize/2; qq++)
                             for (int zz = 0; zz < windowsize - localSize / 2; zz++)
                             {
                                    int localSum = 0;
                                      for(int mm = 0; mm<localSize; mm++)
                                          for (int nn = 0; nn < localSize; nn++)
                                          {

                                              localSum = localSum+Imag_Result[ii + qq+mm, jj + zz + nn];
                                     
                                          }
                                          if (localSum != 0)
                                         for(int mm = 0; mm<localSize; mm++)
                                          for (int nn = 0; nn <localSize; nn++)
                                          {

                                              Imag_Result1[ii+qq+mm,jj+zz+nn] = 1;
                                     
                                          }
                                    
                              */

                                 //Imag_Result1[ii + qq, jj + zz] = 1;
                                 Imag_Result1[ii + qq, jj + zz] = Imag_Result[ii + qq, jj + zz];

                             }

                     }
                    }


                if (Exp_show)
                {
                   ShowImg(Convert.ToString(thre), edge2pic(Imag_Result1));
                }
              
                int result = Caculate_Per(Imag_Result1);   // test
                byte[,] save_Image = edge2pic(Imag_Result1);
                if ((q + numStart) < 100)
                {
                    fileName_Des1 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_" + opt+".pgm");
                }
                else
                {
                    fileName_Des1 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_" + opt+".pgm");
                }
          
               PGM image4Save = image_inp;
                image4Save.mImg = save_Image;
                image4Save.Save(fileName_Des1);
            } 
        
        }
   

        static int[] SAD(int[]vec1, int[]vec2)
        {
            int len1 = 17;
            
            int[] result = new int[len1];
         
            for (int i = 0; i<len1;i++)
            {
            
                int sum = 0;
                for (int j= 0;j<16;j++)
                {
                   sum = sum + Math.Abs(vec1[j]-vec2[i+j]); 
                }
                result[i] = sum;
            }

            
            return result;
        
        }
      /**/
        static int findMov(int[] vec)
        {
            int l = vec.Length;
            int findMov = 0;
            int min = vec[0];
            int magic_Num = 0; // if the window contains only small amount of pixels, it then unmoved.
            int sum=0;
            int average; // see the paper of doctor from hayakawa at P20
            int ii;
            for (ii = 0; ii < l; ii++)
          
            {
                sum = sum+vec[ii];


            }
            average = sum/l/2;

            for ( ii = 0; ii < l; ii++)
            {
                if (vec[ii] != 0)
                    magic_Num = 1;
                if (min != Math.Min(min, vec[ii]))
                {
                    min = vec[ii];
                    findMov = ii;

                }
            }
           
            if (min>=average)

                findMov = 0;
            else
              findMov = findMov - 8;
            
         

            return (findMov);
        }

        static void Generate_Motion(string fileName_Sor, string fileName_Des, int numStart, int num, string opt, int thre, bool Exp_show)
        {

            byte[,] imageInp;
          
           
            string fileName, fileName_Des1;
            int i = 0;
            int[] vec1 = new int[16];
            int[] vec2 = new int[32];
            for (int q = 0; q < num; q++)
            {
                PGM image_inp;
                //define motion vector and inatiate
               
                for (int ii = 0; ii < 16; ii++)
                {
                    vec1[ii] = 0;
                    vec2[ii] = 0;

                }
                for (int jj = 0; jj < 16; jj++)
                {
                    vec2[16 + jj] = 0;
                }

               

                i = q + 1;// startpoint



                if ((q + numStart) < 100)
                {
                    fileName = (fileName_Sor + "_00" + Convert.ToString(q + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                else
                {
                    fileName = (fileName_Sor + "_0" + Convert.ToString(q + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                image_inp = new PGM(fileName);
                // ShowImg("0", image_inp.mImg);
                int[, ] Imag_first = pic2edge(image_inp.mImg);  //first frame

                int w = Imag_first.GetLength(0);
                int h = Imag_first.GetLength(1);

               
                byte[,] forShow = new byte[w, h];

                if ((i + numStart) < 100)
                {
                    image_inp = new PGM(fileName_Sor + "_00" + Convert.ToString(i + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                else
                {
                    image_inp = new PGM(fileName_Sor + "_0" + Convert.ToString(i + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                imageInp = image_inp.mImg;


                int[,] Imag_second = pic2edge(imageInp);//second frame



                int sum_vec, sum_check1, sum_check2;
                int[,] Imag_Result = new int[w, h];
                for (int ii = 0; ii < w; ii++)
                    for (int jj = 0; jj < w; jj++)
                        Imag_Result[ii, jj] = 0;
                int[] result_Vec = new int[17];

                int movStep = 0;
                //  ShowImg("1", edge2pic(Imag_first));
                // ShowImg("2", edge2pic(Imag_second));
                // ShowImg("3", edge2pic(Imag_Result));zheme
                for (int pw = 16; pw < 239; pw++)
                    for (int ph = 16; ph < 239; ph++)
                    {
                        if (opt == "Ver")
                        {
                            sum_check1 = 0;
                            sum_check2 = 0;

                            // when there are only small amount of pixel, just omit
                            for (int ii = -8; ii < 8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check1 = sum_check1 + Imag_first[pw + ii, ph + jj];

                                }
                            }

                            
                          
                            if (sum_check1 > thre )
                            {

                                for (int ii = -8; ii < 8; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {

                                        sum_vec = sum_vec + Imag_first[pw + ii, ph + jj]; // picture 1

                                    }
                                    vec1[ii + 8] = sum_vec;
                                }
                                for (int ii = -16; ii < 16; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {
                                        sum_vec = sum_vec + Imag_second[pw + ii, ph + jj]; // picture 2

                                    }
                                    vec2[ii + 16] = sum_vec;

                                }
                                result_Vec = SAD(vec1, vec2);
                                movStep = findMov(result_Vec);
                            }
                            else
                                movStep = 0;
                        }

                        else if (opt == "Hori")
                        {
                            sum_check1 = 0;
                            sum_check2 = 0;
                            for (int ii = -8; ii < 8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check1 = sum_check1 + Imag_first[pw + ii, ph + jj];

                                }
                            }

                            for (int ii = -8; ii <8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check2 = sum_check2 + Imag_second[pw + ii, ph + jj];

                                }
                            }
 
 
                            if (sum_check1 > thre&&sum_check2 >thre)
                            {
                                for (int ii = -8; ii < 8; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {

                                        sum_vec = sum_vec + Imag_first[pw + jj, ph + ii]; // picture 1

                                    }
                                    vec1[ii + 8] = sum_vec;
                                }
                                for (int ii = -16; ii < 16; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {
                                        sum_vec = sum_vec + Imag_second[pw + jj, ph + ii]; // picture 2

                                    }
                                    vec2[ii + 16] = sum_vec;

                                }
                                result_Vec = SAD(vec1, vec2);
                                movStep = findMov(result_Vec);
                            }
                            else
                                movStep = 0;

                        }
                       
                        Imag_Result[pw, ph] = movStep;
                    }

                //test
                byte[,] Imag_Result1 = new byte[w,h];




                for (int pw = 0; pw < w; pw++)
                    for (int ph = 0; ph <h; ph++)
                    {
                       // if (Math.Abs(Imag_Result[pw, ph]) >= 0)
                            Imag_Result1[pw, ph] = Convert.ToByte((Imag_Result[pw, ph] + 8) * 256 / 17);
                     //   else
                     //       Imag_Result1[pw, ph] = 128;

                    }



                // ShowImg("test",Imag_Result1);








                byte[,] save_Image = Imag_Result1;
                if ((q + numStart) < 100)
                {
                    fileName_Des1 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_" + opt + ".pgm");
                }
                else
                {
                    fileName_Des1 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_" + opt + ".pgm");
                }

                PGM image4Save = image_inp;
                 
                 //if (Exp_show)
                if(false)
                 {
                     ShowImg(opt, save_Image);
                    
                 }
                image4Save.mImg = save_Image;
                image4Save.Save(fileName_Des1);
            } 
              
            }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName_Sor"></param>
        /// <param name="fileName_Des"></param>
        /// <param name="numStart"></param>
        /// <param name="num"></param>
        /// <param name="thre"></param>
        /// <param name="If_Max"></param>
        /// <param name="showAll"></param>

        static void Generate_Motions4b(string fileName_Sor, string fileName_Des, int numStart, int num,int thre, bool If_Max, bool showAll)
        {
            byte[,] imageInp;

            
            string fileName;
            string fileName_Des1V;
            string fileName_Des2V;
            int i = 0;
            int[] vec1 = new int[16];
            int[] vec2 = new int[32];
            for (int q = 0; q < num; q++)
            {
                PGM image_inp;
           
                //define motion vector and inatiate

               

              

                i = q + 1;                                 // startpoint



                if ((q + numStart) < 100)
                {
                    fileName = (fileName_Sor + "_00" + Convert.ToString(q + numStart) + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                else
                {
                    fileName = (fileName_Sor + "_0" + Convert.ToString(q + numStart) + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                image_inp = new PGM(fileName);
                // ShowImg("0", image_inp.mImg);

               int[,] Imag_first = pic2edge(image_inp.mImg);  //first frame

                int w = Imag_first.GetLength(0);  // the width of the picture
                int h = Imag_first.GetLength(1);  // heights of the picture
                byte[,] forShow = new byte[w, h];



                if ((i + numStart) < 100)
                {
                    image_inp = new PGM(fileName_Sor + "_00" + Convert.ToString(i + numStart) + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                else
                {
                    image_inp = new PGM(fileName_Sor + "_0" + Convert.ToString(i + numStart) + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                imageInp = image_inp.mImg;


                int[,] Imag_second = pic2edge(imageInp);//second frame
               
                int[,] Imag_Result11 = new int[w, h];
                int[,] Imag_Result21 = new int[w, h];

                int sum_vec, sum_check1, sum_check2;
                int[,] Imag_ResultPV = new int[w, h];
                int[,] Imag_ResultNV = new int[w, h];
                int[,] Imag_ResultPH = new int[w, h];
                int[,] Imag_ResultNH = new int[w, h];

                for (int ii = 0; ii < w; ii++)
                    for (int jj = 0; jj < w; jj++)
                    {
                        Imag_ResultPV[ii, jj] = 0;
                        Imag_ResultNV[ii, jj] = 0;
                        Imag_ResultPH[ii, jj] = 0;
                        Imag_ResultNH[ii, jj] = 0;

                    }
                int[] result_VecH = new int[17];
                int[] result_VecV = new int[17];

                int movStepH = 0;
                int movStepV = 0;
                //  ShowImg("1", edge2pic(Imag_first));
                // ShowImg("2", edge2pic(Imag_second));
                // ShowImg("3", edge2pic(Imag_Result));

                for (int ii = 0; ii < 16; ii++)
                {
                    vec1[ii] = 0;
                    vec2[ii] = 0;

                }
                for (int jj = 0; jj < 16; jj++)
                {
                    vec2[16 + jj] = 0;
                }


                for (int pw = 16; pw < 239; pw++)
                    for (int ph = 16; ph < 239; ph++)
                    {
                        //if (opt == "Ver")
                        {
                           
                            sum_check1 = 0;
                            sum_check2 = 0;
                            for (int ii = -8; ii < 8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check1 = sum_check1 + Imag_first[pw + ii, ph + jj];

                                }
                            }
                            for (int ii = -8; ii < 8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check2 = sum_check2 + Imag_second[pw + ii, ph + jj];

                                }
                            }
                            if (sum_check1 > thre && sum_check2 > (thre))   // if edges in a window is larger than 
                            {

                                for (int ii = -8; ii < 8; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {

                                        sum_vec = sum_vec + Imag_first[pw + ii, ph + jj]; // picture 1

                                    }
                                    vec1[ii + 8] = sum_vec;
                                }
                                for (int ii = -16; ii < 16; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {
                                        sum_vec = sum_vec + Imag_second[pw + ii, ph + jj]; // picture 2

                                    }
                                    vec2[ii + 16] = sum_vec;

                                }
                                result_VecV = SAD(vec1, vec2);
                                movStepV = findMov(result_VecV);
                            }
                            else
                                movStepV = 0;
                            if (movStepV >= 0)
                            {

                                Imag_ResultPV[pw, ph] = (movStepV);
                            }
                            else
                            {
                                Imag_ResultNV[pw, ph] = (Math.Abs(movStepV));
                            }

                            sum_check1 = 0;
                            sum_check2 = 0;
                            for (int ii = -8; ii < 8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check1 = sum_check1 + Imag_first[pw + ii, ph + jj];

                                }
                            }

                            for (int ii = -8; ii < 8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check2 = sum_check2 + Imag_second[pw + ii, ph + jj];

                                }
                            }
                            if (sum_check1 > thre && sum_check2 > (thre))
                            {
                                for (int ii = -8; ii < 8; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {

                                        sum_vec = sum_vec + Imag_first[pw + jj, ph + ii]; // picture 1

                                    }
                                    vec1[ii + 8] = sum_vec;
                                }
                                for (int ii = -16; ii < 16; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {
                                        sum_vec = sum_vec + Imag_second[pw + jj, ph + ii]; // picture 2

                                    }
                                    vec2[ii + 16] = sum_vec;

                                }
                                result_VecH = SAD(vec1, vec2);
                                movStepH = findMov(result_VecH);
                            }
                            else
                                movStepH = 0;

                        }
                        if (movStepH >= 0)
                        {

                            Imag_ResultPH[pw, ph] = (movStepH);
                        }
                        else
                        {
                            Imag_ResultNH[pw, ph] = (Math.Abs(movStepH));
                        }
                    }

                int[,] Imag_ResultPPV = new int[w, h];// save results after max
                int[,] Imag_ResultNNV = new int[w, h];
                int[,] Imag_ResultPPH = new int[w, h];
                int[,] Imag_ResultNNH = new int[w, h];
                if (If_Max)
                {
                    for (int i_sov = 0; i_sov < w; i_sov++)
                        for (int j_sov = 0; j_sov < h; j_sov++)
                        {
                            Imag_ResultPPV[i_sov, j_sov] = 0;
                            Imag_ResultNNV[i_sov, j_sov] = 0;
                            Imag_ResultPPH[i_sov, j_sov] = 0;
                            Imag_ResultNNH[i_sov, j_sov] = 0;
                        }
                    for (int i_sov = 3; i_sov < w; i_sov = i_sov + 4)
                        for (int j_sov = 3; j_sov < h; j_sov = j_sov + 4)
                        {
                            int max_pV = 0;
                            int max_nV = 0;
                            int sum_test_pV = 0;
                            int sum_test_nV = 0;
                            int max_pH = 0;
                            int max_nH = 0;
                            int sum_test_pH = 0;
                            int sum_test_nH = 0;



                            for (int ii = -3; ii < 5; ii++)
                                for (int jj = -3; jj < 5; jj++)
                                {

                                    max_pV = Math.Max(max_pV, Imag_ResultPV[i_sov + ii, j_sov + jj]);

                                    max_nV = Math.Max(max_nV, Imag_ResultNV[i_sov + ii, j_sov + jj]);

                                    max_pH = Math.Max(max_pH, Imag_ResultPH[i_sov + ii, j_sov + jj]);

                                    max_nH = Math.Max(max_nH, Imag_ResultNH[i_sov + ii, j_sov + jj]);
                                }
                            for (int ii = -3; ii < 5; ii++)
                                for (int jj = -3; jj < 5; jj++)
                                {
                                    //  if (sum_test_h > 32)

                                    Imag_ResultPPV[i_sov + ii, j_sov + jj] = max_pV;
                                    //if (sum_test_v > 32)
                                    Imag_ResultNNV[i_sov + ii, j_sov + jj] = max_nV;

                                    Imag_ResultPPH[i_sov + ii, j_sov + jj] = max_pH;
                                    //if (sum_test_v > 32)
                                    Imag_ResultNNH[i_sov + ii, j_sov + jj] = max_nH;
                                }

                        }

                }
                else
                {
                    Imag_ResultNNV = Imag_ResultNV;
                    Imag_ResultPPV = Imag_ResultPV;
                    Imag_ResultNNH = Imag_ResultNH;
                    Imag_ResultPPH = Imag_ResultPH;



                }



                byte[,] Imag_Result1V = new byte[w,h];
                byte[,] Imag_Result2V = new byte[w,h];
                byte[,] Imag_ResultallV = new byte[w,h];
                byte[,] Imag_Result1H = new byte[w,h];
                byte[,] Imag_Result2H = new byte[w,h];
                byte[,] Imag_ResultallH = new byte[w,h];
                for (int pw = 0; pw < w; pw++)
                    for (int ph = 0; ph < h; ph++)
                    {
                        // if (Math.Abs(Imag_Result[pw, ph]) >= 0)


                        if (showAll)
                        {
                            if (Imag_ResultNNV[pw, ph] > Imag_ResultPPV[pw, ph])

                                Imag_ResultallV[pw, ph] = Convert.ToByte(-Imag_ResultNNV[pw, ph] * 255 / 17 + 128);

                            else if (Imag_ResultNNH[pw, ph] > Imag_ResultPPH[pw, ph])
                                Imag_ResultallH[pw, ph] = Convert.ToByte(-Imag_ResultNNH[pw, ph] * 255 / 17 + 128);

                            else if (Imag_ResultNNV[pw, ph] <= Imag_ResultPPV[pw, ph])
                                Imag_ResultallV[pw, ph] = Convert.ToByte(Imag_ResultPPV[pw, ph] * 255 / 17 + 128);

                            else

                                Imag_ResultallH[pw, ph] = Convert.ToByte(Imag_ResultPPH[pw, ph] * 255 / 17 + 128);

                        }
                        else
                        {

                            Imag_Result1V[pw, ph] = Convert.ToByte((Imag_ResultNNV[pw, ph]) * 255 / 17 + 128);
                            Imag_Result2V[pw, ph] = Convert.ToByte((Imag_ResultPPV[pw, ph]) * 255 / 17 + 128);
                            Imag_Result1H[pw, ph] = Convert.ToByte((Imag_ResultNNH[pw, ph]) * 255 / 17 + 128);
                            Imag_Result2H[pw, ph] = Convert.ToByte((Imag_ResultPPH[pw, ph]) * 255 / 17 + 128);

                        }

                      }          
            
 // ShowImg("test",Imag_Result1);






                        string fileName_Des1H;
                        string fileName_Des2H;

                        //byte[,] save_Image1 = Imag_Result1;

                        if (!showAll)
                        {

                            if ((q + numStart) < 100)
                            {
                                fileName_Des1V = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_left" + ".pgm");
                                fileName_Des2V = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_right" + ".pgm");
                            }
                            else
                            {
                                fileName_Des1V = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_left" + ".pgm");
                                fileName_Des2V = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_right" + ".pgm");
                            }

                            if ((q + numStart) < 100)
                            {
                                fileName_Des1H = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_up" + ".pgm");
                                fileName_Des2H = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_down" + ".pgm");
                            }
                            else
                            {
                                fileName_Des1H = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_up" + ".pgm");
                                fileName_Des2H = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_down" + ".pgm");
                            }





                            PGM image4SavepV = image_inp;
                            PGM image4SavenV = image_inp;
                            PGM image4SavepH = image_inp;
                            PGM image4SavenH = image_inp;


                            // Exp_show show the intermediate level image for input

                            image4SavepV.mImg = Imag_Result1V;
                            image4SavepV.Save(fileName_Des1V);
                            image4SavenV.mImg = Imag_Result2V;
                            image4SavenV.Save(fileName_Des2V);
                            image4SavepH.mImg = Imag_Result1H;
                            image4SavepH.Save(fileName_Des1H);
                            image4SavenH.mImg = Imag_Result2H;
                            image4SavenH.Save(fileName_Des2H);
                        }
                        else
                        {



                            if ((q + numStart) < 100)
                            {
                                fileName_Des1V = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_ver" + ".pgm");

                            }
                            else
                            {
                                fileName_Des1V = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_ver" + ".pgm");

                            }






                            {
                                if ((q + numStart) < 100)
                                {
                                    fileName_Des1H = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_hori" + ".pgm");

                                }
                                else
                                {
                                    fileName_Des1H = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_hori" + ".pgm");

                                }

                            }



                            PGM image4SaveallV = image_inp;
                            PGM image4SaveallH = image_inp;



                            // Exp_show show the intermediate level image for input

                            image4SaveallV.mImg = Imag_ResultallV;
                            image4SaveallV.Save(fileName_Des1V);
                            image4SaveallH.mImg = Imag_ResultallH;
                            image4SaveallH.Save(fileName_Des1H);


                        }
                    }
            }
        
        



      /// <summary>
      /// generate motion field pictures and doing max sampling and save
      /// </summary>
      /// <param name="fileName_Sor"></param>
      /// <param name="fileName_Des"></param>
      /// <param name="numStart"></param>
      /// <param name="num"></param>
      /// <param name="opt"></param>
      /// <param name="thre"></param>
      /// <param name="If_Max"></param>

        static void Generate_Motions(string fileName_Sor, string fileName_Des, int numStart, int num, string opt, int thre, bool If_Max, bool showAll,int max_win)
        {

            byte[,] imageInp;

           // int[,] Imag_first = new int[256, 256];
            string fileName, fileName_Des1;
            int i = 0;
            int[] vec1 = new int[16];
            int[] vec2 = new int[32];
            for (int q = 0; q < num; q++)
            {
                PGM image_inp;
                //define motion vector and inatiate

                for (int ii = 0; ii < 16; ii++)
                {
                    vec1[ii] = 0;
                    vec2[ii] = 0;

                }
                for (int jj = 0; jj < 16; jj++)
                {
                    vec2[16 + jj] = 0;
                }


              

                i = q + 1;// startpoint



                if ((q + numStart) < 100)
                {
                    fileName = (fileName_Sor + "_00" + Convert.ToString(q + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                else
                {
                    fileName = (fileName_Sor + "_0" + Convert.ToString(q + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                image_inp = new PGM(fileName);
                // ShowImg("0", image_inp.mImg);
                int[,] Imag_first = pic2edge(image_inp.mImg);  //first frame

                int w = Imag_first.GetLength(0);
                int h = Imag_first.GetLength(1);

                byte[,] forShow = new byte[w,h];

                if ((i + numStart) < 100)
                {
                    image_inp = new PGM(fileName_Sor + "_00" + Convert.ToString(i + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                else
                {
                    image_inp = new PGM(fileName_Sor + "_0" + Convert.ToString(i + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return;
                    }
                }
                imageInp = image_inp.mImg;


                int[,] Imag_second = pic2edge(imageInp);//second frame
                //int windowsize = ;
                int[,] Imag_Result11 = new int[w,h];
                int[,] Imag_Result21 = new int[w,h];

                int sum_vec, sum_check1, sum_check2;
                int[,] Imag_ResultP = new int[w, h];
                int[,] Imag_ResultN = new int[w, h];

                for (int ii = 0; ii < w; ii++)
                    for (int jj = 0; jj < w; jj++)
                    {
                        Imag_ResultP[ii, jj] = 0;
                        Imag_ResultN[ii, jj] = 0;
                    
                    }
                int[] result_Vec = new int[17];

                int movStep = 0;
              
               
                  if (opt == "Ver")
                  {

                        for (int pw = 16; pw < (w-17); pw++)
                    for (int ph = 16; ph < (h-17); ph++)
                    {
                      
                        
                            sum_check1 = 0;
                            sum_check2 = 0;
                            for (int ii = -8; ii < 8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check1 = sum_check1 + Imag_first[pw + jj, ph + ii];

                                }
                            }
                            for (int ii = -16; ii < 16; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check2 = sum_check2 + Imag_second[pw + ii, ph + jj];

                                }
                            }
                            if (sum_check1 > thre && sum_check2 > (thre))
                            {

                                for (int ii = -8; ii < 8; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {

                                        sum_vec = sum_vec + Imag_first[pw + jj, ph + ii]; // picture 1

                                    }
                                    vec1[ii + 8] = sum_vec;
                                }
                                for (int ii = -16; ii < 16; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {
                                        sum_vec = sum_vec + Imag_second[pw + jj, ph + ii]; // picture 2

                                    }
                                    vec2[ii + 16] = sum_vec;

                                }
                                result_Vec = SAD(vec1, vec2);
                                movStep = findMov(result_Vec);
                            }
                            else
                                movStep = 0;
                            if (movStep >= 0)
                            {
                                Imag_ResultP[pw, ph] = (movStep);
                                // Imag_ResultP[pw, ph] = 7;
                            }
                            else
                            {
                                Imag_ResultN[pw, ph] = (Math.Abs(movStep));
                            }

                        }
                  }
                          else if (opt == "Hori")
                        {
                          //  int ph = 16;
                            for (int pw = 16; pw < (w-17); pw++)
                              for (int ph = 16; ph < (h-17); ph++)
                            
                         {
                            sum_check1 = 0;
                            sum_check2 = 0; 
                            for (int ii = -8; ii < 8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check1 = sum_check1 + Imag_first[pw + ii, ph + jj];

                                }
                            }

                            for (int ii = -16; ii < 16; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                               
                                {
                                    sum_check2 = sum_check2 + Imag_second[pw + ii, ph + jj];

                                }
                            }
                            if (sum_check1 > thre && sum_check2 > (thre))
                            {
                                for (int ii = -8; ii < 8; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    { 

                                        sum_vec = sum_vec + Imag_first[pw + ii, ph + jj]; // picture 1

                                    }
                                    vec1[ii + 8] = sum_vec;
                                }
                                for (int ii = -16; ii < 16; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {
                                        sum_vec = sum_vec + Imag_second[pw + ii, ph + jj]; // picture 2

                                    }
                                    vec2[ii + 16] = sum_vec;

                                }
                                result_Vec = SAD(vec1, vec2);
                                movStep = findMov(result_Vec);
                            }
                            else
                                movStep = 0;
                          if (movStep >= 0)
                        {
                            Imag_ResultP[pw, ph] = (movStep);
                           // Imag_ResultP[pw, ph] = 7;
                        }
                        else {
                            Imag_ResultN[pw, ph] = (Math.Abs(movStep));
                        }

                        }

                        }

                      

                int[,] Imag_ResultPP = new int[w, h];
                int[,] Imag_ResultNN = new int[w, h];
                if (If_Max)
                {
                    for (int i_sov = 0; i_sov < w; i_sov++)
                        for (int j_sov = 0; j_sov < h; j_sov++)
                        {
                            Imag_ResultPP[i_sov, j_sov] = 0;
                            Imag_ResultNN[i_sov, j_sov] = 0;
                        }
                    for (int i_sov = (max_win / 2 - 1); i_sov < (w-(max_win / 2 )); i_sov = i_sov + (max_win / 2 ))//(-3,5,4)
                        for (int j_sov = (max_win / 2 - 1); j_sov < (h - (max_win / 2)); j_sov = j_sov + (max_win / 2))
                        {
                            int max_p= 0;
                            int max_n = 0;
                            int sum_test_p = 0;
                            int sum_test_n = 0;

                            /*
                            for (int ii = -3; ii < 5; ii++)
                                for (int jj = -3; jj < 5; jj++)
                                {
                                    if (Imag_ResultP[i_sov + ii, j_sov + jj] != 0)

                                        sum_test_p++;
                                    if (Imag_ResultN[i_sov + ii, j_sov + jj] != 0)
                                        sum_test_n++;

                                }
                             * 
                             */

                            for (int ii = -(max_win / 2 - 1); ii < (max_win / 2 + 1); ii++)
                                for (int jj = -(max_win / 2 - 1); jj < (max_win / 2 + 1); jj++)
                                {

                                    max_p = Math.Max(max_p, Imag_ResultP[i_sov + ii, j_sov + jj]);

                                    max_n = Math.Max(max_n, Imag_ResultN[i_sov + ii, j_sov + jj]);
                                }
                            for (int ii = -(max_win / 2 - 1); ii < (max_win / 2 + 1); ii++)
                                for (int jj = -(max_win / 2 - 1); jj < (max_win / 2 + 1); jj++)
                                {
                                    //  if (sum_test_h > 32)

                                    Imag_ResultPP[i_sov + ii, j_sov + jj] = max_p;
                                    //if (sum_test_v > 32)
                                    Imag_ResultNN[i_sov + ii, j_sov + jj] = max_n;
                                }
                          

                        }

                }
                else
                {
                    Imag_ResultNN = Imag_ResultN;
                    Imag_ResultPP = Imag_ResultP;
                 

                
                }



                byte[,] Imag_Result1 = new byte[w,h];
                byte[,] Imag_Result2 = new byte[w,h];
                byte[,] Imag_Resultall = new byte[w,h];
                for (int pw = 0; pw < w; pw++)
                    for (int ph = 0; ph < h; ph++)
                    {
                        // if (Math.Abs(Imag_Result[pw, ph]) >= 0)
                       
                       
                      if (showAll)
                      {
                          if (Imag_ResultNN[pw, ph] >= Imag_ResultPP[pw, ph])
                          {
                              Imag_Resultall[pw, ph] = Convert.ToByte(-Imag_ResultNN[pw, ph] * 255 / 17 + 128);

                          }
                          else
                          {
                              Imag_Resultall[pw, ph] = Convert.ToByte(Imag_ResultPP[pw, ph] * 255 / 17 + 128);
                          }
                      }
                      else{
                       Imag_Result1[pw, ph] = Convert.ToByte((Imag_ResultNN[pw, ph]) * 255 / 17 + 128);
                      Imag_Result2[pw, ph] = Convert.ToByte((Imag_ResultPP[pw, ph]) * 255 / 17 + 128);
                      }
                        

                    }



                /*******************************************************************************************
                //this part designed for code testing
             
                for (int ii = 0; ii < 16; ii++)
                {
                    sum_check1 = 0;
                    for (int jj =0; jj <32; jj++)
                    {

                        Imag_Resultall[ jj, ii] = 1; // picture
                        sum_check1 = sum_check1 + Imag_Resultall[jj, ii];
                    }
                             

            }




                /*********************************************************************************************/







                string fileName_Des2;

                byte[,] save_Image1 = Imag_Result1;
                if (!showAll)
                {
                    if (opt == "Ver")
                    {

                        if ((q + numStart) < 100)
                        {
                            fileName_Des1 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_left" + ".pgm");
                            fileName_Des2 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_right" + ".pgm");
                        }
                        else
                        {
                            fileName_Des1 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_left" + ".pgm");
                            fileName_Des2 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_right" + ".pgm");
                        }




                    }
                    else
                    {
                        if ((q + numStart) < 100)
                        {
                            fileName_Des1 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_up" + ".pgm");
                            fileName_Des2 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_down" + ".pgm");
                        }
                        else
                        {
                            fileName_Des1 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_up" + ".pgm");
                            fileName_Des2 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_down" + ".pgm");
                        }

                    }
              


                PGM image4Savep = image_inp;
                PGM image4Saven = image_inp;


                // Exp_show show the intermediate level image for input
            
                image4Savep.mImg = Imag_Result1;
                image4Savep.Save(fileName_Des1);
                image4Saven.mImg = Imag_Result2;
                image4Saven.Save(fileName_Des2);
            }
            else {
                if (opt == "Ver")
                {

                    if ((q + numStart) < 100)
                    {
                        fileName_Des1 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_ver" + ".pgm");
                       
                    }
                    else
                    {
                        fileName_Des1 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_ver" + ".pgm");
                      
                    }




                }
                else
                {
                    if ((q + numStart) < 100)
                    {
                        fileName_Des1 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_hori" + ".pgm");
                        
                    }
                    else
                    {
                        fileName_Des1 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_hori" + ".pgm");
                        
                    }

                }



                PGM image4Saveall = image_inp;
            


                // Exp_show show the intermediate level image for input

                image4Saveall.mImg = Imag_Resultall;
                image4Saveall.Save(fileName_Des1);
               
                
                }
            }

        }

        public class motionDir
        {
           public int Positive;
           public int Negtive;
            public motionDir(int p, int n)
            {
                Positive = p;
                Negtive = n;
            }
            public motionDir()
            {
                Positive = 0;
                Negtive = 0;

            }


        }

     /// <summary>
     /// 
     /// </summary>
     /// <param name="fileName_Sor"></param>
     /// <param name="fileName_Des"></param>
     /// <param name="numStart"></param>
     /// <param name="num"></param>
     /// <param name="opt"></param>
     /// <param name="thre"></param>
     /// <param name="If_Max"></param>
     /// <returns></returns>
   
        static motionfield[][,] Generate_MotionArray(string fileName_Sor, string fileName_Des, int numStart, int num, string opt, int thre, bool If_Max, int maxWin)
        {

            
            motionfield [][,] motionArray= new motionfield[maxFrame][,];
            byte[,] imageInp;
           
            string fileName, fileName_Des1;
            int i = 0;
            int[] vec1 = new int[16];
            int[] vec2 = new int[32];
            for (int q = 0; q < num; q++)
            {
               
                PGM image_inp;
                //define motion vector and inatiate

                for (int ii = 0; ii < 16; ii++)
                {
                    vec1[ii] = 0;
                    vec2[ii] = 0;

                }
                for (int jj = 0; jj < 16; jj++)
                {
                    vec2[16 + jj] = 0;
                }


                
                i = q + 1;// startpoint



                if ((q + numStart) < 100)
                {
                    fileName = (fileName_Sor + "_00" + Convert.ToString(q + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return motionArray ;
                    }
                }
                else
                {
                    fileName = (fileName_Sor + "_0" + Convert.ToString(q + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return motionArray;
                    }
                }
                image_inp = new PGM(fileName);
                int w = image_inp.mImg.GetLength(0);
                int h = image_inp.mImg.GetLength(1);
                int[,] Imag_first = new int[w,h];  //first image to caculate motion
                byte[,] forShow = new byte[w, h];

                // ShowImg("0", image_inp.mImg);
                Imag_first = pic2edge(image_inp.mImg);  //first frame

              
                motionArray[q] = new motionfield[w*2/maxWin, h*2/maxWin];



                if ((i + numStart) < 100)
                {
                    image_inp = new PGM(fileName_Sor + "_00" + Convert.ToString(i + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return motionArray;
                    }
                }
                else
                {
                    image_inp = new PGM(fileName_Sor + "_0" + Convert.ToString(i + numStart) + "_" + opt + ".pgm");
                    if (stopMethod)
                    {
                        stopMethod = false;
                        return motionArray;
                    }
                }
                imageInp = image_inp.mImg;


                int[,] Imag_second = pic2edge(imageInp);//second frame



                int sum_vec, sum_check1, sum_check2;
                int[,] Imag_ResultP = new int[w, h];
                int[,] Imag_ResultN = new int[w, h];

                for (int ii = 0; ii < w; ii++)
                    for (int jj = 0; jj < w; jj++) // problem need to be fixed!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {
                        Imag_ResultP[ii, jj] = 0;
                        Imag_ResultN[ii, jj] = 0;

                    }
                int[] result_Vec = new int[17];

                int movStep = 0;

                if (opt == "Ver")
                {

                    for (int pw = 16; pw < (w - 17); pw++)
                        for (int ph = 16; ph < (h - 17); ph++)
                        {


                            sum_check1 = 0;
                            sum_check2 = 0;
                            for (int ii = -8; ii < 8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check1 = sum_check1 + Imag_first[pw + jj, ph + ii];

                                }
                            }
                            for (int ii = -16; ii < 16; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check2 = sum_check2 + Imag_second[pw + ii, ph + jj];

                                }
                            }
                            if (sum_check1 > thre && sum_check2 > (thre))
                            {

                                for (int ii = -8; ii < 8; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {

                                        sum_vec = sum_vec + Imag_first[pw + jj, ph + ii]; // picture 1

                                    }
                                    vec1[ii + 8] = sum_vec;
                                }
                                for (int ii = -16; ii < 16; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {
                                        sum_vec = sum_vec + Imag_second[pw + jj, ph + ii]; // picture 2

                                    }
                                    vec2[ii + 16] = sum_vec;

                                }
                                result_Vec = SAD(vec1, vec2);
                                movStep = findMov(result_Vec);
                            }
                            else
                                movStep = 0;
                            if (movStep >= 0)
                            {
                                Imag_ResultP[pw, ph] = (movStep);
                                // Imag_ResultP[pw, ph] = 7;
                            }
                            else
                            {
                                Imag_ResultN[pw, ph] = (Math.Abs(movStep));
                            }

                        }
                }
                else if (opt == "Hori")
                {
                    //  int ph = 16;
                    for (int pw = 16; pw < (w - 17); pw++)
                        for (int ph = 16; ph < (h - 17); ph++)
                        {
                            sum_check1 = 0;
                            sum_check2 = 0;
                            for (int ii = -8; ii < 8; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check1 = sum_check1 + Imag_first[pw + ii, ph + jj];

                                }
                            }

                            for (int ii = -16; ii < 16; ii++)
                            {
                                for (int jj = -16; jj < 16; jj++)
                                {
                                    sum_check2 = sum_check2 + Imag_second[pw + ii, ph + jj];

                                }
                            }
                            if (sum_check1 > thre && sum_check2 > (thre))
                            {
                                for (int ii = -8; ii < 8; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {

                                        sum_vec = sum_vec + Imag_first[pw + ii, ph + jj]; // picture 1

                                    }
                                    vec1[ii + 8] = sum_vec;
                                }
                                for (int ii = -16; ii < 16; ii++)
                                {
                                    sum_vec = 0;
                                    for (int jj = -16; jj < 16; jj++)
                                    {
                                        sum_vec = sum_vec + Imag_second[pw + ii, ph + jj]; // picture 2

                                    }
                                    vec2[ii + 16] = sum_vec;

                                }
                                result_Vec = SAD(vec1, vec2);
                                movStep = findMov(result_Vec);
                            }
                            else
                                movStep = 0;
                            if (movStep >= 0)
                            {
                                Imag_ResultP[pw, ph] = (movStep);
                                // Imag_ResultP[pw, ph] = 7;
                            }
                            else
                            {
                                Imag_ResultN[pw, ph] = (Math.Abs(movStep));
                            }

                        }

                }

              
                //int[,] Imag_ResultPP = new int[w, h];
                //int[,] Imag_ResultNN = new int[w, h];
                if (If_Max)
                {
                    /*
                    for (int i_sov = 0; i_sov < 256; i_sov++)
                        for (int j_sov = 0; j_sov < 256; j_sov++)
                        {
                            Imag_ResultPP[i_sov, j_sov] = 0;
                            Imag_ResultNN[i_sov, j_sov] = 0;
                        }
                     * */
                    for (int i_sov1 = 0; i_sov1 < 256*2/maxWin; i_sov1++)
                        for (int j_sov1 = 0; j_sov1 < 256*2/maxWin; j_sov1++)

                                 motionArray[q][i_sov1, j_sov1] = new motionfield();

                    for (int i_sov1 = 0; i_sov1 < 256-maxWin; i_sov1 = i_sov1 + maxWin/2)
                        for (int j_sov1 = 0; j_sov1 < 256-maxWin; j_sov1 = j_sov1 + maxWin/2)
                        {
                            int max_p = 0;
                            int max_n = 0;
                            int sum_test_p = 0;
                            int sum_test_n = 0;

                            /*
                            for (int ii = -3; ii < 5; ii++)
                                for (int jj = -3; jj < 5; jj++)
                                {
                                    if (Imag_ResultP[i_sov + ii, j_sov + jj] != 0)

                                        sum_test_p++;
                                    if (Imag_ResultN[i_sov + ii, j_sov + jj] != 0)
                                        sum_test_n++;

                                }
                             * 
                             */

                            for (int ii = 0; ii < maxWin; ii++)
                                for (int jj = 0; jj < maxWin; jj++)
                                {

                                    max_p = Math.Max(max_p, Imag_ResultP[i_sov1 + ii, j_sov1 + jj]);

                                    max_n = Math.Max(max_n, Imag_ResultN[i_sov1 + ii, j_sov1 + jj]);
                                }


                           
                            motionArray[q][i_sov1 * 2 / maxWin, j_sov1 * 2 / maxWin].pos = max_p;


                            motionArray[q][i_sov1 * 2 / maxWin, j_sov1 * 2 / maxWin].neg = max_n;



                        }
                           

                        

                }
                else
                {
                    //Imag_ResultNN = Imag_ResultN;
                    //Imag_ResultPP = Imag_ResultP;
                    //Later to write the code for comparison


                }
                /*
                  for (int i_sov = 0; i_sov < 256; i_sov++)
                      for (int j_sov = 0; j_sov < 256; j_sov++)
                  {
                      if (Imag_ResultNN[i_sov, j_sov] >= Imag_ResultPP[i_sov,j_sov])
                          motionArray[q][i_sov, j_sov] = -Imag_ResultNN[i_sov, j_sov];
                      else
                          motionArray[q][i_sov, j_sov] = Imag_ResultPP[i_sov, j_sov];



                  }
                 */



                // to generate PGE files
                /*     byte[,] Imag_Result1 = new byte[256, 256];
                     byte[,] Imag_Result2 = new byte[256, 256];


                     for (int pw = 0; pw < 256; pw++)
                         for (int ph = 0; ph < 256; ph++)
                         {
                             // if (Math.Abs(Imag_Result[pw, ph]) >= 0)
                             Imag_Result1[pw, ph] = Convert.ToByte((Imag_ResultNN[pw, ph]) * 255 / 17 + 128);
                             Imag_Result2[pw, ph] = Convert.ToByte((Imag_ResultPP[pw, ph]) * 255 / 17 + 128);


                         }



                     // ShowImg("test",Imag_Result1);






                     string fileName_Des2;

                     byte[,] save_Image1 = Imag_Result1;
                     if (opt == "Ver")
                     {
                         if ((q + numStart) < 100)
                         {
                             fileName_Des1 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_left" + ".pgm");
                             fileName_Des2 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_right" + ".pgm");
                         }
                         else
                         {
                             fileName_Des1 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_left" + ".pgm");
                             fileName_Des2 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_right" + ".pgm");
                         }




                     }
                     else
                     {
                         if ((q + numStart) < 100)
                         {
                             fileName_Des1 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_up" + ".pgm");
                             fileName_Des2 = (fileName_Des + "_00" + Convert.ToString(q + numStart) + "_down" + ".pgm");
                         }
                         else
                         {
                             fileName_Des1 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_up" + ".pgm");
                             fileName_Des2 = (fileName_Des + "_0" + Convert.ToString(q + numStart) + "_down" + ".pgm");
                         }


                     }


                     PGM image4Savep = image_inp;
                     PGM image4Saven = image_inp;x


                     // Exp_show show the intermediate level image for input

                     image4Savep.mImg = Imag_Result1;
                     image4Savep.Save(fileName_Des1);
                     image4Saven.mImg = Imag_Result2;
                     image4Saven.Save(fileName_Des2);*/
            }
            return motionArray;

        }

        static void save2DArray(int[][,] array, string filename, int size)
        {
            int length = array.GetLength(0);
            using (StreamWriter fout = new StreamWriter(filename))
            {

                for (int i_sov = 0; i_sov < length; i_sov++)
                {

                    for (int j_sov = 0; j_sov < size; j_sov++)
                        for (int q_sov = 0; q_sov < size; q_sov++)
                        {
                            try
                            {
                                fout.WriteLine(j_sov + "," + q_sov + "," + array[i_sov][j_sov, q_sov]);
                            }
                            catch (NullReferenceException ex)
                            {
                                return;
                            }
                        }
                }
            }
        }
        static void saveArray(int[] array, string filename, int startnum)
        {
            int length = array.GetLength(0);
            int size;

            using (StreamWriter fout = new StreamWriter(filename))
            {

                for (int i_sov = 0; i_sov < length; i_sov++)
                {
                    //  size = array[i_sov].GetLength(0);
                    // for (int j_sov = 0; j_sov < size; j_sov++)
                    //for (int q_sov = 0; q_sov < size; q_sov++)
                    {
                        try
                        {
                            fout.WriteLine(startnum+i_sov + "," + array[i_sov]);
                        }
                        catch (NullReferenceException ex)
                        {
                            return;
                        }
                    }

                }
            }



        }

          static void saveArray(double [] array, string filename)
        {
            int length = array.GetLength(0);
            int size;

            using (StreamWriter fout = new StreamWriter(filename))
            {

                for (int i_sov = 0; i_sov < length; i_sov++)
                {
                    //  size = array[i_sov].GetLength(0);
                    // for (int j_sov = 0; j_sov < size; j_sov++)
                    //for (int q_sov = 0; q_sov < size; q_sov++)
                    {
                        try
                        {
                            fout.WriteLine(i_sov + "," + array[i_sov]);
                        }
                        catch (NullReferenceException ex)
                        {
                            return;
                        }
                    }

                }
            }



        }

        static void saveArray(int[][] array1, string filename)
        {
            
            int size;
            int[][] array = findNonzero(array1);
            int length = array.GetLength(0);

            using (StreamWriter fout = new StreamWriter(filename))
            {

                for (int i_sov = 0; i_sov < length; i_sov++)
                {
                    

                    size = array[i_sov].GetLength(0);
                    for (int j_sov = 0; j_sov < size; j_sov++)
                    //for (int q_sov = 0; q_sov < size; q_sov++)
                    {
                        try
                        {
                            fout.WriteLine(i_sov + "," + array[i_sov][j_sov]);
                        }
                        catch (NullReferenceException ex)
                        {
                            return;
                        }
                    }
                }
            }



        }
        
        /// <summary>
        /// Generate output for the SVM struct files
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="filename"></param>

        static void TrainTestFiles(int[][] array1, string filename)
        {

            int size;
            int[][] array = findNonzero(array1);
            int length = array.GetLength(0);
         //   float[][] saveArray = new float[length][];

            using (StreamWriter fout = new StreamWriter(filename))
            {

                for (int i_sov = 0; i_sov < length; i_sov++)
                {
                    /////////////////////////////////////////////
                    size = array[i_sov].GetLength(0);
                    fout.Write(array[i_sov][1]+1 + " ");
                    for (int j_sov = 2; j_sov < size; j_sov++)
                    //for (int q_sov = 0; q_sov < size; q_sov++)
                    {
                        try
                        {
                            fout.Write(j_sov + ":" + Convert.ToDouble(array[i_sov][j_sov])/256.0d + " ");
                        }
                        catch (NullReferenceException ex)
                        {
                            return;
                        }
                    }
                    fout.Write("\n");
                    ///////////////////////////////////////////////////

                }
            }



        }

        /// <summary>
        /// save double array
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="filename"></param>
        static void saveArray(double[][] array1, string filename)
        {

            int size;
           // int[][] array = findNonzero(array1);
            int length = array1.GetLength(0);

            using (StreamWriter fout = new StreamWriter(filename))
            {

                for (int i_sov = 0; i_sov < length; i_sov++)
                {
                    size = array1[i_sov].GetLength(0);
                    for (int j_sov = 0; j_sov < size; j_sov++)
                    //for (int q_sov = 0; q_sov < size; q_sov++)
                    {
                        try
                        {
                            fout.Write(Convert.ToSingle(array1[j_sov][i_sov])+" ");
                        }
                        catch (NullReferenceException ex)
                        {
                            return;
                        }
                    }
                    fout.Write("\n");
                }

            }



        }
/// <summary>
///  save results to present erro
/// </summary>
/// <param name="array"></param>
/// <param name="filename"></param>
        static void saveArray(erro[][] array, int folderstart, string filename)
        {

            int length = array.GetLength(0);
            int size;

            using (StreamWriter fout = new StreamWriter(filename))
            {

                for (int i_sov = 0; i_sov < length; i_sov++)
                {
                    try
                    {
                        size = array[i_sov].GetLength(0);
                    }
                    catch (NullReferenceException ex)
                    {
                        return;
                    }
                    for (int j_sov = 0; j_sov < size; j_sov++)
                    //for (int q_sov = 0; q_sov < size; q_sov++)
                    {
                        try
                        {
                            fout.WriteLine((i_sov+folderstart) + ", "+ array[i_sov][j_sov].classnum+" "+ array[i_sov][j_sov].righclass+"->"+ array[i_sov][j_sov].wrongclass);
                        }
                        catch (NullReferenceException ex)
                        {
                            return;
                        }
                    }

                }
            }



        }


     

        static int[][,] load2DArray(string filename, int size)
        {
            int[][,] testarray = new int[maxFrame][,];

            using (StreamReader fin = new StreamReader(filename))
            {
                string rec;
                int count = 0;
                int change = 0;
               
                testarray[0] = new int[size, size];
                int length = size*size;
                while ((rec = fin.ReadLine()) != null)
                {
                    string[] ar = rec.Split(new char[] { ',' });

                    if (count < length)
                    {

                        testarray[change][Convert.ToInt32(ar[0]), Convert.ToInt32(ar[1])] = Convert.ToInt32(ar[2]);
                        count++;
                    }
                    else
                    {
                        count = 1;
                        change++;
                        testarray[change] = new int[size, size];
                        testarray[change][Convert.ToInt32(ar[0]), Convert.ToInt32(ar[1])] = Convert.ToInt32(ar[2]);
                    }

                }

               
            }
            return testarray;
        
        }

        /// <summary>
        /// save array with different length
        /// </summary>
        /// <param name="filename">filenames</param>
        /// <param name="patchNum">number for each length</param>
        /// <param name="vect_length">length for each vector</param>
        /// <returns></returns>
        static int[][] loadArray(string filename, int[] patchNum, int[] vect_length)
        {
            int[][] testarray = new int[ (patchNum[0] + patchNum[1]+patchNum[2])][];
           
            
         //   int[] vect_length = new int[3] { 57, 73, 89 };
               
           

            using (StreamReader fin = new StreamReader(filename))
            {
                string rec;
                int count = 0;
                int change = 0;
                int control;
                if (patchNum[0] + patchNum[1] == 0)
                    control = 2;
                else if (patchNum[0] == 0 && patchNum[1] != 0)
                    control = 1;
                else
                    control = 0;
                //should return
                int finderror = 0;

                testarray[0] = new int[vect_length[control]];
           
                while ((rec = fin.ReadLine()) != null)
                {
                    string[] ar = rec.Split(new char[] { ',' });
                   
                   
                    if (count < vect_length[control])
                    {
                       
                        testarray[change][count] = Convert.ToInt32(ar[1]);
                        count++;
                        
                    }
                    else
                    {
                        change++;
                        if (change > (patchNum[0] - 1) && change < (patchNum[0] + patchNum[1]))
                        {
                            control = 1;


                        }
                        else if (change > (patchNum[0] + patchNum[1] - 1) && change < (patchNum[0] + patchNum[1] + patchNum[2]))
                        {
                            control = 2;

                        }
                        else
                        {
                            control = 0;
                        }

                       
                        
                        testarray[change] = new int[vect_length[control]];
                        count = 0;
                       testarray[change][count] = Convert.ToInt32(ar[1]);
                       count++;
                    }
                    finderror++;

                }


            }
            int[][] testarray1 = findNonzero(testarray);
            return testarray1;

        }


        static int[] loadArray(string filename, int veclenth)
        {
            int[] testarray = new int[veclenth];

            int count = 0;
        



            using (StreamReader fin = new StreamReader(filename))
            {
                string rec;
                
                while ((rec = fin.ReadLine()) != null)
                {
                    string[] ar = rec.Split(new char[] { ',' });


                    
                    testarray[count] = Convert.ToInt32(ar[1]);
                    count++;

                }
            }
            return testarray;

        }

        static int[] loadArray(string filename, int veclenth, int group)
        {
            int[] testarray = new int[veclenth+2];

            int count = 2;
            testarray[0] = 0;
            testarray[1] = group;




            using (StreamReader fin = new StreamReader(filename))
            {
                string rec;

                while ((rec = fin.ReadLine()) != null)
                {
                    string[] ar = rec.Split(new char[] { ',' });


                    testarray[count] = Convert.ToInt32(ar[1]);
                    count++;

                }
            }
            return testarray;

        }
        /// <summary>
        /// generate 4 Directional feature vector
        /// </summary>
        /// <param name="patchp"></param>
        /// <param name="patchn"></param>
        /// <param name="opt"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int[] flatVector4D(int[,] patchp, int[,] patchn, string opt)
        {
            int size = patchp.GetLength(0);
            int[] patchVector = new int[size*2]; // store positive and negtive seperately
            int sumVecp = 0;
            int sumVecn = 0;
            for (int i = 0; i < size; i++)
            {
                sumVecp = 0;
                sumVecn = 0;
                for (int j = 0; j < size; j++)
                {
                    if (opt == "ver")
                    {
                        try
                        {
                            
                                sumVecp = sumVecp + (patchp[j, i]);
                          
                                sumVecn = sumVecn + (patchn[j, i]);
                        }
                        catch (NullReferenceException ex)
                        {
                            return patchVector;
                        }
                    }
                    else
                    {
                        try
                        
                        {
                        sumVecp = sumVecp + (patchp[i, j]);
               
                            sumVecn = sumVecn + (patchn[i, j]);
                        }
                         catch (NullReferenceException ex)
                        {
                            return patchVector;
                        }
                    }
                    patchVector[i] = sumVecp;
                    patchVector[size+i] = Math.Abs(sumVecn);
                }
                
                
            }

                     int[] patchVector1 = reshape(patchVector);
                    return patchVector1;
            
        
        }

/// <summary>
/// for generating vectors with in frame, and extend the concept the both axis
/// </summary>
/// <param name="patchp"></param>
/// <param name="patchn"></param>
/// <param name="opt"></param>
/// <returns></returns>
        public static int[] flatVector4Dnew(int[,] patchp, int[,] patchn, string opt)
        {
            int size = patchp.GetLength(0);
            int[] patchVector = new int[size * 4]; // store positive and negtive seperately
            int sumVecpx = 0;
            int sumVecnx = 0;
            int sumVecpy = 0;
            int sumVecny = 0;
            for (int i = 0; i < size; i++)
            {
                sumVecpx = 0;
                sumVecnx = 0;
                sumVecpy = 0;
                sumVecny = 0;
                for (int j = 0; j < size; j++)
                {
                    
                        try
                        {

                            sumVecpx = sumVecpx + (patchp[i, j]);

                            sumVecnx = sumVecnx + (patchn[i, j]);

                            sumVecpy = sumVecpy + (patchp[j, i]);

                            sumVecny = sumVecny + (patchn[j, i]);
                        }
                        catch (NullReferenceException ex)
                        {
                            return patchVector;
                        }
                    }
                    
                    
                    patchVector[i] = sumVecpx;
                    patchVector[size +i] = sumVecpy;
                    patchVector[size*2 + i] = Math.Abs(sumVecnx);
               
                    patchVector[size*3 + i] = Math.Abs(sumVecny);
                }


            

            int[] patchVector1 = reshape(patchVector);
            return patchVector1;


        }




        /// <summary>
        /// generate feature vector for 2 Direction
        /// </summary>
        /// <param name="patchp"></param>
        /// <param name="patchn"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public static int[] flatVector2D(int[,] patchp, int[,] patchn, string opt)
        {
            int size = patchp.GetLength(0);
            //int[,] patch = new int [size, size]; // to store max value.
            int[] patchVector = new int[size*4];
            int sumVecx = 0;
            int sumVecy = 0;
            
            for (int i = 0; i < size; i++)
            {
                sumVecx = 0;
                sumVecy = 0;
               
                
                for (int j = 0; j < size; j++)
                {
                    try
                    {

                        sumVecx = sumVecx + Math.Max(patchp[i, j], patchn[i, j]);
                        sumVecy = sumVecy + Math.Max(patchp[j, i], patchn[j, i]);
                    }

                    catch (NullReferenceException ex)
                    {
                        return patchVector;
                    }
                   
                     
                  }

                patchVector[i] = sumVecx;
                patchVector[i + size * 2] = sumVecy;
                }
                    
              
               

            int[] patchVector1 = reshape(patchVector);
            return patchVector1;


        }





       /// <summary>
       /// generate patch in 2 direction
       /// </summary>
       /// <param name="patcheshp"></param>
       /// <param name="patcheshn"></param>
       /// <param name="patchesvp"></param>
       /// <param name="patchesvn"></param>
       /// <returns></returns>
        public static int[] PatchVector2D(int[][,] patcheshp, int[][,] patcheshn, int[][,] patchesvp, int[][,] patchesvn)
        {
            int frames = patcheshp.GetLength(0);  // to caculate how many frames 
            //int size = patcheshp[0].GetLength(0);
            // int framesv = patchesh.GetLengtv(0);
            //   int size = patchesh.GetLengtv(1);

            int[][] frameVectorh = new int[frames][];  // to store 2D vector in each frame


            int[][] frameVectorv = new int[frames][];
          

            

            // Caculate the 2-Dimentional vectors from the 
            for (int i = 0; i < frames; i++)
            {

                frameVectorh[i] = flatVector2D(patcheshp[i], patcheshn[i], "hori");





                frameVectorv[i] = flatVector2D(patchesvp[i], patchesvn[i], "ver");
            }
            int size =patcheshn[0].GetLength(0);

            int[] final_vectors = new int[(frames*2+size*2*2)];

            final_vectors[0] = 0;
            ////To form a vetor by concadenate them
          
            int begin1 = frames*2; //the begining of each section of patch vector
            int begin2 = frames*2+size*2;


            int sumh, sumv;
            //accumulate by space
            for (int j = 0; j < frames; j++)
            {  sumh = 0;
                sumv = 0;
                for (int i = 0; i < size*2; i++)
                {
                    sumh += frameVectorh[j][i];
                    sumv += frameVectorv[j][i];
                }
                final_vectors[j] = sumh;
                final_vectors[j+frames] = sumv;
                  }

            //accumulation by time
            
            for (int i = 0; i < size*2; i++)
            {
                sumh = 0;
                sumv = 0;


                for (int j = 0; j < frames; j++)
                {

                    sumh += frameVectorh[j][i];
                    sumv += frameVectorv[j][i];

                }
                final_vectors[begin1 + i] = sumh * size*2 / frames;
                final_vectors[begin2 + i] = sumv * size*2 / frames;
            }
            return final_vectors;

        }




        /// <summary>
        /// Patch vector for 4 direction
        /// </summary>
        /// <param name="patcheshp"></param>
        /// <param name="patcheshn"></param>
        /// <param name="patchesvp"></param>
        /// <param name="patchesvn"></param>
        /// <returns></returns>
        public static int[] PatchVector4D(int[][,] patcheshp, int[][,] patcheshn, int[][,] patchesvp, int[][,] patchesvn)
        {
            int frames = patcheshp.GetLength(0);  // to caculate how many frames 
           // int size = patcheshp[0].GetLength(0);
           // int framesv = patchesh.GetLengtv(0);
         //   int size = patchesh.GetLengtv(1);

            int[][] frameVectorh = new int[frames][];  // to store 2D vector in each frame
           
           
            int[][] frameVectorv = new int[frames][];
            int size = patcheshp[0].GetLength(0);
            int sumhp = 0;
            int sumvp = 0;
            int sumhn = 0;
            int sumvn = 0;
           
           

            // Caculate the 2-Dimentional vectors from the 
            for (int i = 0; i < frames; i++)
            {
                
                    frameVectorh[i] = flatVector4Dnew (patcheshp[i],patcheshn[i], "hori");
                  
                
                

                
                frameVectorv[i] = flatVector4Dnew(patchesvp[i],patchesvn[i], "ver");
            }
            
            int[] final_vectors = new int[(size*2+frames)*4  + 1];
            final_vectors[0] = 0;
            ////To form a vetor by concadenate them

            int begin1 = frames * 4; //the begining of each section of patch vector
            int sizeofflat = size*2;
            int begin2 = frames * 4 + sizeofflat*2;


            int sumh, sumv;
            //accumulate by space
            for (int j = 0; j < frames; j++)
            {
                sumhp = 0;
                sumvp = 0;
                  sumhn = 0;
                sumvn= 0;
                for (int i = 0; i < sizeofflat; i++)
                {
                    sumhp += frameVectorh[j][i];
                    sumvp += frameVectorv[j][i];
                    sumhn += frameVectorh[j][i + sizeofflat];
                    sumvn += frameVectorv[j][i + sizeofflat];
                }
                final_vectors[j] = sumhp;
                final_vectors[j + frames] = sumhn;
                final_vectors[j+frames*2] = sumvp;
                final_vectors[j + frames*3] = sumvn;
            }

            //accumulation by time

            for (int i = 0; i < sizeofflat*2; i++)
            {
                sumh = 0;
                sumv = 0;


                for (int j = 0; j < frames; j++)
                {

                    sumh += frameVectorh[j][i];
                    sumv += frameVectorv[j][i];

                }
                final_vectors[begin1 + i] = sumh * sizeofflat / frames;
                final_vectors[begin2 + i] = sumv * sizeofflat / frames;
            }
            return final_vectors;
        
        }
        
        /// <summary>
        /// <summary>
        /// APMD pach vectors. Assume each patch has the same size
        /// </summary>
        /// <param name="patcheshp">patch_1</param>
        /// <param name="patcheshn">patch_2</param>
        /// <param name="patchesvp">patch_3</param>
        /// <param name="patchesvn">patch_4</param>
        /// <param name="cubid_size">size to accumulate</param>
        /// <param name="maxorsummation">select max or summation to get the feature</param>
        /// <returns></returns>
        public static int[] Apmd_Patch(int[][,] patcheshp, int[][,] patcheshn, int[][,] patchesvp, int[][,] patchesvn,int cubid_size, Boolean domax)
        {
            int length = patcheshp.GetLength(0);
            int size = patcheshp[0].GetLength(0);
            int size_vector = size*size*length/(cubid_size*cubid_size*cubid_size); // the size of vector for each direction
            int[] Final_Vectors = new int[size_vector*4];  // store the final vector for ApmdPatch
            int ind_1 = size_vector;    // index for the second direction and so on
            int ind_2 = size_vector*2;  
            int ind_3 = size_vector * 3;
            for (int ii = 0; ii < size_vector; ii++)
            {
                Final_Vectors[ii] = vector_single(patcheshp, cubid_size, domax)[ii];

                Final_Vectors[ind_1 + ii] = vector_single(patcheshn, cubid_size, domax)[ii];
                Final_Vectors[ind_2 + ii] = vector_single(patchesvp, cubid_size, domax)[ii];
                Final_Vectors[ind_3 + ii] = vector_single(patchesvn, cubid_size, domax)[ii];
                                /* * */

            
            }



                return Final_Vectors;
        }
        /// <summary>
        /// caculate APMD in a single direction patches
        /// </summary>
        /// <param name="patches"></param>
        /// <returns></returns>
        public static int[] vector_single(int[][,] patches, int cubid_size, Boolean domax)
        {
            int length = patches.GetLength(0);
            int size = patches[0].GetLength(0);
            int size_vector = size * size * length / (cubid_size * cubid_size * cubid_size);
            int maxorsum; //depends on the choise for para
            int ind = 0;; // to indicate the index of vector
            int[] Final_Vectors = new int[size_vector];
           for (int ff =0; ff<length-cubid_size+1; ff= ff+cubid_size)
            for (int ii = 0; ii < size-cubid_size+1; ii = ii + cubid_size)
                for (int jj = 0; jj < size-cubid_size+1; jj = jj + cubid_size)
                {

                        maxorsum = 0;
                      for(int nn = ff; nn<ff+cubid_size; nn++)
                          for(int ww= ii; ww<ii+cubid_size;ww++)
                            for (int hh = jj; hh < jj + cubid_size; hh++)
                            {
                                 if (domax)  // if max is selected
                                 {
                                  maxorsum = Math.Max(maxorsum, patches[nn][ww, hh]);
                                 }
                                 else
                                 {
                                   maxorsum = maxorsum+patches[nn][ww, hh];
                                 }
                            }
                      
                    Final_Vectors[ind++] = maxorsum;
                        
                    
                    
                    }
                
                
                
                    return Final_Vectors;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertical">input matrix</param>
        /// <param name="horizontal">input montion field</param>
        /// <param name="time">time span of patches</param>
        /// <param name="size">size of patches</param>
        /// <param name="num">number of patches</param>d
        static void findInterest(int[][,] verticalp, int[][,] verticaln, int[][,] horizontalp, int[][,] horizontaln, int time, int size, int num, string fileDes)
        {
            //generate random number for spatio-temproer
            int x = 0;
            int y = 0;
            int frame = 0;
            int[][] patches = new int[num][];
            int nonmeanning; //to caculate the timeofmotion
            int timeofmotion = 0;
            for (int ii = 0; ii < 800; ii++)
            {
                try
                {

                    nonmeanning = verticaln[ii][0, 0];

                }
                catch (NullReferenceException ex)
                {
                    break;
                }
                timeofmotion++;
            }



            int sizeofmotion = verticaln[0].GetLength(0) + 1;

            ARGB[][,] picforshow = new ARGB[timeofmotion][,];
            //byte[][,] picforshow = new byte[timeofmotion][,];

            bool continued = false;
            int countdown = 0;
            int testPatch;
            bool[][,] preventerror = new bool[timeofmotion][,];
            bool stopsignal = false;
            int threshold = size * size * time / 5;
            int tempmaxv;
            int tempmaxh;
            int tempmax;
            // int threshold ;
            for (int ii = 0; ii < (timeofmotion); ii++)
            {
                preventerror[ii] = new bool[sizeofmotion, sizeofmotion];
                picforshow[ii] = new ARGB[sizeofmotion - 1, sizeofmotion - 1];

                for (int jj = 0; jj < sizeofmotion - 1; jj++)
                    for (int ff = 0; ff < sizeofmotion - 1; ff++)
                    {
                        preventerror[ii][jj, ff] = false;
                        tempmaxv = Math.Max(verticalp[ii][jj, ff], verticaln[ii][jj, ff]);
                        tempmaxh = Math.Max(horizontalp[ii][jj, ff], horizontaln[ii][jj, ff]);



                        //generate color map for 2 directions

                         /*  if (tempmaxv < tempmaxh)
                        {
                           // picforshow[ii][jj, ff] = new ARGB( (8 - tempmaxh) / 8, 0, (8 - tempmaxh) * 255 / 8);
                            picforshow[ii][jj, ff] = new ARGB((8 - tempmaxh) * 255 / 8, (8 - tempmaxh) * 255 / 8, (8 - tempmaxh) * 255 / 8);


                        }
                        else if (tempmaxv > tempmaxh)
                        {
                            picforshow[ii][jj, ff] = new ARGB((8 - tempmaxh) * 255 / 8, (8 - tempmaxh) * 255 / 8, (8 - tempmaxh) * 255 / 8);
                            //picforshow[ii][jj, ff] = new ARGB((tempmaxv) * 255 / 8, 255, (8 - tempmaxv) * 255 / 8);
                        }
                        else
                        {
                            picforshow[ii][jj, ff] = new ARGB(255, 255, 255);
                        }
                        */
                                    // generate color map for 4 direction
                       
                                       tempmax = Math.Max(tempmaxv,tempmaxh);
                                       if (tempmaxv==tempmaxh) 
                                       {
                                          //picforshow[ii][jj, ff] = new ARGB(tempmax * 255 / 8,tempmax*255/8,0); //yellow means equal
                                           picforshow[ii][jj, ff] = new ARGB(255, 255, 255); //yellow means equal

                                       }
                                       else if(tempmax == verticalp[ii][jj,ff])
                                       {
                                          // picforshow[ii][jj, ff] = new ARGB(tempmax*255 / 8,0,0);//red
                                           picforshow[ii][jj, ff] = new ARGB((tempmax) * 255 / 8, 0, 0);
                                       }
                                       else if (tempmax == verticaln[ii][jj, ff])
                                       {
                                          // picforshow[ii][jj, ff] = new ARGB(0, tempmax * 255 / 8,0);//green
                                           picforshow[ii][jj, ff] = new ARGB(0, 0, tempmax * 255 / 8); //yellow means equal
                                          
                                      
                                       }
                                       else if (tempmax == horizontalp[ii][jj, ff])
                                       {
                                          // picforshow[ii][jj, ff] = new ARGB(0, tempmax * 255 / 8, tempmax * 255 / 8);//cyan
                                          // picforshow[ii][jj, ff] = new ARGB(255, 255, 255); //yellow means equal
                                           picforshow[ii][jj, ff] = new ARGB(255, 255, 255); //yellow means equal
                                       }
                                       else {

                                          // picforshow[ii][jj, ff] = new ARGB(tempmax * 255 / 8, 0, tempmax * 255 / 8);//purple

                                           picforshow[ii][jj, ff] = new ARGB(255, 255, 255); //yellow means equal
                                       }
                                       // picforshow[ii][jj, ff] = 0;
                                         
                    }





            }



            for (int i = 0; i < num; i++)
            {

                while (!continued)
                {
                    continued = false;
                    while (!stopsignal)
                    {
                        x = Convert.ToInt16(Rand(0, (sizeofmotion - size - 1)));
                        y = Convert.ToInt16(Rand(0, (sizeofmotion - size - 1)));
                        frame = Convert.ToInt16(Rand(0, (timeofmotion - time)));
                        if (!preventerror[frame][x, y])
                        {
                            preventerror[frame][x, y] = true;
                            stopsignal = true;
                        }
                    }
                    testPatch = 0;
                    for (int ii = 0; ii < time; ii++)
                    {
                        countdown++;
                        if (countdown > 1000000)
                            return;

                        testPatch = 0;
                        for (int jj = 0; jj < size; jj++)
                            for (int hh = 0; hh < size; hh++)
                            {


                                if ((verticalp[frame + ii][x + jj, y + hh] + verticaln[frame + ii][x + jj, y + hh] + horizontalp[frame + ii][x + jj, y + hh] + horizontaln[frame + ii][x + jj, y + hh]) != 0)
                                    testPatch++;

                            }



                    }
                    if (testPatch > threshold)
                    {
                        continued = true;   //if patch has any contents, then copy is success otherwise redo;
                    }
                    stopsignal = false;
                }

                continued = false;
                for (int ii = 0; ii < time; ii++)
                {


                    for (int jj = 0; jj < size; jj++)
                        for (int hh = 0; hh < size; hh++)
                        {
                          /*
                            picforshow[frame + ii][x + 0, y + hh].R = 0;
                            picforshow[frame + ii][x + 0, y + hh].G = 0;
                            picforshow[frame + ii][x + 0, y + hh].B = 0;

                            picforshow[frame + ii][x + jj, y + 0].R = 0;
                            picforshow[frame + ii][x + jj, y + 0].G = 0;
                            picforshow[frame + ii][x + jj, y + 0].B = 0;

                            picforshow[frame + ii][x + size, y + hh].R = 0;
                            picforshow[frame + ii][x + size, y + hh].G = 0;
                            picforshow[frame + ii][x + size, y + hh].B = 0;

                            picforshow[frame + ii][x + jj, y + size].R = 0;
                            picforshow[frame + ii][x + jj, y + size].G = 0;
                            picforshow[frame + ii][x + jj, y + size].B = 0;

                             /* */



                        }



                }



            }
            
            string fileName_Des1;
            for (int q = 0; q < timeofmotion; q++)
            {


                if ((q + 13) < 100)
                {

                    fileName_Des1 = (fileDes + "_00" + Convert.ToString(q + 13) + "");
                }
                else
                {
                    fileName_Des1 = (fileDes + "_0" + Convert.ToString(q + 13) + "");
                }

                saveColorImage(picforshow[q], fileName_Des1);
             

            }
           



        }









        // To get paches from the original pictures
        /// <summary>
        /// to generate patches for feature generation
        /// </summary>
        /// <param name="motionpich">motion field pictures in horizontal direction</param>
        /// <param name="motionpicv">motion field pictures in vertical direction</param>
        /// <param name="size">the size for each frame</param>
        /// <param name="num">number of patches</param>
        /// <param name="time">number of frames for each patch</param>
        /// <param name="nonzeropatches">if elimanate nonzeropatch during process</param>
        /// <returns></returns>
        public static int[][] patch_generation(int[][,] motionpichp, int[][,] motionpichn,int[][,] motionpicvp,int[][,] motionpicvn, int size, int time,int num, bool nonzeropatches, bool if2D, bool domax)
        {
            //generate random number for spatio-temproer
            int x = 0;
            int y = 0;
            int frame = 0;
            int[][] patches = new int[num][];
            int[][,] twoPatchhp = new int[time][,];
            int[][,] twoPatchhn = new int[time][,];
            int[][,] twoPatchvp = new int[time][,];
            int[][,] twoPatchvn = new int[time][,];
           
            bool continued = false;
            int countdown = 0;
            int testPatch;
            int nonmeanning; //to caculate the timeofmotion
            int timeofmotion = 0;
            long stopwatch = 0;
            for (int ii = 0; ii < 1000; ii++)
            {
                try
                {

                    nonmeanning = motionpichp[ii][0, 0];

                }
                catch (NullReferenceException ex)
                {
                    break;
                }
                timeofmotion++;
            }
            int sizeofmotion = motionpichp[0].GetLength(0) + 1;

            bool[][,] preventerror = new bool[timeofmotion][,];
            bool stopsignal = false;
            int threshold = size*size*time/5;
           // int threshold ;
            for (int ii = 0; ii <timeofmotion; ii++)
            {
                preventerror[ii] = new bool[sizeofmotion, sizeofmotion];
                for (int jj = 0; jj < sizeofmotion; jj++)
                    for (int ff = 0; ff < sizeofmotion; ff++)
                        preventerror[ii][jj, ff] = false;
            }
            for (int i = 0; i < num; i++)
            {

                while (!continued)
                {
                    continued = false;
                    while (!stopsignal)
                    {
                        x = Convert.ToInt16(Rand(0, (sizeofmotion - size - 1)));
                        y = Convert.ToInt16(Rand(0, (sizeofmotion - size - 1)));
                        frame = Convert.ToInt16(Rand(0, (timeofmotion - time)));
                        if (!preventerror[frame][x, y])
                        {
                            preventerror[frame][x, y] = true;
                            stopsignal = true;
                        }
                        stopwatch++;
                        if (stopwatch > 2048000)
                            goto stopHere;
                    }
                    testPatch = 0;
                    for (int ii = 0; ii < time; ii++)
                    {
                        countdown++;
                        twoPatchhp[ii] = new int[size, size];
                        twoPatchhn[ii] = new int[size, size];
                        twoPatchvp[ii] = new int[size, size];
                        twoPatchvn[ii] = new int[size, size];
                        testPatch = 0;
                        for (int jj = 0; jj < size; jj++)
                            for (int hh = 0; hh < size; hh++)
                            {
                                twoPatchhp[ii][jj, hh] = motionpichp[frame + ii][x + jj, y + hh];
                                twoPatchhn[ii][jj, hh] = motionpichn[frame + ii][x + jj, y + hh];
                                twoPatchvp[ii][jj, hh] = motionpicvp[frame + ii][x + jj, y + hh];
                                twoPatchvn[ii][jj, hh] = motionpicvn[frame + ii][x + jj, y + hh];

                                if ((twoPatchhp[ii][jj, hh]+twoPatchhn[ii][jj,hh]+ twoPatchvp[ii][jj, hh]+twoPatchvn[ii][jj,hh]) != 0)
                                    testPatch++;

                            }



                    }
                    if (testPatch >= threshold)
                    {
                        continued = true;   //if patch has any contents, then copy is success otherwise redo;
                    }
                    stopsignal = false;
                }
               continued = false;
               if (if2D)
               {
                   patches[i] = PatchVector2D(twoPatchhp, twoPatchhn, twoPatchvp, twoPatchvn);
               }
               else
               {
                   patches[i] = Apmd_Patch(twoPatchhp, twoPatchhn, twoPatchvp, twoPatchvn, 2, domax);
                   //patches[i] = PatchVector4D(twoPatchhp, twoPatchhn, twoPatchvp, twoPatchvn);
               }

            }


        stopHere: int[][] patchesNon = findNonzero(patches);
            return patchesNon;

        }


        // problematic function.
        public static int[][] findNonzero(int[][] originals)
        {
            int num = 0;
            int length = originals.GetLength(0);

            int sum = 0;
            int cacl = 0;
            bool stop = false;
            int ww = originals.GetLength(0);
            // int hh = originals[1].GetLength(0);
            int hh;
            int[][] intermedia = new int[length][];
            int indexnum = 0;
            for (int ii = 0; ii < ww; ii++)
            {

                sum = 0;
                try
                {
                    hh = originals[ii].GetLength(0);
                    for (int jj = 0; jj < hh; jj++)
                    {

                        sum = originals[ii][jj] + sum;


                    }


                    if (sum != 0)
                    {

                        intermedia[indexnum] = (int[])originals[ii].Clone();
                        indexnum++;
                    }
                }
                catch (NullReferenceException ex)
                { break; }



            }
            int[][] result = new int[indexnum][];
            int elementsize;
            try
            {
                for (int ii = 0; ii < indexnum; ii++)
                {
                    elementsize = intermedia[ii].GetLength(0);
                    result[ii] = new int[elementsize];
                   
                    for (int jj = 0; jj < elementsize;jj++ )
                        result[ii][jj] = intermedia[ii][jj];

                }
            }
            catch (IndexOutOfRangeException ex)
            {
                return result;
            }
            catch (NullReferenceException ex)
            {
                return result;
            }
            return result;





        }
       

        public static int[][] kmeans(int[][] originals, int k)
        {

          
            bool ifstop = false;
            int[][] forCompare = new int[k][];
            int[] distance = new int[k];
            int sum;
            // find random point  0<x<size
            int[][] nonzeroVectors = findNonzero(originals);
            int length = nonzeroVectors.GetLength(0);
            if (k == 0)
                return null;
            else if (k >= length)
                return nonzeroVectors;

            int randomindex = 0;
            int Vectornum = nonzeroVectors[1].GetLength(0);
            bool stopsignal = false;
            //initialized: randomly select k vectors;
            bool[] preventerror = new bool[length];
            for (int i = 0; i < length; i++)
            {
                preventerror[i] = false;
            }
            for (int i = 0; i < k; i++)
            {
                while (!stopsignal)
                {
                    randomindex = (Rand(0, length));
                    if (!preventerror[randomindex]) // if the randam value has been assighed ,then change
                    {
                        preventerror[randomindex] = true;

                        forCompare[i] = (int[])nonzeroVectors[randomindex].Clone();// problems!!!!!!!!!!!!!!!!1111
                        forCompare[i][0] = 0;
                        stopsignal = true;
                    }
                }
                stopsignal = false;
            }

            while (!ifstop)
            {
                ifstop = true;


                for (int i = 0; i < length; i++)
                {
                   
                    int minvalue = 10000;
                    int minindex = 0;
                    for (int j = 0; j < k; j++)
                    {
                        sum = 0;
                        for (int h = 1; h < Vectornum; h++)
                        {
                            sum = sum + Math.Abs(nonzeroVectors[i][h] - forCompare[j][h]);

                        }
                        if (minvalue > sum)
                        {
                            minvalue = sum;
                            minindex = j;

                        }
                    }
                    if (nonzeroVectors[i][0] != minindex)
                    {
                        ifstop = false;
                    }
                    nonzeroVectors[i][0] = minindex;

                }
                //caculate center
                int[] num = new int[k];
                int[] sum_vec = new int[k];

               

                for (int i = 1; i < Vectornum; i++)
                {
                    for (int ii = 0; ii < k; ii++)
                    {
                        sum_vec[ii] = 0;
                        num[ii] = 0;
                    
                    }
                    for (int h = 0; h < length; h++)
                    {

                        sum_vec[nonzeroVectors[h][0]] = nonzeroVectors[h][i] + sum_vec[nonzeroVectors[h][0]];
                        num[nonzeroVectors[h][0]]++;
                        
                    }
                   
                    for (int ii = 0; ii < k; ii++)
                    {
                        if (num[ii] != 0)
                        {
                            forCompare[ii][i] = sum_vec[ii] / num[ii];
                        }

                        if (forCompare[ii][0] != 0)
                            return forCompare;
                    } 

                }
               
                
            }
            return forCompare;
        }


        public static int[][] classify(int[][] originals, int k, int startfolder)
        {

            bool ifstop = false;                             // to stop kmeans
            int[][] forCompare = new int[k][];
            int[] distance = new int[k];
            int sum;
            // find random point  0<x<size
           int[][] nonzeroVectors = findNonzero(originals);  //find out array element which has no value
            //int[][] nonzeroVectors = originals;
            int length = nonzeroVectors.GetLength(0);
            int randomindex = 0;
            int Vectornum = nonzeroVectors[0].GetLength(0);
            bool stopsignal = false;                                // to stop loop for random generation
            //initialized: randomly select k vectors;
           // bool[] preventerror = new bool[k]; //to prevent situation if there are two same random number
            int nextcate = -1;    // findout the next category
            int nextindex = startfolder;
            for (int i = 0; i < k; i++)                     //initializing: generate k random vectors 
            {
                
                while (!stopsignal)
                {
                     
                    if(nextcate != nonzeroVectors[nextindex][1])
                    {   nextcate = nonzeroVectors[nextindex][1];
                        forCompare[i] = (int[])nonzeroVectors[nextindex].Clone();// problems!!!!!!!!!!!!!!!!1111
                        forCompare[i][0] = 0;
                        
                        stopsignal = true;
                    }
                    nextindex++;
                }
                stopsignal = false;
            }

    //        while (!ifstop)
            {
      //          ifstop = true;

                
                for (int i = 0; i < length; i++)
                {
                    
                    int minvalue = 1000000000;
                    int minindex = 0;
                    for (int j = 0; j < k; j++)
                    {
                        sum = 0;
                        for (int h = 2; h < Vectornum; h++)
                        {
                            sum = sum + Math.Abs(nonzeroVectors[i][h] - forCompare[j][h]);

                        }
                        if (minvalue > sum)
                        {
                            minvalue = sum;
                            minindex = j;

                        }
                    }
                   
                    nonzeroVectors[i][0] = forCompare[minindex][1];

                }
               
               


            }
            return nonzeroVectors;
        }
        /// <summary>
        /// measure similarity between two vectors
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>

        public static int similarity(int[] vec1, int[] vec2)
        { 
                int sum = 0;
                int length = vec1.GetLength(0);
                int result;
                for (int ii = 0; ii < length; ii++)
                {
                    sum = sum + Math.Abs(vec1[ii] - vec2[ii]);
                }
               
                
                result = 256-Convert.ToInt16((Convert.ToDouble(sum))*0.2);
                
           
            if (result < 0)
                     result = 0;
              /* 
                if (sum == 0)
                    result = 5000;
                else
                {
                    result = Convert.ToInt16(1 / Convert.ToDouble(sum) * 1000);
                } */ 
                return result;
               // return sum;
        }
        /// <summary>
        /// similarity based on dot product
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
       public static int similarityNew(int[] vec1, int[] vec2)
        {
            double sumdot = 0;
            double sumVec1 = 0;
            double sumVec2 = 0;
            int length = vec1.GetLength(0);
            int result;
            for (int ii = 0; ii < length; ii++)
            {
                sumdot = sumdot + Convert.ToDouble(vec1[ii] * vec2[ii]);
                sumVec1 = sumVec1 + Convert.ToDouble(vec1[ii] * vec1[ii]);
                sumVec2 = sumVec2 + Convert.ToDouble(vec2[ii] * vec2[ii]);

            }



            result =Convert.ToInt16(sumdot/(Math.Sqrt(sumVec1)*Math.Sqrt(sumVec2))*100);


            
           
            return result;

        }
       /* */ 

        public static int maxvalue(int[] vectors)
        {
            int max = vectors[0];
            for (int ii = 1; ii < vectors.GetLength(0); ii++)
            {
                if (max < vectors[ii])
                    max = vectors[ii];
            }
            
            return max;
        
        
        }
        /// <summary>
        /// generate featureVectors for a given movie or motion field
        /// </summary>
        /// <param name="motionfieldhp"></param>
        /// <param name="motionfieldhn"></param>
        /// <param name="motionfieldvp"></param>
        /// <param name="motionfieldvn"></param>
        /// <param name="patchtemp">Array that store template patches</param>
        /// <param name="patchnum">number of patch</param>
        /// <param name="patch_size">size of patch</param>
        /// <param name="patchtemp">time size of patch</param>
        /// <param name="startnum">sequences of first element</param>
        /// <param name="elementnum">how many of patch element should be generated</param>
        /// <returns></returns>

        public static int[] featureVectors(int[][,] motionfieldhp, int[][,] motionfieldhn, int[][,] motionfieldvp, int[][,] motionfieldvn, int[][] patchtemp,int startnum, int elementnum, bool if2D, int timespan, int patch_size,bool domax)
        {
            int frames = motionfieldhp.GetLength(0);
            int size = motionfieldhp[0].GetLength(0);    // total size of motionfield
            int[] featureVector = new int[elementnum];
           // int timespan = 4;                             // important indicator 

            int[][,] getpatcheshp = new int[timespan][,];
            int[][,] getpatcheshn = new int[timespan][,];
            int[][,] getpatchesvp = new int[timespan][,];
            int[][,] getpatchesvn = new int[timespan][,];
            int sum4norm = 0;
            int sizeofmotion = motionfieldhp[0].GetLength(0);

            int similarities = 0;
           // int patch_size = 8;
            if (elementnum > patchtemp.GetLength(0))
                elementnum = patchtemp.GetLength(0);
            int testcon;  // to exclude one with little patches
            int testthre;
            testthre = patch_size * patch_size * timespan / 5;
            int[] vector_patch;

            for (int ff = 0; ff < frames - timespan; ff++)
            {



                for (int ss = 0; ss < sizeofmotion - patch_size; ss++)
                    for (int zz = 0; zz < sizeofmotion - patch_size; zz++)
                    {


                        // get patches
                        testcon = 0;

                        for (int patch_f = 0; patch_f < timespan; patch_f++)
                        {
                            getpatcheshp[patch_f] = new int[patch_size, patch_size];
                            getpatcheshn[patch_f] = new int[patch_size, patch_size];
                            getpatchesvp[patch_f] = new int[patch_size, patch_size];
                            getpatchesvn[patch_f] = new int[patch_size, patch_size];
                            for (int patch_w = 0; patch_w < patch_size; patch_w++)
                                for (int patch_h = 0; patch_h < patch_size; patch_h++)
                                {
                                    try
                                    {
                                        getpatcheshp[patch_f][patch_w, patch_h] = motionfieldhp[ff + patch_f][ss + patch_w, zz + patch_h];
                                        getpatcheshn[patch_f][patch_w, patch_h] = motionfieldhn[ff + patch_f][ss + patch_w, zz + patch_h];

                                    }
                                    catch (NullReferenceException ex)
                                    {
                                        //MessageBox.Show("opps!");
                                        goto vect;
                                        
                                    }
                                    try
                                    {
                                        getpatchesvp[patch_f][patch_w, patch_h] = motionfieldvp[ff + patch_f][ss + patch_w, zz + patch_h];
                                        getpatchesvn[patch_f][patch_w, patch_h] = motionfieldvn[ff + patch_f][ss + patch_w, zz + patch_h];
                                        if ((getpatcheshp[patch_f][patch_w, patch_h] + getpatcheshn[patch_f][patch_w, patch_h] + getpatchesvp[patch_f][patch_w, patch_h] + getpatchesvn[patch_f][patch_w, patch_h]) != 0)
                                            testcon++;
                                    }
                                    catch (NullReferenceException ex)
                                    {
                                       // MessageBox.Show("opps!");
                                        goto vect;
                                    }
                                }

                        }
                        if ((testcon) > testthre)
                        {
                            if (if2D)
                            {
                                vector_patch = PatchVector2D(getpatcheshp, getpatcheshn, getpatchesvp, getpatchesvn);
                            }
                            else
                            {
                                //vector_patch = PatchVector4D(getpatcheshp, getpatcheshn, getpatchesvp, getpatchesvn);
                                vector_patch = Apmd_Patch(getpatcheshp, getpatcheshn, getpatchesvp, getpatchesvn, 2, domax);
                            }
                            for (int vecelement = startnum; vecelement < startnum + elementnum; vecelement++)
                            {
                                similarities = similarity(vector_patch, patchtemp[vecelement]);
                                if (similarities > featureVector[vecelement-startnum])
                                    featureVector[vecelement - startnum] = similarities;

                            }
                        }





                    }
            }
            
        vect: int toexsit = 0;
            /*
            int tempvar;
            for (int ii = startnum; ii < elementnum + startnum; ii++)
                sum4norm = sum4norm + featureVector[ii - startnum];
            for (int ii = startnum; ii < elementnum + startnum; ii++)
            {

                featureVector[ii - startnum] = featureVector[ii - startnum] * 800000 / sum4norm;

            }
             */ 

            return featureVector;


        }
        
        /// <summary>
        /// To generate feature vectors by using caculate method
        /// </summary>
        /// <param name="motionfieldh">motionfield in horizontal direction</param>
        /// <param name="motionfieldv">motionfield in vertical direction</param>
        /// <param name="patchtemp">the database for patch</param>
        /// <param name="startnum">when only generate part of the vector, specify the start index of the vector, otherwise 0</param>
        /// <param name="elementnum">specify how many vector elements should the function generate</param>
        /// <param name="patchsize"></param>
        /// <returns></returns>
        public static int[] featureVectorsNew(int[][,] motionfieldhp, int[][,] motionfieldhn, int[][,] motionfieldvp, int[][,] motionfieldvn, int[][] patchtemp, int startnum, int elementnum,bool if2D)
        {

            int frames = motionfieldhp.GetLength(0);
            int size = motionfieldhp[0].GetLength(0);    // total size of motionfield
            int[] featureVector = new int[elementnum];
            int timespan = 4;

            int[][,] getpatcheshp = new int[timespan][,];
            int[][,] getpatcheshn = new int[timespan][,];
            int[][,] getpatchesvp = new int[timespan][,];
            int[][,] getpatchesvn = new int[timespan][,];
            int sizeofmotion = motionfieldhp[0].GetLength(0);
            int sumNorm = 0;
            int similarities = 0;
            int patch_size = 4;
            int patsize = 1;

            int testcon;  // to exclude one with little patches
            int testthre;
            int vect_index = 0;
            int[] vector_patch;


            testthre = patch_size * patch_size * timespan / 5;

            for (int ff = 0; ff < frames - timespan; ff++)
            {



                for (int ss = 0; ss < 63 - patch_size; ss++)
                    for (int zz = 0; zz < 63 - patch_size; zz++)
                    {

                        // get patches
                        testcon = 0;

                        for (int patch_f = 0; patch_f < timespan; patch_f++)
                        {
                            getpatcheshp[patch_f] = new int[patch_size, patch_size];
                            getpatcheshn[patch_f] = new int[patch_size, patch_size];
                            getpatchesvp[patch_f] = new int[patch_size, patch_size];
                            getpatchesvn[patch_f] = new int[patch_size, patch_size];
                            for (int patch_w = 0; patch_w < patch_size; patch_w++)
                                for (int patch_h = 0; patch_h < patch_size; patch_h++)
                                {
                                    try
                                    {
                                        getpatcheshp[patch_f][patch_w, patch_h] = motionfieldhp[ff + patch_f][ss + patch_w, zz + patch_h];
                                        getpatcheshn[patch_f][patch_w, patch_h] = motionfieldhn[ff + patch_f][ss + patch_w, zz + patch_h];

                                    }
                                    catch (NullReferenceException ex)
                                    {
                                        MessageBox.Show("opps!");
                                        goto vect;

                                    }
                                    try
                                    {
                                        getpatchesvp[patch_f][patch_w, patch_h] = motionfieldvp[ff + patch_f][ss + patch_w, zz + patch_h];
                                        getpatchesvn[patch_f][patch_w, patch_h] = motionfieldvn[ff + patch_f][ss + patch_w, zz + patch_h];
                                        if ((getpatcheshp[patch_f][patch_w, patch_h] + getpatcheshn[patch_f][patch_w, patch_h] + getpatchesvp[patch_f][patch_w, patch_h] + getpatchesvn[patch_f][patch_w, patch_h]) != 0)
                                            testcon++;
                                    }
                                    catch (NullReferenceException ex)
                                    {
                                        MessageBox.Show("opps!");
                                        goto vect;
                                    }
                                }


                        }
                        
                        if (if2D)
                        {
                            vector_patch = PatchVector2D(getpatcheshp, getpatcheshn, getpatchesvp, getpatchesvn);
                        }
                        else
                        {
                            vector_patch = PatchVector4D(getpatcheshp, getpatcheshn, getpatchesvp, getpatchesvn);
                        }
                        vect_index = 0;
                        int min = 1000;
                        for (int vecelement = startnum; vecelement < startnum + elementnum; vecelement++)
                        {


                            similarities = similarityNew(vector_patch, patchtemp[vecelement]);
                            if (similarities < min)
                            {
                                min = similarities;
                                vect_index = vecelement - startnum;
                               
                                
                            }
                        }

                        /* int simila = similarity(vector_patch, patchtemp[vect_index]);
                         if (simila>0)*/
                       
                        featureVector[vect_index]++;





                    }




            }
           
        vect: return featureVector;


        }
        /// <summary>
        /// reshape the function 
        /// </summary>
        /// <param name="original">the original 2d array</param>
        /// <returns></returns>
        public static int[][] reshape(int[][] original)
        { 
            
           
            int sum = 0;  // to get the total summation
            int width; // the width of each array
            int[][] original1 = findNonzero(original);
            int length = original1.GetLength(0);  // length of input array
            int[][] result = new int[length][];  // initiate the result array
            for (int i = 0; i < length; i++)
            { 
                width = original1[i].GetLength(0);
                result[i] = new int[width];
                sum = 1;
              /*  for (int j = 2; j < width; j++)
                {
                    sum = sum + original1[i][j];
                
                }*/
                for (int j = 2; j < width; j++)
                {

                    result[i][j] = original1[i][j]/ sum;
                
                }
                result[i][0] = original1[i][0];
                result[i][1] = original1[i][1];
            
            }
            return result;
        
        }
/// <summary>
/// reshape the one dimentional array according 
/// </summary>
/// <param name="original1"></param>
/// <returns></returns>
        public static int[] reshape(int[] original1)
        {


            int sum = 0;  // to get the total summation


            int length = original1.GetLength(0);  // length of input array
            int[] result = new int[length];  // initiate the result array


            sum = 0;
            for (int j = 0; j < length; j++)
            {
                sum = sum + original1[j];

            }
            //if (sum == 0)

            if(true)
                return original1;
           
            for (int j = 0; j < length; j++)
            {

                result[j] = original1[j]*100 / sum;

            }



            return result;
        }

        /// <summary>
        /// form the vector for a given sequence of sets
        /// </summary>
        /// <param name="original"></param>
        /// <param name="learnsets">sets for test or learning</param>
        /// <param name="setnum">set number</param>
        /// <returns></returns>
        public static int[][] reshape(int[][] original, int[] sets, int setnum, int classnum)
        {// bug there are 4 situation in all


            int width; // the width of each array
            int[][] original1 = findNonzero(original);
            int length = classnum * setnum;  // length of input array
            int[][] result = new int[length][];  // initiate the result array
            for (int i = 0; i < setnum; i++)
            {
                for (int jj = 0; jj < classnum; jj++)
                {
                    width = original1[(sets[i]) * classnum + jj].GetLength(0);
                    result[i * classnum + jj] = new int[width];

                    for (int j = 2; j < width; j++)
                    {

                        result[i * classnum + jj][j] = original1[(sets[i]) * classnum + jj][j];

                    }
                    result[i * classnum + jj][0] = original1[(sets[i]) * classnum + jj][0];
                    result[i * classnum + jj][1] = original1[(sets[i]) * classnum + jj][1];
                }
            }
            return result;

        }


        public static void classify(int[][] originals, int classnum, int[] test, int elementused, int numberofelem)
        {


            bool ifstop = false;                             // to stop kmeans
            int[][] forCompare = new int[classnum][];
            int[] distance = new int[classnum];
            int sum;
            int[][] nonzeroVectors = findNonzero(originals);  //find out array element which has no value
            int length = nonzeroVectors.GetLength(0);

            // find random point  0<x<size

            int Vectornum = nonzeroVectors[0].GetLength(0);
            int minvalue = 1000000000;
            int minindex = 0;
           int temprand=0;
            int element= 0;
           bool stopgen = false;
            int randind = 0;
            bool[] erropre = new bool[numberofelem];
            int[] randarray = new int[elementused];  //save random arrays
            for(int ii =0;ii<numberofelem;ii++)
                erropre[ii] = false;  

           for(int ii =0;ii<elementused;ii++)
           {
               stopgen = false;
            while(!stopgen)
            { 
                temprand = Rand(2,elementused);
                if (!erropre[temprand])
                {
                    randarray[ii] = temprand;
                    stopgen = true;
                }
            
            }
           }



            for (int i = 0; i < length; i++)
            {


                //  for (int j = 0; j < classnum; j++)
                {
                    sum = 0;
                    for (int h = 0; h < elementused; h++)

                    {

                     

                        sum = sum + Math.Abs(nonzeroVectors[i][randarray[h]] - test[randarray[h]]);

                    }
                    if (minvalue > sum)
                    {
                        minvalue = sum;
                        minindex = i;

                    }


                }
            }



            test[0] = nonzeroVectors[minindex][1];







        }
        /// <summary>
        /// KNN
        /// </summary>
        /// <param name="originals"></param>
        /// <param name="classnum"></param>
        /// <param name="test"></param>
        /// <param name="elementused"></param>
        /// <param name="numberofelem"></param>
        /// <param name="k">number of k</param>

        public static void classifyKNN(int[][] originals, int classnum, int[] test, int elementused, int numberofelem, int k)
        {


            int[] minClasses = new int[k];
            int[] bigInfo = new int[2]; // including the biggest values and corresponding index
           
            int initialNum = 0;
            int[][] forCompare = new int[classnum][];
            int[] distance = new int[classnum];
            int sum;
            int[][] nonzeroVectors = findNonzero(originals);  //find out array element which has no value
            int length = nonzeroVectors.GetLength(0);
            int[] minValue = new int[k];
            int minInd;
            // find random point  0<x<size

            int Vectornum = nonzeroVectors[0].GetLength(0);
            int temprand = 0;
          
            bool stopgen = false;
          
            bool[] erropre = new bool[numberofelem];
            int[] randarray = new int[elementused];  //save random arrays
            for (int ii = 0; ii < numberofelem; ii++)
                erropre[ii] = false;

            for (int ii = 0; ii < elementused; ii++)
            {
                stopgen = false;
                while (!stopgen)
                {
                    temprand = Rand(2, elementused);
                    if (!erropre[temprand])
                    {
                        randarray[ii] = temprand;
                        stopgen = true;
                    }

                }
            }



            for (int i = 0; i < length; i++)
            {


                //  for (int j = 0; j < classnum; j++)
              
                    sum = 0;
                    for (int h = 0; h < elementused; h++)
                    {



                        sum = sum + Math.Abs(nonzeroVectors[i][randarray[h]] - test[randarray[h]]);

                    }
                    if (initialNum < k)
                    {
                        minClasses[initialNum] = nonzeroVectors[i][1];
                        minValue[initialNum] = sum;
                        initialNum++;
                    }
                    else {
                       bigInfo = findBig(minValue);
                       if (sum < bigInfo[0])
                       {
                           minValue[bigInfo[1]] = sum;
                           minClasses[bigInfo[1]] = nonzeroVectors[i][1]; 
                       
                       }
                    
                    }

                   
                
            }
            test[0] = winnerTakeall(minClasses);

}
        /// <summary>
        /// find biggest valueand corresponding index from array
        /// </summary>
        /// <param name="minValue">Array that contains value</param>
        /// <returns></returns>


        public static int[] findBig(int[] minValue)
        {
            int[] results = new int[2];
            int bigValue = 0;
            for (int ii = 0; ii < minValue.GetLength(0); ii++)
            {
                if (bigValue < minValue[ii])
                {
                    results[0] = minValue[ii];
                    bigValue = minValue[ii];
                    results[1] = ii;

                
                }
            }
                return results;
        }
        /// <summary>
        /// winner takes all
        /// </summary>
        /// <param name="minclass">array contains class</param>
        /// <returns></returns>
        public static int winnerTakeall(int[] minclass)
        {
            int resultVote = minclass[0]; //save the first as the iniatial value
            int[] voteContainer = new int[12]{0,0,0,0,0,0,0,0,0,0,0,0}; //less than 12 classes
            for (int ii = 0; ii < minclass.GetLength(0); ii++)
            {
                voteContainer[minclass[ii]]++;
            
            }
            resultVote = findBig(voteContainer)[1];

                return resultVote;
        }




        /// <summary>
        /// Find the combination of a given array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="length"></param>

        static T[][] getCombinations<T>(T[] array, int length)
        {
            Combinations<T> combinations = new Combinations<T>(array, length);
            T[][] results = new T[combinations.Count][];
            int indic = 0;
            foreach (IList<T> perm in combinations)
            {
                results[indic] = new T[length];
                perm.CopyTo(results[indic++], 0);
            }


            return results;

        }

        
        [System.STAThreadAttribute()]



        public static void Main()
        {


            /************************************************
             ************************************************
             * for final patches with different sizes,      *
             * patchsizes decide how many number of patches *            
             * used for final features                      *
             ************************************************ 
             ***********************************************/


            /////////////////////////////////  experiment setting /////////////////////////////////////////////////////
            int startnum = 0;                                          //startnum for feature element needed to be caculate
          
                           // when learning is true, doing patch template generation
            bool[] sepTemp =new bool[] {true,false};        //if true, generate seperate patche template
            bool[] if2D = new bool[] {false,false };                  //if true, generate motion field in 2-d direction, otherwise in 4 direction.
            int[] patchNum = new int[] { 2400,0,0 }; // indicate the number of patches in template of sizes 4, 8, 12
            int[] originalnum = new int[]{500,100};// indicate the original random selection
            int veclength = 2400;                  ///patchNum[0]+patchNum[1]+patchNum[2];
            string[] fileArrayPatches = new string[] {  "patches\\0401sep.txt", "patches\\0401com.txt" };   // file name to save patches
           // string[] fileArrayPatches = new string[] { "patches\\patches1224sep4D.txt", "patches\\patches1224com4D.txt" };   // file name to save patches
            Boolean domax = false;
            
            
            int timespan = 4; //length of the patch
            int[] patch_size = {4,8};
            int[] patchsizes = new int[3] { 4, 8, 12 };
            
               bool if_Maxop = true;// for comparing the result with max and without max
                        int maxwin = 4; //to contol the window size of max operation
          

                int sizeofmotion = 256*2/maxwin;
            bool preprocessing =false;
            bool analysis = true;
            bool learn = false; 

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            int startclass = 0;
            int endclass = 6;
            int classnum = endclass - startclass;

            int folderstart = 0;
            int folderend = 0;

            if (!learn)
            {
               folderstart = 1;
               folderend = 25;
           
            }
            else
            {
                folderstart = 1;
                folderend = 1;
            }
            int foldernum1 = folderend - folderstart+1;
            
        

           
            int[] patch4each = new int[3] { patchNum[0] / classnum, patchNum[1] / classnum, patchNum[2] / classnum };      // for each category we need to keep how many vectors
           
         
            
            string sor_Diskname = "c";
            string des_Diskname = "c";
            int vectorSeq = 0; //used for save analysis vectors
            int Cut; // this is cut rate
            //int Num_Left = Convert.ToInt32(256 * 256 * Cut);
            string baseDir = "frame256x256";
            string desDir = "frame256x256_des";
            string database = "KTH";
            string[] senario = new string[]{"d1","d2","d3","d4"};
            
            string tem = @"\";
            baseDir = baseDir + tem + database;
            desDir = desDir + tem + database;

            int finalpatchnum =  (patchNum[0] + patchNum[1]+patchNum[2]);
            int[][] finalpatchvector = new int[finalpatchnum][];
           
            //int timeofmotion = 40;    
            int[][] patch4 = new int[16000][]; // to store patches with 4by4 size extracted from database
            int[][] patch8 = new int[16000][]; // to store patches with 8by9 size extracted from database
            int[][] patch12 = new int[5000][]; // to stroe patches with 12by12 size extracted from database
            int[][] testfeather = new int[5][]; //??

            int[] vect_length = new int[3]; // the length of each patch vectors
            string[] cateName = new string[] { "jogging", "handclapping", "handwaving", "boxing", "running", "walking" };
        //    string[] cateName = new string[] { "jack" };
            string[] subName = new string[] { "edge", "deg", "motionfield", "intere" };
            

          





            int[][]vector4compare = new int[1300][];

            string fileFeather;




          int patchOpt = 0;

   //     for (int patchOpt = 0; patchOpt < 2;patchOpt++ ) // indicate the options for patches
            {
                int base4by4 = 0;
                int base8by8 = 0;
              //  int base12by12 = 0;
               // int foldernum = 1;               
                
                for (int foldernum = folderstart; foldernum <= folderend; foldernum++)
                {
                 
                    for(int seo = 0; seo<4; seo++)
                   //int seo = 3;
                    for (int classificagtions = startclass; classificagtions < endclass; classificagtions++)
                    {
                       // if (domax)
                        { fileFeather = "result\\featurevector_" + Convert.ToString(foldernum) + "_" + cateName[classificagtions] +"_"+senario[seo]+"_"+Convert.ToString(startnum) + "-" + Convert.ToString(veclength) + "_" + Convert.ToString(patchOpt) + ".txt"; }
                        //else
                        //{ fileFeather = "result\\featurevectorAPMD_" + Convert.ToString(foldernum) + "_" + cateName[classificagtions] + Convert.ToString(startnum) + "-" + Convert.ToString(veclength) + "_" + Convert.ToString(patchOpt) + ".txt"; }
                        // if need to generate patches then uncomment this instruction
                        //    int classificagtions = 3;


                        if (if2D[patchOpt])
                        {
                           // vect_length[0] = 32;
                            //vect_length[0] = (patchsizes[0]*2 + timespan )* 2 ;
                            vect_length[1] = (patchsizes[1] * 2 + timespan) * 2;
                            vect_length[2] = (patchsizes[2] * 2 + timespan) * 2; //vect_length = (size + time) x #dimensions+1
                        }
                        else
                        {
                            {
                               // vect_length[0] = 32;
                                vect_length[0] = (patchsizes[0] * 2 + timespan) * 4 + 1;
                                vect_length[1] = (patchsizes[1] * 2 + timespan) * 4 + 1;
                                vect_length[2] = (patchsizes[2] * 2 + timespan) * 4 + 1; //vect_length = (size + time) x #dimensions+1
                            }

                        }




                        string dirName = "sequence_p1";
                        string dirNum = Convert.ToString(foldernum);

                        string fileNamebase = dirName + "_";
                        string path_sor = sor_Diskname + @":" + tem + baseDir + tem + dirName + tem + dirNum + tem + cateName[classificagtions] + tem;
                        string path_des = des_Diskname + @":" + tem + desDir + tem + dirName + tem + dirNum + tem + cateName[classificagtions] + tem;
                        int[] featurevector = new int[patchNum[0] + patchNum[1] + patchNum[2]];
                        int[] featurevector4x4;
                        int[] featurevector8x8;
                        int[] loadfeather;

                        int g;
                        int startFile;
                        int thre_motion;



                        {
                            g = 10;
                            startFile = 13;
                            thre_motion = 20;
                            Cut = 2;
                        }
                        bool experiment_show = true;



                        /************************ for Test ****************************************************
                         * 
                         * 

                        string forTest;you
                        for (g = 10; g < 30; g++)
                        {
                            forTest = @"C:\Users\ray\Desktop\result pics\Test\_" + Convert.ToString(g);
                            Generate_Edge(fileName_Sor, forTest, startFile, maxFrame, g, true);

                        }

                         * 
                         * 
                         * 
               
                         ************************* for Test ****************************************************/


                        string fileArray1 = "image\\save_hori_" + Convert.ToString(foldernum) + "_" + cateName[classificagtions] + "_p.txt";
                        string fileArray2 = "image\\save_ver_" + Convert.ToString(foldernum) + "_" + cateName[classificagtions] + "_p.txt";
                        string fileArray3 = "image\\save_hori_" + Convert.ToString(foldernum) + "_" + cateName[classificagtions] + "_n.txt";
                        string fileArray4 = "image\\save_ver_" + Convert.ToString(foldernum) + "_" + cateName[classificagtions] + "_n.txt";
                        if (preprocessing)
                        {
                            //   if (!File.Exists(fileArray1))
                            {



                                //To specific the root directories for source and destination



                                string fileName_Sor = path_sor + fileNamebase;
                                string fileName_Des_Edge = path_des + subName[0] + tem + fileNamebase;

                                string directory_Des_Edge = path_des + subName[0];
                                Directory.CreateDirectory(directory_Des_Edge);
                                //   Generate_Edge(fileName_Sor, fileName_Des_Edge, startFile, maxFrame, g, false, false);
                                //                                goto fin_mid;


                                string fileName_sor = fileName_Des_Edge;
                                string fileName_Edge = fileName_Des_Edge;
                                string fileName_Des_Degh = path_des + subName[1] + "_H" + tem + fileNamebase;
                                string directory_Des_Degh = path_des + subName[1] + "_H";
                                Directory.CreateDirectory(directory_Des_Degh);
                                Generate_DEG(fileName_sor, fileName_Des_Degh, startFile, maxFrame + 100, "Hori", Cut, false);
                                goto fin_mid;

                                string fileName_Des_Degv = path_des + subName[1] + "_V" + tem + fileNamebase;
                                string directory_Des_Degv = path_des + tem + subName[1] + "_V";
                                Directory.CreateDirectory(directory_Des_Degv);

                                Generate_DEG(fileName_sor, fileName_Des_Degv, startFile, maxFrame + 100, "Ver", Cut, false);

                                string fileName_Des_Moveh = path_des + subName[2] + " H" + tem + fileNamebase;
                                string directory_Des_Moveh = path_des + subName[2] + " H";
                                Directory.CreateDirectory(directory_Des_Moveh);



                                string fileName_Des_Movev = path_des + subName[2] + "_V" + tem + fileNamebase;
                                string directory_Des_Movev = path_des + subName[2] + "_V";
                                Directory.CreateDirectory(directory_Des_Movev);

                                /*
                                string fileName_Des_Move = path_des + subName[2] + tem + fileNamebase;
                                string directory_Des_Move = path_des + subName[2];
                                Directory.CreateDirectory(directory_Des_Move);
                                */

                                //     Generate_Motions(fileName_sor, fileName_Des_Moveh, startFile, maxFrame, "Hori", thre_motion, if_Maxop, true, maxwin);
                                //     Generate_Motions(fileName_sor, fileName_Des_Movev, startFile, maxFrame, "Ver", thre_motion, if_Maxop, true, maxwin);

                                Generate_Motions(fileName_Des_Degh, fileName_Des_Moveh, startFile, maxFrame, "Hori", thre_motion, if_Maxop, true, maxwin);




                                Generate_Motions(fileName_Des_Degv, fileName_Des_Movev, startFile, maxFrame, "Ver", thre_motion, if_Maxop, true, maxwin);



                                /**/
                                motionfield[][,] hori_array = Generate_MotionArray(fileName_Des_Degh, fileName_Des_Moveh, startFile, maxFrame, "Hori", thre_motion, if_Maxop, maxwin);
                                motionfield[][,] ver_array = Generate_MotionArray(fileName_Des_Degv, fileName_Des_Movev, startFile, maxFrame, "Ver", thre_motion, if_Maxop, maxwin);


                                //int maxFrame = 1500;
                                int[][,] hori4savep = new int[maxFrame][,];
                                int[][,] hori4saven = new int[maxFrame][,];
                                int[][,] ver4savep = new int[maxFrame][,];
                                int[][,] ver4saven = new int[maxFrame][,];

                                for (int icopy = 0; icopy < maxFrame; icopy++)
                                {

                                    hori4savep[icopy] = new int[sizeofmotion, sizeofmotion];
                                    hori4saven[icopy] = new int[sizeofmotion, sizeofmotion];
                                    ver4savep[icopy] = new int[sizeofmotion, sizeofmotion];
                                    ver4saven[icopy] = new int[sizeofmotion, sizeofmotion];
                                    try
                                    {
                                        for (int jcopy = 0; jcopy < sizeofmotion; jcopy++)
                                            for (int hcopy = 0; hcopy < sizeofmotion; hcopy++)
                                            {
                                                hori4savep[icopy][jcopy, hcopy] = hori_array[icopy][jcopy, hcopy].pos;
                                                hori4saven[icopy][jcopy, hcopy] = hori_array[icopy][jcopy, hcopy].neg;
                                                ver4savep[icopy][jcopy, hcopy] = ver_array[icopy][jcopy, hcopy].pos;
                                                ver4saven[icopy][jcopy, hcopy] = ver_array[icopy][jcopy, hcopy].neg;


                                            }
                                    }

                                    catch (NullReferenceException ex)
                                    { break; }



                                }

                                //TestforSave


                                save2DArray(hori4savep, fileArray1, sizeofmotion);
                                save2DArray(ver4savep, fileArray2, sizeofmotion);
                                save2DArray(hori4saven, fileArray3, sizeofmotion);
                                save2DArray(ver4saven, fileArray4, sizeofmotion);
                                // goto fin_mid;
                            }

                            {
                                /********************** testing for interesting feature detection ***************************/

                                int[][,] hori_array1p = load2DArray(fileArray1, sizeofmotion);
                                int[][,] ver_array1p = load2DArray(fileArray2, sizeofmotion);
                                int[][,] hori_array1n = load2DArray(fileArray3, sizeofmotion);
                                int[][,] ver_array1n = load2DArray(fileArray4, sizeofmotion);
                                string fileName_Des_Moveint = path_des + subName[3] + tem + fileNamebase;
                                string directory_Des_Moveint = path_des + subName[3];
                                Directory.CreateDirectory(directory_Des_Moveint);

                                findInterest(hori_array1p, hori_array1n, ver_array1p, ver_array1n, timespan, patchsizes[1], 500, fileName_Des_Moveint);
                                /* *******************************************************************************************/
                                goto fin_mid;

                            }


                        }
                        else
                        {





                            if (!learn)
                            {

                                ///////////////////////////////////  generate features  ///////////////////////////////////////////
                                //////////////////////////////////////////////////////////////////////////////////////////////////

                                


                                //  string fileFeather = "featurevector_" + Convert.ToString(foldernum) + "_" + cateName[classificagtions] + Convert.ToString(startnum) + "-" + Convert.ToString(startnum+veclength) + "patch4w.txt";

                                if (!File.Exists(fileFeather))
                                {

                                    int[][] test_patch = loadArray(fileArrayPatches[patchOpt], patchNum, vect_length);  //loading templatepatch
                                    int[][,] hori_array1p = load2DArray(fileArray1, sizeofmotion);
                                    int[][,] ver_array1p = load2DArray(fileArray2, sizeofmotion);
                                    int[][,] hori_array1n = load2DArray(fileArray3, sizeofmotion);
                                    int[][,] ver_array1n = load2DArray(fileArray4, sizeofmotion);

                                    //test load


                                    //featurevector = featureVectors(hori_array1, ver_array1, test_patch, startnum, veclength);
                                    //featurevector = featureVectors(hori_array1p, hori_array1n, ver_array1p, ver_array1n,test_patch,startnum,veclength,if2D[patchOpt]);
                                    long start = System.Environment.TickCount;
                                    featurevector4x4 = featureVectors(hori_array1p, hori_array1n, ver_array1p, ver_array1n, test_patch, startnum, patchNum[0], if2D[patchOpt], timespan, patch_size[0], domax);
                                    featurevector8x8 = featureVectors(hori_array1p, hori_array1n, ver_array1p, ver_array1n, test_patch, (startnum), patchNum[1], if2D[patchOpt], timespan, patch_size[1], domax);
                                    long stop = System.Environment.TickCount;
                                    long elapsed = stop - start;
                                    long minutesElapsed = elapsed / 1000;
                                    int index4final = 0;

                                    for (int ii = 0; ii < patchNum[0]; ii++)
                                    {
                                        featurevector[ii] = featurevector4x4[ii];

                                    }
                                    index4final = patchNum[0];

                                    for (int ii = 0; ii < patchNum[1]; ii++)
                                    {
                                        featurevector[ii + index4final] = featurevector8x8[ii];
                                    }

                                    // ShowHisto(featurevector[foldernum], Convert.ToString(foldernum));

                                    saveArray(featurevector, fileFeather, startnum);

                                }

                                //   

                                if (analysis)
                                {
                                    loadfeather = loadArray(fileFeather, patchNum[0] + patchNum[1] + patchNum[2], classificagtions);
                                    vector4compare[vectorSeq++] = (int[])loadfeather.Clone();  
                                    //ShowHisto(loadfeather, Convert.ToString(foldernum * 10 + loadfeather[1]));
                                }
                                else
                                {
                                    loadfeather = loadArray(fileFeather, patchNum[0] + patchNum[1] + patchNum[2]);
                                    //ShowHisto(loadfeather, Convert.ToString(foldernum) + cateName[classificagtions]);
                                }
                            }
                            else
                            {
                                ////////////////////   generate patches template //////////////////////////////////////// 
                                ////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////


                                // The hori_array1 read horizontal data from file
                                int[][,] hori_array1p = load2DArray(fileArray1, sizeofmotion);
                                int[][,] ver_array1p = load2DArray(fileArray2, sizeofmotion);
                                int[][,] hori_array1n = load2DArray(fileArray3, sizeofmotion);
                                int[][,] ver_array1n = load2DArray(fileArray4, sizeofmotion);

                                //the first element in this methods is for classification.
                                int[][] patches_4by4 = (patch_generation(hori_array1p, hori_array1n, ver_array1p, ver_array1n, patchsizes[0], timespan, originalnum[0], true, if2D[patchOpt], domax));
                                int[][] patches_8by8 = (patch_generation(hori_array1p, hori_array1n, ver_array1p, ver_array1n, patchsizes[1], timespan, originalnum[1], true, if2D[patchOpt], domax));
                                // int[][] patches_12by12 = (patch_generation(hori_array1p, hori_array1n, ver_array1p, ver_array1n, 12, 0, timespan, true, if2D[patchOpt]));

                                // if (true)
                                {
                                    ///////////////////////////////  generate seperate template ////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    if (sepTemp[patchOpt])
                                    {


                                        if (originalnum[0] != 0)
                                        {
                                            int[][] patches4by4 = kmeans(patches_4by4, patch4each[0]);         // using K-means to find out the common features within one categore

                                            for (int ii = 0; ii < patch4each[0]; ii++)
                                            {
                                                finalpatchvector[classificagtions * patch4each[0] + ii] = (int[])patches4by4[ii].Clone();
                                            }
                                        }
                                        if (originalnum[1] != 0)
                                        {

                                            int[][] patches8by8 = kmeans(patches_8by8, patch4each[1]);

                                            for (int ii = 0; ii < patch4each[1]; ii++)
                                            {
                                                finalpatchvector[classificagtions * patch4each[1] + classnum * patch4each[0] + ii] = (int[])patches_8by8[ii].Clone();
                                            }
                                        }

                                        //     int[][] patches_nonzeros = findNonzero(patches);

                                        /*
                                       for (int ii = 0; ii < patch4each[2]; ii++)
                                       {
                                           finalpatchvector[classificagtions * patch4each[2] + patchsizes[0] + patchsizes[1] + ii] = (int[])patches_12by12[ii].Clone();
                                       }
                                    }*/


                                    }
                                    else
                                    {
                                        /*
                                        int veclength4by4 = 500;
                                        int veclength8by8 = 500;
                                        int veclength12by12 = 500;*/

                                        int numberofpatches4by4 = patches_4by4.GetLength(0);
                                        for (int ii = 0; ii < numberofpatches4by4; ii++)
                                        {
                                            patch4[base4by4 + ii] = (int[])patches_4by4[ii].Clone();



                                        }
                                        base4by4 = base4by4 + numberofpatches4by4;

                                        int numberofpatches8by8 = patches_8by8.GetLength(0);
                                        for (int ii = 0; ii < numberofpatches8by8; ii++)
                                        {
                                            patch8[base8by8 + ii] = (int[])patches_8by8[ii].Clone();



                                        }
                                        base8by8 = base8by8 + numberofpatches8by8;

                                        /*
                                        for (int ii = 0; ii < originalnum; ii++)
                                        {
                                            patch12[base12by12 + ii] = (int[])patches_12by12[ii].Clone();

                                            

                                        }
                                        base12by12 =originalnum+ base12by12;
                                         */
                                        /**/
                                    }

                                }





                            }
                        }

                    fin_mid: int test = 0;
                    }

                }

                // patch method 2, to generate general patch
                if (learn)
                {
                    if (!sepTemp[patchOpt])
                    {

                        
                        int[][] patche4by4 = kmeans(patch4, patchNum[0]);
                        int[][] patche8by8 = kmeans(patch8, patchNum[1]);
                        //int[][] patche12by12 = kmeans(patch12, patchNum[2]);


                        int index4final = 0;
                      
                        for (int ii = 0; ii < patchNum[0]; ii++)
                        {
                            finalpatchvector[ii] = (int[])patche4by4[ii].Clone();

                        }
                        index4final = patchNum[0];

                      for (int ii = 0; ii < patchNum[1]; ii++)
                       {
                           finalpatchvector[ii + index4final] = (int[])patche8by8[ii].Clone();
                      }

        //                index4final = patchNum[0] + patchNum[1];
/*
                        for (int ii = 0; ii < patchNum[2]; ii++)
                        {
                            finalpatchvector[ii + index4final] = (int[])patche12by12[ii].Clone();

                        }
 */
                    }


                    saveArray(finalpatchvector, fileArrayPatches[patchOpt]);


                }
          /*  else if (analysis) // making files for testing and training
                {
                    int[][] samples = findNonzero(vector4compare); //save learning samples
                    int[][] sample4Testing = new int[classnum*30][];  // There are 24 files in total correspond to 1 sample
                    int[][] sample4Training = new int[classnum * 70][];
                int [] numSelect = new int[100];
                int randomNum;
                for (int numTrail = 0; numTrail < 30; numTrail++)
                {
                    for (int ii = 0; ii < 100; ii++)
                        numSelect[ii] = 0;



                    for (int numTest = 0; numTest < 30; numTest++)
                    {
                       
                    hate:    randomNum = Rand(0, 100);
                    if (numSelect[randomNum] != 1)
                        numSelect[randomNum] = 1;
                    else
                        goto hate;
                        for (int numClass = 0; numClass < classnum; numClass++)
                        {

                            sample4Testing[numTest * 6 + numClass] = (int[])samples[randomNum * 6 + numClass].Clone();
                        }


                    }
                    
                    TrainTestFiles(sample4Testing, "svm\\Testdat"+numTrail.ToString()+".dat");
                    int indexOne = 0;
                    for (int numTrain = 0; numTrain < 100; numTrain++)
                    {
                        if (numSelect[numTrain] != 1)
                        {
                            


                            for (int numClass = 0; numClass < classnum; numClass++)
                            {

                                sample4Training[(indexOne) * 6 + numClass] = (int[])samples[numTrain * 6 + numClass].Clone();
                            }
                            indexOne++;
                        }

                    }
                    TrainTestFiles(sample4Training, "svm\\Traindat"+numTrail.ToString()+".dat");
                }
                */
                
                    
                else if (analysis)
        {
                      int numberofsamples = 100;  
           
                //  int[] elementusedNum =new int[] {100,150,200,250,300,350,400,450,500,550,600,650,700, 750,800,850,900,950,1000,1100,1200,1300,1400,1500,1600,1700,1800,1900,2000,2100,2200,2300,2400};
                   int[] elementusedNum = new int[] { 2400 };
                    double [] elem_results = new double[elementusedNum.GetLength(0)];
                                 // how many samples for learning
                    int num_test = 1;  // how man samples for testing
                    int[] samples_all = new int[numberofsamples-1]; //training for loo

                    double[][] rate = new double[1000][];
                    // when classfication, using startfolder as criteron
                    erro[] erro1 = new erro[100000];



                    double[][] sumrate = new double[classnum][];  // for caculating average recognition rate

                    for (int qq = 0; qq < classnum; qq++)
                        sumrate[qq] = new double[classnum];
                    int seq = 0;
                    int ind = 0;
                    int lengthoftest = num_test; // used for form learning set
                    int num_learn = numberofsamples-num_test;
                   // for( int num_learn = numberofsamples-1;  num_learn>numberofsamples-2; num_learn--)
                    {
                    for (int num_element = 0; num_element < elementusedNum.GetLength(0);num_element++ )
                    {



                        for (int qq = 0; qq < classnum; qq++)
                            sumrate[qq] =new double[classnum];
                         seq = 0;
                         ind = 0;
                 


                        for (int sample_test = 0; sample_test < 30; sample_test++)
                        {
                            int indic = 0;
                            for (int ii = 0; ii < numberofsamples; ii++)
                            {
                                if ((ii) != sample_test)

                                    samples_all[indic++] = ii;


                            }
                          //  int[][] samples_learn = getCombinations<int>(samples_all, num_learn);  //generate learning sets/
                            








                          //  for (int dd = 0; dd < samples_learn.GetLength(0); dd++)
                            {

                                // int learnnum = learnsets[dd].GetLength(0);
                                //  int lengthoftest = testsets[dd].GetLength(0);








                                {







                                    int[][] reshapefun = reshape(vector4compare, samples_all, num_learn, classnum); //save learning samples






                                    int len = 0;



                                    int[][] testseq = new int[lengthoftest * classnum][]; // vectors to store testing samples
                                    int correction = lengthoftest; //store total number for each class


                                    for (int ii = 0; ii < num_test; ii++)
                                    {
                                        for (int jj = 0; jj < classnum; jj++)
                                        {

                                            testseq[ii * classnum + jj] = vector4compare[sample_test * classnum + jj];
                                            classifyKNN(reshapefun, classnum, testseq[ii * classnum + jj], elementusedNum[num_element],veclength,3);
                                            len++;

                                            if (testseq[ii * classnum + jj][1] == testseq[ii * classnum + jj][0])
                                                sumrate[testseq[ii * classnum + jj][1]][testseq[ii * classnum + jj][1]]++;
                                            else
                                            {

                                                erro1[ind].classnum = sample_test;
                                                erro1[ind].wrongclass = testseq[ii * classnum + jj][0];
                                                erro1[ind].righclass = testseq[ii * classnum + jj][1];
                                                sumrate[(erro1[ind].righclass)][(erro1[ind].wrongclass)]++;
                                                ind++;
                                            }


                                        }


                                    }









                                    seq++;

                                    //    saveArray(erro1, folderstart, "erroresults.txt");
                                }

                            }
                        }
                    for (int qq = 0; qq < classnum; qq++)
                        for (int pp = 0; pp < classnum; pp++)
                            sumrate[qq][pp] = sumrate[qq][pp] / (seq);


                    double sum_ave = 0;
                    for (int ii = 0; ii < classnum; ii++)
                        sum_ave = sum_ave + sumrate[ii][ii];

                    elem_results[num_element] = sum_ave / classnum*100;
                    if (elementusedNum[num_element] == 2400)
                    {
                        string recogitonMex = ("rate\\confusionMex" + "_" + Convert.ToSingle(patchOpt) + "_" + Convert.ToString(num_learn) + "_" + Convert.ToString(elementusedNum[num_element]) + ".txt");
                        saveArray(sumrate, recogitonMex);
                    }
                 }  
                    string recogitonrate = ("rate\\patchnumresult" + "_" + Convert.ToSingle(patchOpt) + "_" + Convert.ToString(num_learn)+ ".txt");
                    saveArray(elem_results,recogitonrate);
                    }
                }
            }
           
            int test123 = 0;






   fin:         Application.Run(probsolve);


        }

    }


}
