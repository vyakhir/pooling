/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;

namespace Pooling.Assertions
{
    /// <summary>
    /// Assertion utilities.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Throws a <see cref="NullReferenceException"/> if given object is <c>null</c>. Otherwise does nothing.
        /// </summary>
        /// <param name="obj">object to check</param>
        /// <exception cref="NullReferenceException">if given object is <c>null</c></exception>
        public static void ReferenceNotNull(object obj)
        {
            ReferenceNotNull(obj, null);
        }

        /// <summary>
        /// Throws a <see cref="NullReferenceException"/> if given object is <c>null</c>. Otherwise does nothing.
        /// </summary>
        /// <param name="obj">object to check</param>
        /// <param name="message">error message</param>
        /// <exception cref="NullReferenceException">if given object is <c>null</c></exception>
        public static void ReferenceNotNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new NullReferenceException(message);
            }
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if given argument is <c>null</c>. Otherwise does nothing.
        /// </summary>
        /// <param name="arg">ArgumentNullException to check</param>
        /// <param name="name">argument name</param>
        /// <exception cref="ArgumentNullException">if given argument is <c>null</c></exception>
        public static void ArgumentNotNull(object arg, string name)
        {
            ArgumentNotNull(arg, name, null);
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if given argument is <c>null</c>. Otherwise does nothing.
        /// </summary>
        /// <param name="arg">ArgumentNullException to check</param>
        /// <param name="name">argument name</param>
        /// <param name="message">error message</param>
        /// <exception cref="ArgumentNullException">if given argument is <c>null</c></exception>
        public static void ArgumentNotNull(object arg, string name, string message)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(name, message);
            }
        }

        /// <summary>
        /// Throws a <see cref="ArgumentOutOfRangeException"/> if given validation expression fails for given argument.
        /// Otherwise does nothing.
        /// </summary>
        /// <param name="expression">validation expression to check</param>
        /// <param name="name">argument name</param>
        /// <param name="value">argument value</param>
        /// <exception cref="ArgumentOutOfRangeException">if given argument is <c>null</c></exception>
        public static void ArgumentInRange(bool expression, string name, object value)
        {
            ArgumentInRange(expression, name, value, null);
        }

        /// <summary>
        /// Throws a <see cref="ArgumentOutOfRangeException"/> if given validation expression fails for given argument.
        /// Otherwise does nothing.
        /// </summary>
        /// <param name="expression">validation expression to check</param>
        /// <param name="name">argument name</param>
        /// <param name="value">argument value</param>
        /// <param name="message">error message</param>
        /// <exception cref="ArgumentOutOfRangeException">if given argument is <c>null</c></exception>
        public static void ArgumentInRange(bool expression, string name, object value, string message)
        {
            if (!expression)
            {
                throw new ArgumentOutOfRangeException(name, value, message);
            }
        }
    }
}
