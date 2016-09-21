![Icon](https://raw.github.com/Fody/BasicFodyAddin/master/Icons/package_icon.png)

This is a C#/Fody add-in that allows you to annotate a method with the [UnSwallowExceptions] attribute to make sure no exceptions are swallowed in the method.  It simply injects a `Rethrow` instruction at the end of the `catch` in a `try/catch` block.  All code before that point will remain in tact, but the exception will rethrow so you can deal with it.

Works great in combination with Simon Cropp's [Anotar](https://github.com/Fody/Anotar) project.

## Icon

<a href="http://thenounproject.com/noun/lego/#icon-No16919" target="_blank">Lego</a> designed by <a href="http://thenounproject.com/timur.zima" target="_blank">Timur Zima</a> from The Noun Project