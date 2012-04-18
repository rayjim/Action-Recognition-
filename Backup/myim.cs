using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using myImgp;
using System.IO;
using System.Text;
namespace myImgp
{
	public class exam
	{
		static Bitmap image;//solve the problem about the bitmap's constructor
		static Form2 probsolve=new Form2();//sovle the problem about closing the process
		static  Random random=new Random();
		static int[] result=new int[1000];
		static int[][] PPEDTemplate;
		static int counter;
		
		static public void CreateFile(string filePath,int[][] array)			//Generate PPED File

		{
              filePath=@"d:\works\"+filePath;
				
			using(StreamWriter sw=new StreamWriter(System.IO.File.Open(filePath,System.IO.FileMode.Append)))
			{
				for(int i=0;i<array.LongLength;i++)
				{
					for(int j=0;j<array[i].Length;j++)
					{

						sw.WriteLine(array[i][j]);
					}
					sw.WriteLine(",");
				}

			
			}
			//System.Console.WriteLine("file created with data using the streamWriter class.");

		
		}
		static public void CreateFile(string filePath,int[]array)                  //Generate PPED File

		{
				filePath=@"D:\works\"+filePath;
			using(StreamWriter sw=new StreamWriter(System.IO.File.Open(filePath,System.IO.FileMode.Append)))
			{
				
				{
					for(int j=0;j<array.Length;j++)
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
		/// <param name="filePath"></param>
		/// <returns></returns>
		static public int[][] OutputFile(string filePath)							//Load data...
		{  
			filePath=@"d:\works\"+filePath;
			using(StreamReader sr=new StreamReader(filePath))
			{
					int[][] array=new int[100][];
				for(int q=0;q<100;q++)
					array[q]=new int[64];
				int i=0,j=0;
			
				while(sr.Peek()>0)
				{
				 
					string lineofText=sr.ReadLine();
					if (lineofText==",")
					{i++;j=0;}
					else
					{
						array[i][j++]=Convert.ToInt32(lineofText);
						
					}	
				}
				return array;
			
			}
			
		}
		/// <summary>
		/// //////////////////////////////////////////////////////////////
		/// load picture
		/// <param name="FileName"></param>
		/// <returns></returns>
		public static byte[,] LoadImg(string FileName)
		{
			FileName=(@"d:\works\photos\"+FileName+".bmp");
			Bitmap image=new Bitmap(FileName);
			Color cc=new Color();
			int xres,yres,i,j;
			xres=image.Width;
			yres=image.Height;
			byte [,] imMatrix=new byte[xres,yres];
			for(i=0;i<=xres-1;i++)
				for(j=0;j<=yres-1;j++)
					imMatrix[i,j]=0;
			for(i=0;i<=xres-1;i++)
				for(j=0;j<=yres-1;j++)
				{
					cc=image.GetPixel(i,j);
					imMatrix[i,j]=(byte)((cc.R+cc.G+cc.B)/3);
					
				}
			return imMatrix;



			
			
	
		
		
		}
		/// <summary>
		/// select picture and load it.
		/// </summary>
		/// <returns></returns>
		public static byte[,] LoadImg()
		{
			OpenFileDialog openFile=new OpenFileDialog();
			//openFile.Filter="BMP File(*.bmp)|*.bmp|JPEG File(*.jpg)|*.jpg|All File(*.*)|*.*";
			openFile.Filter="图像文件 (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|所有文件 (*.*)|*.*";
			if(openFile.ShowDialog()==DialogResult.OK)
			{
				//Form2 child=new Form2();
				string Text=openFile.FileName;
				image=new Bitmap(openFile.FileName);
				//child.PictureBox1.Image=image;
				//child.Show();
				Color cc=new Color();
				int xres,yres,i,j;
				xres=image.Width;
				yres=image.Height;
				byte [,] imMatrix=new byte[xres,yres];
				for(i=0;i<=xres-1;i++)
					for(j=0;j<=yres-1;j++)
						imMatrix[i,j]=0;
				for(i=0;i<=xres-1;i++)
					for(j=0;j<=yres-1;j++)
					{
						cc=image.GetPixel(i,j);
						imMatrix[i,j]=(byte)((cc.R+cc.G+cc.B)/3);
					
					}
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
			public ARGB(int r,int g,int b)
			{
				R=r;
				G=g;
				B=b;
			
			}
			public ARGB()
			{
				R=0;
				G=0;
				B=0;
			
			}

	
		}
		/// <summary>
		/// load color picture
		/// </summary>
		/// <returns></returns>
		public static ARGB[,] LoadColorImg()
		{
			OpenFileDialog openFile=new OpenFileDialog();
			openFile.Filter="图像文件 (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|所有文件 (*.*)|*.*";
			if(openFile.ShowDialog()==DialogResult.OK)
			{
				image=new Bitmap(openFile.FileName);
				Color cc=new Color();
				int xres,yres,i,j;
				xres=image.Width;
				yres=image.Height;
				ARGB [,]argb=new ARGB[xres,yres];

				for(i=0;i<=xres-1;i++)
					for(j=0;j<=yres-1;j++)
					{
						argb[i,j]=new ARGB(0,0,0);
					
					}
				for(i=0;i<=xres-1;i++)
					for(j=0;j<=yres-1;j++)
					{
						cc=image.GetPixel(i,j);
						argb[i,j].R=cc.R;
						argb[i,j].G=cc.G;
						argb[i,j].B=cc.B;
							


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
		public static void ShowImg(string str,byte[,] imMatrix)
		{
			int i,j,w,h;
			byte pp;
			Form2 child=new Form2();
			probsolve=child;
			child.Text=str;
			w=imMatrix.GetLength(0);
			h=imMatrix.GetLength(1);
			Graphics g1=child.PictureBox1.CreateGraphics();
			Bitmap box1=new Bitmap(w,h);
			child.Show();
			Color cc=new Color();
			child.CLientSize = new Size(w, h);
			for(i=0;i<w;i++)
				for(j=0;j<h;j++)
				{ 
					pp=imMatrix[i,j];
					cc=Color.FromArgb(pp,pp,pp);
					box1.SetPixel(i,j,cc);

					
				}
			child.PictureBox1.Refresh();
			child.PictureBox1.Image=box1;
			// Application.Run(child);
		
		}

		/// <summary>
		/// show the recongnition result
		/// </summary>
		public static void ShowResult()
		{
			Form1 child=new Form1();
			child.Text="result";
			child.Show();
			string showText="";
			for(int i=0;i<counter;i++)
			{
				switch(result[i])
				{
					case 0: showText+=" zhong ";break;
					case 1: showText+=" feng ";break;
					case 2: showText+=" tian "; break;
					case 3: showText+=" you "; break;
					case 4: showText+=" shen "; break;
					
				}
			
			
			}
			child.textBox1.Text=showText;
			
		
		
		
		}
		/// <summary>
		/// show the result;
		/// </summary>
		/// <param name="num"></param>
		public static void ShowResult(int num)
		{
			Form1 child=new Form1();
			child.Text="result";
			child.Show();
			string showText="";
			for(int i=0;i<num;i++)
			{
				switch(result[i])
				{
					case 0: showText+=" zhong ";break;
					case 1: showText+=" feng ";break;
				    case 2: showText+=" tian "; break;
					case 3: showText+=" you "; break;
					case 4: showText+=" shen "; break;
					
				}
			
			
			}
			child.textBox1.Text=showText;
			
		
		
		
		}
		/// <summary>
		/// show color image
		/// </summary>
		/// <param name="str"></param>
		/// <param name="imMatrix"></param>
		public static void ShowImg(string str,ARGB[,] imMatrix)
		{
			int i,j,w,h;
			int rr,gg,bb;			
			Form2 child=new Form2();
			probsolve=child;
			child.Text=str;
			w=imMatrix.GetLength(0);
			h=imMatrix.GetLength(1);
			Bitmap box1=new Bitmap(w,h);
			child.Show();
			
			
			Color cc=new Color();
			
				
			for(i=0;i<w;i++)
				for(j=0;j<h;j++)
				{ 
					rr=imMatrix[i,j].R;
					gg=imMatrix[i,j].G;
					bb=imMatrix[i,j].B;
					cc=Color.FromArgb(rr,gg,bb);
					box1.SetPixel(i,j,cc);

					
				}
			child.PictureBox1.Refresh();
			child.PictureBox1.Image=box1;
			// Application.Run(child);
		
		}
		public static double Rand()
		{
		 
			return random.NextDouble();
		
		}
		public static byte[,]filter2D(byte [,] f,float[] mask,string name)
		{
			int w= f.GetLength(0);
			int h=f.GetLength(1);

			byte[,]g=new byte[w,h];
			for(int i=0;i<w-1;i++)
				for(int j=0;j<h-1;j++)
				{
					g[i,j]=0;
				}
			
			for(int j=0;j<h;j++)
				for(int i=2;i<w-3;i++)
				{
					float s=0;
					for (int m=-2;m<3;m++)
				
						{
							s+=(f[i+m,j]*mask[m+2]);
					 
						}
					g[i,j]=(byte)Math.Abs(s);

		
		
				}
			ShowImg("test",g);
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
		public static byte[,] filter2D(byte[,]f, int[,] mask, string name)
		{
			int w= f.GetLength(0);
			int h=f.GetLength(1);

			byte[,]g=new byte[w,h];
			for(int i=0;i<w-1;i++)
				for(int j=0;j<h-1;j++)
				{
					g[i,j]=0;
				}
			for(int i=2;i<w-3;i++)
				for(int j=2;j<h-3;j++)
				{
					int s=0;
					for (int m=-2;m<3;m++)
						for(int n=-2;n<3;n++)
						{
							s+=f[i+m,j+n]*mask[m+2,n+2];
					 
						}
					g[i,j]=(byte)Math.Abs(s);
		
		
				}
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
			return g;
		}
			public static byte[,] filter2D(byte[,]f, float[,] mask, string name)
			{
				int w= f.GetLength(0);
				int h=f.GetLength(1);

				byte[,]g=new byte[w,h];
				for(int i=0;i<w-1;i++)
					for(int j=0;j<h-1;j++)
					{
						g[i,j]=0;
					}
				for(int i=2;i<w-3;i++)
					for(int j=2;j<h-3;j++)
					{
						float s=0;
						for (int m=-2;m<3;m++)
							for(int n=-2;n<3;n++)
							{
								s+=f[i+m,j+n]*mask[m+2,n+2];
					 
							}
						g[i,j]=(byte)Math.Abs(s);
		
		
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
		public  static int [] PPED(byte[,] f)
	{
			int w = f.GetLength(0);
			int h = f.GetLength(1);
			int [,] Kv =new int[5,5]{{0,0,0,0,0},{1,1,1,1,1},{0,0,0,0,0},{-1,-1,-1,-1,-1},{0,0,0,0,0}};
			int [,] Kms =new int[5,5]{{0,0,0,1,0},{0,1,1,0,-1},{0,1,0,-1,0},{1,0,-1,-1,0},{0,-1,0,0,0}};
			int [,] Kh =new int[5,5]{{0,1,0,-1,0},{0,1,0,-1,0},{0,1,0,-1,0},{0,1,0,-1,0},{0,1,0,-1,0}};
			int [,] Kps =new int[5,5]{{0,1,0,0,0},{-1,0,1,1,0},{0,-1,0,1,0},{0,-1,-1,0,1},{0,0,0,-1,0}};
			byte [,]Fh= new byte[w,h];
			byte [,]Fps= new byte[w,h];
			byte [,]Fv= new byte[w,h];
			byte [,]Fms= new byte[w,h];
			Fv=filter2D(f,Kv,"Kv");
			Fps=filter2D(f,Kps,"Kps");
			Fh=filter2D(f,Kh,"Kh");
			Fms=filter2D(f,Kms,"Kms");
			int [] Ph=new int[64];
			int [] Pps=new int[127];
			int [] Pv=new int[64];
			int [] Pms=new int[127];
		for(int j=0;j<h;j++)
			{
				Ph[j]=0;
				
				for (int i=0;i<w;i++)
				{
					Ph[j]+=(int)Fh[i,j];
				}
			}
				
		
			for(int i=0;i<w;i++)
			{
				Pv[i]=0;
					for (int j=0;j<h;j++)
				
				{
					Pv[i]+=(int)Fv[i,j];
				}
			}
			
			float sum=0,sum1=0;
			
			for(int i=0;i<64;i++)
			{
				Pv[i]=(int)((float)Pv[i]*10);
				Ph[i]=(int)((float)Ph[i]*10);
			}
        //   ShowHisto(Pv,"vertical");
        //   ShowHisto(Ph,"horizental");
			int[] PPEdh=new int[16];
			for (int j=0;j<16;j++)
			{
				PPEdh[j]=0;
				for (int r=0;r<4;r++)
				{
				
					PPEdh[j]+=Ph[4*j+r];
				}
			}

			

			
			int[] PPEdv=new int[16];
			for (int j=0;j<16;j++)
			{
				PPEdv[j]=0;
				for (int r=0;r<4;r++)
				{
				
					PPEdv[j]+=Ph[4*j+r];
				}
			}
		

			for (int i=0;i<w;i++)
			{
				for(int j=0;j<h;j++)
				{
					if (Fps[i,j]!=0) Pps[i+j]++;
				}
			}
			
			
			for (int i=0;i<w;i++)
			{
				for(int j=0;j<h;j++)
				{
					if (Fms[i,j]!=0) Pms[i+j]++;
				}
			}
			 
			sum=0;sum1=0;

			for(int i=0;i<127;i++)
			{
				sum+=Pps[i];
				sum1+=Pms[i];
			}			
			for(int i=0;i<127;i++)
			{
				Pps[i]=(int)((float)Pps[i]*10);
				Pms[i]=(int)((float)Pms[i]*10);
			
			}
			int[]PPEdps=new int[16];
			int[]PPEdms=new int[16];
			int[]PPEdpsm=new int[64];
			int[]PPEdmsm=new int[64];
			for (int m=1;m<63;m++)
			{
				
				PPEdpsm[m]=Pps[2*m-1]/2+Pps[2*m]+Pps[2*m+1]/2;
			}
			PPEdpsm[0]=Pps[0]/2+Pps[0]+Pps[1]/2;
			PPEdpsm[63]=Pps[2*63-1]/2+Pps[2*63]+Pps[2*63]/2;

			for (int m=1;m<63;m++)
			{
				PPEdmsm[m]=Pps[2*m-1]/2+Pps[2*m]+Pps[2*m+1]/2;
			}
			PPEdmsm[0]=Pms[0]/2+Pms[0]+Pms[1]/2;
			PPEdmsm[63]=Pms[2*63-1]/2+Pms[2*63]+Pms[2*63]/2;
			for(int j=0; j<16;j++)
			{
				PPEdps[j]=0;
				for (int r=0;r<4;r++)
				{
				
					PPEdps[j]+=PPEdpsm[4*j+r];
				}
			}
			for(int j=0; j<16;j++)
			{
				PPEdms[j]=0;
				for (int r=0;r<4;r++)
				{
				
					PPEdms[j]+=PPEdmsm[4*j+r];
				}
			}
			int [] PPEDm=new int[64];
			for (int n=0;n<64;n++)
			{
				if (n<16) PPEDm[n]=PPEdh[n];
				else if (n>15 && n<32) PPEDm[n]=PPEdps[n-16];
				else if (n>31 && n<48) PPEDm[n]=PPEdv[n-32];
				else 
					PPEDm[n]=PPEdms[n-48];
			
			}
			int [] PPED=new int[64];
			for(int n=1;n<63;n++)
			{
				PPED[n]=PPEDm[n-1]/4+PPEDm[n]/2+PPEDm[n+1]/4;

			}
			PPED[0]=PPEDm[0]/4+PPEDm[0]/2+PPEDm[1]/4;
			PPED[63]=PPEDm[62]/4+PPEDm[63]/2+PPEDm[63]/4;
			
			/*float factor=0;
			for (int i=0;i<64;i++)
			{
				factor+=PPED[i];
			}
			double mid;
			for(int i=0;i<64;i++)
			{
				mid =(double)PPED[i]/(double)factor*1000d;
				PPED[i]=(int )mid;
			}*/
			
			return PPED;
					  
		}

		public static int[] PPEDMeans(int [][] PPED, int n)
		{
			int sum;
			int[]PPEDmean=new int[64];
			for (int i=0;i<64;i++)
				PPEDmean[i]=0;
			for (int i=0;i<64;i++)
			{	sum=0;
				for (int j=0;j<n;j++)
				{
					sum+=PPED[j][i];
				
				}
				PPEDmean[i]=sum/n;
			
			
			}
			return PPEDmean;
		
		}
		/// <summary>
		/// show Histo of certain vector
		/// </summary>
		/// <param name="histo"></param>
		/// <param name="name"></param>
		public static void ShowHisto(int[]histo, string name)
		{
			int w = histo.GetLength(0);
			byte [,] hImg = new byte[w*9,300];
			for (int x=0;x<w*9;x++)
				for (int y=0;y<300;y++)
					hImg[x,y]=255;
			for (int x=0;x<w;x++)
				for (int y=0;y<300;y++)
				{
					if (y<histo[x]) hImg[x*9,299-y] = 0;
					else hImg[x*9,299-y] = 255;
				}

			ShowImg(name,hImg);
		
		
		}

		/// <summary>
		/// Generate Temper
		/// </summary>
		/// <param name="Filename"></param>
		/// <param name="PicNum"></param>
		
		public static void TempGenerate(string Filename, int PicNum)
		{
			int [][] pped=new int[10][];
			for(int i=0;i<PicNum;i++)
			{
				byte [,]f = LoadImg(Filename+i.ToString());
				if (f==null) return;
				//ShowImg("PPED"+(i),f);
			
				pped[i]= PPED(f);
				//ShowHisto(pped[i],"PPED"+i);

			}
			
			int [][]PPEDmean=new int[100][];
			PPEDmean[0]=PPEDMeans(pped,PicNum);
			ShowHisto(PPEDmean[0],"mean "+Filename);
			CreateFile("PPEDdata.dat",PPEDmean[0]);
		
		}
		/// <summary>
		///METHOD FOR COMPARING
		/// </summary>
		/// <param name="PPED"></param>
		/// <returns></returns>

		public static int CompVector(int[]PPED)
		{   int[] distance=new int[PPEDTemplate.Length];
			int min=0,minrank=0;

			for(int j=0;j<PPEDTemplate.Length;j++)
			{
				distance[j]=0;
				for(int i=0;i<64;i++)
				{
					distance[j]+=Math.Abs(PPEDTemplate[j][i]-PPED[i]);
				}
			}
			min=distance[0];
			for(int i=0;i<PPEDTemplate.Length;i++)
			{
				if (distance[i]<min) {min=distance[i];minrank=i;}
			}
			return minrank;
			


		
		}
		/// <summary>
		/// converlution method.
		/// 
		/// </summary>
		public static int[] conjj(int[] f, int[]g)
		{
			int i,j,jjsize;
			int lenf=f.GetLength(0);
			int leng=g.GetLength(0);
		   int[] jj=new int[lenf+leng-1];
			for(i=0;i<lenf+leng-1;i++)
				jj[i]=0;
			for(i=0;i<leng;i++)
				for(jjsize=i,j=0;j<lenf;j++)
					jj[jjsize++]+=f[j]*g[i];
		return jj;
		
		}
		public static float[] conjj(float[] f, float[]g)
		{
			int i,j,jjsize;
			int lenf=f.GetLength(0);
			int leng=g.GetLength(0);
			float[] jj=new float[lenf+leng-1];
			for(i=0;i<lenf+leng-1;i++)
				jj[i]=0;
			for(i=0;i<leng;i++)
				for(jjsize=i,j=0;j<lenf;j++)
					jj[jjsize++]+=f[j]*g[i];
			return jj;
		
		}
		public static void Main()
		{  
			counter=0;
			      
			//////////TempGenerate("shen",1);
			/*	/// The following part is use to generate templators
				   int templateNum=6;
			   TempGenerate( "zhong",templateNum);
			   TempGenerate("feng",templateNum);
			   TempGenerate("tian",templateNum);
			   TempGenerate("you",templateNum);
			   TempGenerate("shen",templateNum);



 	 
		
			PPEDTemplate=OutputFile("PPEDdata.dat");   */         
			// Load templator to the matrix
		
			
           	
 		
			/*			
	   /// test section
				   int testNum=8;
				   for( int q=0;q<testNum;q++)
				   {
				   int[]PPEDtest=PPED(LoadImg("test"+q.ToString())); 
					   result[q]=CompVector(PPEDtest);
				   }
				   ShowResult(testNum);

			   int[]PPEDtest=PPED(LoadImg("test"+4.ToString())); 
				   ShowImg("test",LoadImg("test"+4.ToString()));
				   ShowHisto(PPEDTemplate[4],"shen");
				   ShowHisto(PPEDTemplate[3],"you");
				   ShowHisto(PPEDtest,"testyou");
				   result[0]=CompVector(PPEDtest);
				   ShowResult(1);
				   /// The following part is use to generate templators
					   int templateNum=6;
					 TempGenerate( "zhong",templateNum);
				   TempGenerate("feng",templateNum);
				   TempGenerate("tian",templateNum);
				   TempGenerate("you",templateNum);
				   TempGenerate("shen",templateNum);
			
				   ShowHisto(PPEDTemplate[0],"zhong");
				   ShowHisto(PPEDTemplate[2],"tian");
				   ShowHisto(PPEDTemplate[4],"shen");
				   int[] PPEDtest=PPED(LoadImg("test"+7.ToString()));
				 ShowHisto(PPEDtest,"testZhong"); 
				   result[0]=CompVector(PPEDtest);
		
				  ShowResult(1);
	
		 	 
	
				   /// Yoko detection	int [,] Kv =new int[5,5]{{0,0,0,0,0},{1,1,1,1,1},{0,0,0,0,0},{-1,-1,-1,-1,-1},{0,0,0,0,0}};
				   byte[,] g=filter2D(pic1,Kv,"Kv");
			
			   */
      		
			byte [,]pic1=LoadImg("testyoko");
			
		     
			
			float [] mask=new float[5]{.25f,1f,0f,-1f,-.25f};
			byte[,]g=filter2D(pic1,mask,"filtertest");
			
			
			//ShowImg("filter",g);
			int h=pic1.GetLength(1);
			int w=pic1.GetLength(0);
			int[] verDis=new int[h];
			
			
 
		  
			
			
			int sum;
			byte[,] t=new byte[w,h];
			for (int i=0;i<w;i++)
			{
				
				for(int j=0;j<h;j++)
				{
					t[i,j]=Convert.ToByte(g[i,j]);
				}
			}
			ShowImg("t's pic",t);
		
			for (int j=0;j<h;j++)
			{
				sum=0;
				for(int i=0;i<w;i++)
				{
					sum+=g[i,j];
				}
				verDis[j]=sum;

			}
			
			const int windowsize=7;
			
			
			ShowHisto(verDis,"test1");
			int [] rect=new int[windowsize];
			for (int i=0;i<windowsize;i++)
				rect[i]=1;
			int[]result =conjj(verDis,rect);
          
			ShowHisto(result,"conjj");	
			int[]peak=new int[100];
			int con=0;int max=20;int max_num;
			int ii=1;
			bool sw=false;
			while(ii<result.GetLength(0))
			{
				if (result[ii]>20&&result[ii-1]<20)
				{
					sw=true;
				   
				    
				}
				if (result[ii]<20)
				{
					sw=false;
					ii++;
				   
				    
				}
				while(sw==true)
				{
					if(result[ii]>max)
					{
						max=result[ii];
						peak[con]=ii+2;
						
					}
					if (result[ii]<20)
					{
						sw=false;
						con++;
						max=20;
						
				   
				    
					}
					ii++;
				}

				
					
				
			}
			max_num=con;

			/*for(int i=0;i<max_num;i++)
			{
				for(int j=0;j<w;j++)
				{
					
					pic1[j,peak[i]]=0;
					pic1[j,peak[i]-32]=0;
					pic1[j,peak[i]+32]=0;
				}
			}
			*/
			ShowImg("cen",pic1);
            int[][] horDis;
			int[][] result_hori;
			float [,] mask1=new float[5,5] {{0,0,0.25f,0,0},{0,0,1,0,0},{0.25f,1f,0,-1f,-0.25f},{0,0,-1f,0,0},{0,0,-.25f,0,0}};
			byte[,]g2=filter2D(pic1,mask1,"filtertest_2");
            ShowImg("g2's pic",g2);
		
			
			

	    
			
		
		    

		
			

						Application.Run(probsolve);
		
		
		}

	}
	
		
}