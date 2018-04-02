using IClipboard.Properties;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaterialSkin
{
	public class MaterialSkinManager
	{
		public enum Themes : byte
		{
			LIGHT,
			DARK
		}

		private static MaterialSkinManager _instance;

		private readonly List<MaterialForm> _formsToManage = new List<MaterialForm>();

		private Themes _theme;

		private Color _PrimaryColor;

		private ColorScheme _colorScheme;

		private static readonly Color PRIMARY_TEXT_BLACK = Color.FromArgb(222, 0, 0, 0);

		private static readonly Brush PRIMARY_TEXT_BLACK_BRUSH = new SolidBrush(MaterialSkinManager.PRIMARY_TEXT_BLACK);

		public static Color SECONDARY_TEXT_BLACK = Color.FromArgb(138, 0, 0, 0);

		public static Brush SECONDARY_TEXT_BLACK_BRUSH = new SolidBrush(MaterialSkinManager.SECONDARY_TEXT_BLACK);

		private static readonly Color DISABLED_OR_HINT_TEXT_BLACK = Color.FromArgb(66, 0, 0, 0);

		private static readonly Brush DISABLED_OR_HINT_TEXT_BLACK_BRUSH = new SolidBrush(MaterialSkinManager.DISABLED_OR_HINT_TEXT_BLACK);

		private static readonly Color DIVIDERS_BLACK = Color.FromArgb(31, 0, 0, 0);

		private static readonly Brush DIVIDERS_BLACK_BRUSH = new SolidBrush(MaterialSkinManager.DIVIDERS_BLACK);

		private static readonly Color PRIMARY_TEXT_WHITE = Color.FromArgb(255, 255, 255, 255);

		private static readonly Brush PRIMARY_TEXT_WHITE_BRUSH = new SolidBrush(MaterialSkinManager.PRIMARY_TEXT_WHITE);

		public static Color SECONDARY_TEXT_WHITE = Color.FromArgb(179, 255, 255, 255);

		public static Brush SECONDARY_TEXT_WHITE_BRUSH = new SolidBrush(MaterialSkinManager.SECONDARY_TEXT_WHITE);

		private static readonly Color DISABLED_OR_HINT_TEXT_WHITE = Color.FromArgb(77, 255, 255, 255);

		private static readonly Brush DISABLED_OR_HINT_TEXT_WHITE_BRUSH = new SolidBrush(MaterialSkinManager.DISABLED_OR_HINT_TEXT_WHITE);

		private static readonly Color DIVIDERS_WHITE = Color.FromArgb(31, 255, 255, 255);

		private static readonly Brush DIVIDERS_WHITE_BRUSH = new SolidBrush(MaterialSkinManager.DIVIDERS_WHITE);

		private static readonly Color CHECKBOX_OFF_LIGHT = Color.FromArgb(138, 0, 0, 0);

		private static readonly Brush CHECKBOX_OFF_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.CHECKBOX_OFF_LIGHT);

		private static readonly Color CHECKBOX_OFF_DISABLED_LIGHT = Color.FromArgb(66, 0, 0, 0);

		private static readonly Brush CHECKBOX_OFF_DISABLED_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.CHECKBOX_OFF_DISABLED_LIGHT);

		private static readonly Color CHECKBOX_OFF_DARK = Color.FromArgb(179, 255, 255, 255);

		private static readonly Brush CHECKBOX_OFF_DARK_BRUSH = new SolidBrush(MaterialSkinManager.CHECKBOX_OFF_DARK);

		private static readonly Color CHECKBOX_OFF_DISABLED_DARK = Color.FromArgb(77, 255, 255, 255);

		private static readonly Brush CHECKBOX_OFF_DISABLED_DARK_BRUSH = new SolidBrush(MaterialSkinManager.CHECKBOX_OFF_DISABLED_DARK);

		private static readonly Color RAISED_BUTTON_BACKGROUND = Color.FromArgb(255, 255, 255, 255);

		private static readonly Brush RAISED_BUTTON_BACKGROUND_BRUSH = new SolidBrush(MaterialSkinManager.RAISED_BUTTON_BACKGROUND);

		private static readonly Color RAISED_BUTTON_TEXT_LIGHT = MaterialSkinManager.PRIMARY_TEXT_WHITE;

		private static readonly Brush RAISED_BUTTON_TEXT_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.RAISED_BUTTON_TEXT_LIGHT);

		private static readonly Color RAISED_BUTTON_TEXT_DARK = MaterialSkinManager.PRIMARY_TEXT_BLACK;

		private static readonly Brush RAISED_BUTTON_TEXT_DARK_BRUSH = new SolidBrush(MaterialSkinManager.RAISED_BUTTON_TEXT_DARK);

		private static readonly Color FLAT_BUTTON_BACKGROUND_HOVER_LIGHT = Color.FromArgb(20.PercentageToColorComponent(), 10066329.ToColor());

		private static readonly Brush FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_LIGHT);

		private static readonly Color FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT = Color.FromArgb(40.PercentageToColorComponent(), 10066329.ToColor());

		private static readonly Brush FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT);

		private static readonly Color FLAT_BUTTON_DISABLEDTEXT_LIGHT = Color.FromArgb(26.PercentageToColorComponent(), 0.ToColor());

		private static readonly Brush FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_LIGHT);

		private static readonly Color FLAT_BUTTON_BACKGROUND_HOVER_DARK = Color.FromArgb(15.PercentageToColorComponent(), 13421772.ToColor());

		private static readonly Brush FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_DARK);

		private static readonly Color FLAT_BUTTON_BACKGROUND_PRESSED_DARK = Color.FromArgb(25.PercentageToColorComponent(), 13421772.ToColor());

		private static readonly Brush FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_DARK);

		private static readonly Color FLAT_BUTTON_DISABLEDTEXT_DARK = Color.FromArgb(30.PercentageToColorComponent(), 16777215.ToColor());

		private static readonly Brush FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_DARK);

		private static readonly Color CMS_BACKGROUND_LIGHT_HOVER = Color.FromArgb(255, 238, 238, 238);

		private static readonly Brush CMS_BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.CMS_BACKGROUND_LIGHT_HOVER);

		private static readonly Color CMS_BACKGROUND_DARK_HOVER = Color.FromArgb(38, 204, 204, 204);

		private static readonly Brush CMS_BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(MaterialSkinManager.CMS_BACKGROUND_DARK_HOVER);

		private static readonly Color BACKGROUND_LIGHT = Color.FromArgb(255, 255, 255, 255);

		private static Brush BACKGROUND_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.BACKGROUND_LIGHT);

		private static readonly Color BACKGROUND_DARK = Color.FromArgb(255, 51, 51, 51);

		private static Brush BACKGROUND_DARK_BRUSH = new SolidBrush(MaterialSkinManager.BACKGROUND_DARK);

		public readonly Color ACTION_BAR_TEXT = Color.FromArgb(255, 255, 255, 255);

		public readonly Brush ACTION_BAR_TEXT_BRUSH = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

		public readonly Color ACTION_BAR_TEXT_SECONDARY = Color.FromArgb(153, 255, 255, 255);

		public readonly Brush ACTION_BAR_TEXT_SECONDARY_BRUSH = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

		public Font ROBOTO_MEDIUM_12;

		public Font ROBOTO_REGULAR_11;

		public Font ROBOTO_MEDIUM_11;

		public Font ROBOTO_MEDIUM_10;

		public int FORM_PADDING = 14;

		private readonly PrivateFontCollection privateFontCollection = new PrivateFontCollection();

		public Themes Theme
		{
			get
			{
				return this._theme;
			}
			set
			{
				this._theme = value;
				this.UpdateBackgrounds();
			}
		}

		public Color PrimaryColor
		{
			get
			{
				return this._PrimaryColor;
			}
			set
			{
				this._PrimaryColor = value;
			}
		}

		public ColorScheme ColorScheme
		{
			get
			{
				return this._colorScheme;
			}
			set
			{
				this._colorScheme = value;
				this.UpdateBackgrounds();
			}
		}

		public static MaterialSkinManager Instance
		{
			get
			{
				return MaterialSkinManager._instance ?? (MaterialSkinManager._instance = new MaterialSkinManager());
			}
		}

		public Color GetPrimaryTextColor()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.PRIMARY_TEXT_BLACK : MaterialSkinManager.PRIMARY_TEXT_WHITE;
		}

		public Brush GetPrimaryTextBrush()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.PRIMARY_TEXT_BLACK_BRUSH : MaterialSkinManager.PRIMARY_TEXT_WHITE_BRUSH;
		}

		public Color GetSecondaryTextColor()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.SECONDARY_TEXT_BLACK : MaterialSkinManager.SECONDARY_TEXT_WHITE;
		}

		public Brush GetSecondaryTextBrush()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.SECONDARY_TEXT_BLACK_BRUSH : MaterialSkinManager.SECONDARY_TEXT_WHITE_BRUSH;
		}

		public Color GetDisabledOrHintColor()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.DISABLED_OR_HINT_TEXT_BLACK : MaterialSkinManager.DISABLED_OR_HINT_TEXT_WHITE;
		}

		public Brush GetDisabledOrHintBrush()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.DISABLED_OR_HINT_TEXT_BLACK_BRUSH : MaterialSkinManager.DISABLED_OR_HINT_TEXT_WHITE_BRUSH;
		}

		public Color GetDividersColor()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.DIVIDERS_BLACK : MaterialSkinManager.DIVIDERS_WHITE;
		}

		public Brush GetDividersBrush()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.DIVIDERS_BLACK_BRUSH : MaterialSkinManager.DIVIDERS_WHITE_BRUSH;
		}

		public Color GetCheckboxOffColor()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.CHECKBOX_OFF_LIGHT : MaterialSkinManager.CHECKBOX_OFF_DARK;
		}

		public Brush GetCheckboxOffBrush()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.CHECKBOX_OFF_LIGHT_BRUSH : MaterialSkinManager.CHECKBOX_OFF_DARK_BRUSH;
		}

		public Color GetCheckBoxOffDisabledColor()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.CHECKBOX_OFF_DISABLED_LIGHT : MaterialSkinManager.CHECKBOX_OFF_DISABLED_DARK;
		}

		public Brush GetCheckBoxOffDisabledBrush()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.CHECKBOX_OFF_DISABLED_LIGHT_BRUSH : MaterialSkinManager.CHECKBOX_OFF_DISABLED_DARK_BRUSH;
		}

		public Brush GetRaisedButtonBackgroundBrush()
		{
			return MaterialSkinManager.RAISED_BUTTON_BACKGROUND_BRUSH;
		}

		public Brush GetRaisedButtonTextBrush(bool primary)
		{
			return primary ? MaterialSkinManager.RAISED_BUTTON_TEXT_LIGHT_BRUSH : MaterialSkinManager.RAISED_BUTTON_TEXT_DARK_BRUSH;
		}

		public Color GetFlatButtonHoverBackgroundColor()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_LIGHT : MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_DARK;
		}

		public Brush GetFlatButtonHoverBackgroundBrush()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH : MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH;
		}

		public Color GetFlatButtonPressedBackgroundColor()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT : MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_DARK;
		}

		public Brush GetFlatButtonPressedBackgroundBrush()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH : MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH;
		}

		public Brush GetFlatButtonDisabledTextBrush()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH : MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH;
		}

		public Brush GetCmsSelectedItemBrush()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.CMS_BACKGROUND_HOVER_LIGHT_BRUSH : MaterialSkinManager.CMS_BACKGROUND_HOVER_DARK_BRUSH;
		}

		public Color GetApplicationBackgroundColor()
		{
			return (this.Theme == Themes.LIGHT) ? MaterialSkinManager.BACKGROUND_LIGHT : MaterialSkinManager.BACKGROUND_DARK;
		}

		[DllImport("gdi32.dll")]
		private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pvd, [In] ref uint pcFonts);

		private MaterialSkinManager()
		{
			this.ROBOTO_MEDIUM_12 = new Font(this.LoadFont(Resources.Roboto_Medium), 12f);
			this.ROBOTO_MEDIUM_10 = new Font(this.LoadFont(Resources.Roboto_Medium), 10f);
			this.ROBOTO_REGULAR_11 = new Font(this.LoadFont(Resources.Roboto_Regular), 11f);
			this.ROBOTO_MEDIUM_11 = new Font(this.LoadFont(Resources.Roboto_Medium), 11f);
			this.Theme = Themes.LIGHT;
			this.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
		}

		public void AddFormToManage(MaterialForm materialForm)
		{
			this._formsToManage.Add(materialForm);
			this.UpdateBackgrounds();
		}

		public void RemoveFormToManage(MaterialForm materialForm)
		{
			this._formsToManage.Remove(materialForm);
		}

		private FontFamily LoadFont(byte[] fontResource)
		{
			int num = fontResource.Length;
			IntPtr intPtr = Marshal.AllocCoTaskMem(num);
			Marshal.Copy(fontResource, 0, intPtr, num);
			uint num2 = 0u;
			MaterialSkinManager.AddFontMemResourceEx(intPtr, (uint)fontResource.Length, IntPtr.Zero, ref num2);
			this.privateFontCollection.AddMemoryFont(intPtr, num);
			return this.privateFontCollection.Families.Last();
		}

		private void UpdateBackgrounds()
		{
			Color applicationBackgroundColor = this.GetApplicationBackgroundColor();
			foreach (MaterialForm item in this._formsToManage)
			{
				item.BackColor = applicationBackgroundColor;
				this.UpdateControl(item, applicationBackgroundColor);
			}
		}

		private void UpdateToolStrip(ToolStrip toolStrip, Color newBackColor)
		{
			if (toolStrip != null)
			{
				toolStrip.BackColor = newBackColor;
				foreach (ToolStripItem item in toolStrip.Items)
				{
					item.BackColor = newBackColor;
					if (item is MaterialToolStripMenuItem && (item as MaterialToolStripMenuItem).HasDropDownItems)
					{
						this.UpdateToolStrip((item as MaterialToolStripMenuItem).DropDown, newBackColor);
					}
				}
			}
		}

		private void UpdateControl(Control controlToUpdate, Color newBackColor)
		{
			if (controlToUpdate != null)
			{
				if (controlToUpdate.ContextMenuStrip != null)
				{
					this.UpdateToolStrip(controlToUpdate.ContextMenuStrip, newBackColor);
				}
				MaterialTabControl materialTabControl = controlToUpdate as MaterialTabControl;
				if (materialTabControl != null)
				{
					foreach (TabPage tabPage in materialTabControl.TabPages)
					{
						tabPage.BackColor = newBackColor;
					}
				}
				if (controlToUpdate is MaterialDivider)
				{
					controlToUpdate.BackColor = this.GetDividersColor();
				}
				if (controlToUpdate is MaterialListView)
				{
					controlToUpdate.BackColor = newBackColor;
				}
				foreach (Control control in controlToUpdate.Controls)
				{
					this.UpdateControl(control, newBackColor);
				}
				controlToUpdate.Invalidate();
			}
		}
	}
}
