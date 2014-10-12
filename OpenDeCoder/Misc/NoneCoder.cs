namespace OpenDeCoder.Coding
{
    public class Coding_None : ICoder
    {
        public bool Insert
        {
            get { return false; }
            set { }
        }
        public bool IsApplyAble
        {
            get { return false; }
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
            get { return ""; }
            set { }
        }

        public string Name
        {
            get { return "None"; }
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
    }
}
