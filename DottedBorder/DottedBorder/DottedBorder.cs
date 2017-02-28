using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace DottedBorder
{
    public sealed class DottedBorder : ContentControl
    {
        public SolidColorBrush StrokeBrush
        {
            get { return (SolidColorBrush)GetValue(StrokeBrushProperty); }
            set { SetValue(StrokeBrushProperty, value); }
        }

        public static readonly DependencyProperty StrokeBrushProperty =
            DependencyProperty.Register("StrokeBrush", typeof(SolidColorBrush), typeof(DottedBorder), new PropertyMetadata(null));

        public DoubleCollection DashedStroke
        {
            get { return (DoubleCollection)GetValue(DashedStrokeProperty); }
            set { SetValue(DashedStrokeProperty, value); }
        }

        public static readonly DependencyProperty DashedStrokeProperty =
            DependencyProperty.Register("DashedStroke", typeof(DoubleCollection), typeof(DottedBorder), new PropertyMetadata(null));

        public DottedBorder()
        {
            this.DefaultStyleKey = typeof(DottedBorder);
        }
    }
}
