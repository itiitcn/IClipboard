using MaterialSkin.Animations;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialContextMenuStrip : ContextMenuStrip
	{
		public delegate void ItemClickStart(object sender, ToolStripItemClickedEventArgs e);

		internal AnimationManager AnimationManager;

		internal Point AnimationSource;

		private ToolStripItemClickedEventArgs _delayesArgs;

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

		public event ItemClickStart OnItemClickStart;

		public MaterialContextMenuStrip()
		{
			base.Renderer = new MaterialToolStripRender();
			this.AnimationManager = new AnimationManager(false)
			{
				Increment = 0.07,
				AnimationType = AnimationType.Linear
			};
			this.AnimationManager.OnAnimationProgress += delegate
			{
				base.Invalidate();
			};
			this.AnimationManager.OnAnimationFinished += delegate
			{
				this.OnItemClicked(this._delayesArgs);
			};
            base.BackColor = this.SkinManager.GetApplicationBackgroundColor();
		}

		protected override void OnMouseUp(MouseEventArgs mea)
		{
			base.OnMouseUp(mea);
			this.AnimationSource = mea.Location;
		}

		protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem != null && !(e.ClickedItem is ToolStripSeparator))
			{
				if (e == this._delayesArgs)
				{
					base.OnItemClicked(e);
				}
				else
				{
					this._delayesArgs = e;
					ItemClickStart onItemClickStart = this.OnItemClickStart;
					if (onItemClickStart != null)
					{
						onItemClickStart(this, e);
					}
					this.AnimationManager.StartNewAnimation(AnimationDirection.In, null);
				}
			}
		}
	}
}
