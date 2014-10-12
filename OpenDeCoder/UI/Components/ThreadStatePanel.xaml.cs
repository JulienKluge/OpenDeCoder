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

namespace OpenDeCoder.UI
{
    /// <summary>
    /// Interaction logic for ThreadStatePanel.xaml
    /// </summary>
    public partial class ThreadStatePanel : UserControl
    {
        public new bool IsEnabled
        {
            set
            {
                _IsEnabled = value;
                SetState();
            }
            get { return _IsEnabled; }
        }
        private bool _IsEnabled = false;

        public bool IsWorking
        {
            set
            {
                _IsWorking = value;
                SetState();
            }
            get { return _IsWorking; }
        }
        private bool _IsWorking = false;

        private int State = 0;

        Brush[] stateBrushes;
        public ThreadStatePanel()
        {
            InitializeComponent();
        }

        private void SetState()
        {
            if (stateBrushes == null)
            {
                stateBrushes = new Brush[] { Brushes.DarkGray, Brushes.Red, Brushes.Green };
            }
            if (_IsEnabled)
            {
                if (_IsWorking)
                {
                    if (State != 2)
                    {
                        this.Background = stateBrushes[2];
                        State = 2;
                    }
                }
                else
                {
                    if (State != 1)
                    {
                        this.Background = stateBrushes[1];
                        State = 1;
                    }
                }
            }
            else
            {
                if (State != 0)
                {
                    this.Background = stateBrushes[0];
                    State = 0;
                }
            }
        }

        public string ThreadNumber
        {
            set
            {
                ThreadNumberBox.Text = value;
            }
            get
            {
                return ThreadNumberBox.Text;
            }
        }

    }
}
