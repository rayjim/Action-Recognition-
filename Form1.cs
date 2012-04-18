using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace myImgp
{
	/// <summary>
	/// Form1 µÄÕªÒªËµÃ÷¡£
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		public System.Windows.Forms.TextBox textBox1;
		/// <summary>
		/// ±ØĞèµÄÉè¼ÆÆ÷±äÁ¿¡£
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Windows ´°ÌåÉè¼ÆÆ÷Ö§³ÖËù±ØĞèµÄ
			//
			InitializeComponent();

			//
			// TODO: ÔÚ InitializeComponent µ÷ÓÃºóÌúØÓÈÎºÎ¹¹ÔE¯Êı´úÂE
			//
		}

		/// <summary>
		/// ÇåÀúçùÓĞÕıÔÚÊ¹ÓÃµÄ×ÊÔ´¡£
		/// </summary>
		/*protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}*/

		#region Windows ´°ÌåÉè¼ÆÆ÷Éú³ÉµÄ´úÂE
		/// <summary>
		/// Éè¼ÆÆ÷Ö§³ÖËùĞèµÄ·½·¨ - ²»ÒªÊ¹ÓÃ´úÂEà¼­Æ÷ĞŞ¸Ä
		/// ´Ë·½·¨µÄÄÚÈİ¡£
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.textBox1.Location = new System.Drawing.Point(8, 8);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(204, 21);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "textBox1";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(219, 198);
			this.Controls.Add(this.textBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void Form1_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
