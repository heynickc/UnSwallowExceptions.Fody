using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnSwallowExceptions.Fody
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public class UnSwallowExceptionsAttribute : Attribute {
    }
}
