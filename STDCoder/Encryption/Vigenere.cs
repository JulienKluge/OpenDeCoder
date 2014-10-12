using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Encryption_Vigenere : ICoder
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
            get { return "Vigenére Cipher"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Passphrase", ParameterType.TextBox, "password"),
                new ParameterInfo("Crypt Way", ParameterType.ComboBox, new string[] { "Decrypt", "Encrypt" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            string passphrase = (string)parameters[0].Value;
            StringBuilder passphraseBuilder = new StringBuilder(passphrase.ToLowerInvariant());
            int cryptWayIndex = (int)parameters[1].Value;
            char subChar; bool editedPassPhrase = false;
            for (int i = 0; i < passphraseBuilder.Length; ++i)
            {
                subChar = passphraseBuilder[i];
                if (subChar < 'a' || subChar > 'z')
                {
                    editedPassPhrase = true;
                    passphraseBuilder.Remove(i, 1);
                    --i;
                }
            }
            passphrase = passphraseBuilder.ToString();
            if (passphrase.Length > 0)
            {
                if (editedPassPhrase)
                { feedback = new FeedbackInfo_Warning("Invalid characters removed from 'Passphrase' argument"); }
                char[] pp = passphrase.ToCharArray();
                int ppi = 0;
                char[] c = pattern.ToCharArray();
                bool isLowercase; int subtractVal;
                for (int i = 0; i < c.Length; ++i)
                {
                    if ((isLowercase = (c[i] >= 'a' && c[i] <= 'z')) || (c[i] >= 'A' && c[i] <= 'Z'))
                    {
                        if (isLowercase)
                        { subtractVal = (int)'a'; }
                        else
                        { subtractVal = (int)'A'; }
                        int num = (int)c[i] - subtractVal;
                        if (cryptWayIndex == 0)
                        { num = num - (pp[ppi] - (int)'a'); }
                        else
                        { num = num + (pp[ppi] - (int)'a'); }
                        if (num < 0) { num = num + 26; }
                        c[i] = (char)(subtractVal + (num % 26));
                        ppi++;
                        if (ppi == pp.Length)
                        { ppi = 0; }
                    }
                }
                return new string(c);
            }
            else
            {
                feedback = new FeedbackInfo_Error("Passphrase was empty");
            }
            return pattern;
        }

        public string[] BruteForce(string pattern)
        {
            return new string[0];
        }
    }
}
