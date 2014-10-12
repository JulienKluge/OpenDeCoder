using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    /// <summary>
    /// An FeedbackInfo object which contains data for the user.
    /// </summary>
    public interface IFeedbackInfo
    {
        /// <summary>
        /// The level of the feedback.
        /// It changes the behaviour of your message.
        /// </summary>
        FeedbackInfoLevel Level { get; set; }

        /// <summary>
        /// True if the decoded string should be applied, otherwise false.
        /// </summary>
        bool Apply { get; set; }

        /// <summary>
        /// True if the selection shouldn't be recognized, otherwise false.
        /// You know when you should set this to true, if you do not, the let it be false.
        /// </summary>
        bool ExpelSelection { get; set; }

        /// <summary>
        /// The message displayed to the user.
        /// </summary>
        string Message { get; set; }
    }

    /// <summary>
    /// The level of feedback.
    /// </summary>
    public enum FeedbackInfoLevel
    {
        /// <summary>
        /// Just a normal info. Displayed in green next to the apply button.
        /// </summary>
        Info,

        /// <summary>
        /// A warning displayed in orange next to the apply button.
        /// </summary>
        Warning,

        /// <summary>
        /// An error displayed in red next to the apply button.
        /// If it was a feedback for a non-preview process, the the message
        /// will be shown on an overlay!
        /// </summary>
        Error
    }

    /// <summary>
    /// An implementation of the IFeedbackInfo interface.
    /// It stops the apply process and is on error-level.
    /// </summary>
    public class FeedbackInfo_Error : IFeedbackInfo
    {
        public FeedbackInfoLevel Level
        {
            set { }
            get
            {
                return FeedbackInfoLevel.Error;
            }
        }
        public bool Apply
        {
            set { }
            get
            {
                return false;
            }
        }
        private bool _expelSelection;
        public bool ExpelSelection
        {
            set { }
            get
            {
                return _expelSelection;
            }
        }
        private string _message;
        public string Message
        {
            set { }
            get
            {
                return _message;
            }
        }
        public FeedbackInfo_Error(string MessageArg, bool ExpelSelectionArg = false)
        {
            this._message = MessageArg;
            this._expelSelection = ExpelSelectionArg;
        }
    }

    /// <summary>
    /// An implementation of the IFeedbackInfo interface.
    /// The decoded string will be applied. Message on warning-level.
    /// </summary>
    public class FeedbackInfo_Warning : IFeedbackInfo
    {
        public FeedbackInfoLevel Level
        {
            set { }
            get
            {
                return FeedbackInfoLevel.Warning;
            }
        }
        public bool Apply
        {
            set { }
            get
            {
                return true;
            }
        }
        private bool _expelSelection;
        public bool ExpelSelection
        {
            set { }
            get
            {
                return _expelSelection;
            }
        }
        private string _message;
        public string Message
        {
            set { }
            get
            {
                return _message;
            }
        }
        public FeedbackInfo_Warning(string Message, bool ExpelSelectionArg = false)
        {
            this._message = Message;
            this._expelSelection = ExpelSelectionArg;
        }
    }

    /// <summary>
    /// An implementation of the IFeedbackInfo interface.
    /// The decoded string will be applied. Message on info-level.
    /// </summary>
    public class FeedbackInfo_Info : IFeedbackInfo
    {
        public FeedbackInfoLevel Level
        {
            set { }
            get
            {
                return FeedbackInfoLevel.Info;
            }
        }
        public bool Apply
        {
            set { }
            get
            {
                return true;
            }
        }
        private bool _expelSelection;
        public bool ExpelSelection
        {
            set { }
            get
            {
                return _expelSelection;
            }
        }
        private string _message;
        public string Message
        {
            set { }
            get
            {
                return _message;
            }
        }
        public FeedbackInfo_Info(string Message, bool ExpelSelectionArg = false)
        {
            this._message = Message;
            this._expelSelection = ExpelSelectionArg;
        }
    }

}
