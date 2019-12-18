#region Usings

using System.Collections.Generic;

#endregion


namespace Eshva.DockerCompose.Extensions
{
    /// <summary>
    /// Tools for <see cref="IList{T}"/>
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Adds an element <paramref name="value"/> into <paramref name="list"/> if <paramref name="condition"/> is true.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of an element.
        /// </typeparam>
        /// <param name="list">
        /// The target list.
        /// </param>
        /// <param name="condition">
        /// Condition to test.
        /// </param>
        /// <param name="value">
        /// The element to add.
        /// </param>
        /// <returns>
        /// The same list.
        /// </returns>
        // ReSharper disable once UnusedMethodReturnValue.Global Could be used in the future.
        public static IList<TValue> AddConditionally<TValue>(this IList<TValue> list, bool condition, TValue value)
        {
            if (condition)
            {
                list.Add(value);
            }

            return list;
        }
    }
}
