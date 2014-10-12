using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using OpenDeCoder.UI;

namespace OpenDeCoder
{
    public partial class MainWindow
    {
        private void BrailleButton_Click(object sender, RoutedEventArgs e)
        {
            if (BrailleRow.Height == 0.0)
            { AnimateBrailleRowHeight(30.0); }
            else
            { AnimateBrailleRowHeight(0.0); }
        }

        private void BrailleCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (BrailleRow.Height > 0.0)
            { AnimateBrailleRowHeight(0.0); }
        }

        private void AnimateBrailleRowHeight(double h)
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation da = new DoubleAnimation() { To = h, Duration = new Duration(TimeSpan.FromMilliseconds(200.0)) };
            Storyboard.SetTarget(da, BrailleRow);
            Storyboard.SetTargetProperty(da, new PropertyPath("Height"));
            sb.Children.Add(da);
            sb.Begin();
        }

        private void Init_FillBrailleButtons()
        {
            Style flatButtonStyle = (Style)this.Resources["SquareButtonStyle"];
            Button closeButton = new Button() { Width = 100, Content = "Close", FontSize = 14.0, Margin = new Thickness(1.0, 0.0, 3.0, 0.0) };
            closeButton.Style = flatButtonStyle;
            closeButton.Click += BrailleCloseButton_Click;
            BrailleRow.Children.Add(closeButton);
            for (char c = 'A'; c <= 'Z'; ++c)
            {
                BrailleButton b = new BrailleButton() { Content = c.ToString() };
                b.Click += OnBrailleButtonClicked;
                BrailleRow.Children.Add(b);
            }
            for (int i = 0; i < 10; ++i)
            {
                BrailleButton b = new BrailleButton() { Content = i.ToString() };
                b.Click += OnBrailleButtonClicked;
                BrailleRow.Children.Add(b);
            }
            string[] extraCharacters = new string[] { "(", ")", "!", "?", ",", ".", ";", ":", "[", "]", "#", "-" };
            for (int i = 0; i < extraCharacters.Length; ++i)
            {
                BrailleButton b = new BrailleButton() { Content = extraCharacters[i] };
                b.Click += OnBrailleButtonClicked;
                BrailleRow.Children.Add(b);
            }
        }

        private void OnBrailleButtonClicked(object sender, RoutedEventArgs e)
        {
            Pattern.AppendText((string)((BrailleButton)sender).Content);
        }
    }
}
