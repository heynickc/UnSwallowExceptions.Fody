using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;

public class WeavingException : Exception {
    public WeavingException(string message)
        : base(message) {
    }

    public SequencePoint SequencePoint { get; set; }
}
