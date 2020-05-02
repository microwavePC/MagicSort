using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MagicSort
{
    /// <summary>
    /// This is a generic class to sort several "List" instances.
    /// </summary>
    public static class MagicSorter
    {
        private const char dot = '.';

        /// <summary>
        /// Sort method for single sort key.
        /// </summary>
        /// <typeparam name="T">Type of target list class.</typeparam>
        /// <param name="targetList">Target list to sort.</param>
        /// <param name="sortKey">Sort key.</param>
        /// <param name="sortType">Sort type (Asc or Desc).</param>
        /// <exception cref="SortTargetPropertyNotExistException"></exception>
        public static void Sort<T>(ref List<T> targetList, string sortKey, SortType sortType = SortType.Asc)
            where T : class
        {
            if (!HasProperty<T>(sortKey))
            {
                throw new SortTargetPropertyNotExistException(
                    $"Sort key \"{sortKey}\" does not exist in class {typeof(T).Name}.");
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
        /// <exception cref="SortTargetPropertyNotExistException"></exception>
        public static void Sort<T>(ref List<T> targetList, List<Tuple<string, SortType>> sortKeySortTypePairs)
            where T : class
        {
            IOrderedEnumerable<T> orderedTarget = null;

            bool isFirstSort = true;
            foreach (Tuple<string, SortType> sortKeySortTypePair in sortKeySortTypePairs)
            {
                string sortKey = sortKeySortTypePair.Item1;
                SortType sortType = sortKeySortTypePair.Item2;

                if (!HasProperty<T>(sortKey))
                {
                    throw new SortTargetPropertyNotExistException(
                        $"Sort key \"{sortKey}\" does not exist in class {typeof(T).Name}.");
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
    }
}