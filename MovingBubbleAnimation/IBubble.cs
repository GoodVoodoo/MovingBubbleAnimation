using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MovingBubbleAnimation
{
    interface IBubble
    {
        EnumsAndConst.DeviceTypes DeviceType
        {
            get;
            set;
        }

        Canvas BubbleCanv
        {
            get;
            set;
        }

        string BubbleImagePath
        {
            get;
            set;
        }

        string CustomInnnerImagePath
        {
            get;
            set;
        }

        object InnerObject
        {
            get;
            set;
        }


        ThicknessAnimation BubbleAnimation
        {
            get;
            set;
        }


        Point StartBubblePosition
        {
            get;
            set;
        }

        System.Drawing.Bitmap popBubble
        {
            get;
            set;
        }

        BitmapSource popBubbleSource
        {
            get;
            set;
        }

    }
}
