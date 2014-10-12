using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OpenDeCoder.Misc;
using OpenDeCoder.Coding;

namespace OpenDeCoder
{
    public partial class MainWindow
    {
        private List<StateInformationObject> stateStack = new List<StateInformationObject>();
        private int stateStackIndex = 0;

        private void AddStateStack()
        {
            if (coder != null)
            {
                if (coder.Insert)
                {
                    if (stateStackIndex < stateStack.Count)
                    {
                        stateStack.RemoveRange(stateStackIndex, stateStack.Count - stateStackIndex);
                        stateStackIndex = stateStack.Count;
                    }
                    object[] paramValues = new object[pControls.Length];
                    for (int i = 0; i < pControls.Length; ++i)
                    {
                        paramValues[i] = pControls[i].GetParameterValue();
                    }
                    stateStack.Add(new StateInformationObject(Pattern.Text, ActionTab.SelectedIndex, ((ListView)ActionTab.SelectedContent).SelectedIndex, paramValues));
                    stateStackIndex++;
                    UndoActionButton.IsEnabled = true;
                    RedoActionButton.IsEnabled = false;
                    if (stateStackIndex > 50)
                    {
                        stateStack.RemoveAt(0);
                        stateStackIndex--;
                    }
                }
            }
        }

        private void RedoActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (coder != null)
            {
                if (coder.Insert)
                {
                    if ((stateStackIndex + 1) < stateStack.Count)
                    {
                        stateStackIndex++;
                        RedoActionButton.IsEnabled = ((stateStackIndex + 1) < stateStack.Count);
                        UndoActionButton.IsEnabled = true;
                        Pattern.Text = stateStack[stateStackIndex].Text;
                        ActionTab.SelectedIndex = stateStack[stateStackIndex].SelectedTabIndex;
                        ((ListView)ActionTab.SelectedContent).SelectedIndex = stateStack[stateStackIndex].SelectedLVIndex;
                        for (int i = 0; i < pControls.Length; ++i)
                        {
                            pControls[i].SetParameterValue(stateStack[stateStackIndex].parameterValues[i]);
                        }
                    }
                }
            }
        }

        private void UndoActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (coder != null)
            {
                if (coder.Insert)
                {
                    if (stateStackIndex > 0)
                    {
                        if (stateStackIndex == stateStack.Count)
                        {
                            AddStateStack();
                            stateStackIndex--;
                        }
                        stateStackIndex--;
                        UndoActionButton.IsEnabled = (stateStackIndex > 0);
                        RedoActionButton.IsEnabled = true;
                        Pattern.Text = stateStack[stateStackIndex].Text;
                        ActionTab.SelectedIndex = stateStack[stateStackIndex].SelectedTabIndex;
                        ((ListView)ActionTab.SelectedContent).SelectedIndex = stateStack[stateStackIndex].SelectedLVIndex;
                        for (int i = 0; i < pControls.Length; ++i)
                        {
                            pControls[i].SetParameterValue(stateStack[stateStackIndex].parameterValues[i]);
                        }
                    }
                }
            }
        }

    }
}
