using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using OpenDeCoder.Misc;

namespace OpenDeCoder.UI
{
    public class PatternEditor : TextEditor
    {
        public PatternEditor() : base()
        {
            this.ShowLineNumbers = true;
            this.WordWrap = true;
            if (Clipboard.ContainsText())
            {
                this.Text = Clipboard.GetText();
            }
            this.SyntaxHighlighting = new PatternHighlighting();
            this.TextArea.SelectionCornerRadius = 0.0;
            this.FontSize = 16.0;
            this.FontFamily = new FontFamily("Consolas");
            ContextMenu cMenu = new ContextMenu();
            MenuItem mi = new MenuItem() { Header = "Cut", InputGestureText = "STRG + X" };
            mi.Click += (object sender, RoutedEventArgs e) => { this.Cut(); };
            cMenu.Items.Add(mi);
            mi = new MenuItem() { Header = "Copy", InputGestureText = "STRG + C" };
            mi.Click += (object sender, RoutedEventArgs e) => { this.Copy(); };
            cMenu.Items.Add(mi);
            mi = new MenuItem() { Header = "Paste", InputGestureText = "STRG + V" };
            mi.Click += (object sender, RoutedEventArgs e) => { this.Paste(); };
            cMenu.Items.Add(mi);
            cMenu.Items.Add(new Separator());
            mi = new MenuItem() { Header = "Select All", InputGestureText = "STRG + A" };
            mi.Click += (object sender, RoutedEventArgs e) => { this.SelectAll(); };
            cMenu.Items.Add(mi);
            cMenu.Items.Add(new Separator());
            mi = new MenuItem() { Header = "Undo", InputGestureText = "STRG + Z" };
            mi.Click += (object sender, RoutedEventArgs e) => { this.Undo(); };
            cMenu.Items.Add(mi);
            mi = new MenuItem() { Header = "Redo", InputGestureText = "STRG + Y" };
            mi.Click += (object sender, RoutedEventArgs e) => { this.Redo(); };
            cMenu.Items.Add(mi);
            cMenu.Opened += cMenu_Opened;
            this.ContextMenu = cMenu;
        }

        void cMenu_Opened(object sender, RoutedEventArgs e)
        {
            ((MenuItem)((ContextMenu)sender).Items[0]).IsEnabled = !this.IsReadOnly;
            ((MenuItem)((ContextMenu)sender).Items[2]).IsEnabled = !this.IsReadOnly;
            ((MenuItem)((ContextMenu)sender).Items[6]).IsEnabled = (!this.IsReadOnly) && this.CanUndo;
            ((MenuItem)((ContextMenu)sender).Items[7]).IsEnabled = (!this.IsReadOnly) && this.CanRedo;
        }
    }

    public class PatternHighlighting : IHighlightingDefinition
    {
        public string Name { get { return "PatternHighlighting"; } }

        public HighlightingRuleSet MainRuleSet
        {
            get
            {
                HighlightingRuleSet rs = new HighlightingRuleSet();
                if (Program.Patterns == null)
                {
                    rs.Name = "MainRules";
                    return rs;
                }
                for (int i = 0; i < Program.Patterns.Length; ++i)
                {
                    if (Program.Patterns[i].Type == PatternType.ExactCode)
                    {
                        rs.Rules.Add(new HighlightingRule()
                        {
                            Regex = Program.Patterns[i].regex,
                            Color = new HighlightingColor() { Background = new SimpleHighlightingBrush(Color.FromArgb(255, 200, 255, 200)) }
                        });
                    }
                    else if (Program.Patterns[i].Type == PatternType.PossibleCode)
                    {
                        rs.Rules.Add(new HighlightingRule()
                        {
                            Regex = Program.Patterns[i].regex,
                            Color = new HighlightingColor() { Background = new SimpleHighlightingBrush(Color.FromArgb(255, 255, 230, 200)) }
                        });
                    }
                    else if (Program.Patterns[i].Type == PatternType.Keywords)
                    {
                        rs.Rules.Add(new HighlightingRule()
                        {
                            Regex = Program.Patterns[i].regex,
                            Color = new HighlightingColor() { Background = new SimpleHighlightingBrush(Color.FromArgb(255, 200, 200, 255)) }
                        });
                    }
                    else
                    {
                        rs.Rules.Add(new HighlightingRule()
                        {
                            Regex = Program.Patterns[i].regex,
                            Color = new HighlightingColor() { Background = new SimpleHighlightingBrush(Color.FromArgb(255, 255, 230, 230)) }
                        });
                    }
                }
                rs.Rules.Add(new HighlightingRule() //numbers
                {
                    Regex = new Regex(@"\d", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture),
                    Color = new HighlightingColor() { Foreground = new SimpleHighlightingBrush(Color.FromArgb(255, 150, 0, 0)) }
                });
                rs.Rules.Add(new HighlightingRule() //literals
                {
                    Regex = new Regex(@"\w", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture),
                    Color = new HighlightingColor() { Foreground = new SimpleHighlightingBrush(Color.FromArgb(255, 0, 0, 150)) }
                });
                rs.Rules.Add(new HighlightingRule() //tab
                {
                    Regex = new Regex(@"\t", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture),
                    Color = new HighlightingColor() { Background = new SimpleHighlightingBrush(Color.FromArgb(255, 220, 220, 220)) }
                });
                rs.Rules.Add(new HighlightingRule() //whitespace
                {
                    Regex = new Regex(@"\s", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture),
                    Color = new HighlightingColor() { Background = new SimpleHighlightingBrush(Color.FromArgb(255, 240, 240, 240)) }
                });
                rs.Name = "MainRules";
                return rs;
            }
        }

        public HighlightingRuleSet GetNamedRuleSet(string name) { return null; }
        public HighlightingColor GetNamedColor(string name) { return null; }
        public IEnumerable<HighlightingColor> NamedHighlightingColors { get; set; }

        public IDictionary<string, string> Properties
        {
            get
            {
                Dictionary<string, string> propertiesDictionary = new Dictionary<string, string>();
                propertiesDictionary.Add("DocCommentMarker", "///");
                return propertiesDictionary;
            }
        }
    }

    [Serializable]
    public sealed class SimpleHighlightingBrush : HighlightingBrush, ISerializable
    {
        readonly SolidColorBrush brush;

        internal SimpleHighlightingBrush(SolidColorBrush brush)
        {
            brush.Freeze();
            this.brush = brush;
        }

        public SimpleHighlightingBrush(Color color) : this(new SolidColorBrush(color)) { }

        public override Brush GetBrush(ITextRunConstructionContext context)
        {
            return brush;
        }

        public override string ToString()
        {
            return brush.ToString();
        }

        SimpleHighlightingBrush(SerializationInfo info, StreamingContext context)
        {
            this.brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(info.GetString("color")));
            brush.Freeze();
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("color", brush.Color.ToString(CultureInfo.InvariantCulture));
        }

        public override bool Equals(object obj)
        {
            SimpleHighlightingBrush other = obj as SimpleHighlightingBrush;
            if (other == null)
                return false;
            return this.brush.Color.Equals(other.brush.Color);
        }

        public override int GetHashCode()
        {
            return brush.Color.GetHashCode();
        }
    }
}
