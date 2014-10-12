using System;
using System.Text;
using OpenDeCoder.Coding;
using OpenDeCoder.Misc;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls;

namespace OpenDeCoder
{
    public partial class MainWindow
    {
        private GridLength[] codergridLengths = null;

        private void SelectCodingObject(ICoder obj)
        {
            CodingActionName.Text = "Selected Action: " + obj.Name;
            ParameterInfo[] paramInfos = obj.RetrieveArguments();
            int definedCount = 0;
            if (paramInfos != null)
            { definedCount = paramInfos.Length; }
            if (definedCount < 4)
            {
                if (codergridLengths == null)
                {
                    codergridLengths = new GridLength[] { LeftCoderGrid.Width, RightCoderGrid.Width };
                    LeftCoderGrid.Width = new GridLength(1.0, GridUnitType.Star);
                    RightCoderGrid.Width = new GridLength(0.0, GridUnitType.Pixel);
                }
            }
            else
            {
                if (codergridLengths != null)
                {
                    LeftCoderGrid.Width = codergridLengths[0];
                    RightCoderGrid.Width = codergridLengths[1];
                    codergridLengths = null;
                }
            }
            for (int i = 0; i < 6; i++)
            {
                if ((i + 1) <= definedCount)
                { pControls[i].SetParameter(paramInfos[i]); }
                else
                { pControls[i].SetParameter(null); }
            }
            coder = obj;
            ApplyButton.IsEnabled = coder.IsApplyAble;
            ApplyAction(true);
        }

        private void InsertICodingObjects()
        {
            for (int i = 0; i < Program.Coder.Length; ++i)
            {
                ICoder obj = Program.Coder[i];
                if (obj.Insert)
                {
                    string HeaderString = obj.Header;
                    ListView lv = null;
                    for (int k = 0; k < ActionTab.Items.Count; ++k)
                    {
                        if (((string)((MetroTabItem)ActionTab.Items[k]).Header) == HeaderString)
                        {
                            lv = (ListView)((MetroTabItem)ActionTab.Items[k]).Content;
                        }
                    }
                    if (lv == null)
                    {
                        lv = new ListView();
                        lv.BorderThickness = new Thickness(0.0);
                        lv.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
                        lv.SelectionChanged += lv_SelectionChanged;
                        MetroTabItem mti = new MetroTabItem() { Header = HeaderString, Content = lv };
                        ActionTab.Items.Add(mti);
                    }
                    ListViewItem lvi = new ListViewItem() { Content = obj.Name, Tag = obj };
                    lv.Items.Add(lvi);
                }
            }
        }

        private void ApplyAction(bool isPreview = false)
        {
            CodingError.Text = "";
            CodingWarning.Text = "";
            CodingInfo.Text = "";
            string pattern;
            bool inSelection = (Pattern.SelectionLength > 0) && (Pattern.SelectionLength < Pattern.Document.TextLength);
            if (inSelection)
            { pattern = Pattern.SelectedText; }
            else
            { pattern = Pattern.Text; }
            if (coder == null)
            {
                return;
            }
            string processedString = string.Empty;
            int processedLength = 0;
            IFeedbackInfo feedback = null;
            ParameterResult[] paramResults = new ParameterResult[6];
            for (int i = 0; i < 6; ++i)
            {
                paramResults[i] = pControls[i].GetParameterResult();
            }
            try
            {
                processedString = coder.Decode(pattern, paramResults, isPreview, out feedback);
            }
            catch (Exception e)
            {
                Program.DebugEntrys.Add(new ProgramDebugEntry("Error while decoding string (coder: " + coder.Name + ", Error = \"" + e.Message + "\")", true));
                CodingError.Text = "Open DeCoder: Plugin failed while decoding!!!";
                if (isPreview)
                { PreviewPatternBox.Text = string.Empty; }
                return;
            }
            if (feedback != null)
            {
                if (feedback.Level == FeedbackInfoLevel.Info)
                {
                    CodingInfo.Text = feedback.Message;
                }
                else if (feedback.Level == FeedbackInfoLevel.Warning)
                {
                    CodingWarning.Text = feedback.Message;
                }
                else
                {
                    if (isPreview)
                    {
                        CodingError.Text = feedback.Message;
                    }
                    else
                    {
                        CodingError.Text = feedback.Message;
                        ErrorTextBlock.Text = feedback.Message;
                        ErrorBox.IsHitTestVisible = true;
                        Storyboard sb = new Storyboard();
                        DoubleAnimation da_one = new DoubleAnimation() { To = 1.0 };
                        da_one.Duration = new Duration(TimeSpan.FromMilliseconds(200.0));
                        DoubleAnimation da_two = new DoubleAnimation() { To = 0.0 };
                        da_two.Duration = new Duration(TimeSpan.FromMilliseconds(200.0));
                        da_two.BeginTime = TimeSpan.FromMilliseconds(2200.0);
                        da_two.Completed += (object sender, EventArgs e) => { ErrorBox.IsHitTestVisible = false; };
                        Storyboard.SetTarget(da_one, ErrorBox);
                        Storyboard.SetTarget(da_two, ErrorBox);
                        Storyboard.SetTargetProperty(da_one, new PropertyPath("Opacity"));
                        Storyboard.SetTargetProperty(da_two, new PropertyPath("Opacity"));
                        sb.Children.Add(da_one);
                        sb.Children.Add(da_two);
                        sb.Begin();
                    }
                }
                if (!feedback.Apply)
                {
                    if (isPreview)
                    { PreviewPatternBox.Text = string.Empty; }
                    return;
                }
                inSelection = inSelection && (!feedback.ExpelSelection); //nice workaround xD 
            }
            processedLength = processedString.Length;
            int insertIndex = Pattern.SelectionStart;
            if (inSelection)
            {
                StringBuilder outString = new StringBuilder(Pattern.Text);
                outString.Remove(insertIndex, Pattern.SelectionLength);
                outString.Insert(insertIndex, processedString);
                processedString = outString.ToString();
            }
            if (isPreview)
            {
                PreviewPatternBox.Text = processedString;
            }
            else
            {
                AddStateStack();
                Pattern.Text = processedString;
                Pattern.Focus();
                if (inSelection && processedLength > 0)
                { Pattern.Select(insertIndex, processedLength); }
                else
                { Pattern.Select(0, 0); }
            }
        }
    }
}
