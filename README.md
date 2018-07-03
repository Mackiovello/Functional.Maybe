
# Mackiovello.Maybe

[![Build Status](https://travis-ci.org/Mackiovello/Mackiovello.Maybe.svg?branch=master)](https://travis-ci.org/Mackiovello/Mackiovello.Maybe)

Option types for C# with LINQ support and rich fluent syntax for many popular uses.

## Getting started

To add this to your project, execute this command in the directory with your `csproj` file:

```
dotnet add package Mackiovello.Maybe
```

Alternatively, add this directly to your `csproj` file:

```xml
<PackageReference Include="Mackiovello.Maybe" Version="1.0.0" />
```

Then, add `using Mackiovello.Maybe` to the top of all the files where you want to use this library.

## Examples

All these examples require that you have the following using statement:

```cs
using Mackiovello.Maybe
```

### Computing on maybe types

```cs
Maybe<string> maybeGood = "hello".ToMaybe();
Maybe<string> maybeJunk = Maybe<string>.Nothing;

var concat = from good in maybeGood
             from junk in maybeJunk
             select good + junk;

if (concat.IsNothing())
  Console.WriteLine("One of the strings was bad, could not concat");
```

LINQ terminates the computation if there's a `Nothing` at any point in the
computation.

### Running a computation with a maybe type:

```cs
string nullString = null;

nullString.ToMaybe().Do(str => 
{
  // str will never be null, ToMaybe guards against null and Do unwraps the value
});
```

### Guarding

You can check a condition on a maybe type and guard against them:

```cs
string name = "Bill Casarin";
Maybe<string> maybeName = from n in name.ToMaybe()
                          where n.StartsWith("Bill")
                          select n;
```

If the name didn't start with Bill, `maybeName` would be `Maybe<string>.Nothing`

### Maybe coalescing

Maybe has an operator similar to the null coalescing operator `??`. We achieve
optional short-circuit evaluation with lambdas:

```cs
Maybe<string> name1 = Maybe<string>.Nothing;
Maybe<string> name2 = "Some Name".ToMaybe();

Maybe<string> goodNameLazy = name1.Or(() => name2);
// this works too:
Maybe<string> goodName = name1.Or(name2);
// and this:
Maybe<string> goodName = name1.Or("goodName");
```

You can also convert value-kinded maybe types to `Nullable<T>`s:

```cs
Maybe<int> maybeNumber = Maybe<int>.Nothing;
Maybe<int> maybeAnotherNumber = (4).ToMaybe();

int? ok = maybeNumber.ToNullable() ?? maybeAnotherNumber.ToNullable();
```

### Extracting values

Sometime you want to pull out a value with a default value in case of `Nothing`:

```cs
Maybe<string> possibleString = Maybe<string>.Nothing;
string goodString = possibleString.OrElse("default");
```

The default parameter can also be lazy:

```cs
string goodString = possibleString.OrElse(() => doHeavyComputationForString());
```

Or you can throw an exception instead:

```cs
string val = null;
try 
{
  val = (Maybe<string>.Nothing).OrElse(() => new Exception("no value"));
} 
catch (Exception) 
{
  // exception will be thrown
}
```

Or, finally, you can just get the default value for that type:

```cs
string val = maybeString.OrElseDefault();
```

### Why not use Nullable<T> instead?

Nullable<T> only works on value types. Maybe<T> works on both value and
reference types. It also has LINQ support. 

## More interesting examples

### Getting the first element of a list

```cs
public static Maybe<T> Head<T>(this IEnumerable<T> xs) 
{
  foreach(var x in xs)
    return x.ToMaybe();
  return Maybe<T>.Nothing;
}
```

Now lets get a bunch of heads!

```cs
var result = from h1 in list1.Head()
             from h2 in list2.Head()
             from h3 in list3.Head()
             select ConsumeHeads(h1, h2, h3);
```

ConsumeHeads will never run unless all Head() calls return valid results.

### Lookups

Here's a function for getting a value out of a dictionary:

```cs
public static Maybe<T2> Lookup<T, T2>(this IDictionary<T, T2> d, T key) 
{
  var getter = MaybeFunctionalWrappers.Wrap(d.TryGetValue);
  return getter(key);
}
```

### Parsing

```cs

public static Maybe<int> ParseInt(string s) 
{
  var parser = MaybeFunctionalWrappers.Wrap(int.TryParse);
  return parser(s);
}
```

### Lookup + Parsing!

```cs
var parsedFromDict = from val in d.Lookup("key")
                     from parsedVal in ParseInt(val)
                     select parsedVal;
```

## Build and test

To build this library, you need to install [.NET Core SDK 2.1](https://www.microsoft.com/net/download). If it's installed, you should be able to do this:

```
$ dotnet --version
2.1.301
```

If your version is higher or equal to 2.1, you are good to go.

To build, execute `dotnet build` at the root of the repository. Similarly, to test, execute `dotnet test` at the root:

```
$ dotnet test
Build started, please wait...
Build completed.

Test run for /home/user/Mackiovello.Maybe/test/Mackiovello.Maybe.Tests/bin/Debug/netcoreapp2.1/Mackiovello.Maybe.Tests.dll(.NETCoreApp,Version=v2.1)
Microsoft (R) Test Execution Command Line Tool Version 15.7.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

Total tests: 19. Passed: 19. Failed: 0. Skipped: 0.
Test Run Successful.
Test execution time: 1.1823 Seconds
```

The tests are xUnit tests, so you can execute them with any xUnit test runner.
