using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Encryption_Atbash : ICoder
    {
        public bool Insert
        {
            get { return true; }
            set { }
        }
        public bool IsApplyAble
        {
            get { return true; }
            set { }
        }

        public bool BruteForceAble
        {
            get { return true; }
            set { }
        }
        public bool BruteForceDefaultSelected
        {
            get { return true; }
            set { }
        }

        public string Header
        {
            get { return "Encryption"; }
            set { }
        }

        public string Name
        {
            get { return "Atbach"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return null;
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            return Private_Decode(pattern);
        }

        public string[] BruteForce(string pattern)
        {
            return new string[1] { Private_Decode(pattern) };
        }

        private string Private_Decode(string pattern)
        {
            char[] c = pattern.ToCharArray();
            for (int i = 0; i < c.Length; ++i)
            {
                if (c[i] >= 'a' && c[i] <= 'z')
                { c[i] = (char)((int)'z' - ((int)c[i] - (int)'a')); }
                else if (c[i] >= 'A' && c[i] <= 'Z')
                { c[i] = (char)((int)'Z' - ((int)c[i] - (int)'A')); }
            }
            return new string(c);
        }
    }
}
