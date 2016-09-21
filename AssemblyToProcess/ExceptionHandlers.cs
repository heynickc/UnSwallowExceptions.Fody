
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

        public void Rethrown_exception_for_reference() {
            try {
                throw new Exception();
            }
            catch (Exception) {
                throw;
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
        public void UnSwallow_nested_swallowed_exception() {
            try {
                try {
                    throw new Exception();
                }
                catch (Exception) {

                }
            }
            catch (Exception) {
                
                throw;
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
