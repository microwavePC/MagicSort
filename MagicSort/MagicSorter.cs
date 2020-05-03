using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MagicSort
{
    /// <summary>
    /// Static class for sorting several "List" instances.
    /// </summary>
    public static class MagicSorter
    {
        private const char dot = '.';
        private const string sortTargetPropertyNotExistExceptionMessageTemplate = "Sort key \"{0}\" does not exist in class {1}.";

        /// <summary>
        /// Sort method for single sort key.
        /// </summary>
        /// <typeparam name="T">Type of target list class.</typeparam>
        /// <param name="targetList">Target list to sort.</param>
        /// <param name="sortKey">Sort key.</param>
        /// <param name="sortType">Sort type (Asc or Desc).</param>
        /// <exception cref="SortTargetPropertyNotExistException">This exception is triggered when the sort key does not exists in the type T.</exception>
        public static void Sort<T>(ref List<T> targetList, string sortKey, SortType sortType = SortType.Asc)
            where T : class
        {
            if (!HasProperty<T>(sortKey))
            {
                throw new SortTargetPropertyNotExistException(
                    string.Format(sortTargetPropertyNotExistExceptionMessageTemplate, sortKey, typeof(T).Name));
            }

            Func<T, object> orderFunc = AssembleOrderFunc<T>(sortKey);

            switch (sortType)
            {
                case SortType.Asc:
                    targetList = targetList
                        .OrderBy(orderFunc)
                        .ToList();
                    break;
                case SortType.Desc:
                    targetList = targetList
                        .OrderByDescending(orderFunc)
                        .ToList();
                    break;
            }
        }

        /// <summary>
        /// Sort method for multiple sort keys.
        /// </summary>
        /// <typeparam name="T">Type of target list class.</typeparam>
        /// <param name="targetList">Target list to sort.</param>
        /// <param name="sortKeySortTypePairs">Pair of sort key and sort type (Asc or Desc).</param>
        /// <exception cref="SortTargetPropertyNotExistException">This exception is triggered when the sort key does not exists in the type T.</exception>
        public static void Sort<T>(ref List<T> targetList, Dictionary<string, SortType> sortKeySortTypePairs)
            where T : class
        {
            IOrderedEnumerable<T> orderedTarget = null;

            bool isFirstSort = true;
            foreach (KeyValuePair<string, SortType> sortKeySortTypePair in sortKeySortTypePairs)
            {
                string sortKey = sortKeySortTypePair.Key;
                SortType sortType = sortKeySortTypePair.Value;

                if (!HasProperty<T>(sortKey))
                {
                    throw new SortTargetPropertyNotExistException(
                        string.Format(sortTargetPropertyNotExistExceptionMessageTemplate, sortKey, typeof(T).Name));
                }

                Func<T, object> orderFunc = AssembleOrderFunc<T>(sortKey);

                if (isFirstSort)
                {
                    isFirstSort = false;
                    switch (sortType)
                    {
                        case SortType.Asc:
                            orderedTarget = targetList.OrderBy(orderFunc);
                            break;
                        case SortType.Desc:
                            orderedTarget = targetList.OrderByDescending(orderFunc);
                            break;
                    }
                }
                else
                {
                    switch (sortType)
                    {
                        case SortType.Asc:
                            orderedTarget = orderedTarget.ThenBy(orderFunc);
                            break;
                        case SortType.Desc:
                            orderedTarget = orderedTarget.ThenByDescending(orderFunc);
                            break;
                    }
                }
            }

            targetList = orderedTarget.ToList();
        }

        /// <summary>
        /// Sort method for single sort key.
        /// </summary>
        /// <typeparam name="T">Type of target list class.</typeparam>
        /// <param name="targetList">Target list to sort.</param>
        /// <param name="sortKey">Sort key.</param>
        /// <param name="sortType">Sort type (Asc or Desc).</param>
        /// <exception cref="SortTargetPropertyNotExistException">This exception is triggered when the sort key does not exists in the type T.</exception>
        /// <returns>IOrderedEnumerable object.</returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this List<T> targetList, string sortKey, SortType sortType = SortType.Asc)
            where T : class
        {
            if (!HasProperty<T>(sortKey))
            {
                throw new SortTargetPropertyNotExistException(
                    string.Format(sortTargetPropertyNotExistExceptionMessageTemplate, sortKey, typeof(T).Name));
            }

            Func<T, object> orderFunc = AssembleOrderFunc<T>(sortKey);
            IOrderedEnumerable<T> orderedEnumerable = null;

            switch (sortType)
            {
                case SortType.Asc:
                    orderedEnumerable = targetList
                        .OrderBy(orderFunc);
                    break;
                case SortType.Desc:
                    orderedEnumerable = targetList
                        .OrderByDescending(orderFunc);
                    break;
            }

            return orderedEnumerable;
        }

        /// <summary>
        /// Sort method for multiple sort keys.
        /// </summary>
        /// <typeparam name="T">Type of target list class.</typeparam>
        /// <param name="targetList">Target list to sort.</param>
        /// <param name="sortKeySortTypePairs">Pair of sort key and sort type (Asc or Desc).</param>
        /// <exception cref="SortTargetPropertyNotExistException">This exception is triggered when the sort key does not exists in the type T.</exception>
        public static IOrderedEnumerable<T> OrderBy<T>(this List<T> targetList, Dictionary<string, SortType> sortKeySortTypePairs)
            where T : class
        {
            IOrderedEnumerable<T> orderedTarget = null;

            bool isFirstSort = true;
            foreach (KeyValuePair<string, SortType> sortKeySortTypePair in sortKeySortTypePairs)
            {
                string sortKey = sortKeySortTypePair.Key;
                SortType sortType = sortKeySortTypePair.Value;

                if (!HasProperty<T>(sortKey))
                {
                    throw new SortTargetPropertyNotExistException(
                        string.Format(sortTargetPropertyNotExistExceptionMessageTemplate, sortKey, typeof(T).Name));
                }

                Func<T, object> orderFunc = AssembleOrderFunc<T>(sortKey);

                if (isFirstSort)
                {
                    isFirstSort = false;
                    switch (sortType)
                    {
                        case SortType.Asc:
                            orderedTarget = targetList.OrderBy(orderFunc);
                            break;
                        case SortType.Desc:
                            orderedTarget = targetList.OrderByDescending(orderFunc);
                            break;
                    }
                }
                else
                {
                    switch (sortType)
                    {
                        case SortType.Asc:
                            orderedTarget = orderedTarget.ThenBy(orderFunc);
                            break;
                        case SortType.Desc:
                            orderedTarget = orderedTarget.ThenByDescending(orderFunc);
                            break;
                    }
                }
            }

            return orderedTarget;
        }

        #region PRIVATE METHODS

        /// <summary>
        /// Judges the existence of property that aimed by sort key.
        /// </summary>
        /// <typeparam name="T">Type of target list class.</typeparam>
        /// <param name="sortKey">Sort key.</param>
        /// <returns>true: Target property exists. / false: Target property does not exists.</returns>
        private static bool HasProperty<T>(string sortKey)
        {
            List<string> sortKeyHierarchy = sortKey.Split(dot).ToList();
            Type innerType = typeof(T);

            foreach (string key in sortKeyHierarchy)
            {
                List<PropertyInfo> properties = innerType.GetRuntimeProperties().ToList();
                if (properties.Any(p => p.Name == key))
                {
                    innerType = properties.First(p => p.Name == key).PropertyType;
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Assembles the function object for sorting Linq method.
        /// </summary>
        /// <typeparam name="T">Type of target list class.</typeparam>
        /// <param name="sortKey">Sort key.</param>
        /// <returns>Function object.</returns>
        private static Func<T, object> AssembleOrderFunc<T>(string sortKey)
            where T : class
        {
            List<string> sortKeyHierarchy = sortKey.Split(dot).ToList();
            Func<T, object> orderFunc = x =>
            {
                object val = x;
                foreach (string key in sortKeyHierarchy)
                {
                    if (val == null)
                    {
                        return val;
                    }

                    val = val.GetType().GetRuntimeProperty(key).GetValue(val);
                }

                return val;
            };

            return orderFunc;
        }

        #endregion
    }
}