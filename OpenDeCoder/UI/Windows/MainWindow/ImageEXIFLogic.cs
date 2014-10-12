using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using ExifLibrary;

namespace OpenDeCoder
{
    public partial class MainWindow
    {
        private BitmapImage bitmapImage;
        private string bitmapImagePath;

        private void ImageServiceFlyOut_IsOpenChanged(object sender, EventArgs e)
        {
            ImageServicesButton.IsEnabled = !ImageServiceFlyOut.IsOpen;
        }

        private void ImageServicesButton_Click(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                ImageServiceFlyOut.IsOpen = !ImageServiceFlyOut.IsOpen;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                FileInfo fInfo = new FileInfo(((TextBox)sender).Text);
                if (fInfo.Exists)
                {
                    Uri sourceUri = new Uri(fInfo.FullName);
                    BitmapImage bImg = new BitmapImage(sourceUri);
                    this.bitmapImage = bImg.Clone();
                    this.bitmapImagePath = fInfo.FullName;
                    FlyOutImagePreview.Source = this.bitmapImage;
                }
            }
            catch (Exception) { FlyOutImagePreview.Source = null; }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.Filter = "Image Files (*.bmp, *.jpg, *.png, *.ico, *.tiff)|(*.bmp;*.jpg;*.jpeg;*.png;*.ico;*.tiff)|All Files (*.*)|*.*";
            ofd.Multiselect = false;
            ofd.Title = "Select an image";
            ofd.ShowDialog(this);
            if (!string.IsNullOrWhiteSpace(ofd.FileName))
            {
                try
                {
                    FileInfo fInfo = new FileInfo(ofd.FileName);
                    if (fInfo.Exists)
                    {
                        FlyOutImagePath.Text = fInfo.FullName;
                    }
                }
                catch (Exception) { FlyOutImagePreview.Source = null; }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (bitmapImage != null)
            {
                if (File.Exists(bitmapImagePath))
                {
                    try
                    {
                        StringBuilder outString = new StringBuilder();
                        ExifFile eFile = ExifFile.Read(bitmapImagePath);
                        foreach (ExifProperty item in eFile.Properties.Values)
                        {
                            outString.AppendLine(item.Name.PadRight(20, ' ') + ": " + item.Value);
                        }
                        Pattern.Text = outString.ToString() + Pattern.Text;
                    }
                    catch (Exception error) { Pattern.Text = "Error while reading exif data. (" + error.Message + ")" + Environment.NewLine + Pattern.Text; }
                    ImageServiceFlyOut.IsOpen = false;
                }
            }
        }
    }
}
