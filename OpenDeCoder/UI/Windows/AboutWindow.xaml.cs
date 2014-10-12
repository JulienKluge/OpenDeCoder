using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using MahApps.Metro.Controls;
using System.Reflection;

namespace OpenDeCoder.UI
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : MetroWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
            VersionBlock.Content = "v" +  Assembly.GetEntryAssembly().GetName().Version.ToString();
            LicenseField.Text = LicenseString;
            StringBuilder devConsoleStr = new StringBuilder();
            for (int i = 0; i < Program.DebugEntrys.Count; ++i)
            {
                if (Program.DebugEntrys[i].IsError)
                { devConsoleStr.AppendLine("[ERROR] " + Program.DebugEntrys[i].TimeStamp.ToLongTimeString() + ": " + Program.DebugEntrys[i].Message); }
                else
                { devConsoleStr.AppendLine(Program.DebugEntrys[i].TimeStamp.ToLongTimeString() + ": " + Program.DebugEntrys[i].Message); }
            }
            DevConsoleField.Text = devConsoleStr.ToString();
        }

        private void HyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }


        private void ExtendedTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void About_GetInterfaceTemplate(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(InterfaceTemplate);
        }

        private string InterfaceTemplate = @"    public class Coding_Template : ICoder
    {
        public bool Insert
        {
            get { return true; }
            set { }
        }
        public bool IsApplyAble
        {
            get { return true; }
            set { }
        }

        public bool BruteForceAble
        {
            get { return false; }
            set { }
        }
        public bool BruteForceDefaultSelected
        {
            get { return false; }
            set { }
        }

        public string Header
        {
            get { return ""Edit""; }
            set { }
        }

        public string Name
        {
            get { return ""Template""; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return null;
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            return pattern;
        }

        public string[] BruteForce(string pattern)
        {
            return new string[0];
        }
    }";

        private string LicenseString = @"The MIT License (MIT)

Copyright © 2014 Julien Kluge

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.";
    }
}
