using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Encryption_Railfence : ICoder
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
            get { return "Railfence"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Rails", ParameterType.TextBox, "3"),
                new ParameterInfo("Crypt Way", ParameterType.ComboBox, new string[] { "Decrypt", "Encrypt" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            string railsString = (string)parameters[0].Value;
            int cryptWayIndex = (int)parameters[1].Value;
            int rails = 0;
            if (int.TryParse(railsString, out rails))
            {
                if (rails > 1)
                {
                    if (rails < pattern.Length)
                    {
                        return Private_Decrypt(pattern, rails, (cryptWayIndex == 0));
                    }
                    else
                    {
                        feedback = new FeedbackInfo_Error("'Rails' Argument have to be less than text size");
                    }
                }
                else
                {
                    feedback = new FeedbackInfo_Error("'Rails' Argument have to be greater than 1");
                }
            }
            else
            {
                feedback = new FeedbackInfo_Error("'Rails' Argument is not a number");
            }
            return pattern;
        }

        public string[] BruteForce(string pattern)
        {
            int maxRails = Math.Min(5, pattern.Length - 1);
            string[] outStrings = new string[maxRails * 2];
            for (int i = 0; i < maxRails; ++i)
            {
                outStrings[i * 2] = Private_Decrypt(pattern, i + 2, true);
                outStrings[(i * 2) + 1] = Private_Decrypt(pattern, i + 2, false);
            }
            return outStrings;
        }

        private string Private_Decrypt(string pattern, int rails, bool decrypt)
        {
            if (decrypt)
            {
                //http://times.imkrisna.com/2011/01/rail-fence-cipher-c-source-code-2/
                List<List<int>> indexRails = new List<List<int>>();
                for (int i = 0; i < rails; i++) { indexRails.Add(new List<int>()); }
                int number = 0;
                int increment = 1;
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (number + increment == rails)
                    { increment = -1; }
                    else if (number + increment == -1)
                    { increment = 1; }
                    indexRails[number].Add(i);
                    number += increment;
                }
                int counter = 0;
                char[] buffer = new char[pattern.Length];
                for (int i = 0; i < rails; i++)
                {
                    for (int j = 0; j < indexRails[i].Count; j++)
                    {
                        buffer[indexRails[i][j]] = pattern[counter];
                        counter++;
                    }
                }
                return new string(buffer);
            }
            else
            {
                List<StringBuilder> textRails = new List<StringBuilder>();
                for (int i = 0; i < rails; ++i) { textRails.Add(new StringBuilder()); }
                char[] c = pattern.ToCharArray();
                int currentRail = 0;
                bool isIncreasing = true;
                for (int i = 0; i < c.Length; ++i)
                {
                    textRails[currentRail].Append(c[i]);
                    if (isIncreasing)
                    {
                        currentRail++;
                        if ((currentRail + 1) == rails)
                        { isIncreasing = false; }
                    }
                    else
                    {
                        currentRail--;
                        if (currentRail == 0)
                        { isIncreasing = true; }
                    }
                }
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < rails; ++i) { outString.Append(textRails[i].ToString()); }
                return outString.ToString();
            }
        }
    }
}
