using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Transposition_Rotate : ICoder
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
            get { return "Transposition"; }
            set { }
        }

        public string Name
        {
            get { return "Rotate"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Rotation", ParameterType.ComboBox, new string[] { "90° Clockwise", "90° Clockwise Whitespace Alignment", "90° Counter-Clockwise", "90° Counter-Clockwise Whitespace Alignment" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            int rotationIndex = (int)parameters[0].Value;
            return Private_Decrypt(pattern, rotationIndex);
        }

        public string[] BruteForce(string pattern)
        {
            return new string[] { Private_Decrypt(pattern, 0), Private_Decrypt(pattern, 1), Private_Decrypt(pattern, 2), Private_Decrypt(pattern, 3) };
        }

        private string Private_Decrypt(string pattern, int rotationIndex)
        {
            string[] lines = pattern.Split('\n');
            int maxLength = 0;
            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = lines[i].Trim('\r');
                maxLength = Math.Max(maxLength, lines[i].Length);
            }
            if (rotationIndex == 0 || rotationIndex == 1) //clockwise
            {
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < maxLength; ++i)
                {
                    StringBuilder builder = new StringBuilder();
                    for (int j = lines.Length - 1; j >= 0; --j)
                    {
                        if (lines[j].Length > i)
                        {
                            builder.Append(((string)lines[j])[i]);
                        }
                        else if (rotationIndex == 1)
                        { builder.Append(" "); }
                    }
                    if (i == (maxLength - 1))
                    { outString.Append(builder.ToString()); }
                    else
                    { outString.AppendLine(builder.ToString()); }
                }
                return outString.ToString();
            }
            else
            {
                StringBuilder outString = new StringBuilder();
                for (int i = (maxLength - 1); i >= 0; --i)
                {
                    StringBuilder builder = new StringBuilder();
                    for (int j = 0; j < lines.Length; ++j)
                    {
                        if (lines[j].Length > i)
                        {
                            builder.Append(((string)lines[j])[i]);
                        }
                        else if (rotationIndex == 3)
                        { builder.Append(" "); }
                    }
                    if (i == 0)
                    { outString.Append(builder.ToString()); }
                    else
                    { outString.AppendLine(builder.ToString()); }
                }
                return outString.ToString();
            }
        }
    }
}
