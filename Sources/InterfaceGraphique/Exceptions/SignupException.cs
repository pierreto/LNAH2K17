using System;

namespace InterfaceGraphique.Exceptions
{
    class SignupException : Exception
    {
        public SignupException(string message) : base(message)
        {
        }
    }
}
