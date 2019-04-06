using System;
using System.Collections.Generic;
using System.Text;

namespace FreddyBaseApi.Core.Model.ViewModels.Utils
{
    /// <summary>
    /// Method Response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MethodResponse<T>
    {
        /// <summary>
        /// True if everything is ok
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Result of GET method
        /// </summary>
        public T Result { get; set; }
        /// <summary>
        /// Error title (If error occurs)
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// Error description (If error occurs)
        /// </summary>
        public string Error_description { get; set; }
    }
}
