# Stencil Micro IoC Container

Stencil is a micro Inversion of Control container.  It was written with the following design goals in mind:

 - A single file with no dependencies outside the .NET 4.0 Client Profile
 - Support for property injection via public interfaces
 - For use in small projects without the need to take any dependencies

##Configuration

Using stencil is easy.  Copy and paste the [Stencil.cs](https://raw.github.com/flipbit/stencil/master/Source/Stencil/Stencil.cs) file into your project.  The container can be accessed via a built-in Singleton or by creating a new instance of the Stencil class.

```c#
// Using the Singleton
var foo = Stencil.Instance.Resolve<IFoo>();

// Creating a new instance
var stencil = new Stencil();
stencil.Initialize();

var bar = stencil.Resolve<IBar>();
```
By default, Stencil will scan the executing assembly for all interfaces.  You can specify additional assemblies to search by adding them to the Assemblies collection in the Defaults class.  A static Defaults class exists on the Stencil object for use by the Singleton, or in the event no parameters are supplied when calling the Initialize() method.

```c#
// Configuring assemblies to be scanned
Stencil.Defaults.Assemblies.Add(typeof(Foo).Assembly);
Stencil.Defaults.Assemblies.Add(typeof(Bar).Assembly);

// Configuring object resoluton options
var defaults = new Defaults
{
    UsePropertyInjection = true,
    UseSingletons = true
};

var stencil = new Stencil();
stencil.Initilize(defaults);
```

Setting flags for UsePropertyInjection and UseSingletons determine the behavior of objects that are resolved from the container.

##Usage

You can access items from the container by calling the Resolve() method.

```c#
public interface IFoo
{
    string SayFoo();
}

public class Foo : IFoo
{
    public string SayFoo()
    {
        return "foo";
    }
}
  	
var foo = Stencil.Instance.Resolve<IFoo>();
```

##Property Injection

If the object resolved from by the container has any public properties with interfaces known by the container, these are initialized.


```c#
public interface IBar
{
    string SayBar();
}

public class Bar : IBar
{
	// Set by container
    public IFoo Foo { get; set; }

    // Set to null
    public IBar RecursiveBar { get; set;}

    public string SayBar()
    {
        return Foo.SayFoo();
    }
}

var bar = Stencil.Instance.Resolve<IBar>();

Assert.AreEqual("foo", service.SayBar());
```

If a child object references a parent, the value of the parent is set to null.

##Generic Lists

Generic lists are supported for properties implementing an IList.  The list is instantiated with all objects 
in the container matching the declared interface.  This is useful for implementing [Visitor](http://en.wikipedia.org/wiki/Visitor_pattern) 
and [Chain-of-responsibility](http://en.wikipedia.org/wiki/Chain-of-responsibility_pattern) patterns.

```c#
public interface IFizzBuzz
{
    string SayFizzBuzz();
}

[Order(1)]
public class Fizz : IFizzBuzz
{
    public string SayFizzBuzz()
    {
        return "fizz";
    }
}

[Order(2)]
public class Buzz : IFizzBuzz
{
    public string SayFizzBuzz()
    {
        return "buzz";
    }
}

public class FizzBuzz
{
    public IList<IFizzBuzz> FizzBuzzers { get; set; }
}

var fizzbuzz = Stencil.Instance.Resolve<FizzBuzz>();

Assert.AreEqual(2, fizzbuzz.FizzBuzzers.Count);
Assert.AreEqual("fizz", fizzbuzz.FizzBuzzers[0].SayFizzBuzz());
Assert.AreEqual("buzz", fizzbuzz.FizzBuzzers[1].SayFizzBuzz());
```

If order is import when instantiating the list, an Order attribute allows you to specify the order of the list items.  This is optional, and not required if order is unimportant.