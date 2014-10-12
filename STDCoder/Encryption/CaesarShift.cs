using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Encryption_CaeserShift : ICoder
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
            get { return "Caesar Cipher"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Shift", ParameterType.TextBox, "1"),
                new ParameterInfo("Step Increase", ParameterType.TextBox, "0"),
                new ParameterInfo("Shift Type", ParameterType.ComboBox, new string[] { "Caeser (ROT N)", "ASCII" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            string shiftString = (string)parameters[0].Value;
            string increasingString = (string)parameters[1].Value;
            int shiftTypeIndex = (int)parameters[2].Value;
            int shift = 0; int increasing = 0;
            if (int.TryParse(shiftString, out shift))
            {
                if (int.TryParse(increasingString, out increasing))
                {
                    return Private_Decrypt(pattern, shift, increasing, (shiftTypeIndex == 0));
                }
                else
                {
                    feedback = new FeedbackInfo_Error("'Step Increase' Argument is not a number");
                }
            }
            else
            {
                feedback = new FeedbackInfo_Error("'Shift' Argument is not a number");
            }
            return pattern;
        }

        public string[] BruteForce(string pattern)
        {
            string[] outStrings = new string[25];
            for (int i = 0; i < 25; ++i)
            { outStrings[i] = Private_Decrypt(pattern, i + 1, 0, true); }
            return outStrings;
        }

        private string Private_Decrypt(string pattern, int shift, int increasing, bool UseCeasarType)
        {
            if (UseCeasarType)
            {
                char[] c = pattern.ToCharArray();
                for (int i = 0; i < c.Length; ++i)
                {
                    shift = shift - (int)((double)Math.Floor((double)shift / 26.0) * 26.0);
                    if (c[i] >= 'a' && c[i] <= 'z')
                    {
                        c[i] = (char)((int)c[i] + shift);
                        if (c[i] > 'z')
                        { c[i] = (char)((int)c[i] - 26); }
                        else if (c[i] < 'a')
                        { c[i] = (char)((int)c[i] + 26); }

                    }
                    else if (c[i] >= 'A' && c[i] <= 'Z')
                    {
                        c[i] = (char)((int)c[i] + shift);
                        if (c[i] > 'Z')
                        { c[i] = (char)((int)c[i] - 26); }
                        else if (c[i] < 'A')
                        { c[i] = (char)((int)c[i] + 26); }

                    }
                    shift = shift + increasing;
                }
                return new string(c);
            }
            else
            {
                byte[] c = Encoding.ASCII.GetBytes(pattern);
                for (int i = 0; i < c.Length; ++i)
                {
                    shift = shift - (int)((double)Math.Floor((double)shift / 255.0) * 255.0);
                    int num = (int)c[i] + shift;
                    if (num > 255)
                    { num = num - 255; }
                    c[i] = (byte)num;
                    shift = shift + increasing;
                }
                return Encoding.ASCII.GetString(c);
            }
        }
    }
}
