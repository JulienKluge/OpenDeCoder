using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenDeCoder.UI
{
    /// <summary>
    /// Interaction logic for MatchPatternControl.xaml
    /// </summary>
    public partial class MatchPatternControl : UserControl
    {
        private Regex regex;
        private Storyboard OpenAnim, CloseAnim;
        private Storyboard SuccesColorAnim, PartialColorAnim, CollapsedColorAnim;

        private int position = 0;
        private int state = -1; //invalid, no state

        public MatchPatternControl()
        {
            InitializeComponent();
        }
        public MatchPatternControl(string Name, Regex RegexPattern, string RegexName = "")
        {
            InitializeComponent();
            OpenAnim = (Storyboard)this.Resources["OpenControl"];
            CloseAnim = (Storyboard)this.Resources["CloseControl"];
            SuccesColorAnim = (Storyboard)this.Resources["SuccesColorAnim"];
            PartialColorAnim = (Storyboard)this.Resources["PartialColorAnim"];
            CollapsedColorAnim = (Storyboard)this.Resources["CollapsedColorAnim"];
            try
            {
                NameBlock.Text = Name;
                regex = RegexPattern;
                if (string.IsNullOrWhiteSpace(RegexName))
                { RegexName = regex.ToString(); }
                RegexBlock.Text = RegexName;
            }
            catch (Exception) { }
        }

        public void TestPattern(string pattern)
        {
            if (regex == null)
            {
                if (state != 3)
                {
                    DecorLine.Stroke = NameBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));
                    OpenAnim.Begin();
                    state = 3;
                }
            }
            else
            {
                MatchCollection mc = regex.Matches(pattern);
                if (mc.Count > 0)
                {
                    if (mc.Count > 1 || mc[0].Length != pattern.Length)
                    {
                        if (state != 0)
                        {
                            PartialColorAnim.Begin();
                            OpenAnim.Begin();
                            state = 0;
                        }
                    }
                    else
                    {
                        if (state != 2)
                        {
                            SuccesColorAnim.Begin();
                            OpenAnim.Begin();
                            state = 2;
                        }
                        return;
                    }
                }
                else
                {
                    if (state != 1)
                    {
                        CollapsedColorAnim.Begin();
                        CloseAnim.Begin();
                        state = 1;
                    }
                }
            }
        }

        private void This_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string searchtext = Program.window.Pattern.Text;
            if (position >= (searchtext.Length - 1))
            { position = 0; }
            MatchCollection mc = regex.Matches(searchtext, position);
            if (mc.Count == 0)
            {
                if (position > 0)
                {
                    position = 0;
                    mc = regex.Matches(searchtext, position);
                }
            }
            if (mc.Count > 0)
            {
                position = mc[0].Index + mc[0].Length;
                Program.window.Pattern.Select(mc[0].Index, mc[0].Length);
                Program.window.Pattern.ScrollToLine(Program.window.Pattern.Document.GetLineByOffset(mc[0].Index).LineNumber);
            }
        }
    }
}
