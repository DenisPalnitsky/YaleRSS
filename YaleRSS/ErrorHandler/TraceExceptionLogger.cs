using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace YaleRSS.ErrorHandler
{
    public class TraceExceptionLogger : IExceptionLogger
    {
       
        public Task LogAsync(ExceptionLoggerContext context,
                      CancellationToken cancellationToken)
        {
            return Task.Run(() => Trace.WriteLine(context.Exception.ToString()), cancellationToken);
        }
    }
}