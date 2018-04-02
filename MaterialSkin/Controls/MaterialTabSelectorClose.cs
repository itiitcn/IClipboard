using MaterialSkin.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialTabSelectorClose : Control, IMaterialControl
	{
		private MaterialTabControl _baseTabControl;

		private int _previousSelectedTabIndex;

		private Point _animationSource;

		private readonly AnimationManager _animationManager;

		private List<Rectangle> _tabRects;

		private const int TAB_HEADER_PADDING = 24;

		private const int TAB_INDICATOR_HEIGHT = 2;

		[Browsable(false)]
		public int Depth
		{
			get;
			set;
		}

		[Browsable(false)]
		public MaterialSkinManager SkinManager
		{
			get
			{
				return MaterialSkinManager.Instance;
			}
		}

		[Browsable(false)]
		public MouseState MouseState
		{
			get;
			set;
		}

		public MaterialTabControl BaseTabControl
		{
			get
			{
				return this._baseTabControl;
			}
			set
			{
				this._baseTabControl = value;
				if (this._baseTabControl != null)
				{
					this._previousSelectedTabIndex = this._baseTabControl.SelectedIndex;
					this._baseTabControl.Deselected += delegate
					{
						this._previousSelectedTabIndex = this._baseTabControl.SelectedIndex;
					};
					this._baseTabControl.SelectedIndexChanged += delegate
					{
						this._animationManager.SetProgress(0.0);
						this._animationManager.StartNewAnimation(AnimationDirection.In, null);
					};
					this._baseTabControl.ControlAdded += delegate
					{
						base.Invalidate();
					};
					this._baseTabControl.ControlRemoved += delegate
					{
						base.Invalidate();
					};
				}
			}
		}

        public MaterialTabSelectorClose()
		{
			base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
			base.Height = 48;
			this._animationManager = new AnimationManager(true)
			{
				AnimationType = AnimationType.EaseOut,
				Increment = 0.04
			};
			this._animationManager.OnAnimationProgress += delegate
			{
				base.Invalidate();
			};
		}

        private void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (IsDisposed) return;
            // Convert to client position and pass to Form.MouseMove
            var clientCursorPos = PointToClient(e.Location);
            var newE = new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0);
            OnMouseMove(newE);
        }


        public delegate void TabDeleteClickHandler(object sender, EventArgs e);
        public event TabDeleteClickHandler TabDeleteClick;
        protected virtual void OnTabDeleteClick()
        {
            if(TabDeleteClick!=null){
                TabDeleteClick.Invoke(this, EventArgs.Empty);
            }
            
        }
        public delegate void TabPreDeleteClickHandler(object sender, EventArgs e);
        public event TabPreDeleteClickHandler TabPreDeleteClick;
        protected virtual void OnTabPreDeleteClick()
        {
            if (TabPreDeleteClick != null)
            {
                TabPreDeleteClick.Invoke(this, EventArgs.Empty);
            }

        }

        public delegate void TabAddClickHandler(object sender, EventArgs e);
        public event TabAddClickHandler TabAddClick;
        protected virtual void OnTabAddClick()
        {
            if (TabAddClick != null)
            {
                TabAddClick.Invoke(this, EventArgs.Empty);
            }

        }


        public delegate void TabPreAddClickHandler(object sender, EventArgs e);
        public event TabPreAddClickHandler TabPreAddClick;
        protected virtual void OnTabPreAddClick()
        {
            if (TabPreAddClick != null)
            {
                TabPreAddClick.Invoke(this, EventArgs.Empty);
            }

        }


        int CLOSE_SIZE = 18;

        Point ADD_POINT = new Point(0, 0);

      
        //线条TOP
        int LINETOP = 3;
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
            int CLOSETOP = (this.Height-CLOSE_SIZE)/2-3;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(this.SkinManager.ColorScheme.DarkPrimaryColor);
			if (this._baseTabControl != null)
			{
				if (!this._animationManager.IsAnimating() || this._tabRects == null || this._tabRects.Count != this._baseTabControl.TabCount)
				{
					this.UpdateTabRects();
				}
				double progress = this._animationManager.GetProgress();
				if (this._animationManager.IsAnimating())
				{
					SolidBrush solidBrush = new SolidBrush(Color.FromArgb((int)(51.0 - progress * 50.0), Color.White));
					int num = (int)(progress * (double)this._tabRects[this._baseTabControl.SelectedIndex].Width * 1.75);
					graphics.SetClip(this._tabRects[this._baseTabControl.SelectedIndex]);
					graphics.FillEllipse(solidBrush, new Rectangle(this._animationSource.X - num / 2, this._animationSource.Y - num / 2, num, num));
					graphics.ResetClip();
					solidBrush.Dispose();
				}
				foreach (TabPage tabPage in this._baseTabControl.TabPages)
				{
					int i = this._baseTabControl.TabPages.IndexOf(tabPage);
                    
					Brush brush = new SolidBrush(Color.FromArgb(this.CalculateTextAlpha(i, progress), this.SkinManager.ColorScheme.TextColor));
                    if (true)
                    {
                        int xx = (i * WIDTH_RECT + 13) + WIDTH_RECT;
                        using (Pen pen = new Pen(brush))
                        {
                            graphics.DrawLine(pen, new Point(xx, LINETOP), new Point(xx, (this.Height - (LINETOP * 2))));
                        }
                    }
                    StringFormat sf=new StringFormat
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Center
					};
                    Rectangle rect = this._tabRects[i];
                    rect.X -= 15;
                    string txt = CalcWidth(tabPage.Text,graphics);
                    rect.X = WIDTH_RECT * i+5;
                    graphics.DrawString(txt.ToUpper(), this.SkinManager.ROBOTO_MEDIUM_10, brush, rect, sf);
                    int w = (WIDTH_RECT * (i + 1)) - CLOSE_SIZE / 2;
                    using (Pen objpen = new Pen(Color.Black))
                    {
                        Font font = new System.Drawing.Font("微软雅黑", 12);
                        graphics.DrawString("〇", font, brush, w, CLOSETOP);
                        font = new System.Drawing.Font("微软雅黑", 11);
                        graphics.DrawString("×", font, brush, w + 3, CLOSETOP);
                    }

                    brush.Dispose();
				}
				int index = (this._previousSelectedTabIndex == -1) ? this._baseTabControl.SelectedIndex : this._previousSelectedTabIndex;
                Rectangle rectangle;
                try
                {
                    rectangle = this._tabRects[index];
                }
                catch (System.Exception)
                {
                    rectangle = this._tabRects[0];
                }
				Rectangle rectangle2 = this._tabRects[this._baseTabControl.SelectedIndex];
				int y = rectangle2.Bottom - 2;
				int x = rectangle.X + (int)((double)(rectangle2.X - rectangle.X) * progress);
				int width = rectangle.Width + (int)((double)(rectangle2.Width - rectangle.Width) * progress);
				graphics.FillRectangle(this.SkinManager.ColorScheme.AccentBrush, x, y, width, 2);
                using (Pen objpen = new Pen(Color.White))
                {
                    int tx = (int)(22 + (width * this._baseTabControl.TabPages.Count));
                    Point p5 = new Point(tx, 6);
                   // e.Graphics.DrawRectangle(objpen, tx - 6, 0, this.Height-1, this.Height-1);
                    ADD_POINT = new Point(tx - 6, 0);
                    SolidBrush brush = new SolidBrush(Color.White);
                    tx = tx - 7;
                    int ADDWIDTH = this.Height - (ADDTOP * 2);
                    using (Pen pen = new Pen(brush))
                    {
                        graphics.DrawLine(pen, new Point(tx + (this.Height / 2), ADDTOP), new Point(tx + (this.Height / 2), ADDTOP + ADDWIDTH));
                        graphics.DrawLine(pen, new Point(tx + ADDTOP, (this.Height / 2)), new Point(tx + ADDWIDTH + ADDTOP, (this.Height / 2)));
                    }
                }
			}
            

		}



        public string CalcWidth(string text,Graphics g,int num=0)
        {
            SizeF sizeF = g.MeasureString(text, this.SkinManager.ROBOTO_MEDIUM_10);
            if (sizeF.Width > WIDTH_RECT - 30)
            {
                text = text.Remove(text.Length-1);
                num++;
                return CalcWidth(text,g,num);
            }
            else
            {
                if (num == 0)
                {
                    return text;
                }
                else
                {
                    return text + "...";
                }
            }
        }




        int ADDTOP = 10;
		private int CalculateTextAlpha(int tabIndex, double animationProgress)
		{
			Color color = this.SkinManager.ACTION_BAR_TEXT;
			int a = color.A;
			color = this.SkinManager.ACTION_BAR_TEXT_SECONDARY;
			int a2 = color.A;
			if (tabIndex == this._baseTabControl.SelectedIndex && !this._animationManager.IsAnimating())
			{
				return a;
			}
			if (tabIndex != this._previousSelectedTabIndex && tabIndex != this._baseTabControl.SelectedIndex)
			{
				return a2;
			}
			if (tabIndex == this._previousSelectedTabIndex)
			{
				return a - (int)((double)(a - a2) * animationProgress);
			}
			return a2 + (int)((double)(a - a2) * animationProgress);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this._tabRects == null)
			{
				this.UpdateTabRects();
			}
			for (int i = 0; i < this._tabRects.Count; i++)
			{
				if (this._tabRects[i].Contains(e.Location))
				{
					this._baseTabControl.SelectedIndex = i;
				}
			}
			this._animationSource = e.Location;
		}

        public int WIDTH_RECT =240;
		private void UpdateTabRects()
		{
			this._tabRects = new List<Rectangle>();
			if (this._baseTabControl != null && this._baseTabControl.TabCount != 0)
			{
				using (Bitmap image = new Bitmap(1, 1))
				{
					using (Graphics graphics = Graphics.FromImage(image))
					{
						List<Rectangle> tabRects = this._tabRects;
						int fORM_PADDING = this.SkinManager.FORM_PADDING;
						SizeF sizeF = graphics.MeasureString(this._baseTabControl.TabPages[0].Text, this.SkinManager.ROBOTO_MEDIUM_10);
                        tabRects.Add(new Rectangle(fORM_PADDING, 0, WIDTH_RECT, base.Height));
						for (int i = 1; i < this._baseTabControl.TabPages.Count; i++)
						{
							List<Rectangle> tabRects2 = this._tabRects;
							int right = this._tabRects[i - 1].Right;
							sizeF = graphics.MeasureString(this._baseTabControl.TabPages[i].Text, this.SkinManager.ROBOTO_MEDIUM_10);
                            tabRects2.Add(new Rectangle(right, 0, WIDTH_RECT, base.Height));
						}
					}
				}
			}
		}




        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                int CLOSETOP = (this.Height - CLOSE_SIZE) / 2 - 3;
                if (e.X < 16) return;
                int x = e.X, y = e.Y;
                int tabCount = this._baseTabControl.TabPages.Count;
                int SelectedTab = (x - 15) / WIDTH_RECT;

                if (SelectedTab < 0) SelectedTab = 0;
                if (SelectedTab >= tabCount)
                {
                    OnTabPreAddClick();
                    int longw = ((tabCount + 1) * WIDTH_RECT + this.Height);
                    if (this.Width < longw)
                    {
                        return;
                    }
                    if (tabCount >= 20)
                    {
                        return;
                    }
                    bool isAdd = x > ADD_POINT.X && x < (ADD_POINT.X + this.Height) && y > ADD_POINT.Y && y < ADD_POINT.Y + this.Height;
                    if (isAdd)
                    {
                        if ((tabCount + 1) * WIDTH_RECT > (this.Width - (20+this.Height)))
                        {
                            if (WIDTH_RECT <= 60)
                                return;
                            WIDTH_RECT = (this.Width-(20 + this.Height)) / (tabCount + 1);
                        }
                        //this._baseTabControl.Controls.Add(new TabPage("hahah"));
                        OnTabAddClick();
                    }
                }
                else
                {
                    if (tabCount == 1 || SelectedTab >= tabCount)
                    {
                        return;
                    }
                    OnTabPreDeleteClick();
                    int iwidth = WIDTH_RECT * (SelectedTab + 1) - 5;
                    //如果鼠标在区域内就关闭选项卡   
                    bool isClose = x > iwidth && x < iwidth + 18 && y > CLOSETOP && y < this.Width - (CLOSETOP * 2);
                    if (isClose == true)
                    {
                        
                        if (WIDTH_RECT < 240)
                        {
                            WIDTH_RECT = (this.Width-(20 + this.Height)) / (tabCount + 1);
                        }
                        this._baseTabControl.Controls.RemoveAt(SelectedTab);
                        OnTabDeleteClick();
                    }
                }
            }

        }

	}
}
