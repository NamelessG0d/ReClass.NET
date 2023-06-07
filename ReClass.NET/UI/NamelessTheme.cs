using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ReClassNET;
using ReClassNET.Forms;

using static NamelessTheme;

namespace ReClassNET.Forms
{
	//In a effort to make this solution easily updatable,
	//I'm abusing the partial attribute of MainForm to easily access its private variables
	public partial class MainForm
	{
		public void SwitchTheme()
		{
			if (!is_dark)
			{
				copy = Program.Settings.Clone();

				Program.Settings.BackgroundColor = BackgrounColor;
				Program.Settings.HexColor = Color.White;
				Program.Settings.NameColor = Color.LightSteelBlue;
				Program.Settings.SelectedColor = ForegroundColor;

				this.ForeColor = Color.White;
				this.BackColor = BackgrounColor;

				this.statusStrip.BackColor = BackgrounColor;

				this.toolStrip.BackColor = BackgrounColor;
				//Coming from C++, this is making me cringe, so lets cringe together
				if (this.toolStrip.Renderer is not ToolStripCustomRenderer)
					this.toolStrip.Renderer = new ToolStripCustomRenderer();
				//open, save, new class, ect...
				foreach (var item in this.toolStrip.Items.OfType<ToolStripDropDownButton>())
				{
					item.ForeColor = Color.White;
					item.BackColor = BackgrounColor;

					foreach (var sub_item in item.DropDownItems.OfType<ToolStripMenuItem>())
					{
						sub_item.ForeColor = Color.White;
						sub_item.BackColor = BackgrounColor;
					}
				}

				this.mainMenuStrip.BackColor = BackgrounColor;
				//This is painful...
				if (this.mainMenuStrip.Renderer is not ToolStripCustomRenderer)
					this.mainMenuStrip.Renderer = new ToolStripCustomRenderer();

				//File, Process, ect...
				foreach (var item in this.mainMenuStrip.Items.OfType<ToolStripMenuItem>())
				{
					item.ForeColor = Color.White;
					item.BackColor = BackgrounColor;

					//change sub items like Attach to Process...
					foreach (var sub_item in item.DropDownItems.OfType<ToolStripItem>())
					{
						sub_item.ForeColor = Color.White;
						sub_item.BackColor = BackgrounColor;
					}
				}

				//Classes, Enums, ect...
				foreach (var item in this.projectView.Controls.OfType<Control>())
				{
					item.ForeColor = Color.White;
					item.BackColor = BackgrounColor;
				}

				this.selectedNodeContextMenuStrip.BackColor = BackgrounColor;
				//Last time promise...
				if (this.selectedNodeContextMenuStrip.Renderer is not ToolStripCustomRenderer)
					this.selectedNodeContextMenuStrip.Renderer = new ToolStripCustomRenderer();
				//Right click memory viewer
				foreach (var item in this.selectedNodeContextMenuStrip.Items.OfType<ToolStripMenuItem>())
				{
					item.ForeColor = Color.White;
					item.BackColor = BackgrounColor;

					//change sub items like Attach to Process...
					foreach (var sub_item in item.DropDownItems.OfType<ToolStripItem>())
					{
						sub_item.ForeColor = Color.White;
						sub_item.BackColor = BackgrounColor;
					}
				}

				#region Icon resizing
				this.toolStrip.AutoSize = false;
				this.toolStrip.Height = (int)(this.toolStrip.Height * 1.5);

				//Retarded hack, if button not rendered, size all fucked up
				var current_size = Size.Width;
				this.Size = new Size(2314, Size.Height);

				#region Snippet if you wanted to resize properly the image but no-need...
				//var icon_width = 0;
				//var icon_height = 0;
				//foreach (ToolStripItem item in this.toolStrip.Items.OfType<ToolStripButton>())
				//{
				//	item.AutoSize = false;
				//	item.Width = 50;

				//	icon_width = item.Image.Width;
				//	icon_height = item.Image.Height;

				//	//resize the image of the button to the new size
				//	Bitmap b = new Bitmap((int)(icon_width * 1.5), (int)(icon_height * 1.5));
				//	using (Graphics g = Graphics.FromImage((Image)b))
				//	{
				//		g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				//		g.DrawImage(item.Image, 0, 0, b.Width, b.Height);
				//	}
				//	Image myResizedImg = (Image)b;

				//	//put the resized image back to the button
				//	item.Image = myResizedImg;
				//}
				#endregion

				//change toolstrip's ImageScalingSize property 
				this.toolStrip.ImageScalingSize
					= new Size(
						(int)(this.toolStrip.ImageScalingSize.Width * 1.5),
						(int)(this.toolStrip.ImageScalingSize.Height * 1.5)
					);

				//Reversing of the retarded hack
				this.Size = new Size(current_size, Size.Height);
				#endregion

				is_dark = true;
			}
			else
			{
				//Some sweat reflection to not change the privacy of Program.set_Settings()
				Type type = typeof(Program);
				PropertyInfo propertyInfo = type.GetProperty("Settings", BindingFlags.Static | BindingFlags.Public);
				propertyInfo.SetValue(null, NamelessTheme.copy.Clone());


				this.BackColor = DefaultBackColor;
				this.statusStrip.BackColor = StatusStrip.DefaultBackColor;

				this.toolStrip.BackColor = ToolStrip.DefaultBackColor;
				if (this.toolStrip.Renderer is ToolStripCustomRenderer)
					this.toolStrip.Renderer = new ToolStripProfessionalRenderer();
				//open, save, new class, ect...
				foreach (var item in this.toolStrip.Items.OfType<ToolStripDropDownButton>())
				{
					item.ForeColor = Color.Black;
					item.BackColor = Control.DefaultBackColor;
					foreach (var sub_item in item.DropDownItems.OfType<ToolStripMenuItem>())
					{
						sub_item.ForeColor = Color.Black;
						sub_item.BackColor = Control.DefaultBackColor;
					}
				}

				this.mainMenuStrip.BackColor = MenuStrip.DefaultBackColor;
				if (this.mainMenuStrip.Renderer is ToolStripCustomRenderer)
					this.mainMenuStrip.Renderer = new ToolStripProfessionalRenderer();
				//File, Process, ect...
				foreach (var item in this.mainMenuStrip.Items.OfType<ToolStripMenuItem>())
				{
					item.ForeColor = Color.Black;
					item.BackColor = Control.DefaultBackColor;
					foreach (var sub_item in item.DropDownItems.OfType<ToolStripItem>())
					{
						sub_item.ForeColor = Color.Black;
						sub_item.BackColor = Control.DefaultBackColor;
					}
				}

				//Classes, Enums, ect...
				foreach (var item in this.projectView.Controls.OfType<Control>())
				{
					item.ForeColor = Color.Black;
					item.BackColor = Control.DefaultBackColor;
				}

				if (this.selectedNodeContextMenuStrip.Renderer is ToolStripCustomRenderer)
					this.selectedNodeContextMenuStrip.Renderer = new ToolStripProfessionalRenderer();
				//Right click memory viewer
				foreach (var item in this.selectedNodeContextMenuStrip.Items.OfType<ToolStripMenuItem>())
				{
					item.ForeColor = Color.Black;
					item.BackColor = Control.DefaultBackColor;

					//change sub items like Attach to Process...
					foreach (var sub_item in item.DropDownItems.OfType<ToolStripItem>())
					{
						sub_item.ForeColor = Color.Black;
						sub_item.BackColor = Control.DefaultBackColor;
					}
				}

				this.toolStrip.ImageScalingSize = new Size((int)(this.toolStrip.ImageScalingSize.Width / 1.5), (int)(this.toolStrip.ImageScalingSize.Height / 1.5));
				this.toolStrip.Height = (int)(this.toolStrip.Height / 1.5);
				this.toolStrip.AutoSize = false;

				NamelessTheme.is_dark = false;
			}
		}

