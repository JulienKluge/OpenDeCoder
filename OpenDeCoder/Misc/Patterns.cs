using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace OpenDeCoder.Misc
{
    public static class PatternLoader
    {
        public static Pattern[] LoadPatterns()
        {
            List<Pattern> outRegexes = new List<Pattern>();
            string pluginDirectory = Path.Combine(Environment.CurrentDirectory, "coder/Patterns.xml");
            if (File.Exists(pluginDirectory))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(pluginDirectory);
                    if (xmlDoc.ChildNodes.Count > 0)
                    {
                        if (xmlDoc.ChildNodes[0].Name.ToLowerInvariant() == "patterns") //xml is case sensitive? nope!
                        {
                            XmlNodeList nodes = xmlDoc.ChildNodes[0].ChildNodes;
                            XmlNode node;
                            for (int i = 0; i < nodes.Count; ++i)
                            {
                                node = nodes[i];
                                if (node.Name.ToLowerInvariant() == "pattern")
                                {
                                    XmlAttributeCollection attributes = node.Attributes;
                                    string PatternName = string.Empty;
                                    string BruteForcePattern = "0";
                                    string Type = "4";
                                    for (int j = 0; j < attributes.Count; ++j)
                                    {
                                        if (attributes[j].Name.ToLowerInvariant() == "name")
                                        {
                                            PatternName = attributes[j].Value;
                                        }
                                        else if (attributes[j].Name.ToLowerInvariant() == "bruteforce")
                                        {
                                            BruteForcePattern = attributes[j].Value;
                                        }
                                        else if (attributes[j].Name.ToLowerInvariant() == "type")
                                        {
                                            Type = attributes[j].Value;
                                        }
                                        else
                                        {
                                            Program.DebugEntrys.Add(new ProgramDebugEntry("Unkown attribute \"" + attributes[j].Name + "\".", true));
                                        }
                                    }
                                    if (!string.IsNullOrWhiteSpace(PatternName))
                                    {
                                        try
                                        {
                                            Regex r;
                                            string overrideName = "";
                                            if (node.InnerText == "%ODC_IKeywordsRegex%")
                                            { r = IngressKeywords.Get_Ingress_Keywords_Regex(); overrideName = IngressKeywords.ingressKeywords.Length.ToString() + " keywords"; }
                                            else
                                            { r = new Regex(node.InnerText, RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase); }
                                            bool isBruteForceAble = false;
                                            if (!string.IsNullOrWhiteSpace(BruteForcePattern))
                                            { isBruteForceAble = BruteForcePattern != "0"; }
                                            PatternType pType = PatternType.Other;
                                            if (!string.IsNullOrWhiteSpace(Type))
                                            {
                                                int typeInt = 4;
                                                if (int.TryParse(Type, NumberStyles.Any, CultureInfo.InvariantCulture, out typeInt))
                                                {
                                                    if (typeInt < 5 && typeInt > 0)
                                                    {
                                                        pType = (PatternType)typeInt;
                                                    }
                                                }
                                            }
                                            outRegexes.Add(new Pattern(PatternName, r, isBruteForceAble, pType, overrideName));
                                            Program.DebugEntrys.Add(new ProgramDebugEntry("Pattern loaded: '" + PatternName + "'", false));
                                        }
                                        catch (Exception e)
                                        { Program.DebugEntrys.Add(new ProgramDebugEntry("Error while loading regex from pattern '" + PatternName + "' (" + e.Message + ").", true)); }
                                    }
                                    else
                                    {
                                        Program.DebugEntrys.Add(new ProgramDebugEntry("Node without name attribute (" + node.InnerText + ").", true));
                                    }
                                }
                                else
                                {
                                    if (node.NodeType != XmlNodeType.Comment)
                                    {
                                        Program.DebugEntrys.Add(new ProgramDebugEntry("Unkown node \"" + node.Name + "\".", true));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Program.DebugEntrys.Add(new ProgramDebugEntry("No main node. Patterns wont be parsed.", true));
                    }
                }
                catch (Exception e)
                {
                    Program.DebugEntrys.Add(new ProgramDebugEntry("Error while loading and parsing \\coder\\Patterns.xml!(" + e.Message + ")", true));
                }
            }
            else
            {
                Program.DebugEntrys.Add(new ProgramDebugEntry("\\coder\\Patterns.xml could not be found!", true));
            }
            Program.DebugEntrys.Add(new ProgramDebugEntry(outRegexes.Count.ToString() + " patterns loaded.", false));
            return outRegexes.ToArray();
        }
    }

    public class Pattern
    {
        public string Name = string.Empty;
        public Regex regex = null;
        public bool BruteForceAble = false;
        public string OverrideName = "";
        public PatternType Type;

        public Pattern(string patternName, Regex testRegex, bool isBruteForceAble, PatternType pType, string overrideName = "")
        {
            this.Name = patternName;
            this.regex = testRegex;
            this.BruteForceAble = isBruteForceAble;
            this.Type = pType;
            this.OverrideName = overrideName;
        }
    }

    public enum PatternType
    {
        ExactCode = 1,
        PossibleCode = 2,
        Keywords = 3,
        Other = 4
    }
}
