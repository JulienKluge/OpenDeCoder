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
using MahApps.Metro.Controls;
using OpenDeCoder.Coding;
using OpenDeCoder.Misc;
using System.Threading;
using System.Windows.Threading;

namespace OpenDeCoder.UI
{
    /// <summary>
    /// Interaction logic for BruteForceWindow.xaml
    /// </summary>
    public partial class BruteForceWindow : MetroWindow
    {
        public BruteForceSolution[] PublicSolutions;
        public bool PublicSolutionsAvailable = false;
        public bool PublishSolutionsCouldBeAvailable = false;
        private ICoder[] coder;
        private DispatcherTimer statusTimer;
        private Regex[] codeRegexes;
        private Regex keyWordRegex;

#if (DEBUG)
        private TextBlock arrayAccesssBox;
        private int W_ArrayAccesses = 0;
#endif

        public BruteForceWindow()
        {
            InitializeComponent();
            StartButton.IsEnabled = false;
        }
        public BruteForceWindow(string pattern)
        {
            InitializeComponent();
            PatternBox.Text = pattern;
            RegisteredCoderBox.Text = Program.Coder.Length.ToString() + " registered coder";
            List<ICoder> coderList = new List<ICoder>();
            for (int i = 0; i < Program.Coder.Length; ++i)
            {
                if (Program.Coder[i].BruteForceAble)
                {
                    ICoder c = Program.Coder[i];
                    CoderGrid.Items.Add(new CustomDataGridLine() { IsDefault = c.BruteForceDefaultSelected, Name = c.Name.PadRight(20, ' ') + " (" + c.Header + ")" });
                    coderList.Add(Program.Coder[i]);
                }
            }
            coder = coderList.ToArray();
            RegisteredBFCoderBox.Text = coder.Length.ToString() + " brute force able";
            if (coder.Length < 1) { StartButton.IsEnabled = false; }
            List<Regex> codeRegexesList = new List<Regex>();
            for (int i = 0; i < Program.Patterns.Length; ++i)
            {
                if (Program.Patterns[i].BruteForceAble)
                { codeRegexesList.Add(Program.Patterns[i].regex); }
            }
            codeRegexes = codeRegexesList.ToArray();
            keyWordRegex = IngressKeywords.Get_Ingress_Keywords_Regex();
#if (DEBUG)
            arrayAccesssBox = new TextBlock()
            { HorizontalAlignment = HorizontalAlignment.Right,  VerticalAlignment = VerticalAlignment.Top
                , Margin = new Thickness(0.0, 5.0, 2.0, 0.0), Text = "Array Accesses: 00000000"};
            Grid.SetColumn(arrayAccesssBox, 1);
            MainBFGrid.Children.Add(arrayAccesssBox);
#endif
        }

