using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anotar.Serilog;
using Serilog;
using UnSwallowExceptions.Fody;

namespace FodyTest {

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
        }
    }
}
