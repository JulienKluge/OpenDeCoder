using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    /// <summary>
    /// An object to specify a parameter for a coder.
    /// </summary>
    public class ParameterInfo
    {
        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string Name;

        /// <summary>
        /// The type of the parameter.
        /// This field is influencing the 'Value' field.
        /// </summary>
        public ParameterType Type;

        /// <summary>
        /// The value of the parameter.
        /// This depends on which Type is set:
        /// None: null
        /// TextBox: string
        /// ComboBox: string[]
        /// CheckBox: bool
        /// A wrong value can lead to errors and will throw exceptions!!!
        /// </summary>
        public object Value;

        /// <summary>
        /// Create a new instance of a ParameterInfo object.
        /// You should set all informations in this constructor because only here,
        /// the value-types are checked and will throw an exception on false types.
        /// </summary>
        /// <param name="ParamName">Name of the parameter.</param>
        /// <param name="ParamType">Type of the parameter.</param>
        /// <param name="ParamValue">Value of the parameter. This param depends on the type: 
        /// None: null; TextBox: string; ComboBox: string[]; CheckBox: bool</param>
        public ParameterInfo(string ParamName, ParameterType ParamType, object ParamValue)
        {
            this.Name = ParamName;
            this.Type = ParamType;
            if (ParamType == ParameterType.None)
            {
                Value = null;
            }
            else if (ParamType == ParameterType.TextBox)
            {
                if (ParamValue is string)
                { this.Value = ParamValue; }
                else
                { throw new Exception("Parameter type does not match its corresponding value type (" + ParamName + "::string)."); }
            }
            else if (ParamType == ParameterType.ComboBox)
            {
                if (ParamValue is string[])
                { this.Value = ParamValue; }
                else
                { throw new Exception("Parameter type does not match its corresponding value type (" + ParamName + "::string[])."); }
            }
            else if (ParamType == ParameterType.CheckBox)
            {
                if (ParamValue is bool)
                { this.Value = ParamValue; }
                else
                { throw new Exception("Parameter type does not match its corresponding value type (" + ParamName + "::bool)."); }
            }
        }
    }

    /// <summary>
    /// A result of a parameter.
    /// </summary>
    public class ParameterResult
    {
        /// <summary>
        /// The type of the parameter.
        /// </summary>
        public ParameterType Type;

        /// <summary>
        /// The value of the parameter.
        /// This field depends on the 'Type' field.
        /// None: null
        /// TextBox: string
        /// ComboBox: int (index of the selected item)
        /// CheckBox: bool
        /// </summary>
        public object Value;

        public ParameterResult(ParameterType ParamType, object ParamValue)
        {
            this.Type = ParamType;
            this.Value = ParamValue;
        }
    }

    /// <summary>
    /// Type enum for a ParameterInfo/-Result class.
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// An empty field.
        /// </summary>
        None,

        /// <summary>
        /// A TextBox
        /// </summary>
        TextBox,

        /// <summary>
        /// A ComboBox, readonly
        /// </summary>
        ComboBox,

        /// <summary>
        /// A CheckBox, not nullable
        /// </summary>
        CheckBox
    }
}
