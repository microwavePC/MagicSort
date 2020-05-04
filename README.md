# <img src="MagicSort.png" width="48" height="48"/>MagicSort
A wide-use dynamically multiple sorting library for .NET Core.<br>
You can sort several List by specifying property as string.

## Supported platforms
* .NET Core 2.0~
* .NET Framework 4.5~
* .NET Standard 2.0~

## Methods

### Linq-like style methods

```csharp
IOrderedEnumerable<T> OrderBy<T>(this List<T> targetList, string sortKey, SortType sortType = SortType.Asc)
```
This is a sort method for single sort key.<br>
* **List\<T\> targetList**<br>
  Sorting target list.
* **string sortKey**<br>
  Sort key string.<br>
  When you want to sort by deep-hierarchy property, please write this item using "." as delimiter.<br>
  e.g.) "SomeClass.SomeInnerClass.SomePropertyName"
* **SortType sortType**<br>
  Sort type (SortType.Asc or SortType.Desc).

```csharp
IOrderedEnumerable<T> OrderBy<T>(this List<T> targetList, Dictionary<string, SortType> sortKeySortTypePairs)
```
This is a sort method for multiple sort keys.<br>
* **List\<T\> targetList**<br>
  Sorting target list.
* **Dictionary<string, SortType> sortKeySortTypePairs**<br>
  Sort key string and sort type pairs.<br>
  When you want to sort by deep-hierarchy property, please write sort key using "." as delimiter.<br>

### Static methods on MagicSorter class (Method-like style)

```csharp
void MagicSorter.Sort<T>(ref List<T> targetList, string sortKey, SortType sortType = SortType.Asc)
```
This is a sort method for single sort key.<br>
* **List\<T\> targetList**<br>
  Sorting target list.
* **string sortKey**<br>
  Sort key string.<br>
  When you want to sort by deep-hierarchy property, please write this item using "." as delimiter.<br>
  e.g.) "SomeClass.SomeInnerClass.SomePropertyName"
* **SortType sortType**<br>
  Sort type (SortType.Asc or SortType.Desc).

```csharp
void MagicSorter.Sort<T>(ref List<T> targetList, Dictionary<string, SortType> sortKeySortTypePairs)
```
This is a sort method for multiple sort keys.<br>
* **List\<T\> targetList**<br>
  Sorting target list.
* **Dictionary<string, SortType> sortKeySortTypePairs**<br>
  Sort key string and sort type pairs.<br>
  When you want to sort by deep-hierarchy property, please write sort key using "." as delimiter.<br>

## How to use

1. Install ["MagicSort" NuGet package](https://www.nuget.org/packages/MagicSort) into your .NET Core project.
2. Add using-statement below into your source code.<br>
```csharp
using MagicSort;
```
3. Call method like below.
```csharp
var targetList = new List<SomeClass>
{
    new SomeClass // This is first item.
    {
        Prop1 = 123,
        Prop2 = 45.6,
        Prop3 = "ABC",
        Prop4 = new InnerClass
        {
            InnerProp1 = 23,
            InnerProp2 = "45"
            InnerProp3 = new DeepClass
            {
                DeepProp1 = 6.7
            }
        }
    },
    // ... some other items here ...
    new SomeClass // This is last item.
    {
        Prop1 = 987,
        Prop2 = 65.4,
        Prop3 = "ZYX",
        Prop4 = new InnerClass
        {
            InnerProp1 = 54,
            InnerProp2 = "32"
            InnerProp3 = new DeepClass
            {
                DeepProp1 = 1.0
            }
        }
    },
};

//========================================
// HOW TO USE METHOD-LIKE STYLE METHODS
//========================================

// Ascending sort by property "Prop1".
MagicSorter.Sort(ref list, "Prop1", SortType.Asc);

// Descending sort by deep property "Prop4.InnerProp3.DeepProp1".
MagicSorter.Sort(ref list, "Prop4.InnerProp3.DeepProp1", SortType.Desc);

// Sorting like
// "ORDER BY Prop2 ASC, Prop4.InnerProp2 DESC".
var sortKeySortTypePairs = new Dictionary<string, SortType>
{
    { "Prop2", SortType.Asc },
    { "Prop4.InnerProp2", SortType.Desc },
};
MagicSorter.Sort(ref list, sortKeySortTypePairs);

//========================================
// HOW TO USE LINQ-LIKE STYLE METHODS
//========================================

var sortedList = new List<SomeClass>();

// Ascending sort by property "Prop1".
sortedList = list
    .OrderBy("Prop1", SortType.Asc)
    .ToList();

// Descending sort by deep property "Prop4.InnerProp3.DeepProp1".
sortedList = list
    .OrderBy("Prop4.InnerProp3.DeepProp1", SortType.Desc)
    .ToList();

// Sorting like
// "ORDER BY Prop2 ASC, Prop4.InnerProp2 DESC".
sortedList = list
    .OrderBy(new Dictionary<string, SortType>
    {
        { "Prop2", SortType.Asc },
        { "Prop4.InnerProp2", SortType.Desc },
    })
    .ToList();
```
<br>

# Release Notes

### Version 1.2.0 (2020/05/04)

Added Linq-like methods.<br>

### Version 1.1.0 (2020/05/03)

Supported new frameworks and versions below.<br>
* .NET Core 2.0~
* .NET Framework 4.5~
* .NET Standard 2.0~
