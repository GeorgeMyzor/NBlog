using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace LoggingModule
{
    public static class MyLogger
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Test(string name)
        {
            logger.Error("New test");
        }
    }
}
