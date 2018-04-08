namespace WebApiBoilerplate.Protocol
{
    /// <summary>
    /// Server error description
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Unique ID of current error
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Code of error type
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Human readable error description
        /// </summary>
        public string Message { get; set; }
    }
}
