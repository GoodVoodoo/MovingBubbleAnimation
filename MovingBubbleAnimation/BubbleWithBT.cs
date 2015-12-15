using MovingBubbleAnimation.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace MovingBubbleAnimation
{
    class BubbleWithBT:IBubble
    {
       
        private Canvas canvas;
        private System.Windows.Controls.Image bubbleImage;
        private System.Windows.Controls.Image innerObjectImage;
        private Size mainWindowSize;
        private Size bubbleSize;
        private TimeSpan duration;
        private System.Windows.Controls.Image popBubbleGif;
        private ThicknessAnimation animation;
        private Point startPosition;
        private EnumsAndConst.DeviceTypes deviceType;


        public BubbleWithBT (Size mainWindowSize, Size bubbleSize, Point startPosition, TimeSpan duration)
        {
            canvas = new Canvas();
            this.mainWindowSize = mainWindowSize;
            this.bubbleSize = bubbleSize;
            this.duration = duration;
            this.startPosition = startPosition;
        }


        public Storyboard BubbleBoard
        {
            get;
            set;
        }

        public string BubbleImagePath
        {
            get;
            set;
        }

        public string CustomInnnerImagePath
        {
            get;
            set;
        }

        public object InnerObject
        {
            get;
            set;
        }

        public Canvas BubbleCanv
        {
            get
            {
                if (this.deviceType == null)
                {
                    setCanv();
                }
                return canvas;
            }
            set { ;}
        }

        public EnumsAndConst.DeviceTypes DeviceType
        {
            get {return deviceType;}
            set
            {
                deviceType = value;
                setCanv();
            }

        }

        public ThicknessAnimation BubbleAnimation
        {
            get {return setAnimation();}
            set { ;}
        }


        public Point StartBubblePosition
        {
            get;
            set;
        }

        public System.Drawing.Bitmap popBubble
        {
            get { return Resources.bubblePop; }
            set {}
        }


        public BitmapSource popBubbleSource
        {
            get { return GetSource(); }
            set {}
        }



        private BitmapSource GetSource()
        {
      
            IntPtr handle = IntPtr.Zero;
            handle = Resources.bubblePop.GetHbitmap();

            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }





        private void setCanv()
        {
           
            canvas.Height = 100;
            canvas.Width = 100;

            bubbleImage = new System.Windows.Controls.Image();


            if (BubbleImagePath == null)
                bubbleImage.Source = loadBitmap(Resources.bubble);
            else
                bubbleImage.Source = new BitmapImage(new Uri(BubbleImagePath));

            bubbleImage.Height = canvas.Height;
            bubbleImage.Width = canvas.Width;

            canvas.Children.Add(bubbleImage);


            if (deviceType != null)
            {
                innerObjectImage = new System.Windows.Controls.Image();
                innerObjectImage.Source = loadBitmap(ChoosingInnerImage(deviceType));

                innerObjectImage.Height = 60;
                innerObjectImage.Width = 60;
                innerObjectImage.Margin = new Thickness(
                    (bubbleImage.Width-innerObjectImage.Width) / 2,
                    (bubbleImage.Height-innerObjectImage.Height) / 2, 0, 0);

                canvas.Children.Add(innerObjectImage);
            }

            Panel.SetZIndex(canvas, 10);

            this.BubbleCanv = canvas;
            
        }


        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }

        private ThicknessAnimation setAnimation()
        {
            ThicknessAnimation result;
            Random rd = new Random(DateTime.Now.Millisecond);
            animation = new ThicknessAnimation();
            int rdLeftMax = Convert.ToInt32(mainWindowSize.Width - bubbleImage.Width);
            int rdTopMax = Convert.ToInt32(mainWindowSize.Height - bubbleImage.Height);
            animation.Duration = duration;
            

            double fromLeft = StartBubblePosition.X;
            double fromTop = StartBubblePosition.Y;

            int toLeft = rd.Next(0, rdLeftMax);
            int toTop = rd.Next(0, rdTopMax);
            
            animation.From = new System.Windows.Thickness
                (fromLeft, fromTop, 0.0, 0.0);

            animation.To = new System.Windows.Thickness(toLeft,
                toTop, 0.0, 0.0);

            result = animation;

            return result;
        }


        public System.Windows.Media.Animation.ThicknessAnimation MovingAnimation()
        {
            throw new NotImplementedException();
        }


        private System.Drawing.Bitmap ChoosingInnerImage(EnumsAndConst.DeviceTypes deviceType)
        {
            System.Drawing.Bitmap result;
            switch (deviceType)
            {
                case EnumsAndConst.DeviceTypes.CellPhone:
                    result = Resources.cell_phone;
                    break;
                case EnumsAndConst.DeviceTypes.HeadPhones:
                    result = Resources.head_phones;
                    break;
                case EnumsAndConst.DeviceTypes.Loudspeaker:
                    result = Resources.loud_speaker;
                    break;
                case EnumsAndConst.DeviceTypes.PC:
                    result = Resources.PC;
                    break;
                default:
                    result = Resources.unknown_device;
                    break;

            }

            return result;
        }

    }
}
