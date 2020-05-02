using MagicSort.Test.DummyClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace MagicSort.Test
{
    public class UnitTest
    {
        [Theory(DisplayName ="Unit test for single sort")]
        [MemberData(nameof(TestParamsForSingleSort))]
        public void TestForSingleSort(
            List<DummyClass1> list,
            string sortKey,
            SortType sortType)
        {
            MagicSorter.Sort(ref list, sortKey, sortType);

            for (int i = 1; i < list.Count; i++)
            {
                object itemBefore = GetTargetValue(list[i - 1], sortKey);
                object itemAfter = GetTargetValue(list[i], sortKey);

                CompareItems(itemBefore, itemAfter, sortType);
            }
        }


        [Theory(DisplayName = "Unit test for multi sort")]
        [MemberData(nameof(TestParamsForMultiSort))]
        public void TestForMultiSort(
            List<DummyClass1> list,
            List<Tuple<string, SortType>> sortKeySortTypePairs)
        {
            MagicSorter.Sort(ref list, sortKeySortTypePairs);

            for (int i = 1; i < list.Count; i++)
            {
                int lookingDepthNo = 0;
                string sortKey = sortKeySortTypePairs[lookingDepthNo].Item1;
                SortType sortType = sortKeySortTypePairs[lookingDepthNo].Item2;

                object itemBefore = GetTargetValue(list[i - 1], sortKey);
                object itemAfter = GetTargetValue(list[i], sortKey);

                while (itemBefore.Equals(itemAfter))
                {
                    lookingDepthNo++;

                    if (lookingDepthNo >= sortKeySortTypePairs.Count)
                    {
                        break;
                    }

                    sortKey = sortKeySortTypePairs[lookingDepthNo].Item1;
                    sortType = sortKeySortTypePairs[lookingDepthNo].Item2;

                    itemBefore = GetTargetValue(list[i - 1], sortKey);
                    itemAfter = GetTargetValue(list[i], sortKey);
                }

                CompareItems(itemBefore, itemAfter, sortType);
            }
        }


        private static object GetTargetValue<T>(T targetObject, string sortKey)
            where T : class
        {
            List<string> sortKeyHierarchy = sortKey.Split('.').ToList();

            object value = targetObject;
            foreach (string key in sortKeyHierarchy)
            {
                if (value == null)
                {
                    return value;
                }

                value = value.GetType().GetRuntimeProperty(key).GetValue(value);
            }

            return value;
        }


        private void CompareItems(object itemBefore, object itemAfter, SortType sortType)
        {
            if (itemBefore is int)
            {
                int itemBeforeVal = (int)itemBefore;
                int itemAfterVal = (int)itemAfter;

                if (sortType == SortType.Asc)
                {
                    Assert.True(itemBeforeVal <= itemAfterVal);
                }
                else if (sortType == SortType.Desc)
                {
                    Assert.True(itemBeforeVal >= itemAfterVal);
                }
            }
            else if (itemBefore is double)
            {
                double itemBeforeVal = (double)itemBefore;
                double itemAfterVal = (double)itemAfter;

                if (sortType == SortType.Asc)
                {
                    Assert.True(itemBeforeVal <= itemAfterVal);
                }
                else if (sortType == SortType.Desc)
                {
                    Assert.True(itemBeforeVal >= itemAfterVal);
                }
            }
            else if (itemBefore is decimal)
            {
                decimal itemBeforeVal = (decimal)itemBefore;
                decimal itemAfterVal = (decimal)itemAfter;

                if (sortType == SortType.Asc)
                {
                    Assert.True(itemBeforeVal <= itemAfterVal);
                }
                else if (sortType == SortType.Desc)
                {
                    Assert.True(itemBeforeVal >= itemAfterVal);
                }
            }
            else if (itemBefore is long)
            {
                long itemBeforeVal = (long)itemBefore;
                long itemAfterVal = (long)itemAfter;

                if (sortType == SortType.Asc)
                {
                    Assert.True(itemBeforeVal <= itemAfterVal);
                }
                else if (sortType == SortType.Desc)
                {
                    Assert.True(itemBeforeVal >= itemAfterVal);
                }
            }
            else if (itemBefore is string)
            {
                string itemBeforeStr = (string)itemBefore;
                string itemAfterStr = (string)itemAfter;

                int compareResult = itemBeforeStr.CompareTo(itemAfterStr);

                if (sortType == SortType.Asc)
                {
                    Assert.True(compareResult == -1 || compareResult == 0);
                }
                else if (sortType == SortType.Desc)
                {
                    Assert.True(compareResult == 0 || compareResult == 1);
                }
            }
        }


        public static IEnumerable<object[]> TestParamsForSingleSort => new List<object[]>
        {
            new object[]
            {
                TestData.DummyClass1List,
                "Property1",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property1",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property2",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property2",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property3",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property3",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyX",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyX",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyY",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyY",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyZ",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyZ",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyA",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyA",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyB",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyB",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyC",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyC",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyI",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyI",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyJ",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyJ",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyK.Property1",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyK.Property1",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyK.Property2",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyK.Property2",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyK.Property3",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyK.Property3",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyK.Property4.PropertyW.PropertyD.PropertyI",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyK.Property4.PropertyW.PropertyD.PropertyI",
                SortType.Desc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyK.Property4.PropertyW.PropertyD.PropertyJ",
                SortType.Asc
            },
            new object[]
            {
                TestData.DummyClass1List,
                "Property4.PropertyW.PropertyD.PropertyK.Property4.PropertyW.PropertyD.PropertyJ",
                SortType.Desc
            },
        };

        public static IEnumerable<object[]> TestParamsForMultiSort => new List<object[]>
        {
            new object[]
            {
                TestData.DummyClass1List,
                new List<Tuple<string, SortType>>
                {
                    Tuple.Create("Property1", SortType.Asc),
                    Tuple.Create("Property4.PropertyW.PropertyA", SortType.Desc),
                    Tuple.Create("Property4.PropertyZ", SortType.Desc),
                }
            },
            new object[]
            {
                TestData.DummyClass1List,
                new List<Tuple<string, SortType>>
                {
                    Tuple.Create("Property4.PropertyW.PropertyD.PropertyK.Property4.PropertyW.PropertyD.PropertyI", SortType.Desc),
                    Tuple.Create("Property1", SortType.Asc),
                    Tuple.Create("Property4.PropertyW.PropertyA", SortType.Desc),
                    Tuple.Create("Property2", SortType.Asc),
                    Tuple.Create("Property4.PropertyW.PropertyD.PropertyJ", SortType.Asc),
                }
            },
        };
    }
}
