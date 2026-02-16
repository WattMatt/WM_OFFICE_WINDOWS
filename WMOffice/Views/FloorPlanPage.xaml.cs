using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI;
using System;
using WMOffice.Models;
using WMOffice.ViewModels;

namespace WMOffice.Views
{
    public sealed partial class FloorPlanPage : Page
    {
        private FloorPlanViewModel ViewModel => (FloorPlanViewModel)DataContext;

        public FloorPlanPage()
        {
            this.InitializeComponent();
            // DataContext is set in XAML
        }

        private void FloorPlanImage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var image = sender as Image;
            if (image == null) return;

            var point = e.GetCurrentPoint(image).Position;
            var width = image.ActualWidth;
            var height = image.ActualHeight;

            if (width == 0 || height == 0) return;

            var normalizedX = point.X / width;
            var normalizedY = point.Y / height;

            var newPhoto = new Photo
            {
                NormalizedX = normalizedX,
                NormalizedY = normalizedY,
                Note = "New Pin"
            };

            // Assuming ViewModel handles persistence
            _ = ViewModel.AddPinAsync(newPhoto);

            AddPinVisual(point.X, point.Y);
        }

        private void AddPinVisual(double x, double y)
        {
            var pin = new Grid
            {
                Width = 24,
                Height = 24,
                Margin = new Thickness(-12, -12, 0, 0)
            };

            var ellipse = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Red),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            };

            var textBlock = new TextBlock
            {
                Text = "📍",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16
            };

            pin.Children.Add(ellipse);
            pin.Children.Add(textBlock);

            Canvas.SetLeft(pin, x);
            Canvas.SetTop(pin, y);

            FloorPlanCanvas.Children.Add(pin);
        }
    }
}