        public class CustomDataGridLine
        {
            public bool IsDefault { get; set; }
            public string Name { get; set; }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatternBox.Text.Length < 4)
            {
                MessageBox.Show(this, "Pattern have to be at least 4 characters long!", "Too short", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<ICoder> activeCoderList = new List<ICoder>();
            int activeCoderCount = 0;
            for (int i = 0; i < CoderGrid.Items.Count; ++i)
            {
                CustomDataGridLine cdgl = (CustomDataGridLine)CoderGrid.Items[i];
                if (cdgl.IsDefault)
                {
                    activeCoderList.Add(coder[i]);
                    ++activeCoderCount;
                }
            }
            if (activeCoderCount < 1)
            {
                MessageBox.Show(this, "You have to select at least 1 coder!", "Too few coders selected", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            coder = activeCoderList.ToArray();
            Storyboard sb = (Storyboard)this.Resources["StartUIAnim"];
            Overlay_Left.IsHitTestVisible = true;
            Overlay_Right.IsHitTestVisible = false;
            StateProgress.Value = 0.0;
            sb.Begin();
            W_OpenLast = W_OpenStep = 1;
            statusTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100.0) };
            statusTimer.Tick += statusTimer_Tick;
            statusTimer.Start();
            PublishSolutionsCouldBeAvailable = true;
#if (DEBUG)
            W_ArrayAccesses = 0;
#endif
            StartBruteForcing();
        }

        private void statusTimer_Tick(object sender, EventArgs e)
        {
            int deltaOpen = W_OpenLast - W_Open;
            if (deltaOpen > 0)
            { W_LastSeconds = ((1.0f / 5.0f) * (((float)W_Open / (float)deltaOpen) / 10.0f)) + ((4.0f / 5.0f) * W_LastSeconds); }
            W_OpenLast = W_Open;
            GeneratedBox.Text = "Generated: " + W_Created.ToString("N0");
            CheckedBox.Text = "Open: " + W_Open.ToString("N0");
            SolutionBox.Text = "Possible solutions: " + W_PossibleSolution.ToString("N0");
            StepBox.Text = "Current step: " + step;
            if (W_ThreadIsInWork != null)
            {
                for (int i = 0; i < W_ThreadIsInWork.Length; ++i)
                {
                    ((ThreadStatePanel)ActivePanel.Children[i]).IsWorking = W_ThreadIsInWork[i];
                }
            }
            TimeRemainingBox.Text = "Approximate time remaining for step: " + W_LastSeconds.ToString("n0") + " seconds";
            double stepSize = (1.0d / (double)W_Param_Depth);
            if (W_OpenStep > 0 && W_Open > 0)
            {
                StateProgress.Value = (Math.Max((double)step - 1.0, 0.0) * stepSize) + ((((double)W_OpenStep - (double)W_Open) / (double)W_OpenStep) * stepSize);
            }
            if (W_TimerSolutionCount < W_PossibleCodeSolutions.Count)
            {
                for (int i = W_TimerSolutionCount; i < W_PossibleCodeSolutions.Count; ++i)
                {
                    if (W_PossibleCodeSolutions[i].MatchesWithKeyword)
                    { ResultView.Items.Add((string)(W_PossibleCodeSolutions[i].Pattern + " KEYWORDED")); }
                    else
                    { ResultView.Items.Add((string)W_PossibleCodeSolutions[i].Pattern); }
                }
                W_TimerSolutionCount = W_PossibleCodeSolutions.Count;
            }
#if (DEBUG)
            arrayAccesssBox.Text = "Array Accesses: " + W_ArrayAccesses.ToString("n0").PadLeft(8, '0');
#endif
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (W_Work)
            {
                W_Running = false;
                W_Work = false;
                this.ShowCloseButton = false;
                e.Cancel = true;
            }
        }

        /*
        ** You're here, because you was curious, how I've managed to write the brute force algorithm.
        ** Well the principle behind it is simple.
         * At first, a MainThread is started 'MainWorker()'. It initializes and starts all WorkerThreads in an iddle mode.
         * The modes of the worker a determinated via some variables. 'bool W_Work = true' is keeping all threads alive.
         * If 'bool W_Running' is true, the Worker are trying to get Tasks from the TaskList 'List<BruteForceTask> W_Tasks'.
         * When they get some (they prefere to get more than one [tipp: 'const int MAX_TASKS_PER_THREAD']), the are processing
         * them with all avaialable, brute-force-able coder located in 'ICoder[] coder'. The results ar added to
         * 'List<BruteForceTask> W_Solutions'. BUT attention, these are NOT actual solutions. These patterns are checked with
         * regex patterns if they are matching any known ingress passcode. If they do, they are added to the actual-solution-list
         * 'List<BruteForceSolution> W_PossibleCodeSolutions'. If there are no more tasks to do, the mainthread will stall all worker
         * (W_Running = false) and:
         *      terminate all threads because the process is over
         *      OR
         *      copy the 'W_Solutions' to 'W_Tasks' and continue the process.
         * You should NOT:
         *      - Increase the WorkerThread-priority to gain performance (UI won't respond)
         *      - Access non-primitives types without locking them (except you don't need exact data e.g. the timer) (you'll risk a deadlock)
         *      - make more than 16 threads available (this just makes no sense)
         *      - make more than 7 iterations-levels available (too much memory needed in 32bit versions...and I'm not willing to provide 64 AND 32 bit assemblies)
         * What would be cool:
         *      - an option of predictive brute force (analyze pattern => choose coder) (should be implemented parallel to the normal mode)
         *      - make the coder faster so the brute force also gains speed
        ** 
        */

        private bool W_Work = false;
        private bool W_Running = false;
        private int step = 0;
        private List<BruteForceTask> W_Tasks;
        private List<BruteForceTask> W_Solutions;
        private List<BruteForceSolution> W_PossibleCodeSolutions;
        private bool[] W_ThreadIsInWork;

        private string W_Param_Pattern;
        private int W_Param_Depth;
        private int W_Param_ThreadCount;
        private bool W_PreProcessPossibleSolutions;
        private bool W_StopOnPossibleSolution;
        private bool W_DropStrangePatterns;

        private int W_Created;
        private int W_Open;
        private int W_OpenLast; //just for the timer
        private int W_OpenStep;
        private float W_LastSeconds = 0.0f;
        private int W_PossibleSolution;

        private int W_TimerSolutionCount = 0;

        private const int MAX_TASKS_PER_THREAD = 30;

        private void StartBruteForcing() //just to seperate it from the button_click event
        {
            W_Param_Pattern = this.PatternBox.Text;
            W_Param_ThreadCount = ThreadCountBox.SelectedIndex + 1;
            W_Param_Depth = DepthBox.SelectedIndex + 1;
            for (int i = 0; i < W_Param_ThreadCount; ++i)
            {
                ((ThreadStatePanel)ActivePanel.Children[i]).IsEnabled = true;
            }
            W_PreProcessPossibleSolutions = PreProcessBox.IsChecked.Value;
            W_StopOnPossibleSolution = StopOnSolutionBox.IsChecked.Value;
            W_DropStrangePatterns = DropStrangePatternBox.IsChecked.Value;
            Thread t = new Thread(new ThreadStart(MainWorker));
            t.Start();
        }

        private void MainWorker()
        {
            string pattern = W_Param_Pattern;
            int depth = W_Param_Depth;
            int threadCount = W_Param_ThreadCount;
            W_Work = true;
            W_Running = true;
            step = 1;
            W_Created = W_PossibleSolution = 0;
            W_Tasks = new List<BruteForceTask>();
            W_Tasks.Add(new BruteForceTask(pattern));
            W_Open = 1;
            W_Solutions = new List<BruteForceTask>();
            W_PossibleCodeSolutions = new List<BruteForceSolution>();
            Thread[] threads = new Thread[threadCount];
            W_ThreadIsInWork = new bool[threadCount];
            for (int i = 0; i < threadCount; ++i)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(Worker));
                threads[i].Priority = ThreadPriority.BelowNormal;
                threads[i].Start(i);
            }
            while (W_Work)
            {
                if (W_Open == 0)
                {
                    W_Running = false;
                    bool ThreadsInWork = false;
                    do
                    {
                        Thread.Sleep(100);
                        ThreadsInWork = false;
                        for (int i = 0; i < threadCount; ++i)
                        {
                            ThreadsInWork = ThreadsInWork | W_ThreadIsInWork[i];
                        }
                    }
                    while (ThreadsInWork);
                    step++;
                    if (step <= depth)
                    {
                        lock (W_Solutions)
                        {
                            lock (W_Tasks)
                            {
                                W_Tasks.Clear();
                                W_Tasks.AddRange(W_Solutions);
                                W_Solutions.Clear();
                                W_Tasks = (W_Tasks.Distinct(new TaskCompare())).ToList();
                                W_Open = W_Tasks.Count;
                                W_OpenStep = W_Open;
                                for (int i = 0; i < W_Tasks.Count; ++i)
                                { W_Tasks[i].Done = false; }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                    GC.Collect();
                    W_Running = true;
                }
                Thread.Sleep(100);
            }
            W_Work = false;
            W_Running = false;
            GC.Collect(); //supply more time for the worker to terminate
            for (int i = 0; i < threadCount; ++i)
            { threads[i].Join(3000); } //TODO: allow all threads 3 seconds, not every thread...
            this.Dispatcher.Invoke(() =>
            {
                if (statusTimer != null)
                {
                    statusTimer.Stop();
                }
                PublicSolutions = (W_PossibleCodeSolutions.Distinct(new SolutionCompare())).ToArray();
                PublicSolutionsAvailable = true;
                this.Close();
            });
        }

        private void Worker(object obj)
        {
            int index = (int)obj;
            Thread.Sleep(200); //intitial sleep
            while (W_Work)
            {
                while (W_Running)
                {
                    lock (W_ThreadIsInWork)
                    {
                        W_ThreadIsInWork[index] = true;
                    }
                    List<string> patternList = new List<string>();
                    lock (W_Tasks)
                    {
#if (DEBUG)
                        ++W_ArrayAccesses;
#endif
                        if (W_Open > 0)
                        {
                            int descreaseAmount = Math.Min(MAX_TASKS_PER_THREAD, W_Open);
                            for (int i = 0; i < descreaseAmount; ++i)
                            {
                                patternList.Add(W_Tasks[W_Tasks.Count - W_Open].Pattern);
                                W_Tasks[W_Tasks.Count - W_Open].Done = true;
                                --W_Open;
                            }
                        }
                        else
                        {
                            lock (W_ThreadIsInWork)
                            {
                                W_ThreadIsInWork[index] = false;
                            }
                            break;
                        }
                    }
                    List<BruteForceTask> privateSolutionList = new List<BruteForceTask>();
                    string[] patterns = patternList.ToArray();
                    for (int i = 0; i < patterns.Length; ++i)
                    {
                        for (int j = 0; j < coder.Length; ++j)
                        {
                            string[] solutions = coder[j].BruteForce(patterns[i]);
                            for (int k = 0; k < solutions.Length; ++k)
                            {
                                if (!string.IsNullOrWhiteSpace(solutions[k]))
                                {
                                    if (solutions[k].Length > 3)
                                    {
                                        ++W_Created;
                                        privateSolutionList.Add(new BruteForceTask(solutions[k]));
                                    }
                                }
                            }
                        }
                    }
                    string solution;
                    for (int i = 0; i < privateSolutionList.Count; ++i)
                    {
                        solution = privateSolutionList[i].Pattern;
                        if (W_DropStrangePatterns)
                        {
                            if (IsStrangePattern(solution))
                            {
                                privateSolutionList.RemoveAt(i);
                                --i;
                                continue;
                            }
                        }
                        Match regexMatch;
                        for (int j = 0; j < codeRegexes.Length; ++j)
                        {
                            regexMatch = codeRegexes[j].Match(solution);
                            if (regexMatch.Success)
                            {
                                if (regexMatch.Index == 0 && regexMatch.Length == solution.Length)
                                {
                                    bool isDistinct = true;
                                    for (int k = 0; k < W_PossibleCodeSolutions.Count; ++k)
                                    {
                                        if (W_PossibleCodeSolutions[k].Pattern == solution)
                                        {
                                            isDistinct = false;
                                            break;
                                        }
                                    }
                                    if (isDistinct)
                                    {
                                        ++W_PossibleSolution;
                                        W_PossibleCodeSolutions.Add(new BruteForceSolution(solution, keyWordRegex.IsMatch(solution)));
                                        if (!W_PreProcessPossibleSolutions)
                                        {
                                            privateSolutionList.RemoveAt(i);
                                            --i;
                                        }
                                        if (W_StopOnPossibleSolution)
                                        {
                                            W_Running = false;
                                            W_Work = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (step < W_Param_Depth)
                    {
                        lock (W_Solutions)
                        {
                            W_Solutions.AddRange(privateSolutionList);
                        }
                    }
                }
                lock (W_ThreadIsInWork)
                {
                    W_ThreadIsInWork[index] = false;
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// This method is used, to detect strange patterns. 
        /// It is allowing following characters: ascii, whitespace, length above 4
        /// 
        /// It seems, that this is totally waste of time, but it isn't!
        /// You have to see, that one pattern in iteration-level 1: can result in million patterns in iteration-level 5
        /// So we try to drop as much patterns as possible.
        /// </summary>
        /// <param name="pattern">String to check</param>
        /// <returns>true if pattern is strange, otherwise false</returns>
        private bool IsStrangePattern(string pattern)
        {
            int length = pattern.Length;
            if (length < 4)
            { return true; } //drop everything with a length 3 or less
            char c; //subchar used in for-loop
            for (int i = 0; i < length; ++i)
            {
                c = pattern[i]; //copy our char into our sub-var
                if ((c < ' ' || c > '~') && c != '\n' && c != '\r' && c != '\t') //check for false characters
                {
                    return true; //YOU SHALL NOT PASS
                }
            }
            return false; //everything is fine
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            W_Running = false;
            W_Work = false;
        }

        private class BruteForceTask
        {
            public string Pattern;
            public bool Done = false;
            public BruteForceTask(string pattern)
            {
                this.Pattern = pattern;
            }
        }

        private class SolutionCompare : IEqualityComparer<BruteForceSolution>
        {
            public bool Equals(BruteForceSolution x, BruteForceSolution y)
            {
                return (x.Pattern == y.Pattern);
            }
            public int GetHashCode(BruteForceSolution x)
            {
                return x.Pattern.GetHashCode();
            }
        }
        private class TaskCompare : IEqualityComparer<BruteForceTask>
        {
            public bool Equals(BruteForceTask x, BruteForceTask y)
            {
                return (x.Pattern == y.Pattern);
            }
            public int GetHashCode(BruteForceTask x)
            {
                return x.Pattern.GetHashCode();
            }
        }


    }

    public class BruteForceSolution
    {
        public string Pattern;
        public bool MatchesWithKeyword;
        public BruteForceSolution(string pattern, bool matchesWithKeyword)
        {
            this.Pattern = pattern;
            this.MatchesWithKeyword = matchesWithKeyword;
        }
    }
}
