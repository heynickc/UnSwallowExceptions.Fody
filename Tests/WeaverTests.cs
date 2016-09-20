using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class WeaverTests
{
    string beforeAssemblyPath;
    Assembly assembly;
    string afterAssemblyPath;

    [TestFixtureSetUp]
    public void Setup()
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\AssemblyToProcess\bin\Debug\AssemblyToProcess.dll");

        AppDomainAssemblyFinder.Attach();
        beforeAssemblyPath = Path.GetFullPath(path);
#if (!DEBUG)
        beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif
        afterAssemblyPath = WeaverHelper.Weave(beforeAssemblyPath);
        assembly = Assembly.LoadFile(afterAssemblyPath);
    }

    [Test]
    public void Pre_weaved_exception_gets_swallowed() {
        var type = assembly.GetType("AssemblyToProcess.OnException");
        var instance = (dynamic) Activator.CreateInstance(type);

        Assert.DoesNotThrow(() => instance.Swallowed_exception());
    }

    [Test]
    public void Pre_weaved_unmatched_catch_statement_swallowed() {
        var type = assembly.GetType("AssemblyToProcess.OnException");
        var instance = (dynamic) Activator.CreateInstance(type);

        Assert.DoesNotThrow(() => instance.Swallowed_wrong_type_exception());
    }

    [Test]
    public void Expected_weaved_method_throws() {
        var type = assembly.GetType("AssemblyToProcess.OnException");
        var instance = (dynamic) Activator.CreateInstance(type);

        Assert.Throws<Exception>(() => instance.Swallowed_exception_to_be_unswallowed());
    }

    [Test]
    public void Uncaught_exception_type_gets_rethrown() {
        var type = assembly.GetType("AssemblyToProcess.OnException");
        var instance = (dynamic) Activator.CreateInstance(type);

        Assert.Throws<ArgumentException>(() => instance.Swallowed_exception_not_filtered_gets_unswallowed());
    }

#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
    }
#endif
}