![Icon](https://raw.github.com/Fody/BasicFodyAddin/master/Icons/package_icon.png)

## This is an add-in for [Fody](https://github.com/Fody/Fody) 

This is a C#/Fody add-in that allows you to annotate a method with the [UnSwallowExceptions] attribute to make sure no exceptions are swallowed with empty `catch` statements.  It simply injects a `rethrow` instruction at the end of the `catch` in a `try/catch` block.  All code before that point will remain in-tact, but the exception will rethrow so you can deal with it.

Works great in combination with Simon Cropp's [Anotar](https://github.com/Fody/Anotar) project.

Install via Nuget:

```
Install-Package UnSwallowExceptions.Fody
```

### Usage

Say you have a method that looks like this:

```
public void Swallowed_exception() {
    try {
        // Fragile code someone wrote...
    }
    catch (Exception) {
    	// Ignore exceptions
    }
}
```

Simply add the attribute `[UnSwallowExceptions]` to the method and a `rethrow` instruction will be inserted at the end of the `catch` block:

```
[UnSwallowExceptions]
public void Swallowed_exception() {
    try {
        // Fragile code someone wrote...
    }
    catch (Exception) {
    	// Ignore exceptions
    	throw;
    }
}
```

### Using with Anotar

You can use Anotar in conjunction with this add-in to wrap the thrown exception with a logging statement.  Just make sure UnSwallowExceptions is before Anotar in FodyWeavers.xml.

```
<?xml version="1.0" encoding="utf-8"?>
<Weavers>
  <UnSwallowExceptions />
  <Anotar.Serilog />
</Weavers>
```

And simply comma-separate the attributes:

```
class Program {

    static Program() {
        var log = new LoggerConfiguration()
            .WriteTo.LiterateConsole()
            .CreateLogger();
        Log.Logger = log;
    }

    static void Main(string[] args) {
        DivideByZero();
        Console.ReadKey();
    }

    [UnSwallowExceptions, LogToErrorOnException]
    public static void DivideByZero() {
        try {
            int a = 10, b = 0;
            Console.WriteLine(a / b);
        }
        catch (Exception) { }
        // This will rethrow and log to Log.Error() using Serilog
    }
}
```

## Icon

<a href="http://thenounproject.com/noun/lego/#icon-No16919" target="_blank">Lego</a> designed by <a href="http://thenounproject.com/timur.zima" target="_blank">Timur Zima</a> from The Noun Project