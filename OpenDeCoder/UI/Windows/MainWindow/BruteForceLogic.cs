using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using OpenDeCoder.UI;

namespace OpenDeCoder
{
    public partial class MainWindow
    {
        private void BruteForceButton_Click(object sender, RoutedEventArgs e)
        {
            BruteForceWindow bfWindow = new BruteForceWindow(Pattern.Text) { Owner = this, ShowInTaskbar = false };
            bfWindow.ShowDialog();
            if (bfWindow.PublishSolutionsCouldBeAvailable)
            {
                int iterations = 20;
                while (!bfWindow.PublicSolutionsAvailable)
                {
                    Thread.Sleep(50);
                    iterations++;
                    if (iterations == 20)
                    {
                        break;
                    }
                }
            }
            int solutionCount = 0;
            if (bfWindow.PublicSolutions != null)
            {
                if (bfWindow.PublicSolutions.Length > 0)
                {
                    StringBuilder outString = new StringBuilder();
                    for (int i = 0; i < bfWindow.PublicSolutions.Length; ++i)
                    {
                        if (bfWindow.PublicSolutions[i].MatchesWithKeyword)
                        {
                            solutionCount++;
                            outString.AppendLine(solutionCount.ToString().PadLeft(4, '0') + ": " + bfWindow.PublicSolutions[i].Pattern);
                        }
                    }
                    for (int i = 0; i < bfWindow.PublicSolutions.Length; ++i)
                    {
                        if (!bfWindow.PublicSolutions[i].MatchesWithKeyword)
                        {
                            solutionCount++;
                            outString.AppendLine(solutionCount.ToString().PadLeft(4, '0') + ": " + bfWindow.PublicSolutions[i].Pattern);
                        }
                    }
                    Pattern.Document.Insert(0, outString.ToString().Trim() + Environment.NewLine);
                }
                else
                {
                    Pattern.Document.Insert(0, "No solutions found" + Environment.NewLine);
                }
            }
        }
    }
}
