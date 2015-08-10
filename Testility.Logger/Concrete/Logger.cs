using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Logger.Abstract;

namespace Testility.Logger.Concrete
{
    public static class Logger
    {
        private static ILogger loger;
        private static object syncRoot = new Object();
        public static void Initalize(ILogger iLoger)
        {
            Contract.Requires<ArgumentException>(iLoger != null, "Logger.Initalize(iLoger) iLoger can not be null");
            loger = iLoger;
        }
        public static ILogger GetInstance()
        {
            if (loger == null)
            {
                lock (syncRoot)
                {
                    if (loger == null)
                        loger = new TraceLogger();
                }
            }

            return loger;
        }
    }
}
