using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Encryption_ROT13 : ICoder
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
            get { return "Encryption"; }
            set { }
        }

        public string Name
        {
            get { return "ROT13"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return null;
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            char[] c = pattern.ToCharArray();
            for (int i = 0; i < c.Length; ++i)
            {
                if (c[i] >= 'a' && c[i] <= 'z')
                {
                    c[i] = (char)((int)c[i] + 13);
                    if (c[i] > 'z')
                    { c[i] = (char)((int)c[i] - 26); }
                }
                else if (c[i] >= 'A' && c[i] <= 'Z')
                {
                    c[i] = (char)((int)c[i] + 13);
                    if (c[i] > 'Z')
                    { c[i] = (char)((int)c[i] - 26); }
                }
            }
            return new string(c);
        }

        public string[] BruteForce(string pattern)
        {
            return new string[0];
        }
    }
}
