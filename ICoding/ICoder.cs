namespace OpenDeCoder.Coding
{
    /// <summary>
    /// The ICoder interface provides base coder functionality for a 'Open DeCoder' Plugin.
    /// You must inherit from this interface to get your coder loaded!
    /// </summary>
    public interface ICoder
    {
        /// <summary>
        /// True, if the coder should be inserted into the user tabcontrol, otherwise false.
        /// This does NOT effect the BruteForceAble field.
        /// </summary>
        bool Insert { get; set; }

        /// <summary>
        /// True, if the coder should be able to applied to a text, otherwise false.
        /// (This is usefull, if you just want to display informations in the preview field)
        /// </summary>
        bool IsApplyAble { get; set; }

        /// <summary>
        /// True, if the coder should be registered for the brute force algorithm, otherwise false.
        /// ATTENTION: implement the 'string[] BruteForce(string)' method!
        /// </summary>
        bool BruteForceAble { get; set; }

        /// <summary>
        /// True, if the coder should be selected by default in the brute force menu, otherwise false.
        /// NOTE: Do set this to true ONLY when you're absolutely sure, it will be very helpfull for the process.
        /// </summary>
        bool BruteForceDefaultSelected { get; set; }

        /// <summary>
        /// The name of the Tabitem used. When possible, use an existing one.
        /// If the 'Insert' field is set to false, ignore this field.
        /// </summary>
        string Header { get; set; }

        /// <summary>
        /// The name of the coder.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// This method get called, when your coder is selected.
        /// You can either return null if you don't need any parameters, or return 
        /// a collection (max. 6 objects) of ParameterInfo objects.
        /// </summary>
        /// <returns>ParameterInfo-collection or null.</returns>
        ParameterInfo[] RetrieveArguments();

        /// <summary>
        /// This method get called, when the program wants a decoding from the coder.
        /// (Either a preview or an apply)
        /// </summary>
        /// <param name="pattern">The string to decode.</param>
        /// <param name="parameters">A ParameterResult-collection from your specified parameters.</param>
        /// <param name="isPreview">True if the decoding process is for a preview, otherwise false.</param>
        /// <param name="feedback">Overload null when everything went fine or a FeedbackInfo if something happened, the user needs feedback for.</param>
        /// <returns>The decoded string.</returns>
        string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback);

        /// <summary>
        /// This method get called, one time for each brute-force pattern.
        /// You can return decoded strings or an empty array.
        /// </summary>
        /// <param name="pattern">Decoded string array or an empty array.</param>
        /// <returns></returns>
        string[] BruteForce(string pattern);
    }
}
