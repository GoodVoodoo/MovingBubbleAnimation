using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;
using WpfAnimatedGif;

using MovingBubbleAnimation.Properties;

namespace MovingBubbleAnimation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int timerInterval = 10000;//ms
        BubbleWithBT bubbl;
        Timer tm;
        int bubbleCounter=0; 

        public MainWindow()
        {
            InitializeComponent();
            
        }

        void tm_Elapsed(IBubble bubble, Storyboard sBoard, Canvas bubbleCanvas)
        {
            tm.Interval = timerInterval;    
            this.Dispatcher.Invoke(new Action(() => LaunchBubble(bubble, sBoard, bubbleCanvas)));
        }

        private void bubble_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Canvas canv = sender as Canvas;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Size canvasSize = new Size(mainCanv.ActualWidth, mainCanv.ActualHeight);
            Size bubbleSize = new Size(0, 0);

            bubbl = new BubbleWithBT(canvasSize, bubbleSize, new Point(0, 0), TimeSpan.FromSeconds(10));

            #region Test Part
            if (bubbleCounter == 0)
            {
                bubbl.DeviceType = EnumsAndConst.DeviceTypes.CellPhone;
            }
            else if (bubbleCounter == 1)
            {
                bubbl.DeviceType = EnumsAndConst.DeviceTypes.Loudspeaker;
            }
            else
            {
                bubbl.DeviceType = EnumsAndConst.DeviceTypes.PC;
            }
            #endregion

            bubbleCounter++;
            Canvas canv =  bubbl.BubbleCanv;
            canv.Tag = bubbleCounter;
            canv.AddHandler(Canvas.MouseUpEvent, new MouseButtonEventHandler(canv_MouseUp), true);
            mainCanv.Children.Add(canv);
            Storyboard sBoard = new Storyboard();
            tm = new Timer();
            tm.Interval = 1;
            tm.Elapsed += (sdr, evtTm) => { tm_Elapsed(bubbl, sBoard, canv); };
            tm.Start();

        }
         
        private void canv_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Canvas canv = sender as Canvas;
            var bitmap = BitmapFrame.Create(new Uri(
                "pack://application:,,,/Resources/bubblePop.gif", UriKind.RelativeOrAbsolute));
            var bubbleImage = canv.Children[0] as Image;
            var popBubbleImage = new Image();
            ImageBehavior.SetAnimatedSource(bubbleImage, bitmap);
            ImageBehavior.SetRepeatBehavior(bubbleImage, RepeatBehavior.Forever);
        }



        private void LaunchBubble(IBubble bubble, Storyboard sBoard, Canvas bubbleCanvas)
        {
            sBoard.Children.Clear();
            bubble.StartBubblePosition = new Point(bubbleCanvas.Margin.Left, bubbleCanvas.Margin.Top); 
            ThicknessAnimation animation = bubbl.BubbleAnimation;
            sBoard.Duration = TimeSpan.FromMilliseconds(timerInterval);
            sBoard.Children.Add(animation);
            Storyboard.SetTarget(animation, bubbleCanvas);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.MarginProperty));
            sBoard.Begin(this, true);
        }

    }
}
