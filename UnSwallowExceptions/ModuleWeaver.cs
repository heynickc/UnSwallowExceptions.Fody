using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Cecil.Cil;
using MethodBody = Mono.Cecil.Cil.MethodBody;

public partial class ModuleWeaver {
    // Will log an informational message to MSBuild
    public Action<string> LogInfo { get; set; }

    // An instance of Mono.Cecil.ModuleDefinition for processing
    public ModuleDefinition ModuleDefinition { get; set; }
    public IAssemblyResolver AssemblyResolver { get; set; }

    // Init logging delegates to make testing easier
    public ModuleWeaver() {
        LogInfo = m => { };
    }

    public void Execute() {
        LoadSystemTypes();
        var allTypes = ModuleDefinition.GetAllTypes();
        foreach (var type in allTypes) {
            var allMethods = type.GetMethods();
            foreach (var method in allMethods) {
                CustomAttribute unSwallowAttribute;
                if (!TryGetCustomAttribute(method, "UnSwallowExceptions.Fody.UnSwallowExceptionsAttribute", out unSwallowAttribute)) continue;

                ProcessMethodDefinition(method);
            }
        }
    }

    private bool TryGetCustomAttribute(MethodDefinition method, string attributeType, out CustomAttribute result) {
        result = null;
        if (!method.HasCustomAttributes)
            return false;

        foreach (CustomAttribute attribute in method.CustomAttributes) {
            if (attribute.AttributeType.FullName != attributeType)
                continue;

            result = attribute;
            return true;
        }

        return false;
    }

    private void ProcessMethodDefinition(MethodDefinition method) {
        MethodBody body = method.Body;
        body.SimplifyMacros();
        ILProcessor ilProcessor = body.GetILProcessor();

        if (body.HasExceptionHandlers) {
            for (int i = 0; i < body.ExceptionHandlers.Count; i++) {

                //var exceptionType = typeof(Exception);
                //var exceptionCtor = exceptionType.GetConstructor(new Type[] {});
                //var constructorReference = ModuleDefinition.ImportReference(exceptionCtor);

                //ilProcessor.InsertBefore(body.ExceptionHandlers[i].HandlerEnd.Previous, Instruction.Create(OpCodes.Newobj, constructorReference));
                //ilProcessor.InsertBefore(body.ExceptionHandlers[i].HandlerEnd.Previous, Instruction.Create(OpCodes.Throw));    
                
                ilProcessor.Replace(body.ExceptionHandlers[i].HandlerEnd.Previous, Instruction.Create(OpCodes.Rethrow));       
            }
        }
        body.InitLocals = true;
        body.OptimizeMacros();
    }
}