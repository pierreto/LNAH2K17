using AirHockeyServer.Entities;
using System;

namespace AirHockeyServer.Services
{
    public class SignupService : ISignupService, IService
    {
        public void Signup(SignupMessage message)
        {
            //TODO: add to db
            System.Diagnostics.Debug.WriteLine(message.username);
            System.Diagnostics.Debug.WriteLine(message.password);
            throw new System.NotImplementedException();
        }
    }

    public class SignupException : Exception
    {
        private string message;
        public SignupException(string message)
        {
            this.message = message;
        }
    }
}