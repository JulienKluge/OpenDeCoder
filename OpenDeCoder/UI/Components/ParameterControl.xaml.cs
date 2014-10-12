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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenDeCoder.Coding;

namespace OpenDeCoder.UI
{
    public partial class ParameterControl : UserControl
    {
        public static readonly RoutedEvent ParameterChangedEvent = EventManager.RegisterRoutedEvent(
        "ParameterChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ParameterControl));

        private Storyboard NoneFade;
        private Storyboard TextBoxFade;
        private Storyboard ComboBoxFade;
        private Storyboard CheckBoxFade;
        private ParameterType currentType = ParameterType.None;

        public ParameterControl()
        {
            InitializeComponent();
            NoneFade = (Storyboard)this.Resources["NoneFade"];
            TextBoxFade = (Storyboard)this.Resources["TextBoxFade"];
            ComboBoxFade = (Storyboard)this.Resources["ComboBoxFade"];
            CheckBoxFade = (Storyboard)this.Resources["CheckBoxFade"];
        }

        public void SetParameter(ParameterInfo pInfo)
        {
            if (pInfo == null)
            { pInfo = new ParameterInfo("", ParameterType.None, null); }
            if (pInfo.Type == ParameterType.None)
            {
                if (currentType != ParameterType.None)
                {
                    currentType = ParameterType.None;
                    ParamTextBox.IsHitTestVisible = false;
                    ParamComboBox.IsHitTestVisible = false;
                    ParamCheckBox.IsHitTestVisible = false;
                    NoneFade.Begin();
                }
            }
            else if (pInfo.Type == ParameterType.TextBox)
            {
                ParamName.Text = pInfo.Name;
                ParamTextBox.Text = (string)pInfo.Value;
                if (currentType != ParameterType.TextBox)
                {
                    currentType = ParameterType.TextBox;
                    ParamTextBox.IsHitTestVisible = true;
                    ParamComboBox.IsHitTestVisible = false;
                    ParamCheckBox.IsHitTestVisible = false;
                    TextBoxFade.Begin();
                }
            }
            else if (pInfo.Type == ParameterType.ComboBox)
            {
                ParamName.Text = pInfo.Name;
                ParamComboBox.Items.Clear();
                string[] newItems = (string[])pInfo.Value;
                if (newItems != null)
                {
                    for (int i = 0; i < newItems.Length; ++i)
                    {
                        ParamComboBox.Items.Add(new ComboBoxItem() { Content = newItems[i] });
                    }
                    if (newItems.Length > 0)
                    {
                        ParamComboBox.SelectedIndex = 0;
                    }
                }
                if (currentType != ParameterType.ComboBox)
                {
                    currentType = ParameterType.ComboBox;
                    ParamTextBox.IsHitTestVisible = false;
                    ParamComboBox.IsHitTestVisible = true;
                    ParamCheckBox.IsHitTestVisible = false;
                    ComboBoxFade.Begin();
                }
            }
            else if (pInfo.Type == ParameterType.CheckBox)
            {
                ParamName.Text = string.Empty;
                ParamCheckBox.Content = pInfo.Name;
                ParamCheckBox.IsChecked = (bool)pInfo.Value;
                if (currentType != ParameterType.CheckBox)
                {
                    currentType = ParameterType.CheckBox;
                    ParamTextBox.IsHitTestVisible = false;
                    ParamComboBox.IsHitTestVisible = false;
                    ParamCheckBox.IsHitTestVisible = true;
                    CheckBoxFade.Begin();
                }
            }
        }

        public ParameterResult GetParameterResult()
        {
            if (currentType == ParameterType.TextBox)
            {
                return new ParameterResult(currentType, (object)ParamTextBox.Text);
            }
            else if (currentType == ParameterType.ComboBox)
            {
                return new ParameterResult(currentType, (object)ParamComboBox.SelectedIndex);
            }
            else if (currentType == ParameterType.CheckBox)
            {
                return new ParameterResult(currentType, (object)ParamCheckBox.IsChecked.Value);
            }
            else
            {
                return null;
            }
        }

        public object GetParameterValue()
        {
            if (currentType == ParameterType.TextBox)
            {
                return (object)ParamTextBox.Text;
            }
            else if (currentType == ParameterType.ComboBox)
            {
                return (object)ParamComboBox.SelectedIndex;
            }
            else if (currentType == ParameterType.CheckBox)
            {
                return (object)ParamCheckBox.IsChecked.Value;
            }
            else
            {
                return null;
            }
        }

        public void SetParameterValue(object value)
        {
            if (currentType == ParameterType.TextBox)
            {
                ParamTextBox.Text = (string)value;
            }
            else if (currentType == ParameterType.ComboBox)
            {
                ParamComboBox.SelectedIndex = (int)value;
            }
            else if (currentType == ParameterType.CheckBox)
            {
                ParamCheckBox.IsChecked = (bool)value;
            }
        }

        public event RoutedEventHandler ParameterChanged
        {
            add { AddHandler(ParameterChangedEvent, value); }
            remove { RemoveHandler(ParameterChangedEvent, value); }
        }

        private void PrivateParametersChangedEvent(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs eventArgs = new RoutedEventArgs(ParameterControl.ParameterChangedEvent, this);
            RaiseEvent(eventArgs);
        }
    }
}
