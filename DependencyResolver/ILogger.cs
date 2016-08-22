using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DependencyResolver
{
    public interface ILogger
    {
        void Log(string content);
    }
}
