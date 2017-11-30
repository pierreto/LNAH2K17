using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Exceptions
{
    public class FriendBusyException : Exception
    {
        public FriendBusyException(string message) : base(message)
        {
        }
    }
}