		internal void MainForm_Closing(object sender, FormClosingEventArgs e)
		{
			Program.Settings.CustomData["dark_theme"] = is_dark.ToString();

			if (NamelessTheme.is_dark)
				SwitchTheme();
		}
	}
}

//Making this class static for simplicity sake
//Feel free to create a pull request if you want to bother converting it to a proper singleton
internal class NamelessTheme
{
	internal static Color BackgrounColor = Color.FromArgb(24, 24, 24);
	internal static Color SelectedColor = Color.FromArgb(40, 40, 40);
	internal static Color ForegroundColor = Color.FromArgb(64, 64, 64);
	internal static bool is_init { get; private set; } = false;
	internal static bool is_dark { get; set; } = false;
	internal static Settings copy;

	public static bool Init()
	{
#if !DEBUG
		try
		{
#endif
			if (!is_init)
			{
				Program.MainForm.FormClosing += Program.MainForm.MainForm_Closing;

				var saved = Program.Settings.CustomData["dark_theme"];

				if(!String.IsNullOrEmpty(saved) && Convert.ToBoolean(saved))
					Program.MainForm.SwitchTheme();

				is_init = true;
			}
#if !DEBUG
		}
		catch { }
#endif

		return is_init;
	}

	//https://stackoverflow.com/a/15926450/12298228
	internal class ToolStripCustomRenderer : ToolStripProfessionalRenderer
	{
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			if ((e.Item as ToolStripSeparator) == null)
			{
				base.OnRenderSeparator(e);
				return;
			}

			// Get the separator's width and height.
			ToolStripSeparator toolStripSeparator = (ToolStripSeparator)e.Item;
			int width = toolStripSeparator.Width;
			int height = toolStripSeparator.Height;

			// Choose the colors for drawing.
			// I've used Color.White as the foreColor.
			Color foreColor = toolStripSeparator.ForeColor;
			// Color.Teal as the backColor.
			Color backColor = toolStripSeparator.BackColor;

			// Fill the background.
			e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, width, height);

			// Draw the line.
			e.Graphics.DrawLine(new Pen(foreColor), 4, height / 2, width - 4, height / 2);

		}

		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			var brush = new SolidBrush(BackgrounColor);

			// Customize the background color when the item is selected, pressed
			if (e.Item.Selected || e.Item.Pressed)
				brush = new SolidBrush(SelectedColor);

			e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
		}
	}
}

