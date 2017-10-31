using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Exceptions
{
    class ConnectServerException: Exception
    {
        public ConnectServerException(string message) : base(message)
        {
        }
    }
}
