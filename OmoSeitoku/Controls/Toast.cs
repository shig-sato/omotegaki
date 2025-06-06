using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OmoSeitoku.Controls
{
    public sealed class Toast : Panel
    {
        public enum Position
        {
            Top,
            Bottom
        }

        private const double ANIMATION_FRAME_SPAN = 24d;

        public Position ToastPosition = Position.Bottom;
        public TimeSpan AnimationDuration = TimeSpan.FromMilliseconds(400);
        public bool HideOnClick = true;
        public bool DontHideOnMouseHover = true;


        private System.ComponentModel.IContainer _components = null;
        private Label _label;
        private Timer _timer;

        private bool _pause;

        public string Message
        {
            get { return _label.Text; }
            set
            {
                _label.Text = value ?? string.Empty;
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value.A == 255 ? Color.FromArgb(180, value) : value;
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                _label.Font = value;
            }
        }

        public Toast()
        {
            _components = new System.ComponentModel.Container();

            _label = new Label();
            _timer = new Timer(_components);


            this.SuspendLayout();

            // Label
            _label.AutoSize = true;
            _label.BackColor = Color.FromArgb(247, Color.White);
            _label.Image = Properties.Resources.action_delete;
            _label.ImageAlign = ContentAlignment.MiddleRight;
            _label.Padding = new Padding(6);
            _label.TextAlign = ContentAlignment.MiddleCenter;
            _label.MouseMove += delegate (object sndr, MouseEventArgs evt)
            {
                this.OnMouseMove(evt);
            };
            _label.MouseLeave += delegate (object sndr, EventArgs evt)
            {
                this.OnMouseLeave(evt);
            };
            _label.Click += delegate (object sndr, EventArgs evt)
            {
                this.OnClick(evt);
            };


            // Timer
            _timer.Tick += new EventHandler(_timer_Tick);


            // Toast
            this.AutoScroll = true;
            this.BackColor = Color.Gold;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(_label);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font(Font.FontFamily, 16f);
            this.Padding = new Padding(6);
            this.Visible = false;

            _label.Location = new Point(Padding.Left, Padding.Top);

            this.ResumeLayout(false);
        }


        private Color? _tempForOnceFrameColor;

        /// <summary>
        /// トーストを表示
        /// </summary>
        /// <param name="message"></param>
        /// <param name="interval">表示している時間</param>
        /// <param name="onceFrameColor">一時的に使用するフレームカラー</param>
        public void Show(string message, TimeSpan interval, Color onceFrameColor)
        {
            if (!_tempForOnceFrameColor.HasValue)
            {
                _tempForOnceFrameColor = BackColor;
            }
            BackColor = onceFrameColor;

            _ShowToast(message, interval);
        }

        /// <summary>
        /// トーストを表示
        /// </summary>
        /// <param name="message"></param>
        /// <param name="interval">表示している時間</param>
        public void Show(string message, TimeSpan interval)
        {
            if (_tempForOnceFrameColor.HasValue)
            {
                BackColor = _tempForOnceFrameColor.Value;
                _tempForOnceFrameColor = null;
            }

            _ShowToast(message, interval);
        }

        [Obsolete("use ShowToast", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new void Show()
        {
            base.Show();
        }

        public void HideToast(bool animation)
        {
            _timer.Stop();

            int end;

            switch (ToastPosition)
            {
                case Position.Top:
                    end = -Height;
                    break;

                case Position.Bottom:
                    end = MaximumSize.Height;
                    break;

                default:
                    throw new Exception("未対応の ToastPosition");
            }

            if (animation)
                StartAnimation(Top, end, TransitionQuadEaseIn);

            Visible = false;

            if (_tempForOnceFrameColor.HasValue)
            {
                BackColor = _tempForOnceFrameColor.Value;
                _tempForOnceFrameColor = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (DontHideOnMouseHover && _timer.Enabled)
            {
                _pause = true;
                _timer.Stop();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (DontHideOnMouseHover && _pause)
            {
                _pause = false;
                _timer.Start();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (HideOnClick)
            {
                HideToast(false);
            }
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            HideToast(true);
        }



        private void _ShowToast(string message, TimeSpan interval)
        {
            this.SuspendLayout();

            if (Visible)
                _timer.Stop();


            Padding margin = Margin;

            MaximumSize = Parent.ClientSize;

            Width = MaximumSize.Width - margin.Horizontal;

            _label.MinimumSize = _label.MaximumSize =
                new Size(ClientSize.Width - Padding.Horizontal, 0);

            Message = message; // _label size changed

            int height = _label.Height + Padding.Vertical;
            int y;

            switch (ToastPosition)
            {
                case Position.Top:
                    y = -height;
                    break;
                case Position.Bottom:
                    y = MaximumSize.Height;
                    break;
                default:
                    throw new Exception("未対応の ToastPosition");
            }

            SetBounds(margin.Left, y, Width, height);

            BringToFront();

            Visible = true;

            int end;

            switch (ToastPosition)
            {
                case Position.Top:
                    end = margin.Top;
                    break;

                case Position.Bottom:
                    end = MaximumSize.Height - height - margin.Bottom;
                    break;

                default:
                    throw new Exception("未対応の ToastPosition");
            }

            StartAnimation(y, end, TransitionQuadEaseOut);


            _timer.Interval = (int)interval.TotalMilliseconds;
            _timer.Start();

            this.ResumeLayout(false);
        }


        private double _runningDuration;

        private void StartAnimation(int start, int end, Func<double, double> transition)
        {
            double duration = AnimationDuration.TotalMilliseconds - _runningDuration;
            bool reverse = end < start;
            int dif = reverse ? -1 : 1;
            double range = (end - start) * dif;
            int y = start;
            DateTime stTime = DateTime.Now;
            DateTime prev = stTime;

            while (Visible)
            {
                DateTime now = DateTime.Now;

                if ((now - prev).TotalMilliseconds < ANIMATION_FRAME_SPAN)
                    continue;

                prev = now;


                double pos = (now - stTime).TotalMilliseconds;
                if (duration <= pos)
                {
                    _runningDuration = 0;
                    break;
                }
                _runningDuration = pos;
                pos /= duration; // 進行率
                // range を かけて進行距離に戻す。
                // start からの位置に直す。
                pos = range * transition(pos) * dif + start;
                this.SetBounds(0, (int)pos, 0, 0, BoundsSpecified.Y);

                Application.DoEvents();
            }

            this.SetBounds(0, end, 0, 0, BoundsSpecified.Y);
        }

        private static double TransitionQuadEaseIn(double pos)
        {
            return Math.Pow(pos, 2d);
        }

        private static double TransitionQuadEaseOut(double pos)
        {
            return 1d - Math.Pow(1d - pos, 2d);
        }
    }
}