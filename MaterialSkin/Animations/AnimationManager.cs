using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MaterialSkin.Animations
{
	internal class AnimationManager
	{
		public delegate void AnimationFinished(object sender);

		public delegate void AnimationProgress(object sender);

		private readonly List<double> _animationProgresses;

		private readonly List<Point> _animationSources;

		private readonly List<AnimationDirection> _animationDirections;

		private readonly List<object[]> _animationDatas;

		private const double MIN_VALUE = 0.0;

		private const double MAX_VALUE = 1.0;

		private readonly Timer _animationTimer = new Timer
		{
			Interval = 5,
			Enabled = false
		};

		public bool InterruptAnimation
		{
			get;
			set;
		}

		public double Increment
		{
			get;
			set;
		}

		public double SecondaryIncrement
		{
			get;
			set;
		}

		public AnimationType AnimationType
		{
			get;
			set;
		}

		public bool Singular
		{
			get;
			set;
		}

		public event AnimationFinished OnAnimationFinished;

		public event AnimationProgress OnAnimationProgress;

		public AnimationManager(bool singular = true)
		{
			this._animationProgresses = new List<double>();
			this._animationSources = new List<Point>();
			this._animationDirections = new List<AnimationDirection>();
			this._animationDatas = new List<object[]>();
			this.Increment = 0.03;
			this.SecondaryIncrement = 0.03;
			this.AnimationType = AnimationType.Linear;
			this.InterruptAnimation = true;
			this.Singular = singular;
			if (this.Singular)
			{
				this._animationProgresses.Add(0.0);
				this._animationSources.Add(new Point(0, 0));
				this._animationDirections.Add(AnimationDirection.In);
			}
			this._animationTimer.Tick += this.AnimationTimerOnTick;
		}

		private void AnimationTimerOnTick(object sender, EventArgs eventArgs)
		{
			for (int i = 0; i < this._animationProgresses.Count; i++)
			{
				this.UpdateProgress(i);
				int num;
				if (!this.Singular)
				{
					if (this._animationDirections[i] == AnimationDirection.InOutIn && this._animationProgresses[i] == 1.0)
					{
						this._animationDirections[i] = AnimationDirection.InOutOut;
						continue;
					}
					if (this._animationDirections[i] == AnimationDirection.InOutRepeatingIn && this._animationProgresses[i] == 0.0)
					{
						this._animationDirections[i] = AnimationDirection.InOutRepeatingOut;
						continue;
					}
					if (this._animationDirections[i] == AnimationDirection.InOutRepeatingOut && this._animationProgresses[i] == 0.0)
					{
						this._animationDirections[i] = AnimationDirection.InOutRepeatingIn;
						continue;
					}
					if (this._animationDirections[i] == AnimationDirection.In && this._animationProgresses[i] == 1.0)
					{
						goto IL_0160;
					}
					if (this._animationDirections[i] == AnimationDirection.Out && this._animationProgresses[i] == 0.0)
					{
						goto IL_0160;
					}
					num = ((this._animationDirections[i] == AnimationDirection.InOutOut && this._animationProgresses[i] == 0.0) ? 1 : 0);
					goto IL_0161;
				}
				if (this._animationDirections[i] == AnimationDirection.InOutIn && this._animationProgresses[i] == 1.0)
				{
					this._animationDirections[i] = AnimationDirection.InOutOut;
				}
				else if (this._animationDirections[i] == AnimationDirection.InOutRepeatingIn && this._animationProgresses[i] == 1.0)
				{
					this._animationDirections[i] = AnimationDirection.InOutRepeatingOut;
				}
				else if (this._animationDirections[i] == AnimationDirection.InOutRepeatingOut && this._animationProgresses[i] == 0.0)
				{
					this._animationDirections[i] = AnimationDirection.InOutRepeatingIn;
				}
				continue;
				IL_0161:
				if (num != 0)
				{
					this._animationProgresses.RemoveAt(i);
					this._animationSources.RemoveAt(i);
					this._animationDirections.RemoveAt(i);
					this._animationDatas.RemoveAt(i);
				}
				continue;
				IL_0160:
				num = 1;
				goto IL_0161;
			}
			AnimationProgress onAnimationProgress = this.OnAnimationProgress;
			if (onAnimationProgress != null)
			{
				onAnimationProgress(this);
			}
		}

		public bool IsAnimating()
		{
			return this._animationTimer.Enabled;
		}

		public void StartNewAnimation(AnimationDirection animationDirection, object[] data = null)
		{
			this.StartNewAnimation(animationDirection, new Point(0, 0), data);
		}

		public void StartNewAnimation(AnimationDirection animationDirection, Point animationSource, object[] data = null)
		{
			if (!this.IsAnimating() || this.InterruptAnimation)
			{
				if (this.Singular && this._animationDirections.Count > 0)
				{
					this._animationDirections[0] = animationDirection;
				}
				else
				{
					this._animationDirections.Add(animationDirection);
				}
				if (this.Singular && this._animationSources.Count > 0)
				{
					this._animationSources[0] = animationSource;
				}
				else
				{
					this._animationSources.Add(animationSource);
				}
				if (!this.Singular || this._animationProgresses.Count <= 0)
				{
					switch (this._animationDirections[this._animationDirections.Count - 1])
					{
					case AnimationDirection.In:
					case AnimationDirection.InOutIn:
					case AnimationDirection.InOutRepeatingIn:
						this._animationProgresses.Add(0.0);
						break;
					case AnimationDirection.Out:
					case AnimationDirection.InOutOut:
					case AnimationDirection.InOutRepeatingOut:
						this._animationProgresses.Add(1.0);
						break;
					default:
						throw new Exception("Invalid AnimationDirection");
					}
				}
				if (this.Singular && this._animationDatas.Count > 0)
				{
					this._animationDatas[0] = (data ?? new object[0]);
				}
				else
				{
					this._animationDatas.Add(data ?? new object[0]);
				}
			}
			this._animationTimer.Start();
		}

		public void UpdateProgress(int index)
		{
			switch (this._animationDirections[index])
			{
			case AnimationDirection.In:
			case AnimationDirection.InOutIn:
			case AnimationDirection.InOutRepeatingIn:
				this.IncrementProgress(index);
				break;
			case AnimationDirection.Out:
			case AnimationDirection.InOutOut:
			case AnimationDirection.InOutRepeatingOut:
				this.DecrementProgress(index);
				break;
			default:
				throw new Exception("No AnimationDirection has been set");
			}
		}

		private void IncrementProgress(int index)
		{
			List<double> animationProgresses = this._animationProgresses;
			animationProgresses[index] += this.Increment;
			if (this._animationProgresses[index] > 1.0)
			{
				this._animationProgresses[index] = 1.0;
				for (int i = 0; i < this.GetAnimationCount(); i++)
				{
					if (this._animationDirections[i] == AnimationDirection.InOutIn || this._animationDirections[i] == AnimationDirection.InOutRepeatingIn || this._animationDirections[i] == AnimationDirection.InOutRepeatingOut || (this._animationDirections[i] == AnimationDirection.InOutOut && this._animationProgresses[i] != 1.0) || (this._animationDirections[i] == AnimationDirection.In && this._animationProgresses[i] != 1.0))
					{
						return;
					}
				}
				this._animationTimer.Stop();
				AnimationFinished onAnimationFinished = this.OnAnimationFinished;
				if (onAnimationFinished != null)
				{
					onAnimationFinished(this);
				}
			}
		}

		private void DecrementProgress(int index)
		{
			List<double> animationProgresses = this._animationProgresses;
			animationProgresses[index] -= ((this._animationDirections[index] == AnimationDirection.InOutOut || this._animationDirections[index] == AnimationDirection.InOutRepeatingOut) ? this.SecondaryIncrement : this.Increment);
			if (this._animationProgresses[index] < 0.0)
			{
				this._animationProgresses[index] = 0.0;
				for (int i = 0; i < this.GetAnimationCount(); i++)
				{
					if (this._animationDirections[i] == AnimationDirection.InOutIn || this._animationDirections[i] == AnimationDirection.InOutRepeatingIn || this._animationDirections[i] == AnimationDirection.InOutRepeatingOut || (this._animationDirections[i] == AnimationDirection.InOutOut && this._animationProgresses[i] != 0.0) || (this._animationDirections[i] == AnimationDirection.Out && this._animationProgresses[i] != 0.0))
					{
						return;
					}
				}
				this._animationTimer.Stop();
				AnimationFinished onAnimationFinished = this.OnAnimationFinished;
				if (onAnimationFinished != null)
				{
					onAnimationFinished(this);
				}
			}
		}

		public double GetProgress()
		{
			if (!this.Singular)
			{
				throw new Exception("Animation is not set to Singular.");
			}
			if (this._animationProgresses.Count == 0)
			{
				throw new Exception("Invalid animation");
			}
			return this.GetProgress(0);
		}

		public double GetProgress(int index)
		{
			if (index >= this.GetAnimationCount())
			{
				throw new IndexOutOfRangeException("Invalid animation index");
			}
			switch (this.AnimationType)
			{
			case AnimationType.Linear:
				return AnimationLinear.CalculateProgress(this._animationProgresses[index]);
			case AnimationType.EaseInOut:
				return AnimationEaseInOut.CalculateProgress(this._animationProgresses[index]);
			case AnimationType.EaseOut:
				return AnimationEaseOut.CalculateProgress(this._animationProgresses[index]);
			case AnimationType.CustomQuadratic:
				return AnimationCustomQuadratic.CalculateProgress(this._animationProgresses[index]);
			default:
				throw new NotImplementedException("The given AnimationType is not implemented");
			}
		}

		public Point GetSource(int index)
		{
			if (index >= this.GetAnimationCount())
			{
				throw new IndexOutOfRangeException("Invalid animation index");
			}
			return this._animationSources[index];
		}

		public Point GetSource()
		{
			if (!this.Singular)
			{
				throw new Exception("Animation is not set to Singular.");
			}
			if (this._animationSources.Count == 0)
			{
				throw new Exception("Invalid animation");
			}
			return this._animationSources[0];
		}

		public AnimationDirection GetDirection()
		{
			if (!this.Singular)
			{
				throw new Exception("Animation is not set to Singular.");
			}
			if (this._animationDirections.Count == 0)
			{
				throw new Exception("Invalid animation");
			}
			return this._animationDirections[0];
		}

		public AnimationDirection GetDirection(int index)
		{
			if (index >= this._animationDirections.Count)
			{
				throw new IndexOutOfRangeException("Invalid animation index");
			}
			return this._animationDirections[index];
		}

		public object[] GetData()
		{
			if (!this.Singular)
			{
				throw new Exception("Animation is not set to Singular.");
			}
			if (this._animationDatas.Count == 0)
			{
				throw new Exception("Invalid animation");
			}
			return this._animationDatas[0];
		}

		public object[] GetData(int index)
		{
			if (index >= this._animationDatas.Count)
			{
				throw new IndexOutOfRangeException("Invalid animation index");
			}
			return this._animationDatas[index];
		}

		public int GetAnimationCount()
		{
			return this._animationProgresses.Count;
		}

		public void SetProgress(double progress)
		{
			if (!this.Singular)
			{
				throw new Exception("Animation is not set to Singular.");
			}
			if (this._animationProgresses.Count == 0)
			{
				throw new Exception("Invalid animation");
			}
			this._animationProgresses[0] = progress;
		}

		public void SetDirection(AnimationDirection direction)
		{
			if (!this.Singular)
			{
				throw new Exception("Animation is not set to Singular.");
			}
			if (this._animationProgresses.Count == 0)
			{
				throw new Exception("Invalid animation");
			}
			this._animationDirections[0] = direction;
		}

		public void SetData(object[] data)
		{
			if (!this.Singular)
			{
				throw new Exception("Animation is not set to Singular.");
			}
			if (this._animationDatas.Count == 0)
			{
				throw new Exception("Invalid animation");
			}
			this._animationDatas[0] = data;
		}
	}
}
