using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using OpenDeCoder.Coding;
using OpenDeCoder.UI;

namespace OpenDeCoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ICoder coder;
        private ParameterControl[] pControls;

        public MainWindow()
        {
            InitializeComponent();
            pControls = new ParameterControl[] { Param1, Param2, Param3, Param4, Param5, Param6 };
            Init_FillBrailleButtons();
            for (int i = 0; i < Program.Patterns.Length; ++i)
            { TestPatternsPanel.Children.Add(new MatchPatternControl(Program.Patterns[i].Name, Program.Patterns[i].regex, Program.Patterns[i].OverrideName)); }
            Pattern.TextArea.SelectionChanged += Pattern_CChanged;
            Pattern.TextArea.Caret.PositionChanged += Pattern_CChanged;
            Pattern.TextChanged += Pattern_CChanged;
            Pattern_CChanged(null, null);
            InsertICodingObjects();
            SelectCodingObject(new Coding_None());
        }

        private void Pattern_CChanged(object sender, EventArgs e)
        {
            EditCaptions_Lines.Text = "Lines: " + Pattern.LineCount.ToString();
            EditCaptions_Length.Text = "Length: " + Pattern.Text.Length.ToString();
            EditCaptions_Position.Text = "Position: " + Pattern.CaretOffset.ToString();
            int selectionLength = Pattern.SelectionLength;
            EditCaptions_SelectionLength.Text = "Length of Selection: " + selectionLength.ToString();
            string testPattern;
            if (selectionLength > 0)
            { testPattern = Pattern.SelectedText; }
            else
            { testPattern = Pattern.Text; }
            this.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < TestPatternsPanel.Children.Count; ++i)
                {
                    ((MatchPatternControl)TestPatternsPanel.Children[i]).TestPattern(testPattern);
                }
            });
            ApplyAction(true);
        }

        private void ArgumentsChanged(object sender, RoutedEventArgs e)
        {
            ApplyAction(true);
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyAction(false);
        }

        void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListView)sender).SelectedValue != null)
            {
                ICoder coding = ((ICoder)((ListViewItem)((ListView)sender).SelectedValue).Tag);
                SelectCodingObject(coding);
            }
        }

        private void ActionTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ActionTab.Items.Count; ++i)
            {
                if (i != ActionTab.SelectedIndex)
                {
                    ((ListView)((MetroTabItem)ActionTab.Items[i]).Content).SelectedIndex = -1;
                }
            }
        }

        private void ExtendedTitleBarMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aWindow = new AboutWindow() { Owner = this, ShowInTaskbar = false };
            aWindow.ShowDialog();
        }
    }
}
