using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class UnitOfWorkException : Exception
    {
        public UnitOfWorkException() { }
        public UnitOfWorkException(string message) : base(message) { }
        public UnitOfWorkException(string message, Exception innerException) : base(message, innerException) { }
    }
}
