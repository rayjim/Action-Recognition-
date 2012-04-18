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

namespace mytry1007
{
	public class ImageToolBox
	{
		public ImageToolBox ()
		{
		}
		
		
        public struct PixelData
        {
            public byte Blue;
            public byte Green;
            public byte Red;
        }
        static Bitmap image;//solve the problem about the bitmap's constructor
     //   static Form2 probsolve = new Form2();//sovle the problem about closing the process
        static Random random = new Random();
      //  static int[] result = new int[1000];
        static public bool stopMethod = false;
        static public int maxFrame = 1500; // indicate maxFrame


        static public void createFile(string filePath, int[][] array)			//Generate PPED File
        {
            filePath = filePath;

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
            System.Console.WriteLine(filePath+"is correctlly written");


        }
        static public void createFile(string filePath, int[] array)                  //Generate PPED File
        {
            filePath = filePath;
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
            System.Console.WriteLine("file"+filePath+"created with data.");


        }
        /// <summary>
        /// maximum num of database is 100
        /// </summary>
        /// <param name="filePath">Input the file name of the Database</param>
        /// <returns></returns>
        /// 
        static public List<Int32> readFile(string filePath)							//Load data...
        {
           
			List<Int32> result = new List();
            using (StreamReader sr = new StreamReader(filePath))
            {
                

                while (sr.Peek() > 0)
                {

                    string lineofText = sr.ReadLine();
					string[] array = lineofText.Split(",");
					
                   /* if (lineofText == ",")  //
                    { i++; j = 0; }
                    else
                    {
                        array[i][j++] = Convert.ToInt32(lineofText);

                    }*/
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
            openFile.Filter = "ÍŒÏñÎÄŒþ (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|ËùÓÐÎÄŒþ (*.*)|*.*";
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
	
	}
	
	
}

