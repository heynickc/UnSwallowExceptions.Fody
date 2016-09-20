
using System;
using UnSwallowExceptions.Fody;

namespace AssemblyToProcess
{
    public class OnException {
        public void Swallowed_exception() {
            try {
                throw new Exception();
            }
            catch (Exception) {

            }
        }

        public void Swallowed_wrong_type_exception() {
            try {
                throw new ArgumentException();
            }
            catch (Exception) {

            }
        }

        [UnSwallowExceptions]
        public void Swallowed_exception_to_be_unswallowed() {
            try {
                throw new Exception();
            }
            catch (Exception) {

            }
        }

        [UnSwallowExceptions]
        public void Swallowed_exception_not_filtered_gets_unswallowed() {
            try {
                throw new ArgumentException();
            }
            catch (Exception) {

            }
        }
    }
}
