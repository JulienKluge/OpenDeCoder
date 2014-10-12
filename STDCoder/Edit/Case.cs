using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Edit_Case : ICoder
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
            get { return false; }
            set { }
        }

        public string Header
        {
            get { return "Edit"; }
            set { }
        }

        public string Name
        {
            get { return "Lower/Upper-Case"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Case Change", ParameterType.ComboBox, new string[] { "To Lower Case", "To Upper Case", "Exchange"})
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            int caseChangeIndex = (int)parameters[0].Value;
            return Private_Decrypt(pattern, caseChangeIndex);
        }

        public string[] BruteForce(string pattern)
        {
            return new string[] { Private_Decrypt(pattern, 0), Private_Decrypt(pattern, 1), Private_Decrypt(pattern, 2) };
        }

        private string Private_Decrypt(string pattern, int caseChangeIndex)
        {
            if (caseChangeIndex == 0)
            {
                return pattern.ToLowerInvariant();
            }
            else if (caseChangeIndex == 1)
            {
                return pattern.ToUpperInvariant();
            }
            else
            {
                char[] c = pattern.ToCharArray();
                for (int i = 0; i < c.Length; ++i)
                {
                    if (c[i] >= 'a' && c[i] <= 'z')
                    { c[i] = (char)((int)'A' + ((int)c[i] - (int)'a')); }
                    else if (c[i] >= 'A' && c[i] <= 'Z')
                    { c[i] = (char)((int)'a' + ((int)c[i] - (int)'A')); }
                }
                return new string(c);
            }
        }
    }
}
