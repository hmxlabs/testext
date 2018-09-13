# HMx Labs Test Ext
Test Ext is essentially just utility code to help more concise, fluent and reusable test code. Operating on the principle that your test cases are as important if not more important than your production code we started to write the sorts of helper and utility code that you’d normally see for production code but targeted at writing good tests.

The code depends on HMx Labs Core and relies on usage of some of the interfaces that the core library defines.

Broadly it provides the following

+ Mock network interface implementation (HmxLabs.Core.Net.INetworkInterface)

+ NUnit constraints to allow logging output to be tested. Allows writing code like:
````csharp
Assert.That(logger, HasNotLogged.Above(LogLevel.Info))
````
As part of your test cases. Useful when using unit test frameworks to do component level testing.

+ NUnit constraints to allow nice test code for serialization code such that you can write code like
````csharp
Assert.That(myObject, Serializes.With(mySerializer).ToFileContents(myReferenceFile)
Assert.That(myTestFile, Deserializes.With(mySerializer).FromFile().AndPasses(objectToTest => Assert.AreEqual(referenceObject, objectToTest)
````

## Documentation
Most of the code is commented inline, however there is no good documentation showing examples or use cases currently.

## Downloads
All versions are available either packaged as a NuGet package from [NuGet](http://nuget.org) or as a zip artefact from the [HMx Labs](http://hmxlabs.uk/software) website

## License
HMx Labs Core is Open Source software and is licensed under an MIT style license. This allows use in both free and commercial applications without restriction. The [complete licence text](http://hmxlabs.uk/software/license.html) is available on the [HMx Labs website](http://hmxlabs.uk) or [here](./license.txt)

## Contributing, Bug Reports and Support
If you’ve found a bug or thought of a new feature that would be useful feel free to either implement it and raise a pull request for us or if you’d like us to take a look then just get in touch.

Support is provided very much on a best effort only basis and by electronic means (emails, pull requests, bug reports etc) only.

Unless you’re a client of HMx Labs of course, in which case just give us a call!